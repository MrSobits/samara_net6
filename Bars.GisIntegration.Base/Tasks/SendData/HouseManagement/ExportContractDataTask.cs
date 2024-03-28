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
    /// Задача отправки данных договоров управления
    /// </summary>
    public class ExportContractDataTask : BaseSendDataTask<importContractRequest, getStateResult, HouseManagementPortsTypeAsyncClient, RequestHeader>
    {
        /// <summary>
        /// Выполнить запрос к асинхронному сервису ГИС
        /// </summary>
        /// <param name="header">Заголовок запроса</param>
        /// <param name="request">Объект запроса</param>
        /// <returns>Идентификатор сообщения для получения результата</returns>
        protected override string ExecuteRequest(RequestHeader header, importContractRequest request)
        {
            AckRequest result = null;
            var soapClient = this.ServiceProvider.GetSoapClient();

            if (soapClient != null)
            {
                soapClient.importContractData(header, request, out result);
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
            if (!transportGuidDictByType.ContainsKey(typeof(RisContract)) &&
                !transportGuidDictByType.ContainsKey(typeof(ContractObject)))
            {
                throw new Exception("Не удалось обработать результат выполнения метода getState");
            }

            var result = new PackageProcessingResult
            {
                State = PackageState.SuccessProcessed,
                Objects = new List<ObjectProcessingResult>()
            };

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
                            var processingResult = this.CheckResponseItem(importResultCommonResult, transportGuidDictByType);
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
        /// Проверить строку элемент response и добавить объекты ГИС на сохранение
        /// </summary>
        /// <param name="responseItem">Элемент response</param>
        /// <param name="transportGuidDict">Словарь transportGuid-ов</param>
        private ObjectProcessingResult CheckResponseItem(CommonResultType responseItem, Dictionary<Type, Dictionary<string, long>> transportGuidDict)
        {
            ObjectProcessingResult processingResult = null;
            var contractDomain = this.Container.ResolveDomain<RisContract>();
            var contractObjectDomain = this.Container.ResolveDomain<ContractObject>();
            var contractsByTransportGuid = transportGuidDict[typeof(RisContract)];
            var contractObjectsByTransportGuid = transportGuidDict[typeof(ContractObject)];

            try
            {
                if (!contractsByTransportGuid.ContainsKey(responseItem.TransportGUID) &&
                    !contractObjectsByTransportGuid.ContainsKey(responseItem.TransportGUID))
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
                    var contractId = contractsByTransportGuid.Get(responseItem.TransportGUID);
                    var contractObjectId = contractObjectsByTransportGuid?.Get(responseItem.TransportGUID);

                    if (responseItem.GUID.IsEmpty())
                    {
                        var error = responseItem.Items.FirstOrDefault() as CommonResultTypeError;
                        var errorNotation = error != null ? error.Description : "Вернулся пустой GUID";

                        processingResult = new ObjectProcessingResult
                        {
                            Description = contractId > 0 ? "Договор управления" : "Объект управления",
                            RisId = (contractId > 0 ? contractId : contractObjectId) ?? 0,
                            State = ObjectProcessingState.Error,
                            Message = errorNotation
                        };
                    }
                    else
                    {
                        if (contractId > 0)
                        {
                            var contract = contractDomain.Get(contractId);
                            contract.Guid = responseItem.GUID;

                            processingResult = new ObjectProcessingResult
                            {
                                Description = "Договор управления",
                                GisId = contract.Guid,

                                RisId = contractId,
                                State = ObjectProcessingState.Success,
                                ObjectsToSave = new List<PersistentObject> { contract }
                            };
                        }
                        else if (contractObjectId > 0)
                        {
                            var contractObject = contractObjectDomain.Get(contractObjectId);
                            contractObject.Guid = responseItem.GUID;

                            processingResult = new ObjectProcessingResult
                            {
                                Description = "Объект управления",
                                GisId = contractObject.Guid,
                                RisId = (long) contractObjectId,
                                State = ObjectProcessingState.Success,
                                ObjectsToSave = new List<PersistentObject> { contractObject }
                            };
                        }
                    }
                }
            }
            finally
            {
                this.Container.Release(contractDomain);
                this.Container.Release(contractObjectDomain);
            }

            return processingResult;
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
                ItemElementName = ItemChoiceType.orgPPAGUID,
                IsOperatorSignature = package.IsDelegacy,
                IsOperatorSignatureSpecified = package.IsDelegacy
            };
        }
    }
}