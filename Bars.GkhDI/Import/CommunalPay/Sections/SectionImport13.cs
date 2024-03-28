namespace Bars.GkhDi.Import.Sections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.GkhDi.Entities;
    using Bars.GkhDi.Enums;

    using Castle.Windsor;

    public class SectionImport13 : ISectionImport
    {
        public string Name => "Импорт из комплат секция #13";

        public IWindsorContainer Container { get; set; }

        public void ImportSection(ImportParams importParams)
        {
            var listCommunal = new List<FinActivityCommunalService>();
            var listManag = new List<FinActivityManagCategory>();

            var inn = importParams.Inn;
            var sectionsData = importParams.SectionData;

            if (sectionsData.Section13.Count == 0)
            {
                return;
            }

            var logImport = importParams.LogImport;

            var finCommunalService = this.Container.Resolve<IDomainService<FinActivityCommunalService>>();
            var finManagCatService = this.Container.Resolve<IDomainService<FinActivityManagCategory>>();
            var disinfoService = this.Container.Resolve<IDomainService<DisclosureInfo>>();

            using (this.Container.Using(finCommunalService, finManagCatService, disinfoService))
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
                    foreach (var section13Record in sectionsData.Section13)
                    {
                        var finManagCatServiceList = finManagCatService
                            .GetAll()
                            .Where(x => x.DisclosureInfo.Id == disclosureInfo.Id)
                            .ToList();

                        var finCommunalServiceList = finCommunalService
                            .GetAll()
                            .Where(x => x.DisclosureInfo.Id == disclosureInfo.Id)
                            .ToList();

                        // Показатели по коммунальным услугам
                        var finCommunal = finCommunalServiceList.FirstOrDefault(x => x.TypeServiceDi == TypeServiceDi.Summury);

                        if (finCommunal == null)
                        {
                            finCommunal = new FinActivityCommunalService
                            {
                                Id = 0,
                                DisclosureInfo = disclosureInfo,
                                TypeServiceDi = TypeServiceDi.Summury
                            };
                        }

                        finCommunal.IncomeFromProviding = section13Record.IncomeCommunal;
                        finCommunal.DebtPopulationEnd = section13Record.DebtPopulationEndCommunal;
                        finCommunal.DebtPopulationStart = section13Record.DebtPopulationStartCommunal;
                        finCommunal.PaidByMeteringDevice = section13Record.PaidByMeteringDevice;
                        finCommunal.PaidByGeneralNeeds = section13Record.PaidByGeneralNeeds;

                        if (finCommunal.Id > 0)
                        {
                            logImport.CountChangedRows++;
                        }
                        else
                        {
                            logImport.CountAddedRows++;
                        }

                        listCommunal.Add(finCommunal);

                        // Показатели по управление по категориям
                        var finManagCat = finManagCatServiceList.FirstOrDefault(x => x.TypeCategoryHouseDi == TypeCategoryHouseDi.Summury);

                        if (finManagCat == null)
                        {
                            finManagCat = new FinActivityManagCategory()
                            {
                                Id = 0,
                                DisclosureInfo = disclosureInfo,
                                TypeCategoryHouseDi = TypeCategoryHouseDi.Summury
                            };
                        }

                        finManagCat.DebtPopulationEnd = section13Record.DebtPopulationEndManagCategory;
                        finManagCat.DebtPopulationEnd = section13Record.DebtPopulationEndManagCategory;
                        finManagCat.DebtPopulationStart = section13Record.DebtPopulationStartManagCategory;
                        finManagCat.IncomeManaging = section13Record.IncomeManagCategory;
                        
                        if (finManagCat.Id > 0)
                        {
                            logImport.CountChangedRows++;
                        }
                        else
                        {
                            logImport.CountAddedRows++;
                        }

                        listManag.Add(finManagCat);
                    }

                    this.InTransaction(listManag, finManagCatService);
                    this.InTransaction(listCommunal, finCommunalService);

                    listManag.Clear();
                    listCommunal.Clear();
                }
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
