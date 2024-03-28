namespace Bars.GkhDi.Import.Sections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Entities;
    using Bars.GkhDi.Entities;
    using Bars.GkhDi.Enums;

    using Castle.Windsor;

    public class SectionImport12 : ISectionImport
    {
        public string Name => "Импорт из комплат секция #12";

        public IWindsorContainer Container { get; set; }

        public void ImportSection(ImportParams importParams)
        {
            var resultList = new List<TariffForRso>();

            var inn = importParams.Inn;
            var sectionsData = importParams.SectionData;

            if (sectionsData.Section12.Count == 0)
            {
                return;
            }

            var logImport = importParams.LogImport;
            var realityObjects = importParams.RealityObjectIds;
            var realityObjectDict = importParams.RealObjsImportInfo;

            var communalService = this.Container.Resolve<IDomainService<CommunalService>>();
            var tariffForRsoService = this.Container.Resolve<IDomainService<TariffForRso>>();
            var disclosureInfoRealityObjService = this.Container.Resolve<IDomainService<DisclosureInfoRealityObj>>();
            var templateServiceDomain = this.Container.Resolve<IDomainService<TemplateService>>();

            using (this.Container.Using(communalService, tariffForRsoService, disclosureInfoRealityObjService, templateServiceDomain))
            {
                var communalServiceDict = communalService
                    .GetAll()
                    .Where(x => x.DisclosureInfoRealityObj.PeriodDi.Id == importParams.PeriodDiId)
                    .GroupBy(x => x.DisclosureInfoRealityObj.Id)
                    .ToDictionary(x => x.Key, y => y.AsEnumerable());

                var tariffForRsoDict = tariffForRsoService
                    .GetAll()
                    .Where(x => x.BaseService.DisclosureInfoRealityObj.PeriodDi.Id == importParams.PeriodDiId)
                    .GroupBy(x => x.BaseService.Id)
                    .ToDictionary(x => x.Key);

                var disclosureInfoRealityObjServiceDict = disclosureInfoRealityObjService
                    .GetAll()
                    .Where(x => x.PeriodDi.Id == importParams.PeriodDiId)
                    .GroupBy(x => x.RealityObject.Id)
                    .ToDictionary(x => x.Key, y => y.First());

                var templateServiceDict = templateServiceDomain
                    .GetAll()
                    .Where(x => x.Code != null)
                    .GroupBy(x => x.Code)
                    .ToDictionary(x => x.Key, y => y.FirstOrDefault());

                foreach (var section12Record in sectionsData.Section12)
                {
                    // Получаем дом по коду ЕРЦ
                    var realityObject = realityObjectDict.ContainsKey(section12Record.CodeErc) ? realityObjectDict[section12Record.CodeErc] : null;
                    if (realityObject == null)
                    {
                        logImport.Warn(this.Name, string.Format("Не удалось получить дом с кодом ЕРЦ {0}", section12Record.CodeErc));
                        continue;
                    }

                    if (!realityObjects.Contains(realityObject.Id))
                    {
                        logImport.Warn(this.Name,
                            string.Format("Для дома с кодом ЕРЦ {0} и упр. организацией с инн {1} нет договора управления в данном периоде",
                                section12Record.CodeErc,
                                inn));
                        continue;
                    }

                    // Получаем раскрытие в доме (или если нету создаем его)
                    var disclosureInfoRealityObj = disclosureInfoRealityObjServiceDict.ContainsKey(realityObject.Id)
                        ? disclosureInfoRealityObjServiceDict[realityObject.Id]
                        : null;

                    if (disclosureInfoRealityObj == null)
                    {
                        disclosureInfoRealityObj = new DisclosureInfoRealityObj
                        {
                            Id = 0,
                            PeriodDi = new PeriodDi { Id = importParams.PeriodDiId },
                            RealityObject = new RealityObject { Id = realityObject.Id }
                        };
                        disclosureInfoRealityObjService.Save(disclosureInfoRealityObj);
                    }

                    // По коду получаем шаблонную услугу
                    var templateService = templateServiceDict.ContainsKey(section12Record.Code) ? templateServiceDict[section12Record.Code] : null;

                    // Только для коммунальных услуг есть тарифы РСО
                    if (templateService != null && templateService.KindServiceDi == KindServiceDi.Communal)
                    {
                        var communal = communalServiceDict.ContainsKey(disclosureInfoRealityObj.Id)
                            ? communalServiceDict[disclosureInfoRealityObj.Id].FirstOrDefault(x => x.TemplateService.Id == templateService.Id)
                            : null;

                        if (communal == null)
                        {
                            logImport.Warn(this.Name, string.Format("Не удалось получить услугу с кодом {0}", section12Record.CodeCommunalPay));
                            continue;
                        }

                        // Для коммунальной услуги заполняем или добавляем тарифы РСО
                        var tariffForRso = tariffForRsoDict.ContainsKey(communal.Id)
                            ? tariffForRsoDict[communal.Id].FirstOrDefault(x => x.Cost == section12Record.Tariff)
                            : null;
                        if (tariffForRso == null)
                        {
                            logImport.Info(this.Name, string.Format("Добавлен тариф в услугу {0}", communal.TemplateService.Name));

                            tariffForRso = new TariffForRso
                            {
                                Id = 0,
                                BaseService = new BaseService { Id = communal.Id },
                                Cost = section12Record.Tariff,
                                DateStart = section12Record.TarifRsoDate
                            };

                        }

                        tariffForRso.NumberNormativeLegalAct = section12Record.ActNum;
                        tariffForRso.DateNormativeLegalAct = section12Record.ActDate;
                        tariffForRso.OrganizationSetTariff = section12Record.OrganizationSetTariff;

                        if (tariffForRso.Id > 0)
                        {
                            logImport.CountChangedRows++;
                        }
                        else
                        {
                            logImport.CountAddedRows++;
                        }

                        resultList.Add(tariffForRso);
                    }
                }

                this.InTransaction(resultList, tariffForRsoService);
                resultList.Clear();
            }
        }

        #region Транзакция

        private IDataTransaction BeginTransaction()
        {
            return Container.Resolve<IDataTransaction>();
        }

        private void InTransaction(IEnumerable<PersistentObject> list, IDomainService repos)
        {
            using (var transaction = BeginTransaction())
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

        #endregion
    }
}

