namespace Bars.GisIntegration.Base.Tasks.SendData.OrgRegistryCommon
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.OrgRegistryCommonAsync;

    /// <summary>
    /// Задача отправки данных и получения результатов для ExportOrgRegistry
    /// </summary>
    public class ExportOrgRegistryTask : BaseSendDataTask<exportOrgRegistryRequest, getStateResult, RegOrgPortsTypeAsyncClient, HeaderType>
    {
        /// <summary>
        /// Выполнить запрос к асинхронному сервису ГИС
        /// </summary>
        /// <param name="header">Заголовок запроса</param>
        /// <param name="request">Объект запроса</param>
        /// <returns>Идентификатор сообщения для получения результата</returns>
        protected override string ExecuteRequest(HeaderType header, exportOrgRegistryRequest request)
        {
            AckRequest result = null;
            var soapClient = this.ServiceProvider.GetSoapClient();

            if (soapClient != null)
            {
                soapClient.exportOrgRegistry((ISRequestHeader)header, request, out result);
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
            var result = new PackageProcessingResult
            {
                State = PackageState.SuccessProcessed,
                Objects = new List<ObjectProcessingResult>()
            };

            var selector = this.Container.Resolve<IDataSelector<ContragentProxy>>("ContragentSelector");
            var risContragentDomain = this.Container.ResolveDomain<RisContragent>();

            try
            {
                var gkhContragents = selector
                    .GetExternalEntities(new DynamicDictionary {{"selectedList", "ALL"}})
                    .Where(x => x.Ogrn != null && (x.Ogrn.Length == 13 || x.Ogrn.Length == 15))
                    .ToList();

                foreach (var item in responce.Items)
                {
                    var exportOrgRegistryResult = item as exportOrgRegistryResultType;

                    if (exportOrgRegistryResult == null)
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
                        else
                        {
                            result.Objects.Add(new ObjectProcessingResult
                            {
                                State = ObjectProcessingState.Error,
                                Message = string.Format("Не обрабатываемый тип результата {0}", item?.GetType().Name)
                            });
                        }
                    }
                    else
                    {
                        result.Objects.Add(this.ProcessExportOrgRegistryResult(exportOrgRegistryResult, gkhContragents, risContragentDomain));
                    }
                }
            }
            finally
            {
                this.Container.Release(selector);
                this.Container.Release(risContragentDomain);
            }

            return result;
        }

        /// <summary>
        /// Получить заголовок запроса
        /// </summary>
        /// <param name="messageGuid">Идентификатор запроса</param>
        /// <param name="package"></param>
        protected override HeaderType GetHeader(string messageGuid, RisPackage package)
        {
            return new HeaderType
            {
                Date = DateTime.Now,
                MessageGUID = messageGuid
            };
        }

        private ObjectProcessingResult ProcessExportOrgRegistryResult(exportOrgRegistryResultType exportOrgRegistryResult, List<ContragentProxy> gkhContragents, IDomainService<RisContragent> risContragentDomainService)
        {
            string ogrn;
            string fullName;
            GisOrganizationType orgType;

            var legalTypeItem = exportOrgRegistryResult.OrgVersion?.Item as LegalType;

            if (legalTypeItem != null)
            {
                ogrn = legalTypeItem.OGRN;
                fullName = legalTypeItem.FullName;
                orgType = GisOrganizationType.Legal;
            }
            else
            {
                var entpsTypeItem = exportOrgRegistryResult.OrgVersion?.Item as EntpsType;

                if (entpsTypeItem != null)
                {
                    ogrn = entpsTypeItem.OGRNIP;
                    fullName = $"{entpsTypeItem.Surname} {entpsTypeItem.FirstName} {entpsTypeItem.Patronymic}";
                    orgType = GisOrganizationType.Entps;
                }
                else
                {
                    var subsidiaryTypeItem = exportOrgRegistryResult.OrgVersion?.Item as exportOrgRegistryResultTypeOrgVersionSubsidiary;

                    if (subsidiaryTypeItem != null)
                    {
                        ogrn = subsidiaryTypeItem.OGRN;
                        fullName = subsidiaryTypeItem.FullName;
                        orgType = GisOrganizationType.Subsidiary;
                    }
                    else
                    {
                        return new ObjectProcessingResult
                        {
                            Description = $"Идентификатор зарегистрированной в ГИС ЖКХ организации: {exportOrgRegistryResult.orgPPAGUID}",
                            State = ObjectProcessingState.Error,
                            Message = $"Не обрабатываемый тип организации {exportOrgRegistryResult.OrgVersion?.Item?.GetType().Name}"
                        };
                    }
                }
            }

            var gkhContragent = gkhContragents.FirstOrDefault(x => x.Ogrn == ogrn);

            if (gkhContragent == null)
            {
                return new ObjectProcessingResult
                {
                    Description = $"{orgType.GetDisplayName()}. ОГРН: {ogrn}",
                    State = ObjectProcessingState.Error,
                    Message = $"В системе не найден контрагент с ОГРН: {ogrn}, соответствующим полученному из ГИС ЖКХ"
                };
            }

            if (exportOrgRegistryResult.orgRootEntityGUID.IsEmpty())
            {
                return new ObjectProcessingResult
                {
                    ExternalId = gkhContragent.Id,
                    Description = $"{orgType.GetDisplayName()}. Наименование: {fullName}; ОГРН: {ogrn}",
                    State = ObjectProcessingState.Error,
                    Message = "Из ГИС ЖКХ не получен идентификатор данной ограницазии orgRootEntityGUID"
                };
            }
           
            var risContragent = this.GetRisContragent(gkhContragent, risContragentDomainService);

            risContragent.OrgPpaGuid = exportOrgRegistryResult.orgPPAGUID;
            risContragent.OrganizationType = orgType;
            risContragent.FactAddress = gkhContragent.FactAddress;
            risContragent.JuridicalAddress = gkhContragent.JuridicalAddress;
            risContragent.FullName = fullName;
            risContragent.Ogrn = ogrn;
            risContragent.OrgRootEntityGuid = exportOrgRegistryResult.orgRootEntityGUID;
            risContragent.OrgVersionGuid = exportOrgRegistryResult.OrgVersion.orgVersionGUID;

            if (exportOrgRegistryResult.orgPPAGUID.IsEmpty())
            {
                return new ObjectProcessingResult
                {
                    Description = $"{orgType.GetDisplayName()}. Наименование: {risContragent.FullName}; ОГРН: {risContragent.Ogrn}",
                    RisId = risContragent.Id,
                    ExternalId = risContragent.GkhId,
                    ObjectsToSave = new List<PersistentObject> { risContragent },
                    GisId = exportOrgRegistryResult.orgRootEntityGUID,
                    State = ObjectProcessingState.Warning,
                    Message = "Из ГИС ЖКХ не получен идентификатор зарегистрированной ограницазии orgPPAGUID"
                };
            }

            return new ObjectProcessingResult
            {
                Description = $"{orgType.GetDisplayName()}. Наименование: {risContragent.FullName}; ОГРН: {risContragent.Ogrn}",
                RisId = risContragent.Id,
                ExternalId = risContragent.GkhId,
                State = ObjectProcessingState.Success,
                ObjectsToSave = new List<PersistentObject> { risContragent },
                GisId = exportOrgRegistryResult.orgRootEntityGUID
            };
        }

        private RisContragent GetRisContragent(ContragentProxy gkhContragent, IDomainService<RisContragent> risContragentDomainService)
        {
            var result = risContragentDomainService.GetAll().FirstOrDefault(x => x.GkhId == gkhContragent.Id);

            if (result == null)
            {
                result = new RisContragent
                {
                    GkhId = gkhContragent.Id
                };

                risContragentDomainService.Save(result);
            }

            return result;
        }
    }
}
