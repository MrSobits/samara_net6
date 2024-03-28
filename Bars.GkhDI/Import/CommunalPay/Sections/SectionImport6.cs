namespace Bars.GkhDi.Import.Sections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.GkhDi.Entities;

    using Castle.Windsor;

    public class SectionImport6 : ISectionImport
    {
        public string Name => "Импорт из комплат секция #6";

        public IWindsorContainer Container { get; set; }

        public void ImportSection(ImportParams importParams)
        {
            var resultList = new List<FundsInfo>();

            var inn = importParams.Inn;
            var sectionsData = importParams.SectionData;

            if (sectionsData.Section6.Count == 0)
            {
                return;
            }

            var logImport = importParams.LogImport;

            var disinfoService = this.Container.Resolve<IDomainService<DisclosureInfo>>();
            var fundsInfoService = this.Container.Resolve<IDomainService<FundsInfo>>();

            using (this.Container.Using(disinfoService, fundsInfoService))
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
                    var fundsInfoServiceList = fundsInfoService
                        .GetAll()
                        .Where(x => x.DisclosureInfo.Id == disclosureInfo.Id)
                        .ToList();

                    foreach (var section6Record in sectionsData.Section6)
                    {
                        var fundsInfo = fundsInfoServiceList.FirstOrDefault(x => x.DocumentName == section6Record.DocumentName);

                        if (fundsInfo == null)
                        {
                            logImport.Info(this.Name, string.Format("Добавлена запись в раздел 'Сведения о фондах'  {0}", section6Record.DocumentName));

                            fundsInfo = new FundsInfo
                            {
                                Id = 0,
                                DisclosureInfo = disclosureInfo,
                                DocumentName = section6Record.DocumentName
                            };
                        }

                        fundsInfo.Size = section6Record.Size;
                        fundsInfo.DocumentDate = section6Record.DocumentDate;

                        if (fundsInfo.Id > 0)
                        {
                            logImport.CountChangedRows++;
                        }
                        else
                        {
                            logImport.CountAddedRows++;
                        }

                        resultList.Add(fundsInfo);

                        disclosureInfo.SizePayments = section6Record.SizePayments;
                    }

                    this.InTransaction(resultList, fundsInfoService);
                    resultList.Clear();

                    disinfoService.Update(disclosureInfo);
                    logImport.CountChangedRows++;
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
