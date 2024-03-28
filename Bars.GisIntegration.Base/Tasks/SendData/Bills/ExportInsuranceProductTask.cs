namespace Bars.GisIntegration.Base.Tasks.SendData.Bills
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.BillsAsync;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Entities.Bills;
    using Bars.GisIntegration.Base.Enums;

    /// <summary>
    /// Задача отправки данных и получения результатов для 
    /// </summary>
    public class ExportInsuranceProductTask : BaseSendDataTask<importInsuranceProductRequest, getStateResult, BillsPortsTypeAsyncClient, RequestHeader>
    {
        /// <summary>
        /// Выполнить запрос к асинхронному сервису ГИС
        /// </summary>
        /// <param name="header">Заголовок запроса</param>
        /// <param name="request">Объект запроса</param>
        /// <returns>Идентификатор сообщения для получения результата</returns>
        protected override string ExecuteRequest(RequestHeader header, importInsuranceProductRequest request)
        {
            AckRequest result = null;
            var soapClient = this.ServiceProvider.GetSoapClient();

            if (soapClient != null)
            {
                soapClient.importInsuranceProduct(header, request, out result);
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
            var request = new getStateRequest {MessageGUID = ackMessageGuid};

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
        /// <param name="response">Ответ от сервиса</param>
        /// <param name="transportGuidDictByType">Словарь transportGuid-ов для типа</param>
        /// <returns>Результат обработки пакета</returns>
        protected override PackageProcessingResult ProcessResult(
            getStateResult response,
            Dictionary<Type, Dictionary<string, long>> transportGuidDictByType)
        {
            if (!transportGuidDictByType.ContainsKey(typeof(InsuranceProduct)))
            {
                throw new Exception("Не удалось обработать результат выполнения метода getState");
            }
            var accountsByTransportGuid = transportGuidDictByType[typeof(InsuranceProduct)];

            var result = new PackageProcessingResult {State = PackageState.SuccessProcessed, Objects = new List<ObjectProcessingResult>()};

            foreach (var item in response.Items)
            {
                var errorItem = item as CommonResultTypeError;
                var errorMessageTypeItem = item as ErrorMessageType;
                var responseItem = item as CommonResultType;

                if (errorItem != null)
                {
                    result.Objects.Add(
                        new ObjectProcessingResult
                        {
                            State = ObjectProcessingState.Error,
                            Message = errorItem.Description
                        });
                }
                else if (errorMessageTypeItem != null)
                {
                    result.Objects.Add(
                        new ObjectProcessingResult
                        {
                            State = ObjectProcessingState.Error,
                            Message = errorMessageTypeItem.Description
                        });
                }
                else if (responseItem != null)
                {
                    var processingResult = this.CheckResponseItem(responseItem, accountsByTransportGuid);
                    result.Objects.Add(processingResult);
                }
            }

            return result;
        }

        /// <summary>
        /// Получить заголовок запроса
        /// </summary>
        /// <param name="messageGuid">Идентификатор запроса</param>
        /// <param name="package"></param>
        /// <returns>Заголовок запроса</returns>>
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

        /// <summary>
        /// Проверить строку элемент response и добавить объекты ГИС на сохранение.
        /// </summary>
        /// <param name="responseItem">Элемент response</param>
        /// <param name="transportGuidDict">Словарь transportGuid-ов</param>
        private ObjectProcessingResult CheckResponseItem(CommonResultType responseItem, Dictionary<string, long> transportGuidDict)
        {
            var domain = this.Container.ResolveDomain<InsuranceProduct>();

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
                            Description = string.Format("Не найден объект с TransportGuid = {0}", responseItem.TransportGUID),
                            State = ObjectProcessingState.Error,
                            Message = errorNotation
                        };
                    }

                    return new ObjectProcessingResult
                    {
                        GisId = responseItem.GUID,
                        Description = string.Format("Не найден объект с TransportGuid = {0}", responseItem.TransportGUID),
                        State = ObjectProcessingState.Success
                    };
                }

                var insuranceProductId = transportGuidDict[responseItem.TransportGUID];

                if (responseItem.GUID.IsEmpty())
                {
                    var error = responseItem.Items.FirstOrDefault() as CommonResultTypeError;
                    var errorNotation = error != null ? error.Description : "Вернулся пустой GUID";

                    return new ObjectProcessingResult
                    {
                        Description = "Страховой объект",
                        RisId = insuranceProductId,
                        State = ObjectProcessingState.Error,
                        Message = errorNotation
                    };
                }

                var insuranceProduct = domain.Get(insuranceProductId);
                insuranceProduct.Guid = responseItem.GUID;

                return new ObjectProcessingResult
                {
                    Description = "Страховой объект",
                    RisId = insuranceProductId,
                    State = ObjectProcessingState.Success,
                    ObjectsToSave = new List<PersistentObject> { insuranceProduct }
                };
            }
            finally
            {
                this.Container.Release(domain);
            }
        }
    }
}