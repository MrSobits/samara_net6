namespace Bars.GisIntegration.Base.Tasks.SendData.CapitalRepair
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.GisIntegration.Base.CapitalRepair;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Entities.CapitalRepair;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.Tasks.SendData;

    /// <summary>
    /// Задача получения результатов для экспортера договоров на выполнение работ (оказание услуг) по капитальному ремонту
    /// </summary>
    public class CapitalRepairContractsExportDataTask : BaseSendDataTask<importContractsRequest, getStateResult, CapitalRepairAsyncPortClient, RequestHeader>
    {
        /// <summary>
        /// Домен-сервис договоров на выполнение работ (оказание услуг) по капитальному ремонту  (РИС)
        /// </summary>
        public IDomainService<RisCrContract> CapitalRepairContractDomain { get; set; }

        /// <summary>
        /// Выполнить запрос к асинхронному сервису ГИС
        /// </summary>
        /// <param name="header">Заголовок запроса</param>
        /// <param name="request">Объект запроса</param>
        /// <returns>Идентификатор сообщения для получения результата</returns>
        protected override string ExecuteRequest(RequestHeader header, importContractsRequest request)
        {
            var soapClient = this.ServiceProvider.GetSoapClient();

            if (soapClient == null)
            {
                throw new Exception("Не удалось получить SOAP клиент");
            }

            AckRequest result;

            soapClient.importContracts(header, request, out result);

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
        /// <param name="response">Ответ от сервиса</param>
        /// <param name="transportGuidDictByType">Словарь transportGuid-ов для типа</param>
        /// <returns>Результат обработки пакета</returns>
        protected override PackageProcessingResult ProcessResult(getStateResult response, Dictionary<Type, Dictionary<string, long>> transportGuidDictByType)
        {
            if (!transportGuidDictByType.ContainsKey(typeof(RisCrContract)))
            {
                throw new Exception("Не удалось обработать результат выполнения метода getState");
            }

            var subsidiariesByTransportGuid = transportGuidDictByType[typeof(RisCrContract)];

            var result = new PackageProcessingResult
            {
                State = PackageState.SuccessProcessed,
                Objects = new List<ObjectProcessingResult>()
            };

            foreach (var item in response.Items)
            {
                var responseItem = item as CommonResultType;

                if (responseItem != null)
                {
                    var processingResult = this.CheckResponseItem(responseItem, subsidiariesByTransportGuid);

                    result.Objects.Add(processingResult);

                    continue;
                }

                var errorMessageItem = item as ErrorMessageType;

                if (errorMessageItem != null)
                {
                    result.Objects.Add(new ObjectProcessingResult
                    {
                        State = ObjectProcessingState.Error,
                        Message = errorMessageItem.Description
                    });

                    continue;
                }

                result.Objects.Add(new ObjectProcessingResult
                {
                    State = ObjectProcessingState.Error,
                    Message = "Неизвестный тип результата запроса"
                });
            }

            return result;
        }

        /// <summary>
        /// Получить заголовок запроса
        /// </summary>
        /// <param name="messageGuid">Идентификатор запроса</param>
        /// <param name="package"></param>
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

        private ObjectProcessingResult CheckResponseItem(CommonResultType responseItem, Dictionary<string, long> transportGuidDict)
        {
            if (!transportGuidDict.ContainsKey(responseItem.TransportGUID))
            {
                if (string.IsNullOrEmpty(responseItem.GUID))
                {
                    var error = responseItem.Items.FirstOrDefault() as CommonResultTypeError;
                    var errorNotation = error != null ? error.Description : "Вернулся пустой GUID";

                    return new ObjectProcessingResult
                    {
                        Description = "Не найден объект с TransportGuid = " + responseItem.TransportGUID,
                        State = ObjectProcessingState.Error,
                        Message = errorNotation
                    };
                }

                return new ObjectProcessingResult
                {
                    GisId = responseItem.GUID,
                    Description = "Не найден объект с TransportGuid = " + responseItem.TransportGUID,
                    State = ObjectProcessingState.Success
                };
            }

            var crContractId = transportGuidDict[responseItem.TransportGUID];

            if (string.IsNullOrEmpty(responseItem.GUID))
            {
                var error = responseItem.Items.FirstOrDefault() as CommonResultTypeError;
                var errorNotation = error != null ? error.Description : "Вернулся пустой GUID";

                return new ObjectProcessingResult
                {
                    Description = "Сведения о договорах на выполнение работ (оказание услуг) по капитальному ремонту",
                    RisId = crContractId,
                    State = ObjectProcessingState.Error,
                    Message = errorNotation
                };
            }

            var crContract = this.CapitalRepairContractDomain.Get(crContractId);

            crContract.Guid = responseItem.GUID;

            return new ObjectProcessingResult
            {
                Description = "Сведения о договорах на выполнение работ (оказание услуг) по капитальному ремонту",
                RisId = crContractId,
                State = ObjectProcessingState.Success,
                ObjectsToSave = new List<PersistentObject> { crContract }
            };
        }
    }
}