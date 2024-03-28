namespace Bars.GkhDi.Import.Sections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    using Castle.Windsor;

    public class SectionImport2 : ISectionImport
    {
        public string Name => "Импорт из комплат секция #2";

        public IWindsorContainer Container { get; set; }

        public void ImportSection(ImportParams importParams)
        {
            var resultList = new List<ManOrgContractRealityObject>();

            var inn = importParams.Inn;
            var sectionsData = importParams.SectionData;

            if (sectionsData.Section2.Count == 0)
            {
                return;
            }

            var logImport = importParams.LogImport;
            var realityObjects = importParams.RealityObjectIds;
            var realityObjectDict = importParams.RealObjsImportInfo;

            var managingOrganizationService = this.Container.Resolve<IDomainService<ManagingOrganization>>();
            var manOrgContractBaseService = this.Container.Resolve<IDomainService<ManOrgBaseContract>>();
            var manOrgContractRealityObjectService = this.Container.Resolve<IDomainService<ManOrgContractRealityObject>>();

            using (this.Container.Using(managingOrganizationService, manOrgContractBaseService, manOrgContractRealityObjectService))
            {
                var managingOrganization = managingOrganizationService.GetAll().FirstOrDefault(x => x.Contragent.Inn == inn);

                if (managingOrganization == null)
                {
                    logImport.Warn(this.Name, string.Format("Не удалось управляющую организацию с ИНН {0}", inn));
                    return;
                }

                foreach (var section2Record in sectionsData.Section2)
                {
                    var realityObject = realityObjectDict.ContainsKey(section2Record.CodeErc) ? realityObjectDict[section2Record.CodeErc] : null;
                    if (realityObject == null)
                    {
                        logImport.Warn(this.Name, string.Format("Не удалось получить дом с кодом ЕРЦ {0}", section2Record.CodeErc));
                        continue;
                    }

                    if (!realityObjects.Contains(realityObject.Id))
                    {
                        logImport.Warn(this.Name,
                            string.Format("Для дома с кодом ЕРЦ {0} и упр. организацией с инн {1} нет договора управления в данном периоде",
                                section2Record.CodeErc,
                                inn));
                        continue;
                    }

                    var typeContract = TypeContractManOrg.ManagingOrgOwners;
                    switch (managingOrganization.TypeManagement)
                    {
                        case TypeManagementManOrg.UK:
                            typeContract = TypeContractManOrg.ManagingOrgOwners;
                            break;
                        case TypeManagementManOrg.JSK:
                            typeContract = TypeContractManOrg.JskTsj;
                            break;
                        case TypeManagementManOrg.TSJ:
                            typeContract = TypeContractManOrg.ManagingOrgJskTsj;
                            break;
                    }

                    var manorgBaseContract =
                        manOrgContractBaseService.GetAll()
                            .Where(x => x.TypeContractManOrgRealObj == typeContract)
                            .FirstOrDefault(x => x.ManagingOrganization.Id == managingOrganization.Id);

                    if (manorgBaseContract == null)
                    {
                        logImport.Error(Name,
                            string.Format("Для дома с кодом ЕРЦ {0} и упр. организацией с инн {1} нет договора управления в данном периоде",
                                section2Record.CodeErc,
                                inn));
                        continue;
                    }

                    manorgBaseContract.StartDate = section2Record.DateBegin;
                    manorgBaseContract.EndDate = section2Record.DateEnd;
                    manorgBaseContract.TerminateReason = section2Record.TerminateReason;

                    manOrgContractBaseService.Update(manorgBaseContract);
                    logImport.CountChangedRows++;

                    var manOrgContractRealityObject =
                        manOrgContractRealityObjectService.GetAll()
                            .Where(x => x.RealityObject.Id == realityObject.Id)
                            .FirstOrDefault(x => x.ManOrgContract.Id == manorgBaseContract.Id);

                    if (manOrgContractRealityObject == null)
                    {
                        logImport.Info(this.Name,
                            string.Format("Добавлен дом {0} в договор {1}", realityObject.FiasAddressName, manorgBaseContract.DocumentNumber));

                        manOrgContractRealityObject = new ManOrgContractRealityObject
                        {
                            Id = 0,
                            ManOrgContract = manorgBaseContract,
                            RealityObject = new RealityObject { Id = realityObject.Id }
                        };

                        resultList.Add(manOrgContractRealityObject);
                        logImport.CountAddedRows++;
                    }
                }

                this.InTransaction(resultList, manOrgContractRealityObjectService);
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
