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

    using Castle.Windsor;

    public class SectionImport4 : ISectionImport
    {
        public string Name => "Импорт из комплат секция #4";

        public IWindsorContainer Container { get; set; }

        public void ImportSection(ImportParams importParams)
        {
            var resultList = new List<InfoAboutReductionPayment>();

            var inn = importParams.Inn;
            var sectionsData = importParams.SectionData;

            if (sectionsData.Section4.Count == 0)
            {
                return;
            }

            var logImport = importParams.LogImport;
            var realityObjects = importParams.RealityObjectIds;
            var realityObjectDict = importParams.RealObjsImportInfo;

            //фильтрация по домам
            var codeErcList = sectionsData.Section4.Select(x => x.CodeErc).Distinct().ToList();
            var realityObjectIdList = realityObjectDict
                .Where(x => codeErcList.Contains(x.Key))
                .Select(x => x.Value.Id)
                .ToList();

            var infoAboutReductionPaymentDomain = this.Container.Resolve<IDomainService<InfoAboutReductionPayment>>();
            var baseServiceDomain = this.Container.Resolve<IDomainService<BaseService>>();
            var templateServiceDomain = this.Container.Resolve<IDomainService<TemplateService>>();
            var disclosureInfoRealityObjDomain = this.Container.Resolve<IDomainService<DisclosureInfoRealityObj>>();

            using (this.Container.Using(infoAboutReductionPaymentDomain, baseServiceDomain, templateServiceDomain, disclosureInfoRealityObjDomain))
            {
                var disclosureInfoRealityObjServiceDict = disclosureInfoRealityObjDomain
                    .GetAll()
                    .Where(x =>
                        x.PeriodDi.Id == importParams.PeriodDiId
                        &&
                        realityObjectIdList.Contains(x.RealityObject.Id)
                    )
                    .GroupBy(x => x.RealityObject.Id)
                    .ToDictionary(x => x.Key, y => y.First());

                var infoReductPayServiceDict = infoAboutReductionPaymentDomain
                    .GetAll()
                    .Where(x => x.DisclosureInfoRealityObj.PeriodDi.Id == importParams.PeriodDiId)
                    .GroupBy(x => x.DisclosureInfoRealityObj.Id)
                    .ToDictionary(x => x.Key);

                var baseServiceDict = baseServiceDomain.GetAll()
                    .Where(x => x.DisclosureInfoRealityObj.PeriodDi.Id == importParams.PeriodDiId)
                    .Select(
                        x =>
                            new
                            {
                                x.Id,
                                DisclosureInfoRealityObjId = x.DisclosureInfoRealityObj.Id,
                                TemplateServiceId = x.TemplateService.Id,
                                TemplateServiceName = x.TemplateService.Name
                            })
                    .AsEnumerable()
                    .GroupBy(x => x.DisclosureInfoRealityObjId)
                    .ToDictionary(x => x.Key);

                var templateServiceDict = templateServiceDomain.GetAll()
                    .Where(x => x.Code != null)
                    .AsEnumerable()
                    .GroupBy(x => x.Code)
                    .ToDictionary(x => x.Key, y => y.FirstOrDefault());

                foreach (var section4Record in sectionsData.Section4)
                {
                    // Получаем дом по коду ЕРЦ
                    var realityObject = realityObjectDict.ContainsKey(section4Record.CodeErc) ? realityObjectDict[section4Record.CodeErc] : null;
                    if (realityObject == null)
                    {
                        logImport.Warn(this.Name, string.Format("Не удалось получить дом с кодом ЕРЦ {0}", section4Record.CodeErc));
                        continue;
                    }

                    if (!realityObjects.Contains(realityObject.Id))
                    {
                        logImport.Warn(this.Name,
                            string.Format("Для дома с кодом ЕРЦ {0} и упр. организацией с инн {1} нет договора управления в данном периоде",
                                section4Record.CodeErc,
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
                        disclosureInfoRealityObjDomain.Save(disclosureInfoRealityObj);
                        disclosureInfoRealityObjServiceDict.Add(realityObject.Id, disclosureInfoRealityObj);
                    }

                    // По коду получаем шаблонную услугу
                    var templateService = templateServiceDict.ContainsKey(section4Record.Code) ? templateServiceDict[section4Record.Code] : null;
                    if (templateService == null)
                    {
                        logImport.Warn(this.Name, string.Format("Не удалось получить услугу с кодом {0}", section4Record.CodeCommunalPay));
                        continue;
                    }

                    var baseService = baseServiceDict.ContainsKey(disclosureInfoRealityObj.Id)
                        ? baseServiceDict[disclosureInfoRealityObj.Id].FirstOrDefault(x => x.TemplateServiceId == templateService.Id)
                        : null;
                    if (baseService == null)
                    {
                        logImport.Warn(this.Name,
                            string.Format("Не удалось получить сведение об услуге для услуги с кодом {0}", section4Record.CodeCommunalPay));
                        continue;
                    }

                    var infoReductPay = infoReductPayServiceDict.ContainsKey(disclosureInfoRealityObj.Id)
                        ? infoReductPayServiceDict[disclosureInfoRealityObj.Id].FirstOrDefault(x => x.BaseService.Id == baseService.Id)
                        : null;

                    if (infoReductPay == null)
                    {
                        logImport.Info(this.Name,
                            string.Format("Добавлена запись в раздел 'Сведения о снижения плат' для услуги {0}", baseService.TemplateServiceName));

                        infoReductPay = new InfoAboutReductionPayment
                        {
                            Id = 0,
                            BaseService = new BaseService { Id = baseService.Id },
                            DisclosureInfoRealityObj = disclosureInfoRealityObj
                        };
                    }

                    infoReductPay.RecalculationSum = section4Record.RecalculationSum;

                    if (infoReductPay.Id > 0)
                    {
                        logImport.CountChangedRows++;
                    }
                    else
                    {
                        logImport.CountAddedRows++;
                    }

                    resultList.Add(infoReductPay);
                }

                this.InTransaction(resultList, infoAboutReductionPaymentDomain);
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
