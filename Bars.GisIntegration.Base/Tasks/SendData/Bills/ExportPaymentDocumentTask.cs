namespace Bars.GisIntegration.Base.Tasks.SendData.Bills
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.BillsAsync;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.Tasks.SendData;
    using Entities.Bills;

    /// <summary>
    /// Задача отправки сведений о платежных документах
    /// </summary>
    public class ExportPaymentDocumentTask : BaseSendDataTask<importPaymentDocumentRequest, getStateResult, BillsPortsTypeAsyncClient, RequestHeader>
    {
        /// <summary>
        /// Выполнить запрос к асинхронному сервису ГИС
        /// </summary>
        /// <param name="header">Заголовок запроса</param>
        /// <param name="request">Объект запроса</param>
        /// <returns>Идентификатор сообщения для получения результата</returns>
        protected override string ExecuteRequest(RequestHeader header, importPaymentDocumentRequest request)
        {
            AckRequest result;
            var soapClient = this.ServiceProvider.GetSoapClient();

            if (soapClient == null)
            {
                throw new Exception("Не удалось получить SOAP клиент");
            }

            soapClient.importPaymentDocumentData(header, request, out result);

            if (result?.Ack == null)
            {
                throw new Exception("Пустой результат выполенния запроса");
            }

            return result.Ack.MessageGUID;
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
        protected override PackageProcessingResult ProcessResult(
            getStateResult responce,
            Dictionary<Type, Dictionary<string, long>> transportGuidDictByType)
        {
            var paymentDocumentDomain = this.Container.ResolveDomain<RisPaymentDocument>();

            if (!transportGuidDictByType.ContainsKey(typeof(RisPaymentDocument)))
            {
                throw new Exception("Не удалось обработать результат выполнения метода getState");
            }
            var paymentDocumentByTransportGuid = transportGuidDictByType[typeof(RisPaymentDocument)];

            try
            {
                return this.ParseStateResult(responce, paymentDocumentDomain, paymentDocumentByTransportGuid);
            }
            finally
            {
                this.Container.Release(paymentDocumentDomain);
            }
        }

        private PackageProcessingResult ParseStateResult(
            getStateResult responce, 
            IDomainService<RisPaymentDocument> paymentDocumentDomain,
            Dictionary<string, long> transportGuidDict)
        {
            var result = new PackageProcessingResult { State = PackageState.SuccessProcessed, Objects = new List<ObjectProcessingResult>()};

            foreach (var item in responce.Items)
            {
                var errorItem = item as CommonResultTypeError;
                var errorMessageTypeItem = item as ErrorMessageType;
                var responseItem = item as CommonResultType;

                if (errorItem != null)
                {                   
                    result.Objects.Add(new ObjectProcessingResult
                                       {
                                           State = ObjectProcessingState.Error,
                                           Message = errorItem.Description
                    });
                }
                else if (errorMessageTypeItem != null)
                {
                    result.Objects.Add(new ObjectProcessingResult
                    {
                        State = ObjectProcessingState.Error,
                        Message = errorMessageTypeItem.Description
                    });
                }
                else if (responseItem != null)
                {
                   var processingResult = this.CheckResponseItem(responseItem, paymentDocumentDomain, transportGuidDict);
                   result.Objects.Add(processingResult);
                }
            }

            return result;
        }

        /// <summary>
        /// Проверить строку элемент response и добавить объекты ГИС на сохранение.
        /// </summary>
        /// <param name="responseItem">Элемент response</param>
        /// <param name="paymentDocumentDomain"></param>
        /// <param name="transportGuidDict">Словарь transportGuid-ов</param>
        private ObjectProcessingResult CheckResponseItem(
            CommonResultType responseItem, 
            IDomainService<RisPaymentDocument> paymentDocumentDomain,
            Dictionary<string, long> transportGuidDict)
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
                           Description =
                               $"Не найден объект с TransportGuid = {responseItem.TransportGUID}",
                           State = ObjectProcessingState.Success
                       };
            }

            var paymentDocumentId = transportGuidDict[responseItem.TransportGUID];

            if (responseItem.GUID.IsEmpty())
            {
                var error = responseItem.Items.FirstOrDefault() as CommonResultTypeError;

                var errorNotation = error != null ? error.Description : "Вернулся пустой GUID";

                return new ObjectProcessingResult
                {
                    Description = "Сведения о платежных документах",
                    RisId = paymentDocumentId,
                    State = ObjectProcessingState.Error,
                    Message = errorNotation
                };
            }

            var paymentDocument = paymentDocumentDomain.Get(paymentDocumentId);

            paymentDocument.Guid = responseItem.GUID;

            return new ObjectProcessingResult
            {
                Description = "Сведения о платежных документах",
                RisId = paymentDocumentId,
                State = ObjectProcessingState.Success,
                ObjectsToSave = new List<PersistentObject>
                {
                    paymentDocument
                }
            };
        }

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
                Item = package.RisContragent.OrgPpaGuid,
                ItemElementName = ItemChoiceType2.orgPPAGUID,
                IsOperatorSignature = package.IsDelegacy,
                IsOperatorSignatureSpecified = package.IsDelegacy
            };
        }
    }
}
