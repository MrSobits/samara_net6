namespace Bars.GisIntegration.Base.Tasks.SendData.HouseManagement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.Tasks.SendData;
    using Entities.HouseManagement;
    using HouseManagementAsync;

    /// <summary>
    /// Задача отправки данных Списков ДОИ
    /// </summary>
    public class ExportPublicPropertyContractDataTask : BaseSendDataTask<importPublicPropertyContractRequest, getStateResult, HouseManagementPortsTypeAsyncClient, RequestHeader>
    {
        /// <summary>
        /// Выполнить запрос к асинхронному сервису ГИС
        /// </summary>
        /// <param name="header">Заголовок запроса</param>
        /// <param name="request">Объект запроса</param>
        /// <returns>Идентификатор сообщения для получения результата</returns>
        protected override string ExecuteRequest(RequestHeader header, importPublicPropertyContractRequest request)
        {
            AckRequest result = null;
            var soapClient = this.ServiceProvider.GetSoapClient();

            if (soapClient != null)
            {
                soapClient.importPublicPropertyContract(header, request, out result);
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
            if (!transportGuidDictByType.ContainsKey(typeof(RisPublicPropertyContract)))
            {
                throw new Exception("Не удалось обработать результат выполнения метода getState");
            }
            var publicPropertyContractByTransportGuid = transportGuidDictByType[typeof(RisPublicPropertyContract)];

            var result = new PackageProcessingResult { State = PackageState.SuccessProcessed, Objects = new List<ObjectProcessingResult>() };

            foreach (var item in responce.Items)
            {
                var errorMessageType = item as ErrorMessageType;
                var importResult = item as ImportResult;

                if (errorMessageType != null)
                {
                    result.Objects.Add(new ObjectProcessingResult
                    {
                        State = ObjectProcessingState.Error,
                        Message = errorMessageType.Description
                    });
                }
                else if (importResult != null)
                {
                    foreach (var importResultItem in importResult.Items)
                    {
                        var errorMessageTypeInner = importResultItem as ErrorMessageType;
                        var importResultCommonResult = importResultItem as ImportResultCommonResult;

                        if (errorMessageTypeInner != null)
                        {
                            result.Objects.Add(new ObjectProcessingResult
                            {
                                State = ObjectProcessingState.Error,
                                Message = errorMessageTypeInner.Description
                            });
                        }
                        else if (importResultCommonResult != null)
                        {
                            var processingResult = this.CheckResponseItem(importResultCommonResult, publicPropertyContractByTransportGuid);
                            result.Objects.Add(processingResult);
                        }
                        else
                        {
                            result.Objects.Add(new ObjectProcessingResult
                            {
                                State = ObjectProcessingState.Error,
                                Message = "Не удалось разобрать getStateResult"
                            });
                        }
                    }
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
                ItemElementName = ItemChoiceType.orgPPAGUID,
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
            var domain = this.Container.ResolveDomain<Charter>();

            try
            {
                long publicPropertyContractId = 0;

                if (responseItem.GUID.IsEmpty())
                {
                    var error = responseItem.Items.FirstOrDefault() as CommonResultTypeError;
                    var errorNotation = error != null ? error.Description : "Вернулся пустой GUID";

                    return new ObjectProcessingResult
                    {
                        Description = "Список ДОИ",
                        RisId = publicPropertyContractId,
                        State = ObjectProcessingState.Error,
                        Message = errorNotation
                    };
                }

                if (!transportGuidDict.TryGetValue(responseItem.TransportGUID, out publicPropertyContractId))
                {
                    return new ObjectProcessingResult
                    {
                        Description = "Список ДОИ",
                        State = ObjectProcessingState.Success,
                        GisId = responseItem.GUID,
                        Message = "В списках транспортных идентификаторов пакета не найден вернувшийся идентификатор"
                    };
                }

                var publicPropertyContract = domain.Get(publicPropertyContractId);
                publicPropertyContract.Guid = responseItem.GUID;

                return new ObjectProcessingResult
                {
                    Description = "Список ДОИ",
                    RisId = publicPropertyContractId,
                    State = ObjectProcessingState.Success,
                    ObjectsToSave = new List<PersistentObject> { publicPropertyContract }
                };
            }
            finally
            {
                this.Container.Release(domain);
            }
        }
    }
}
