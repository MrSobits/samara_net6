namespace Bars.GisIntegration.Base.Tasks.SendData.Payment
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4.DataAccess;
    using B4.Utils;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.PaymentAsync;
    using Bars.GisIntegration.Base.Tasks.SendData;
    using Entities.Payment;

    /// <summary>
    /// Задача отправки данных документов «Извещение о принятии к исполнению распоряжения, размещаемых исполнителем»
    /// </summary>
    public class ExportSupplierNotificationsOfOrderExecutionTask : BaseSendDataTask<importSupplierNotificationsOfOrderExecutionRequest, getStateResult, PaymentPortsTypeAsyncClient, RequestHeader>
    {
        /// <summary>
        /// Получить новый заголовок запроса
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
                IsOperatorSignatureSpecified = package.IsDelegacy
            };
        }

        /// <summary>
        /// Выполнить запрос к асинхронному сервису ГИС
        /// </summary>
        /// <param name="header">Заголовок запроса</param>
        /// <param name="request">Объект запроса</param>
        /// <returns>Идентификатор сообщения для получения результата</returns>
        protected override string ExecuteRequest(RequestHeader header, importSupplierNotificationsOfOrderExecutionRequest request)
        {
            AckRequest result = null;
            var soapClient = this.ServiceProvider.GetSoapClient();

            if (soapClient != null)
            {
                soapClient.importSupplierNotificationsOfOrderExecution(header, request, out result);
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
            if (!transportGuidDictByType.ContainsKey(typeof(NotificationOfOrderExecution)))
            {
                throw new Exception("Не удалось обработать результат выполнения метода getState");
            }
            var contractsByTransportGuid = transportGuidDictByType[typeof(NotificationOfOrderExecution)];

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
                    var processingResult = this.CheckResponseItem(responseItem, contractsByTransportGuid);
                    result.Objects.Add(processingResult);
                }
            }

            return result;
        }

        /// <summary>
        /// Проверить строку элемент response и добавить объекты ГИС на сохранение
        /// </summary>
        /// <param name="responseItem">Элемент response</param>
        /// <param name="transportGuidDict">Словарь transportGuid-ов</param>
        private ObjectProcessingResult CheckResponseItem(CommonResultType responseItem, Dictionary<string, long> transportGuidDict)
        {
            var domain = this.Container.ResolveDomain<NotificationOfOrderExecution>();

            try
            {
                if (!transportGuidDict.ContainsKey(responseItem.TransportGUID))
                {
                    if (responseItem.GUID.IsEmpty())
                    {
                        var error = responseItem.Items.FirstOrDefault() as CommonResultTypeError;
                        var errorNotation = error != null ? error.Description : "Вернулся пустой GUID";

                        return new ObjectProcessingResult
                        {
                            Description = $"Не найден объект с TransportGuid = {responseItem.TransportGUID}",
                            State = ObjectProcessingState.Error,
                            Message = errorNotation
                        };
                    }

                    return new ObjectProcessingResult
                    {
                        GisId = responseItem.GUID,
                        Description = $"Не найден объект с TransportGuid = {responseItem.TransportGUID}",
                        State = ObjectProcessingState.Success
                    };
                }

                var notificationId = transportGuidDict[responseItem.TransportGUID];

                if (responseItem.GUID.IsEmpty())
                {
                    var error = responseItem.Items.FirstOrDefault() as CommonResultTypeError;
                    var errorNotation = error != null ? error.Description : "Вернулся пустой GUID";

                    return new ObjectProcessingResult
                    {
                        Description = "Извещение о принятии к исполнению распоряжения",
                        RisId = notificationId,
                        State = ObjectProcessingState.Error,
                        Message = errorNotation
                    };
                }

                var contract = domain.Get(notificationId);
                contract.Guid = responseItem.GUID;

                return new ObjectProcessingResult
                {
                    Description = "Извещение о принятии к исполнению распоряжения",
                    RisId = notificationId,
                    State = ObjectProcessingState.Success,
                    ObjectsToSave = new List<PersistentObject> { contract }
                };
            }
            finally
            {
                this.Container.Release(domain);
            }
        }
    }
}
