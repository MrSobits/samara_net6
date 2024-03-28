namespace Bars.GisIntegration.Base.Tasks.SendData.OrgRegistryCommon
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.OrgRegistryCommonAsync;

    /// <summary>
    /// Задача отправки данных о квитировании
    /// </summary>
    [Obsolete("Метод exportDataProvider упразднен")]
    public class ExportDataProviderTask : BaseSendDataTask<exportDataProviderRequest, getStateResult, RegOrgPortsTypeAsyncClient, HeaderType>
    {
        /// <summary>
        /// Выполнить запрос к асинхронному сервису ГИС
        /// </summary>
        /// <param name="header">Заголовок запроса</param>
        /// <param name="request">Объект запроса</param>
        /// <returns>Идентификатор сообщения для получения результата</returns>
        protected override string ExecuteRequest(HeaderType header, exportDataProviderRequest request)
        {
            AckRequest result = null;
            var soapClient = this.ServiceProvider.GetSoapClient();

            if (soapClient != null)
            {
                soapClient.exportDataProvider((ISRequestHeader)header, request, out result);
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
        protected override sbyte GetStateResult(HeaderType header, string ackMessageGuid, out getStateResult result)
        {
            var request = new getStateRequest
            {
                MessageGUID = ackMessageGuid
            };

            var soapClient = this.ServiceProvider.GetSoapClient();
            result = null;

            if (soapClient != null)
            {
                soapClient.getState((ISRequestHeader)header, request, out result);
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
            var risContragentDomain = this.Container.ResolveDomain<RisContragent>();
            IQueryable<RisContragent> risContragents;

            try
            {
                risContragents = risContragentDomain.GetAll();
            }
            finally
            {
                this.Container.Release(risContragentDomain);
            }

            var result = new PackageProcessingResult
            {
                State = PackageState.SuccessProcessed,
                Objects = new List<ObjectProcessingResult>()
            };

            foreach (var item in responce.Items)
            {
                var exportDataProviderResult = item as exportDataProviderResultType;

                if (exportDataProviderResult == null)
                {
                    var errorItem = item as ErrorMessageType;

                    if (errorItem != null)
                    {
                        result.Objects.Add(new ObjectProcessingResult
                        {
                            State = ObjectProcessingState.Error,
                            Message = errorItem.Description
                        });
                    }
                }
                else
                {
                    var regOrg = exportDataProviderResult.RegOrg;

                    if (regOrg != null)
                    {
                        var risContragent = risContragents.FirstOrDefault(x => x.OrgRootEntityGuid == regOrg.orgRootEntityGUID);
                        var processingResult = this.HandleContragent(exportDataProviderResult, ref risContragent);

                        result.Objects.Add(processingResult);
                    }
                    else
                    {
                        result.Objects.Add(new ObjectProcessingResult
                        {
                            Description = "DataProviderGUID: " + exportDataProviderResult.DataProviderGUID,
                            State = ObjectProcessingState.Error,
                            Message = "Для поставщика информации не указан orgRootEntityGUID"
                        });
                    }
                }
            }

            return result;
        }

        private ObjectProcessingResult HandleContragent(exportDataProviderResultType exportDataProviderResult, ref RisContragent risContragent)
        {
            var dataProviderGuid = exportDataProviderResult.DataProviderGUID;

            if (dataProviderGuid.IsEmpty())
            {
                return new ObjectProcessingResult
                {
                    Description = $"Наименование: {risContragent.FullName}; ОГРН: {risContragent.Ogrn}",
                    State = ObjectProcessingState.Error,
                    Message = "В ГИС ЖКХ не указан DataProviderGUID"
                };
            }

            if (risContragent == null)
            {
                return new ObjectProcessingResult
                {
                    Description = "DataProviderGUID: " + dataProviderGuid,
                    State = ObjectProcessingState.Error,
                    Message = "Контрагент не найден в РИС ЖКХ"
                };
            }

            //risContragent.SenderId = dataProviderGuid;

            return new ObjectProcessingResult
            {
                Description = $"Наименование: {risContragent.FullName}; ОГРН: {risContragent.Ogrn}",
                RisId = risContragent.Id,
                State = ObjectProcessingState.Success,
                ObjectsToSave = new List<PersistentObject> { risContragent },
                GisId = exportDataProviderResult.RegOrg.orgRootEntityGUID
            };
        }

        protected override HeaderType GetHeader(string messageGuid, RisPackage package)
        {
            return new HeaderType
            {
                Date = DateTime.Now,
                MessageGUID = messageGuid
            };
        }
    }
}
