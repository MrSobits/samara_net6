namespace Bars.GisIntegration.Base.Tasks.SendData.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Entities.Services;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.ServicesAsync;
    using Bars.GisIntegration.Base.Tasks.SendData;

    /// <summary>
    /// Задача экспорта актов выполненных работ
    /// </summary>
    public class CompletedWorkExportTask : BaseSendDataTask<importCompletedWorksRequest, getStateResult, ServicesPortsTypeAsyncClient, RequestHeader>
    {
        /// <summary>
        /// Домен сервис
        /// </summary>
        public IDomainService<RisCompletedWork> RisCompletedWorkDomainService { get; set; }

        /// <summary>
        /// Выполнить запрос к асинхронному сервису ГИС
        /// </summary>
        /// <param name="header">Заголовок запроса</param>
        /// <param name="request">Объект запроса</param>
        /// <returns>Идентификатор сообщения для получения результата</returns>
        protected override string ExecuteRequest(RequestHeader header, importCompletedWorksRequest request)
        {
            var soapClient = this.ServiceProvider.GetSoapClient();

            if (soapClient == null)
            {
                throw new Exception("Не удалось получить SOAP клиент");
            }

            AckRequest result;

            soapClient.importCompletedWorks(header, request, out result);

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
                ItemElementName = ItemChoiceType1.orgPPAGUID,
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
            var request = new getStateRequest {MessageGUID = ackMessageGuid };
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
            var privateTransportGuidByType = transportGuidDictByType
                .Where(x => x.Key == typeof(RisCompletedWork))
                .Where(x => x.Value != null)
                .ToDictionary(x => x.Key, y => y.Value);

            if (privateTransportGuidByType.Count != 1)
            {
                throw new Exception("Не удалось обработать результат выполнения метода getState");
            }

            var result = new PackageProcessingResult
            {
                State = PackageState.SuccessProcessed,
                Objects = new List<ObjectProcessingResult>()
            };

            foreach (var responseItem in responce.Items)
            {
                var commonResult = responseItem as CommonResultType;

                if (commonResult != null)
                {
                    var errors = commonResult.Items
                        .Select(x => x as CommonResultTypeError)
                        .Where(x => x != null)
                        .ToArray();

                    if (errors.Any())
                    {
                        var descriptions = errors.Select(x => x.Description);
                        var errorCodes = errors.Select(x => x.ErrorCode);

                        result.Objects.Add(
                            new ObjectProcessingResult
                            {
                                GisId = commonResult.GUID,
                                State = ObjectProcessingState.Error,
                                Message = string.Join(";", errorCodes),
                                Description = string.Join(";", descriptions)
                            });
                    }

                    var processingResult = this.ProcessCommonResult(
                        commonResult,
                        privateTransportGuidByType);

                    result.Objects.Add(processingResult);

                    continue;
                }

                var errorMessage = responseItem as ErrorMessageType;

                if (errorMessage != null)
                {
                    result.Objects.Add(
                        new ObjectProcessingResult
                        {
                            State = ObjectProcessingState.Error,
                            Message = errorMessage.ErrorCode,
                            Description = errorMessage.Description
                        });

                    continue;
                }

                result.Objects.Add(
                    new ObjectProcessingResult
                    {
                        State = ObjectProcessingState.Error,
                        Message = "Неизвестный тип результата запроса"
                    });
            }

            return result;
        }

        private ObjectProcessingResult ProcessCommonResult(
            CommonResultType commonResult,
            Dictionary<Type, Dictionary<string, long>> transportGuidByType)
        {
            var result = new ObjectProcessingResult
            {
                GisId = commonResult.GUID,
                State = ObjectProcessingState.Success
            };

            var transportGuid = commonResult.TransportGUID;

            var risEntityInfo = transportGuidByType
                .Where(x => x.Value.ContainsKey(transportGuid))
                .Select(
                    x => new
                    {
                        Type = x.Key,
                        Id = x.Value.Get(transportGuid)
                    })
                .FirstOrDefault();

            if (risEntityInfo == null)
            {
                // Не удалось найти данные РИС-объекта по трансфер гуиду 
                result.State = ObjectProcessingState.Error;
                return result;
            }

            result.Description = risEntityInfo.Type.Name;
            result.RisId = risEntityInfo.Id;

            var risEntity = this.RisCompletedWorkDomainService.Get(risEntityInfo.Id) as BaseRisEntity;

            if (risEntity == null)
            {
                // Не удалось найти РИС-объект
                result.State = ObjectProcessingState.Error;
                return result;
            }

            risEntity.Guid = commonResult.GUID;

            result.ObjectsToSave = new List<PersistentObject> {risEntity};

            return result;
        }
    }
}