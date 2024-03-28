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

    public class SectionImport14 : ISectionImport
    {
        public string Name => "Импорт из комплат секция #14";

        public IWindsorContainer Container { get; set; }

        public void ImportSection(ImportParams importParams)
        {
            var resultList = new List<NonResidentialPlacement>();

            var inn = importParams.Inn;
            var sectionsData = importParams.SectionData;
            var realityObjectDict = importParams.RealObjsImportInfo;

            if (sectionsData.Section14.Count == 0)
            {
                return;
            }

            var logImport = importParams.LogImport;
            var realityObjects = importParams.RealityObjectIds;

            var nonResidentPlacementService = this.Container.Resolve<IDomainService<NonResidentialPlacement>>();
            var disclosureInfoRealityObjService = this.Container.Resolve<IDomainService<DisclosureInfoRealityObj>>();

            using (this.Container.Using(nonResidentPlacementService, disclosureInfoRealityObjService))
            {
                var nonResidentPlacementServiceDict = nonResidentPlacementService
                    .GetAll()
                    .Where(x => x.DisclosureInfoRealityObj.PeriodDi.Id == importParams.PeriodDiId)
                    .GroupBy(x => x.DisclosureInfoRealityObj.Id)
                    .ToDictionary(x => x.Key, y => y.AsEnumerable());

                var disclosureInfoRealityObjServiceDict = disclosureInfoRealityObjService
                    .GetAll()
                    .Where(x => x.PeriodDi.Id == importParams.PeriodDiId)
                    .GroupBy(x => x.RealityObject.Id)
                    .ToDictionary(x => x.Key, y => y.First());

                foreach (var section14Record in sectionsData.Section14)
                {
                    // Получаем дом по коду ЕРЦ
                    var realityObject = realityObjectDict.ContainsKey(section14Record.CodeErc) ? realityObjectDict[section14Record.CodeErc] : null;
                    if (realityObject == null)
                    {
                        logImport.Warn(this.Name, string.Format("Не удалось получить дом с кодом ЕРЦ {0}", section14Record.CodeErc));
                        continue;
                    }

                    if (!realityObjects.Contains(realityObject.Id))
                    {
                        logImport.Warn(this.Name,
                            string.Format("Для дома с кодом ЕРЦ {0} и упр. организацией с инн {1} нет договора управления в данном периоде",
                                section14Record.CodeErc,
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

                    var nonResidentPlacement = nonResidentPlacementServiceDict.ContainsKey(disclosureInfoRealityObj.Id)
                        ? nonResidentPlacementServiceDict[disclosureInfoRealityObj.Id]
                            .FirstOrDefault(x => x.ContragentName == section14Record.ContragentName)
                        : null;

                    if (nonResidentPlacement == null)
                    {
                        logImport.Info(this.Name,
                            string.Format("Добавлена запись в раздел 'Сведения об использование нежилых помещений' для {0}",
                                section14Record.ContragentName));

                        nonResidentPlacement = new NonResidentialPlacement
                        {
                            Id = 0,
                            DisclosureInfoRealityObj = disclosureInfoRealityObj,
                            ContragentName = section14Record.ContragentName
                        };
                    }

                    nonResidentPlacement.Area = section14Record.Area;
                    nonResidentPlacement.DateStart = section14Record.DateStart;
                    nonResidentPlacement.DateEnd = section14Record.DateEnd;
                    nonResidentPlacement.TypeContragentDi = TypeContragentDi.NotSet;

                    if (nonResidentPlacement.Id > 0)
                    {
                        logImport.CountChangedRows++;
                    }
                    else
                    {
                        logImport.CountAddedRows++;
                    }

                    resultList.Add(nonResidentPlacement);
                }

                InTransaction(resultList, nonResidentPlacementService);

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

