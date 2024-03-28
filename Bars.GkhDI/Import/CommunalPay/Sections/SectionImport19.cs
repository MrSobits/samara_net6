namespace Bars.GkhDi.Import.Sections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Utils;
    using Bars.GkhDi.Entities;
    using Bars.GkhDi.Enums;

    using Castle.Windsor;

    public class SectionImport19 : ISectionImport
    {
        public string Name => "Импорт из комплат секция #19";

        public IWindsorContainer Container { get; set; }

        public void ImportSection(ImportParams importParams)
        {
            var baseServiceDomain = this.Container.Resolve<IDomainService<BaseService>>();
            var disinfoService = this.Container.Resolve<IDomainService<DisclosureInfo>>();
            var disclosureInfoRelationService = this.Container.Resolve<IDomainService<DisclosureInfoRelation>>();
            var periodDiService = this.Container.Resolve<IDomainService<PeriodDi>>();
            var infoAboutPaymentCommunalService = this.Container.Resolve<IDomainService<InfoAboutPaymentCommunal>>();

            using (this.Container.Using(baseServiceDomain,
                disinfoService,
                disclosureInfoRelationService,
                periodDiService,
                infoAboutPaymentCommunalService))
            {
                var infoPaymentComm = new List<InfoAboutPaymentCommunal>();

                var sectionsData = importParams.SectionData;

                if (sectionsData.Section19.Count == 0)
                {
                    return;
                }

                var inn = importParams.Inn;
                var logImport = importParams.LogImport;
                var realityObjects = importParams.RealityObjectIds;
                var realityObjectDict = importParams.RealObjsImportInfo;

                if (realityObjects.Count == 0)
                {
                    logImport.Warn(this.Name, string.Format("Нет домов под управлением УК с ИНН {0}", inn));
                    return;
                }

                var disclosureInfo = disinfoService
                    .GetAll()
                    .FirstOrDefault(
                        x => x.ManagingOrganization.Contragent.Inn == inn
                            && x.PeriodDi.Id == importParams.PeriodDiId);

                if (disclosureInfo == null)
                {
                    logImport.Warn(this.Name,
                        string.Format("Для УК с ИНН {0} не начато раскрытие делятельности в периоде {1}",
                            inn,
                            periodDiService.GetAll().Where(x => x.Id == importParams.PeriodDiId).Select(x => x.Name)));
                    return;
                }

                var disclosureInfoRelationsDict = disclosureInfoRelationService.GetAll()
                    .WhereContainsBulked(x => x.DisclosureInfoRealityObj.RealityObject.Id, realityObjects)
                    .Where(x => x.DisclosureInfo.Id == disclosureInfo.Id)
                    .Select(x => new
                    {
                        DiscoId = x.DisclosureInfoRealityObj.Id,
                        DiscoRoId = x.DisclosureInfoRealityObj.RealityObject.Id,
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.DiscoRoId)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.DiscoId).First());

                var baseServiceDict = baseServiceDomain.GetAll()
                    .WhereContainsBulked(x => x.DisclosureInfoRealityObj.RealityObject.Id, realityObjects)
                    .Where(x => x.TemplateService.TypeGroupServiceDi == TypeGroupServiceDi.Communal)
                    .Select(x => new
                    {
                        x.Id,
                        TemplateCode = x.TemplateService.Code
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.TemplateCode)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.Id).First());

                var infoAboutPaymentCommunalDict = infoAboutPaymentCommunalService.GetAll()
                    .WhereContainsBulked(x => x.DisclosureInfoRealityObj.RealityObject.Id, realityObjects)
                    .AsEnumerable()
                    .GroupBy(x => x.BaseService.TemplateService.Code)
                    .ToDictionary(x => x.Key, y => y.Select(x => x).First());

                foreach (var section19Record in sectionsData.Section19)
                {
                    var realityObject = realityObjectDict.ContainsKey(section19Record.CodeErc) ? realityObjectDict[section19Record.CodeErc] : null;
                    if (realityObject == null)
                    {
                        logImport.Warn(this.Name, $"Не удалось получить дом с кодом ЕРЦ {section19Record.CodeErc}");
                        continue;
                    }

                    if (section19Record.Code.IsEmpty())
                    {
                        logImport.Warn(this.Name, $"Не найдена услуга с кодом  {section19Record.CodeCommunalPay}");
                        continue;
                    }

                    var disclosureInfoRelation = disclosureInfoRelationsDict.Get(realityObject.Id);

                    if (disclosureInfoRelation == 0)
                    {
                        logImport.Warn(this.Name, $"Не добавлена услуга с кодом  {section19Record.Code}");
                        continue;
                    }

                    var baseService = baseServiceDict.Get(section19Record.Code);

                    if (baseService == 0)
                    {
                        logImport.Warn(this.Name, $"Не добавлена услуга с кодом  {section19Record.Code}");
                        continue;
                    }

                    var infoAboutPaymentCommunal = infoAboutPaymentCommunalDict.Get(section19Record.Code);

                    if (infoAboutPaymentCommunal == null)
                    {
                        infoAboutPaymentCommunal = new InfoAboutPaymentCommunal
                        {
                            DisclosureInfoRealityObj = new DisclosureInfoRealityObj
                            {
                                Id = disclosureInfoRelation
                            },
                            BaseService = new BaseService
                            {
                                Id = baseService
                            }
                        };
                    }

                    infoAboutPaymentCommunal.CounterValuePeriodStart = section19Record.MeterBegin;
                    infoAboutPaymentCommunal.CounterValuePeriodEnd = section19Record.MeterEnd;
                    infoAboutPaymentCommunal.TotalConsumption = section19Record.ConsVol;
                    infoAboutPaymentCommunal.Accrual = section19Record.AssessedCons;
                    infoAboutPaymentCommunal.Payed = section19Record.PaidCons;
                    infoAboutPaymentCommunal.Debt = section19Record.DebtCons;
                    infoAboutPaymentCommunal.AccrualByProvider = section19Record.AssessedSupp;
                    infoAboutPaymentCommunal.PayedToProvider = section19Record.PaidSupp;
                    infoAboutPaymentCommunal.DebtToProvider = section19Record.DebtSupp;
                    infoAboutPaymentCommunal.ReceivedPenaltySum = section19Record.ConsPenaltySum;

                    if (infoAboutPaymentCommunal.Id > 0)
                    {
                        logImport.CountChangedRows++;
                    }
                    else
                    {
                        logImport.CountAddedRows++;
                    }

                    infoPaymentComm.Add(infoAboutPaymentCommunal);
                }

                this.InTransaction(infoPaymentComm, infoAboutPaymentCommunalService);
            }
        }

        /// <summary>
        /// Транзакция
        /// </summary>
        /// <param name="list"></param>
        /// <param name="repos"></param>
        private void InTransaction(IEnumerable<PersistentObject> list, IDomainService repos)
        {
            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                try
                {
                    foreach (var entity in list)
                    {
                        if (entity.Id > 0)
                        {
                            repos.Update(entity);
                        }
                        else
                        {
                            repos.Save(entity);
                        }
                    }

                    transaction.Commit();
                }
                catch (Exception exc)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (Exception e)
                    {
                        throw new Exception(
                            string.Format(
                                "Произошла не известная ошибка при откате транзакции: \r\nMessage: {0}; \r\nStackTrace:{1};",
                                e.Message,
                                e.StackTrace),
                            exc);
                    }

                    throw;
                }
            }
        }
    }
}