namespace Bars.GkhDi.Import.Sections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhDi.Entities;

    using Castle.Windsor;

    public class SectionImport18 : ISectionImport
    {
        public string Name => "Импорт из комплат секция #18";

        public IWindsorContainer Container { get; set; }

        public void ImportSection(ImportParams importParams)
        {
            var resultList = new List<CommunalService>();
            var sectionsData = importParams.SectionData;

            if (sectionsData.Section18.Count == 0)
            {
                return;
            }

            var logImport = importParams.LogImport;
            var realityObjects = importParams.RealityObjectIds;
            var realityObjectDict = importParams.RealObjsImportInfo;

            var communalService = this.Container.Resolve<IDomainService<CommunalService>>();
            var unitMeasureService = this.Container.Resolve<IDomainService<UnitMeasure>>();
            var disclosureInfoRealityObjService = this.Container.Resolve<IDomainService<DisclosureInfoRealityObj>>();

            using (this.Container.Using(communalService, disclosureInfoRealityObjService, unitMeasureService))
            {
                var disclosureInfoRealityObjs = disclosureInfoRealityObjService.GetAll()
                    .Where(x => x.PeriodDi.Id == importParams.PeriodDiId && realityObjects.Contains(x.RealityObject.Id))
                    .ToList();

                foreach (var section18Record in sectionsData.Section18)
                {
                    var realityObject = realityObjectDict.ContainsKey(section18Record.CodeErc) ? realityObjectDict[section18Record.CodeErc] : null;
                    if (realityObject == null)
                    {
                        logImport.Warn(this.Name, string.Format("Не удалось получить дом с кодом ЕРЦ {0}", section18Record.CodeErc));
                        continue;
                    }

                    if (section18Record.Code.IsEmpty())
                    {
                        logImport.Warn(this.Name, string.Format("Не найдена услуга с кодом  {0}", section18Record.CodeCommunalPay));
                        continue;
                    }

                    if (section18Record.UntitsNormCode.IsEmpty())
                    {
                        logImport.Warn(this.Name, string.Format("Не найдена единица измерения с кодом {0}", section18Record.UnitsNorm));
                        continue;
                    }

                    var disclosureInfoRealityObj = disclosureInfoRealityObjs.FirstOrDefault(x => x.RealityObject.Id == realityObject.Id);

                    var commService = communalService
                        .GetAll()
                        .FirstOrDefault(x => x.DisclosureInfoRealityObj.Id == disclosureInfoRealityObj.Id
                            && x.TemplateService.Code == section18Record.CodeCommunalPay);

                    if (commService == null)
                    {
                        logImport.Warn(this.Name, string.Format("Не добавлена услуга с кодом  {0}", section18Record.CodeCommunalPay));
                        continue;
                    }

                    var unitMeasure = unitMeasureService.GetAll().FirstOrDefault(x => x.Name == section18Record.UntitsNormCode);

                    commService.ConsumptionNormLivingHouse = section18Record.NormComm;
                    commService.UnitMeasureLivingHouse = unitMeasure;

                    if (commService.Id > 0)
                    {
                        logImport.CountChangedRows++;
                    }
                    else
                    {
                        logImport.CountAddedRows++;
                    }

                    resultList.Add(commService);
                }

                this.InTransaction(resultList, communalService);
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