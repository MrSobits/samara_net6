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
    /// Задача отправки перечней работ и услуг и получения результатов экспорта пакетов данных
    /// </summary>
    public class WorkingListExportTask : BaseSendDataTask<importWorkingListRequest, getStateResult, ServicesPortsTypeAsyncClient, RequestHeader>
    {
        private Dictionary<Type, IDomainService> domainServiceByType;

        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="workListDomainService">Домен сервис перечней работ и услуг</param>
        /// <param name="workListItemDomainService">Домен сервис работ и услуг перечня</param>
        public WorkingListExportTask(
            IDomainService<WorkList> workListDomainService,
            IDomainService<WorkListItem> workListItemDomainService)
        {
            this.domainServiceByType = new Dictionary<Type, IDomainService>
            {
                { typeof(WorkList), workListDomainService },
                { typeof(WorkListItem), workListItemDomainService }
            };
        }

        /// <summary>
        /// Выполнить запрос к асинхронному сервису ГИС
        /// </summary>
        /// <param name="header">Заголовок запроса</param>
        /// <param name="request">Объект запроса</param>
        /// <returns>Идентификатор сообщения для получения результата</returns>
        protected override string ExecuteRequest(RequestHeader header, importWorkingListRequest request)
        {
            var soapClient = this.ServiceProvider.GetSoapClient();

            if (soapClient == null)
            {
                throw new Exception("Не удалось получить SOAP клиент");
            }

            AckRequest result;

            soapClient.importWorkingList(header, request, out result);

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
        /// <returns></returns>
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
            var privateTransportGuidByType = transportGuidDictByType
                .Where(x => x.Key == typeof(WorkList) || x.Key == typeof(WorkListItem))
                .Where(x => x.Value != null)
                .ToDictionary(x => x.Key, y => y.Value);

            if (privateTransportGuidByType.Count != 2)
            {
                throw new Exception("Не удалось обработать результат выполнения метода getState");
            }

            var packageResult = new PackageProcessingResult
            {
                State = PackageState.SuccessProcessed,
                Objects = new List<ObjectProcessingResult>()
            };

            foreach (var responseItem in response.Items)
            {
                var objectResult = new ObjectProcessingResult
                {
                    State = ObjectProcessingState.Success
                };

                packageResult.Objects.Add(objectResult);

                var commonResult = responseItem as CommonResultType;
                if (commonResult != null)
                {
                    this.ProcessCommonResult(commonResult, privateTransportGuidByType, objectResult);

                    continue;
                }

                var errorMessage = responseItem as ErrorMessageType;
                if (errorMessage != null)
                {
                    objectResult.State = ObjectProcessingState.Error;
                    objectResult.Message = $"Код ошибки: {errorMessage.ErrorCode}. Описание: {errorMessage.Description}";

                    continue;
                }

                objectResult.State = ObjectProcessingState.Error;
                objectResult.Message = "Неизвестный тип результата запроса";
            }

            return packageResult;
        }

        private void ProcessCommonResult(
            CommonResultType commonResult,
            Dictionary<Type, Dictionary<string, long>> transportGuidByType,
            ObjectProcessingResult objectResult)
        {
            objectResult.GisId = commonResult.GUID;
            objectResult.Description = "Транспортный GUID: " + commonResult.TransportGUID;

            var errors = commonResult.Items
                .Select(x => x as CommonResultTypeError)
                .Where(x => x != null)
                .ToArray();

            if (errors.Any())
            {
                var errorMessages = errors.Select(x => $"Код ошибки: {x.ErrorCode}. Описание: {x.Description}");
                
                objectResult.State = ObjectProcessingState.Error;
                objectResult.Message = string.Join(";", errorMessages);

                return;
            }

            var risEntityInfo = transportGuidByType
                .Where(x => x.Value.ContainsKey(commonResult.TransportGUID))
                .Select(x => new
                {
                    Type = x.Key,
                    Id = x.Value.Get(commonResult.TransportGUID)
                })
                .FirstOrDefault();

            if (risEntityInfo == null)
            {
                objectResult.State = ObjectProcessingState.Error;
                objectResult.Message = "Не удалось найти RIS-объект по транспортному GUID";

                return;
            }
            
            objectResult.RisId = risEntityInfo.Id;
            objectResult.Description += "; Тип RIS-объекта: " + risEntityInfo.Type.Name;

            var risEntityDomainService = this.domainServiceByType[risEntityInfo.Type];

            var risEntity = risEntityDomainService.Get(risEntityInfo.Id) as BaseRisEntity;

            if (risEntity == null)
            {
                objectResult.State = ObjectProcessingState.Error;
                objectResult.Message = "Не удалось получить RIS-объект";

                return;
            }

            if (string.IsNullOrEmpty(commonResult.GUID))
            {
                objectResult.State = ObjectProcessingState.Error;
                objectResult.Message = "От GIS получен пустой GUID объекта";

                return;
            }

            risEntity.Guid = commonResult.GUID;

            objectResult.Message = "OK";
            objectResult.ObjectsToSave = new List<PersistentObject> { risEntity };
        }
    }
}
