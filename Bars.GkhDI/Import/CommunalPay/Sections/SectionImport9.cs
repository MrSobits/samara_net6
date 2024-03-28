namespace Bars.GkhDi.Import.Sections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Entities;

    using Castle.Windsor;

    public class SectionImport9 : ISectionImport
    {
        public string Name => "Импорт из комплат секция #9";

        public IWindsorContainer Container { get; set; }

        public void ImportSection(ImportParams importParams)
        {
            var listResult = new List<ManagingOrgMembership>();

            var inn = importParams.Inn;
            var sectionsData = importParams.SectionData;

            if (sectionsData.Section9.Count == 0)
            {
                return;
            }

            var logImport = importParams.LogImport;

            var membershipService = this.Container.Resolve<IDomainService<ManagingOrgMembership>>();
            var managingOrganizationService = this.Container.Resolve<IDomainService<ManagingOrganization>>();

            using (this.Container.Using(membershipService, managingOrganizationService))
            {
                var managingOrg = managingOrganizationService
                    .GetAll()
                    .FirstOrDefault(x => x.Contragent.Inn == inn);

                if (managingOrg == null)
                {
                    logImport.Error(Name, string.Format("Не найдена управляющая организация по данному ИНН = {0}", inn));
                    return;
                }

                var membershiplist = membershipService.GetAll().Where(x => x.ManagingOrganization.Id == managingOrg.Id).ToList();

                foreach (var section9Record in sectionsData.Section9)
                {
                    var membership = membershiplist.FirstOrDefault(x => x.Name == section9Record.UnionName);

                    if (membership == null)
                    {
                        logImport.Info(this.Name, string.Format("Добавлена запись членства в объединение  {0}", section9Record.UnionName));

                        membership = new ManagingOrgMembership
                        {
                            Id = 0,
                            ManagingOrganization = managingOrg,
                            Name = section9Record.UnionName
                        };
                    }

                    membership.Address = section9Record.Address;
                    membership.OfficialSite = section9Record.OfficialSite;
                    membership.DateStart = section9Record.DateBegin;
                    membership.DateEnd = section9Record.DateEnd;

                    if (membership.Id > 0)
                    {
                        logImport.CountChangedRows++;
                    }
                    else
                    {
                        logImport.CountAddedRows++;
                    }

                    listResult.Add(membership);
                }

                this.InTransaction(listResult, membershipService);
                listResult.Clear();
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
