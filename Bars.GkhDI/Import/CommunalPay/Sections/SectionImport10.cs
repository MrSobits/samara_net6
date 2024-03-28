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

    public class SectionImport10 : ISectionImport
    {
        public string Name => "Импорт из комплат секция #10";

        public IWindsorContainer Container { get; set; }

        public void ImportSection(ImportParams importParams)
        {
            var resultList = new List<InformationOnContracts>();

            var inn = importParams.Inn;
            var sectionsData = importParams.SectionData;

            if (sectionsData.Section10.Count == 0)
            {
                return;
            }

            var logImport = importParams.LogImport;
            var realityObjects = importParams.RealityObjectIds;
            var realityObjectDict = importParams.RealObjsImportInfo;

            var infoContractService = this.Container.Resolve<IDomainService<InformationOnContracts>>();
            var disinfoService = this.Container.Resolve<IDomainService<DisclosureInfo>>();

            using (this.Container.Using(infoContractService, disinfoService))
            {
                var disclosureInfo = disinfoService
                    .GetAll()
                    .FirstOrDefault(x => x.ManagingOrganization.Contragent.Inn == inn
                        && x.PeriodDi.Id == importParams.PeriodDiId);

                if (disclosureInfo == null)
                {
                    logImport.Warn(this.Name, string.Format("Не удалось получить сведения об УО с ИНН {0} в выбраном периоде", inn));
                }
                else
                {
                    var infoContractServiceList = infoContractService.GetAll()
                        .Where(x => x.DisclosureInfo.Id == disclosureInfo.Id)
                        .ToList();

                    foreach (var section10Record in sectionsData.Section10)
                    {
                        var realityObject = realityObjectDict.ContainsKey(section10Record.CodeErc) ? realityObjectDict[section10Record.CodeErc] : null;

                        if (realityObject == null)
                        {
                            logImport.Warn(this.Name, string.Format("Не удалось получить дом с кодом ЕРЦ {0}", section10Record.CodeErc));
                            continue;
                        }

                        if (!realityObjects.Contains(realityObject.Id))
                        {
                            logImport.Warn(this.Name,
                                string.Format("Для дома с кодом ЕРЦ {0} и упр. организацией с инн {1} нет договора управления в данном периоде",
                                    section10Record.CodeErc,
                                    inn));
                            continue;
                        }

                        var infoContract =
                            infoContractServiceList.FirstOrDefault(x => x.RealityObject.Id == realityObject.Id && x.Number == section10Record.Number);
                        if (infoContract == null)
                        {
                            logImport.Info(this.Name, string.Format("Добавлена запись в раздел 'Сведения о договорах №' {0}", section10Record.Number));

                            infoContract = new InformationOnContracts
                            {
                                Id = 0,
                                DisclosureInfo = disclosureInfo,
                                RealityObject = new RealityObject { Id = realityObject.Id },
                                Number = section10Record.Number
                            };
                        }

                        infoContract.From = section10Record.From;
                        infoContract.PartiesContract = section10Record.ContractMembers;
                        infoContract.DateStart = section10Record.DateStart;
                        infoContract.DateEnd = section10Record.DateEnd;
                        infoContract.Cost = section10Record.Cost;
                        infoContract.Comments = section10Record.Description;

                        if (infoContract.Id > 0)
                        {
                            logImport.CountChangedRows++;
                        }
                        else
                        {
                            logImport.CountAddedRows++;
                        }

                        resultList.Add(infoContract);
                    }

                }

                this.InTransaction(resultList, infoContractService);
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
