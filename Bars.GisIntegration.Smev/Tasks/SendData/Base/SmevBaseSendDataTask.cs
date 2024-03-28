namespace Bars.GisIntegration.Smev.Tasks.SendData.Base
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;
    using System.Xml.Linq;
    using System.Xml.Serialization;

    using Bars.B4;
    using Bars.B4.Config;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.Events.Listeners.RisTask;
    using Bars.GisIntegration.Base.Listeners;
    using Bars.GisIntegration.Base.Tasks.SendData;
    using Bars.GisIntegration.Smev.Entity;
    using Bars.GisIntegration.Smev.SmevExchangeService;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Gis.RabbitMQ;
    using Bars.Gkh.Quartz.Scheduler.Entities;
    using Bars.Gkh.Smev3;
    using Bars.Gkh.Smev3.Attachments;
    using Bars.Gkh.Utils;

    using Fasterflect;

    using Quartz;
    using Quartz.Impl.Matchers;

    using ItemChoiceType = Bars.GisIntegration.Smev.SmevExchangeService.ItemChoiceType;

    public abstract class SmevBaseSendDataTask<TServiceResponseType, TLetterFromType, TIntegrationEntity> 
        : BaseSendDataTask<ServiceRequestType, ServiceResponseType, ServiceConsumerClient, object> 
        where TIntegrationEntity : BaseIntegrationEntity
    {
        /// <summary>
        /// Url коммуникатора СМЭВа
        /// </summary>
        protected virtual string SmevCommunicationUrl { get; set; }

        /// <summary>
        /// Сервис для отправки сообщений в очередь Rabbit
        /// </summary>
        public IProducerService RabbitProducerService { get; set; }

        /// <inheritdoc />
        protected override string ExecuteRequest(object header, ServiceRequestType request)
        {
            var appSettings = this.Container.Resolve<IConfigProvider>().GetConfig().GetModuleConfig("Bars.Gkh.Gis");
            var smev3Request = new Smev3Request
            {
                RequestId = Guid.NewGuid().ToString(),
                IsTestMessage = request.isTestMessage,
                Message = Encoding.UTF8.GetString(Convert.FromBase64String(request.base64Request)),
                Attachments = this.GetAttachments()
            };
            
            if (appSettings != null && appSettings[SettingsKeyStore.Enable].To<bool>())
            {
                this.RabbitProducerService.SendCustomMessage(
                    "smev3-integration-exchange", 
                    "smev3.message", 
                    smev3Request);
            }
            else
            {
                throw new ConfigurationException("Модуль Rabbit не активирован либо его настройки отсутствуют в файле конфигурации");
            }

            return smev3Request.RequestId;
        }

        protected virtual Smev3Attachment[] GetAttachments()
        {
            return new Smev3Attachment[] { };
        }

        /// <inheritdoc />
        protected override sbyte GetStateResult(object header, string ackMessageGuid, out ServiceResponseType result)
        {
            var storableSmev3ResponseDomain = this.Container.ResolveDomain<StorableSmev3Response>();
            result = null;

            using (this.Container.Using(storableSmev3ResponseDomain))
            {
                var smev3Response = storableSmev3ResponseDomain
                    .GetAll()
                    .FirstOrDefault(x => x.requestGuid.ToLower() == ackMessageGuid.ToLower())
                    ?.Response;

                if (smev3Response == null)
                {
                    return 2;
                }

                switch (smev3Response.State)
                {
                    case TransferState.Failure:
                        result = new ServiceResponseType
                        {
                            messageId = smev3Response.MessageId,
                            resultMessage = smev3Response.Message,
                            ItemElementName = ItemChoiceType.FaultResponse,
                        };
                        
                        this.UpdateResponseXmlIdInTask(ackMessageGuid, result);
                        
                        return 3;
                    case TransferState.Success:

                        result = new ServiceResponseType
                        {
                            messageId = smev3Response.MessageId,
                            ItemElementName = Bars.GisIntegration.Smev.SmevExchangeService.ItemChoiceType.MessagePrimaryContent,
                            resultMessage = smev3Response.Message
                        };

                        var doc = XDocument.Parse(smev3Response.Message);
                        var serializer = new XmlSerializer(typeof(TLetterFromType));

                        using (var reader = doc.CreateReader())
                        {
                            result.Item = serializer.Deserialize(reader);
                        }

                        this.UpdateResponseXmlIdInTask(ackMessageGuid, result);
                        
                        return 3;
                    default:
                        return 2;
                }
            }
        }

        /// <inheritdoc />
        protected sealed override PackageProcessingResult ProcessResult(ServiceResponseType response, Dictionary<Type, Dictionary<string, long>> transportGuidDictByType)
        {
            switch (response.ItemElementName)
            {
                case ItemChoiceType.FaultResponse:
                    //порождение исключения с целью записи в лог сообщения от ЕРП
                    throw new Exception(response.resultMessage);

                case ItemChoiceType.RequestRejected:
                    var rejectResponse = (SenderProvidedResponseDataRequestRejected) response.Item;
                    return new PackageProcessingResult
                    {
                        Message = rejectResponse.RejectionReason,
                        State = PackageState.ProcessedWithErrors
                    };

                case ItemChoiceType.MessagePrimaryContent:
                    var content = response.Item;
                    var setResult = (TServiceResponseType)content.GetPropertyValue("Item");
                    var result = this.ProcessSmevResponse(setResult, transportGuidDictByType);
                    switch (result.State)
                    {
                        case PackageState.SuccessProcessed:
                            result.Message = "Результат успешно обработан";
                            break;
                        case PackageState.ProcessedWithErrors:
                        {
                            var items = (object[])setResult.TryGetPropertyValue("Items");
                            if (items != null && items.Any())
                            {
                                var sb = new StringBuilder();
                                sb.Append("Ошибка обработки результата: ");
                                foreach (var item in items)
                                {
                                    var errors = (object[])item?.TryGetPropertyValue("Errors");
                                    if (errors != null && errors.Any())
                                    {
                                        var errorsText = errors.Select(x => x?.TryGetPropertyValue("text").ToString()).ToList();
                                        foreach (var error in errorsText)
                                        {
                                            sb.Append($"<br>{error}");
                                        }
                                    }
                                }
                                result.Message = sb.ToString();
                            }
                            break;
                        }
                    }
                    return result;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        /// <summary>
        /// Обработать результат экспорта пакета данных
        /// </summary>
        protected abstract PackageProcessingResult ProcessSmevResponse(TServiceResponseType response, Dictionary<Type, Dictionary<string, long>> transportGuidDictByType);

        /// <summary>
        /// Получить типизированное содерживое пакета
        /// </summary>
        protected T GetTypedContent<T>(XmlElement element)
        {
            using (var memoryStream = new MemoryStream())
            {
                var content = Encoding.UTF8.GetBytes(element.OuterXml);
                memoryStream.Write(content, 0, content.Length);
                memoryStream.Seek(0, SeekOrigin.Begin);
                using (TextReader streamReader = new StreamReader(memoryStream, Encoding.UTF8))
                {
                    var xmlSerializer = new XmlSerializer(typeof(T));
                    return (T)xmlSerializer.Deserialize(streamReader);
                }
            }
        }

        /// <summary>
        /// Обновить GUID сущности
        /// </summary>
        protected void UpdateGuid(string newGuid)
        {
            if (string.IsNullOrWhiteSpace(newGuid))
            {
                return;
            }

            var integrationEntityDomain = this.Container.ResolveDomain<TIntegrationEntity>();
            var taskTriggerDomain = this.Container.ResolveDomain<RisTaskTrigger>();

            using (this.Container.Using(integrationEntityDomain, taskTriggerDomain))
            {
                var task = taskTriggerDomain.GetAll()
                    .FirstOrDefault(x => x.Trigger.Id == this.StorableTrigger.Id)?.Task;

                var integrationEntityList = integrationEntityDomain.GetAll()
                    .Where(w => w.Guid.ToUpper() == newGuid.ToUpper() && w.Task == task)
                    .ToList();

                if (!integrationEntityList.Any())
                {
                    return;
                }

                var entityDomainDict = integrationEntityList.Distinct(x => x.EntityType)
                    .ToDictionary(x => x.EntityType,
                        x =>
                        {
                            var entityType = typeof(IDomainService<>).MakeGenericType(
                                Type.GetType($"{x.EntityType}, {x.AssemblyType}"));

                            return this.Container.Resolve(entityType) as IDomainService;
                        });

                using (this.Container.Using(entityDomainDict.Values))
                {
                    foreach (var integrationEntity in integrationEntityList)
                    {
                        var entityDomain = entityDomainDict[integrationEntity.EntityType];
                        var item = entityDomain.Get(integrationEntity.EntityId);
                        var propertyValue = string.Empty;

                        try
                        {
                            propertyValue = item?.GetPropertyValue(integrationEntity.FieldName)?.ToString();
                        }
                        catch (MissingMemberException)
                        {
                            continue;
                        }
                        
                        if (string.Equals(propertyValue, newGuid, StringComparison.CurrentCultureIgnoreCase))
                        {
                            continue;
                        }

                        item.SetPropertyValue(integrationEntity.FieldName, newGuid);

                        this.Container.InTransaction(() =>
                        {
                            entityDomain.Update(item);
                            if (!integrationEntity.IsAnswered)
                            {
                                integrationEntity.IsAnswered = true;
                                integrationEntityDomain.Update(integrationEntity);
                            }
                        });
                    }
                }
            }
        }

        /// <inheritdoc />
        protected override object GetHeader(string messageGuid, RisPackage package)
        {
            return new object();
        }

        private void UpdateResponseXmlIdInTask(string ackMessageGuid, ServiceResponseType result)
        {
            var taskTrigger = this.GetRisTaskTrigger(ackMessageGuid, out var trigger);
            if (taskTrigger == null || trigger == null)
            {
                return;
            }

            this.CheckAndAddTriggerListener(taskTrigger, trigger);

            var xmlElement = XElement.Parse(result.resultMessage);
            this.SaveResponseXml(taskTrigger.Task, xmlElement);
        }

        /// <summary>
        /// Проверяет наличие слушателя триггера, в случае отсутствия добавляет слушателя.
        /// </summary>
        /// <param name="taskTrigger">Связка задачи с выполняющим ее триггером</param>
        /// <param name="trigger">Хранимый триггер</param>
        private void CheckAndAddTriggerListener(RisTaskTrigger taskTrigger, Trigger trigger)
        {
            var scheduler = this.Container.Resolve<IScheduler>("TaskScheduler");
            using (this.Container.Using(scheduler))
            {
                var listener = scheduler.ListenerManager.GetTriggerListener(typeof(SubTaskListener).Name + taskTrigger.Task?.Id);
                var executingJob = scheduler.GetCurrentlyExecutingJobs().FirstOrDefault(x => x.Trigger.Key.Name == trigger.QuartzTriggerKey);

                if (listener != null || executingJob == null)
                {
                    return;
                }

                executingJob.MergedJobDataMap.Put("TaskTrigger", taskTrigger);
                RisTaskStateCalculator.HandleSendDataStage(taskTrigger.Task);

                var subTaskListener = new SubTaskListener(this.Container, taskTrigger.Task, TriggerType.SendingData);
                scheduler.ListenerManager.AddJobListener(subTaskListener, KeyMatcher<JobKey>.KeyEquals<JobKey>(executingJob.JobDetail.Key));
                scheduler.ListenerManager.AddTriggerListener(subTaskListener, KeyMatcher<TriggerKey>.KeyEquals<TriggerKey>(executingJob.Trigger.Key));
            }
        }

        /// <summary>
        /// Сохраняет файл запроса.
        /// </summary>
        /// <param name="task">Задача.</param>
        /// <param name="serializedResponse">Файл ответа.</param>
        private void SaveResponseXml(RisTask task, XElement serializedResponse)
        {
            var fileManager = this.Container.Resolve<IFileManager>();
            var taskDomain = this.Container.ResolveDomain<RisTask>();
            using (this.Container.Using(fileManager, taskDomain))
            {
                var fileData = new FileData($"Response_xml_{task.DocumentGji?.Id}", "xml", Encoding.UTF8.GetBytes(serializedResponse.ToString()));
                var file = fileManager.SaveFile(fileData);
                task.ResponseXmlFile = file;
                taskDomain.Update(task);
            }
        }

        private RisTaskTrigger GetRisTaskTrigger(string ackMessageGuid, out Trigger trigger)
        {
            if (string.IsNullOrWhiteSpace(ackMessageGuid))
            {
                trigger = null;
                return null;
            }

            var packageTriggerDomain = this.Container.ResolveDomain<RisPackageTrigger>();
            var taskTriggerDomain = this.Container.ResolveDomain<RisTaskTrigger>();
            using (this.Container.Using(packageTriggerDomain, taskTriggerDomain))
            {
                trigger = packageTriggerDomain.FirstOrDefault(x => string.Equals(ackMessageGuid, x.AckMessageGuid))?.Trigger;
                var triggerId = trigger?.Id ?? default(long);
                return trigger == null 
                    ? null 
                    : taskTriggerDomain.FirstOrDefault(x => x.Trigger.Id == triggerId);
            }
        }
    }
}