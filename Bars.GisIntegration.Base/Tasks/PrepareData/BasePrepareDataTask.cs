namespace Bars.GisIntegration.Base.Tasks.PrepareData
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Xml;
    using System.Xml.Serialization;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Domain;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.Exporters;
    using Bars.GisIntegration.Base.Package;
    using Bars.GisIntegration.Base.Service;
    using Bars.GisIntegration.Base.Service.Impl;
    using Bars.Gkh.Quartz.Scheduler;
    using Bars.Gkh.Quartz.Scheduler.Entities;
    using Bars.Gkh.Quartz.Scheduler.Extensions;
    using Bars.Gkh.Quartz.Scheduler.Log;

    using Quartz;

    /// <summary>
    /// Базовый класс задачи подготовки данных
    /// </summary>
    /// <typeparam name="TRequestType">Тип объекта запроса к сервису ГИС</typeparam>
    public abstract class BasePrepareDataTask<TRequestType> : BaseTask
    {
        private byte createPackagesPercent;

        protected byte extractDataPercent;

        protected byte validateDataPercent;

        protected byte validateAttachmentsPercent;

        protected byte getRequestDataPercent;

        protected byte uploadAttachmentsPercent;

        private byte processedDataSuppliers;

        /// <summary>
        /// Менеджер задач
        /// </summary>
        public ITaskManager TaskManager { get; set; }

        /// <summary>
        /// Менеджер пакетов
        /// </summary>
        public IPackageManager<RisPackage, long> PackageManager { get; set; }

        public IDomainService<RisTaskTrigger> RisTaskTriggerDomain { get; set; }

        /// <summary>
        /// Поставщик данных
        /// </summary>
        public RisContragent Contragent => DataSupplierContext.Current?.DataSupplier;

        /// <summary>
        /// Параметры извлечения данных в разбивке по поставщикам - резолвится из контекста
        /// </summary>
        public IDictionary<long, DynamicDictionary> ExtractParams { get; set; }

        /// <summary>
        /// Наименование хранилища данных ГИС для загрузки вложений
        /// </summary>
        public FileStorageName? FileStorageName { get; set; }

        /// <summary>
        /// Признак того, что данные готовятся для подписывания  - резолвится из контекста
        /// </summary>
        public bool DataForSigning { get; set; }

        /// <summary>
        /// Процент выполнения работы
        /// </summary>
        public override byte Percent {
            get
            {
                var percent = (40 * this.extractDataPercent / 100) 
                    + (5 * this.validateDataPercent / 100) 
                    + (25 * this.uploadAttachmentsPercent / 100)
                    + (5 * this.validateAttachmentsPercent / 100)
                    + (5 * this.getRequestDataPercent / 100)
                    + (20 * this.createPackagesPercent / 100);

                return (byte)((this.processedDataSuppliers * (100 / this.ExtractParams.Count)) + (percent / this.ExtractParams.Count));
            }
        }

        /// <summary>
        /// Отменить запланированное выполнение триггера при возникновении ошибки
        /// </summary>
        protected override bool UnscheduleOnError => true;

        /// <summary>
        /// Метод подготовки данных к отправке, включая валидацию, формирование пакетов
        /// </summary>
        /// <param name="context">Контекст выполнения</param>
        /// <returns>Результат подготовки данных</returns>
        protected override object DoWork(IJobExecutionContext context)
        {
            var validateResult = new List<ValidateObjectResult>();
            var uploadResult = new List<UploadAttachmentResult>();
            var packageIds = new List<long>();

            foreach (var extractParam in this.ExtractParams)
            {
                using (new DataSupplierContext(extractParam.Key))
                {
                    var result = this.DoWork(context, extractParam.Value);

                    validateResult.AddRange(result.ValidateResult);
                    if (result.UploadResult != null)
                    {
                        uploadResult.AddRange(result.UploadResult);
                    }

                    packageIds.AddRange(result.PackageIds);
                }

                this.processedDataSuppliers++;
            }

            if (packageIds.Count > 0)
            {
                var taskTriggerLink = this.RisTaskTriggerDomain.GetAll()
                    .First(x => x.Trigger.QuartzTriggerKey == context.Trigger.Key.Name);

                //Если пактеты созданы, подписаны и отсутствуют невалидные, то вызвыть метод отправки пакета
                if (validateResult.IsEmpty() || validateResult.All(x => x.State == ObjectValidateState.Success))
                {
                    var taskManager = this.Container.Resolve<ITaskManager>();
                    var exporter = this.Container.Resolve<IDataExporter>(taskTriggerLink.Task.ClassName);

                    try
                    {
                        taskManager.CreateSendDataSubTask(exporter, taskTriggerLink.Task, packageIds.ToArray());
                    }
                    finally
                    {
                        this.Container.Release(taskManager);
                        this.Container.Release(exporter);
                    }
                }
            }

            return new SerializablePrepareDataResult
                       {
                           ValidateResult = validateResult,
                           UploadResult = uploadResult.Count > 0 ? uploadResult : null,
                           PackageIds = packageIds
                       };
        }

        protected SerializablePrepareDataResult DoWork(IJobExecutionContext context, DynamicDictionary extractParams)
        {
            if (this.Contragent != null)
            {
                this.AddLogRecord(new BaseLogRecord(MessageType.Info, $"Извлечение данных для поставщика \"{this.Contragent.FullName}\""));
            }

            IReadOnlyList<Attachment> attachments;
            if (!this.InterruptRequested)
            {
                using (var attachmentContext = new AttachmentContext(this.FileStorageName))
                {
                    this.CallExtractData(extractParams);
                    attachments = attachmentContext.Attachments;
                }
            }
            else
            {
                this.HandleInterrupt();
                return new SerializablePrepareDataResult();
            }

            List<ValidateObjectResult> validateResult;

            if (!this.InterruptRequested)
            {
                validateResult = this.CallValidateData();
            }
            else
            {
                this.HandleInterrupt();
                return new SerializablePrepareDataResult();
            }

            List<UploadAttachmentResult> uploadResult = null;
            if (attachments.Count > 0)
            {
                if (!this.InterruptRequested)
                {
                    using (new AttachmentContext(this.FileStorageName))
                    {
                        uploadResult = this.CallUploadAttachments(attachments);
                    }
                }
                else
                {
                    this.HandleInterrupt();
                    return new SerializablePrepareDataResult();
                }
            }

            if (!this.InterruptRequested)
            {
                validateResult.AddRange(this.CallValidateAttachments());
            }
            else
            {
                this.HandleInterrupt();
                return new SerializablePrepareDataResult();
            }

            Dictionary<TRequestType, Dictionary<Type, Dictionary<string, long>>> requestData;

            if (!this.InterruptRequested)
            {
                requestData = this.CallGetRequestData();
            }
            else
            {
                this.HandleInterrupt();
                return new SerializablePrepareDataResult();
            }

            List<RisPackage> packages;

            if (!this.InterruptRequested)
            {
                packages = this.CallCreatePackages(requestData);
            }
            else
            {
                this.HandleInterrupt();
                return new SerializablePrepareDataResult();
            }

            return new SerializablePrepareDataResult
                       {
                           ValidateResult = validateResult,
                           UploadResult = uploadResult,
                           PackageIds = packages.Select(x => x.Id).ToList()
                       };
        }

        /// <summary>
        /// Собрать данные
        /// </summary>
        /// <param name="parameters">Параметры извлечения данных</param>
        protected abstract void ExtractData(DynamicDictionary parameters);

        /// <summary>
        /// Валидация данных
        /// </summary>
        /// <returns>Результат валидации</returns>
        protected abstract List<ValidateObjectResult> ValidateData();

        /// <summary>
        /// Валидация прикрепленных файлов
        /// </summary>
        /// <returns>Результат валидации</returns>
        protected virtual List<ValidateObjectResult> ValidateAttachments()
        {
            return new List<ValidateObjectResult>();
        }

        /// <summary>
        /// Сформировать объекты запросов к асинхронному сервису ГИС
        /// </summary>
        /// <returns>Словарь Объект запроса - Словарь Транспортных идентификаторов: Тип обектов - Словарь: Транспортный идентификатор - Идентификатор объекта</returns>
        protected abstract Dictionary<TRequestType, Dictionary<Type, Dictionary<string, long>>> GetRequestData();

        private void HandleInterrupt()
        {
            this.AddLogRecord(new BaseLogRecord(MessageType.Info, "Запрошено прерывание выполнения задачи"));
        }

        private void CallExtractData(DynamicDictionary extractParams)
        {
            this.AddLogRecord(new BaseLogRecord(MessageType.Info, "Начато извлечение данных"));

            try
            {
                this.ExtractData(extractParams);

                this.extractDataPercent = 100;

                this.AddLogRecord(new BaseLogRecord(MessageType.Info, "Завершено извлечение данных"));
            }
            catch (Exception exception)
            {
                this.AddLogRecord(new BaseLogRecord(MessageType.Error, string.Format("Ошибка извлечения данных: {0}", exception.Message)));
                throw;
            }
        }

        private List<ValidateObjectResult> CallValidateData()
        {
            this.AddLogRecord(new BaseLogRecord(MessageType.Info, "Начата валидация данных"));

            try
            {
                var validateData = this.ValidateData();
            
                var warning = validateData.Count(x => x.State == ObjectValidateState.Warning);
                var error = validateData.Count(x => x.State == ObjectValidateState.Error);

                this.validateDataPercent = 100;

                this.AddLogRecord(new BaseLogRecord(MessageType.Info, string.Format("Завершена валидация данных: предупреждений {0}, ошибок {1}", warning, error)));

                return validateData;
            }
            catch (Exception exception)
            {
                this.AddLogRecord(new BaseLogRecord(MessageType.Error, string.Format("Ошибка валидации данных: {0}", exception.Message)));
                throw;
            }
        }

        private List<ValidateObjectResult> CallValidateAttachments()
        {
            this.AddLogRecord(new BaseLogRecord(MessageType.Info, "Начата проверка результатов отправки вложений"));

            try
            {
                var validateAttachments = this.ValidateAttachments();

                var warning = validateAttachments.Count(x => x.State == ObjectValidateState.Warning);
                var error = validateAttachments.Count(x => x.State == ObjectValidateState.Error);

                this.validateAttachmentsPercent = 100;

                this.AddLogRecord(new BaseLogRecord(MessageType.Info, string.Format("Завершена проверка результатов отправки вложений: предупреждений {0}, ошибок {1}", warning, error)));

                return validateAttachments;
            }
            catch (Exception exception)
            {
                this.AddLogRecord(new BaseLogRecord(MessageType.Error, string.Format("Ошибка проверки результатов отправки вложений: {0}", exception.Message)));
                throw;
            }
        }

        private List<UploadAttachmentResult> CallUploadAttachments(IReadOnlyList<Attachment> attachments)
        {
            this.AddLogRecord(new BaseLogRecord(MessageType.Info, "Начата загрузка вложений"));

            var orgPpaGuid = this.Contragent?.OrgPpaGuid;
            if (orgPpaGuid.IsEmpty())
            {
                throw new InvalidOperationException("Ошибка отправки вложений: не определен идентификатор организации в ГИС");
            }

            var attachmentService = this.Container.Resolve<IAttachmentService>();
            try
            {
                var result = new List<UploadAttachmentResult>();

                var processed = 0;
                foreach (var attachment in attachments.Where(x => x.IsValid()))
                {
                    try
                    {
                        attachmentService.UploadAttachment(attachment, orgPpaGuid);
                        result.Add(
                            new UploadAttachmentResult
                            {
                                Id = attachment.Id,
                                Description = attachment.Description,
                                Name = attachment.Name,
                                State = ObjectValidateState.Success
                            });
                    }
                    catch (Exception e)
                    {
                        result.Add(
                            new UploadAttachmentResult
                            {
                                Id = attachment.Id,
                                Description = attachment.Description,
                                Name = attachment.Name,
                                Message = e.Message,
                                State = ObjectValidateState.Error
                            });

                       // this.AddLogRecord(new BaseLogRecord(MessageType.Error, string.Format("Ошибка отправки вложения: Id = {0}, Name = {1}, Message = {2}", attachment.Id, attachment.Name, e.Message)));
                    }

                    processed++;
                    this.uploadAttachmentsPercent = (byte)((processed * 100) / attachments.Count);
                }

                this.AddLogRecord(new BaseLogRecord(MessageType.Info, "Завершена загрузки вложений"));

                return result;
            }
            finally
            {
                this.Container.Release(attachmentService);
            }
        }

        private Dictionary<TRequestType, Dictionary<Type, Dictionary<string, long>>> CallGetRequestData()
        {
            this.AddLogRecord(new BaseLogRecord(MessageType.Info, "Начато формирование объектов запроса"));

            try
            {
                var requestData = this.GetRequestData();

                this.getRequestDataPercent = 100;

                this.AddLogRecord(new BaseLogRecord(MessageType.Info, "Завершено формирование объектов запроса"));

                return requestData;
            }
            catch (Exception exception)
            {
                this.AddLogRecord(new BaseLogRecord(MessageType.Error, string.Format("Ошибка формирования объектов запроса: {0}", exception.Message)));
                throw;
            }
        }

        private List<RisPackage> CallCreatePackages(Dictionary<TRequestType, Dictionary<Type, Dictionary<string, long>>> requestData)
        {
            this.AddLogRecord(new BaseLogRecord(MessageType.Info, "Начато создание пакетов данных"));

            try
            {
                var packages = new List<RisPackage>();

                var i = 1;

                foreach (var requestDataItem in requestData)
                {
                    var package = this.CreateRisPackage(requestDataItem.Key, requestDataItem.Value, string.Format("Пакет {0}", i));
                    packages.Add(package);
                    i++;
                }

                this.createPackagesPercent = 20;

                this.SavePackages(packages);

                this.createPackagesPercent = 60;

                this.SetPackageTriggerRelations(this.StorableTrigger, packages);

                this.createPackagesPercent = 100;

                this.AddLogRecord(new BaseLogRecord(MessageType.Info, string.Format("Завершено создание пакетов данных: {0} пакетов", packages.Count)));

                return packages;
            }
            catch (Exception exception)
            {
                this.AddLogRecord(new BaseLogRecord(MessageType.Error, string.Format("Ошибка создания пакетов данных: {0}", exception.Message)));
                throw;
            }
        }

        private RisPackage CreateRisPackage(TRequestType requestDataItem, Dictionary<Type, Dictionary<string, long>> transportGuidDictionary, string name)
        {
            var package = this.PackageManager.CreatePackage(name, requestDataItem, transportGuidDictionary);
            package.RisContragent = this.Contragent;
            package.IsDelegacy = DataSupplierContext.Current?.IsDelegacy ?? false;

            return package;
        }

        private void SavePackages(List<RisPackage> packages)
        {
            TransactionHelper.InsertInManyTransactions(this.Container, packages);
        }

        private void SetPackageTriggerRelations(Trigger trigger, IEnumerable<RisPackage> packages)
        {
            var packageTriggerRepository = this.Container.ResolveDomain<RisPackageTrigger>();

            try
            {
                var packageTriggerList = new List<RisPackageTrigger>();

                foreach (var package in packages)
                {
                    packageTriggerList.Add(new RisPackageTrigger
                    {
                        Package = package,
                        Trigger = trigger,
                        State = PackageState.New
                    });
                }

                TransactionHelper.InsertInManyTransactions(this.Container, packageTriggerList, 1000, true, true);
            }
            finally
            {
                this.Container.Release(packageTriggerRepository);
            }
        }

        /// <summary>
        /// Запустить экстрактор на выполнение
        /// </summary>
        /// <typeparam name="TRisEntity">Тип Ris-сущности</typeparam>
        /// <param name="extractor">Экстрактор</param>
        /// <param name="parameters">Параметры выполнения экстрактора</param>
        /// <returns>Результат выполнения экстрактора</returns>
        protected List<TRisEntity> RunExtractor<TRisEntity>(IDataExtractor<TRisEntity> extractor, DynamicDictionary parameters) where TRisEntity : BaseEntity
        {
            var result = extractor.Extract(parameters);
            this.AddLogRecord(extractor.Log);

            return result;
        }

        /// <summary>
        /// Вспомогательный метод валидации списков объектов.
        /// Невалидные объекты исключаются из списка
        /// </summary>
        /// <typeparam name="T">Тип объекта</typeparam>
        /// <param name="objectList">Список объектов для валидации</param>
        /// <param name="validateObjectFunc">Функция валидатор</param>
        /// <returns>Список ошибок валидации</returns>
        protected List<ValidateObjectResult> ValidateObjectList<T>(List<T> objectList, Func<T, ValidateObjectResult> validateObjectFunc)
            where T : BaseEntity
        {
            var result = new List<ValidateObjectResult>();
            var objectsToRemove = new List<T>();

            foreach (var obj in objectList)
            {
                var validateResult = validateObjectFunc(obj);

                if (validateResult.State != ObjectValidateState.Success)
                {
                    result.Add(validateResult);
                    objectsToRemove.Add(obj);
                }
            }

            foreach (var objToRemove in objectsToRemove)
            {
                objectList.Remove(objToRemove);
            }

            return result;
        }
    }
}
