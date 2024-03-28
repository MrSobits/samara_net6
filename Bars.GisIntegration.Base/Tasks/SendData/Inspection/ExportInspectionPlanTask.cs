namespace Bars.GisIntegration.Base.Tasks.SendData.Inspection
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Entities.Inspection;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.InspectionAsync;
    using Bars.GisIntegration.Base.Tasks.SendData;

    /// <summary>
    /// Задача отправки данных по планам проверок
    /// </summary>
    public class ExportInspectionPlanTask : BaseSendDataTask<importInspectionPlanRequest, getStateResult, InspectionPortsTypeAsyncClient, RequestHeader>
    {
        /// <summary>
        /// Получить заголовок запроса
        /// </summary>
        /// <param name="messageGuid">Идентификатор запроса</param>
        /// <param name="package"></param>
        /// <returns>Заголовок запроса</returns>
        protected override RequestHeader GetHeader(string messageGuid, RisPackage package)
        {
            return new RequestHeader
            {
                Date = DateTime.Now,
                MessageGUID = messageGuid,
                IsOperatorSignature = package.IsDelegacy,
                IsOperatorSignatureSpecified = package.IsDelegacy,
                Item = package.RisContragent.OrgPpaGuid,
                ItemElementName = ItemChoiceType1.orgPPAGUID
            };
        }

        /// <summary>
        /// Выполнить запрос к асинхронному сервису ГИС
        /// </summary>
        /// <param name="header">Заголовок запроса</param>
        /// <param name="request">Объект запроса</param>
        /// <returns>Идентификатор сообщения для получения результата</returns>
        protected override string ExecuteRequest(RequestHeader header, importInspectionPlanRequest request)
        {
            AckRequest result = null;
            var soapClient = this.ServiceProvider.GetSoapClient();

            if (soapClient != null)
            {
                soapClient.importInspectionPlan(header, request, out result);
            }

            return result != null ? result.Ack.MessageGUID : string.Empty;
        }

        /// <summary>
        /// Получить результат экспорта пакета данных
        /// </summary>
        /// <param name="header">Заголовок запроса</param>
        /// <param name="ackMessageGuid">Идентификатор сообщения</param>
        /// <param name="result">Результат экспорта</param>
        /// <returns>Статус обработки запроса</returns>
        protected override sbyte GetStateResult(RequestHeader header, string ackMessageGuid, out getStateResult result)
        {
            var request = new getStateRequest { MessageGUID = ackMessageGuid };
            var soapClient = this.ServiceProvider.GetSoapClient();
            result = null;

            if (soapClient != null)
            {
                soapClient.getState(header, request, out result);
            }

            return result?.RequestState ?? 0;
        }

        /// <summary>
        /// Обработать результат экспорта пакета данных
        /// </summary>
        /// <param name="responce">Ответ от сервиса</param>
        /// <param name="transportGuidDictByType">Словарь transportGuid-ов для типа</param>
        /// <returns>Результат обработки пакета</returns>
        protected override PackageProcessingResult ProcessResult(getStateResult responce, Dictionary<Type, Dictionary<string, long>> transportGuidDictByType)
        {
            if (!transportGuidDictByType.ContainsKey(typeof(InspectionPlan)) &&
                !transportGuidDictByType.ContainsKey(typeof(Examination)))
            {
                throw new Exception("Не удалось обработать результат выполнения метода getState");
            }

            var plansByTransportGuid = transportGuidDictByType[typeof(InspectionPlan)];
            var examinationsByTransportGuid = transportGuidDictByType[typeof(Examination)];
            var result = new PackageProcessingResult
            {
                State = PackageState.SuccessProcessed,
                Objects = new List<ObjectProcessingResult>()
            };

            foreach (var item in responce.Items)
            {
                var errorMessageTypeItem = item as ErrorMessageType;
                var responseItem = item as CommonResultType;

                if (errorMessageTypeItem != null)
                {
                    result.Objects.Add(new ObjectProcessingResult
                    {
                        State = ObjectProcessingState.Error,
                        Message = errorMessageTypeItem.Description
                    });
                }
                else if (responseItem != null)
                {
                    ObjectProcessingResult processingResult = null;
                    var inspectionPlanDomain = this.Container.ResolveDomain<InspectionPlan>();
                    var examinationDomain = this.Container.ResolveDomain<Examination>();

                    try
                    {
                        if (!plansByTransportGuid.ContainsKey(responseItem.TransportGUID) &&
                            !examinationsByTransportGuid.ContainsKey(responseItem.TransportGUID))
                        {
                            if (responseItem.GUID.IsEmpty())
                            {
                                var error = responseItem.Items.FirstOrDefault() as CommonResultTypeError;
                                var errorNotation = error != null ? error.Description : "Вернулся пустой GUID";

                                processingResult = new ObjectProcessingResult
                                {
                                    Description = $"Не найден объект с TransportGuid = {responseItem.TransportGUID}",
                                    State = ObjectProcessingState.Error,
                                    Message = errorNotation
                                };
                            }
                            else
                            {
                                processingResult = new ObjectProcessingResult
                                {
                                    GisId = responseItem.GUID,
                                    Description = $"Не найден объект с TransportGuid = {responseItem.TransportGUID}",
                                    State = ObjectProcessingState.Success
                                };
                            }
                        }
                        else
                        {
                            long planId;
                            long examinationId;

                            plansByTransportGuid.TryGetValue(responseItem.TransportGUID, out planId);
                            examinationsByTransportGuid.TryGetValue(responseItem.TransportGUID, out examinationId);

                            if (responseItem.GUID.IsEmpty())
                            {
                                var error = responseItem.Items.FirstOrDefault() as CommonResultTypeError;
                                var errorNotation = error != null ? error.Description : "Вернулся пустой GUID";

                                processingResult = new ObjectProcessingResult
                                {
                                    Description = planId > 0 ? "План проверки" : "Проверка",
                                    RisId = planId > 0 ? planId : examinationId,
                                    State = ObjectProcessingState.Error,
                                    Message = errorNotation
                                };
                            }
                            else
                            {
                                if (planId > 0)
                                {
                                    var plan = inspectionPlanDomain.Get(planId);
                                    plan.Guid = responseItem.GUID;

                                    processingResult = new ObjectProcessingResult
                                    {
                                        Description = "План проверки",
                                        RisId = planId,
                                        GisId = plan.Guid,
                                        State = ObjectProcessingState.Success,
                                        ObjectsToSave = new List<PersistentObject> { plan }
                                    };
                                }
                                else if (examinationId > 0)
                                {
                                    var examination = examinationDomain.Get(examinationId);
                                    examination.Guid = responseItem.GUID;

                                    processingResult = new ObjectProcessingResult
                                    {
                                        Description = "Проверка",
                                        RisId = examinationId,
                                        GisId = examination.Guid,
                                        State = ObjectProcessingState.Success,
                                        ObjectsToSave = new List<PersistentObject> { examination }
                                    };
                                }
                            }
                        }
                    }
                    finally
                    {
                        this.Container.Release(inspectionPlanDomain);
                        this.Container.Release(examinationDomain);
                    }

                    result.Objects.Add(processingResult);
                }
            }

            return result;
        }
    }
}