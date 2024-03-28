namespace Bars.GisIntegration.Base.Tasks.SendData.OrgRegistry
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Entities.OrgRegistry;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.OrgRegistryAsync;
    using Bars.GisIntegration.Base.Tasks.SendData;

    /// <summary>
    /// Задача обработки результатов экспорта обособленных подразделений
    /// </summary>
    public class SubsidiaryExportTask : BaseSendDataTask<importSubsidiaryRequest, getStateResult, RegOrgPortsTypeAsyncClient, RequestHeader>
    {
        /// <summary>
        /// Домен-сервис обособленных подразделенией (РИС)
        /// </summary>
        public IDomainService<RisSubsidiary> SubsidiaryDomain { get; set; }

        /// <summary>
        /// Выполнить запрос к асинхронному сервису ГИС
        /// </summary>
        /// <param name="request">Объект запроса</param>
        /// <param name="header">Заголовок запроса</param>
        /// <returns>Идентификатор сообщения для получения результата</returns>
        protected override string ExecuteRequest(RequestHeader header, importSubsidiaryRequest request)
        {
            var soapClient = this.ServiceProvider.GetSoapClient();

            if (soapClient == null)
            {
                throw new Exception("Не удалось получить SOAP клиент");
            }

            AckRequest result;

            soapClient.importSubsidiary(header, request, out result);

            if (result?.Ack == null)
            {
                throw new Exception("Пустой результат выполенния запроса");
            }

            return result.Ack.MessageGUID;
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
        protected override PackageProcessingResult ProcessResult(
            getStateResult response,
            Dictionary<Type, Dictionary<string, long>> transportGuidDictByType)
        {
            if (!transportGuidDictByType.ContainsKey(typeof(RisSubsidiary)))
            {
                throw new Exception("Не удалось обработать результат выполнения метода getState");
            }

            var subsidiariesByTransportGuid = transportGuidDictByType[typeof(RisSubsidiary)];

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

            var subsidiaryId = transportGuidDict[responseItem.TransportGUID];

            if (string.IsNullOrEmpty(responseItem.GUID))
            {
                var error = responseItem.Items.FirstOrDefault() as CommonResultTypeError;
                var errorNotation = error != null ? error.Description : "Вернулся пустой GUID";

                return new ObjectProcessingResult
                {
                    Description = "Сведения об обособленном подразделении",
                    RisId = subsidiaryId,
                    State = ObjectProcessingState.Error,
                    Message = errorNotation
                };
            }

            var subsidiary = this.SubsidiaryDomain.Get(subsidiaryId);

            subsidiary.Guid = responseItem.GUID;

            return new ObjectProcessingResult
            {
                Description = "Сведения об обособленном подразделении",
                RisId = subsidiaryId,
                State = ObjectProcessingState.Success,
                ObjectsToSave = new List<PersistentObject> { subsidiary }
            };
        }
    }
}
