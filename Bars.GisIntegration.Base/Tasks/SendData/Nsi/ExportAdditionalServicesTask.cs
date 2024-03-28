namespace Bars.GisIntegration.Base.Tasks.SendData.Nsi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Entities.Nsi;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.NsiAsync;
    using Bars.GisIntegration.Base.Tasks.SendData;

    /// <summary>
    /// Задача отправки данных договоров управления
    /// </summary>
    public class ExportAdditionalServicesTask : BaseSendDataTask<importAdditionalServicesRequest, getStateResult, NsiPortsTypeAsyncClient, RequestHeader>
    {
        /// <summary>
        /// Выполнить запрос к асинхронному сервису ГИС
        /// </summary>
        /// <param name="header">Заголовок запроса</param>
        /// <param name="request">Объект запроса</param>
        /// <returns>Идентификатор сообщения для получения результата</returns>
        protected override string ExecuteRequest(RequestHeader header, importAdditionalServicesRequest request)
        {
            AckRequest result = null;
            var soapClient = this.ServiceProvider.GetSoapClient();

            if (soapClient != null)
            {
                soapClient.importAdditionalServices(header, request, out result);
            }

            return result?.Ack.MessageGUID;
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
            if (!transportGuidDictByType.ContainsKey(typeof(RisAdditionalService)))
            {
                throw new Exception("Не удалось обработать результат выполнения метода getState");
            }
            var contractsByTransportGuid = transportGuidDictByType[typeof(RisAdditionalService)];

            var result = new PackageProcessingResult { State = PackageState.SuccessProcessed, Objects = new List<ObjectProcessingResult>() };

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
                    var processingResult = this.CheckResponseItem(responseItem, contractsByTransportGuid);
                    result.Objects.Add(processingResult);
                }
            }

            return result;
        }

        /// <summary>
        /// Проверить строку элемент response и добавить объекты ГИС на сохранение.
        /// </summary>
        /// <param name="responseItem">Элемент response</param>
        /// <param name="transportGuidDict">Словарь transportGuid-ов</param>
        private ObjectProcessingResult CheckResponseItem(CommonResultType responseItem, Dictionary<string, long> transportGuidDict)
        {
            var domain = this.Container.ResolveDomain<RisAdditionalService>();

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

                var additionalServiceId = transportGuidDict[responseItem.TransportGUID];

                if (responseItem.GUID.IsEmpty())
                {
                    var error = responseItem.Items.FirstOrDefault() as CommonResultTypeError;
                    var errorNotation = error != null ? error.Description : "Вернулся пустой GUID";

                    return new ObjectProcessingResult
                    {
                        Description = "Дополнительная услуга",
                        RisId = additionalServiceId,
                        State = ObjectProcessingState.Error,
                        Message = errorNotation
                    };
                }

                var contract = domain.Get(additionalServiceId);
                contract.Guid = responseItem.GUID;

                return new ObjectProcessingResult
                {
                    Description = "Дополнительная услуга",
                    RisId = additionalServiceId,
                    State = ObjectProcessingState.Success,
                    ObjectsToSave = new List<PersistentObject> {contract}
                };
            }
            finally
            {
                this.Container.Release(domain);
            }
        }

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
                Item = package.RisContragent.OrgPpaGuid,
                ItemElementName = ItemChoiceType1.orgPPAGUID,
                IsOperatorSignature = package.IsDelegacy,
                IsOperatorSignatureSpecified = package.IsDelegacy
            };
        }
    }
}