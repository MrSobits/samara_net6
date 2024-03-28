namespace Bars.GkhDi.Import.Sections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Enums;
    using Bars.GkhDi.Entities;

    using Castle.Windsor;

    using FileInfo = Bars.B4.Modules.FileStorage.FileInfo;

    public class SectionImport5 : ISectionImport
    {
        public string Name => "Импорт из комплат секция #5";

        public IWindsorContainer Container { get; set; }

        public void ImportSection(ImportParams importParams)
        {
            var resultList = new List<AdminResp>();
            var resultActionList = new List<Actions>();

            var inn = importParams.Inn;
            var sectionsData = importParams.SectionData;

            if (sectionsData.Section5.Count == 0)
            {
                return;
            }

            var logImport = importParams.LogImport;


            var disclosureInfoService = this.Container.Resolve<IDomainService<DisclosureInfo>>();
            var adminRespService = this.Container.Resolve<IDomainService<AdminResp>>();
            var adminRespActionService = this.Container.Resolve<IDomainService<Actions>>();
            var fileManager = this.Container.Resolve<IFileManager>();
            var supervisoryOrgService = this.Container.Resolve<IDomainService<SupervisoryOrg>>();
            var fileinfoService = this.Container.Resolve<IDomainService<FileInfo>>();

            using (this.Container.Using(disclosureInfoService,
                adminRespService,
                adminRespActionService,
                fileManager,
                supervisoryOrgService,
                fileinfoService))
            {
                var disclosureInfo = disclosureInfoService
                    .GetAll()
                    .FirstOrDefault(x => x.ManagingOrganization.Contragent.Inn == inn
                        && x.PeriodDi.Id == importParams.PeriodDiId);

                if (disclosureInfo == null)
                {
                    logImport.Warn(this.Name, string.Format("Не удалось получить сведения об УО с ИНН {0} в выбраном периоде", inn));
                }
                else
                {
                    var adminRespServiceList = adminRespService
                        .GetAll()
                        .Where(x => x.DisclosureInfo.Id == disclosureInfo.Id)
                        .ToList();

                    var adminRespActionDict = adminRespActionService
                        .GetAll()
                        .Where(x => x.AdminResp.DisclosureInfo.Id == disclosureInfo.Id)
                        .Select(x => new
                        {
                            AdminRespId = x.AdminResp.Id,
                            x.Action
                        })
                        .GroupBy(x => x.AdminRespId)
                        .ToDictionary(x => x.Key, y => y.Select(x => x.Action).ToArray());
                    
                    foreach (var section5Record in sectionsData.Section5)
                    {
                        var nameOrg = supervisoryOrgService.GetAll().FirstOrDefault(x => x.Code == section5Record.OrganizationName);

                        if (nameOrg == null)
                        {
                            logImport.Warn(this.Name, string.Format("Не найден контролирующий орган с кодом {0}", section5Record.OrganizationName));
                        }

                        var fileName = section5Record.FileName;
                        var fileInfo = FileCreater.Create(importParams.zipName, fileName, fileinfoService, fileManager);

                        var adminResp = adminRespServiceList.FirstOrDefault(x => x.AmountViolation == section5Record.AmountViolation &&
                            x.DateImpositionPenalty == section5Record.DateImpositionPenalty
                            && x.SumPenalty == section5Record.SumPenalty);

                        if (adminResp == null)
                        {
                            logImport.Info(this.Name,
                                string.Format("Добавлена запись в раздел 'Административная ответственность с суммой' для услуги {0}",
                                    section5Record.AmountViolation));

                            adminResp = new AdminResp
                            {
                                Id = 0,
                                DisclosureInfo = disclosureInfo,
                                AmountViolation = section5Record.AmountViolation
                            };
                        }

                        adminResp.SupervisoryOrg = nameOrg;
                        adminResp.DateImpositionPenalty = section5Record.DateImpositionPenalty;
                        adminResp.SumPenalty = section5Record.SumPenalty;
                        adminResp.DatePaymentPenalty = section5Record.DatePaymentPenalty;
                        adminResp.File = fileInfo;

                        if (!adminRespActionDict.ContainsKey(adminResp.Id) || adminRespActionDict[adminResp.Id].All(x => x != section5Record.Actions))
                        {
                            resultActionList.Add(new Actions
                            {
                                AdminResp = adminResp,
                                Action = section5Record.Actions
                            });
                        }

                        if (adminResp.Id > 0)
                        {
                            logImport.CountChangedRows++;
                        }
                        else
                        {
                            logImport.CountAddedRows++;
                        }

                        resultList.Add(adminResp);
                    }

                    if (resultList.Count != 0)
                    {
                        disclosureInfo.AdminResponsibility = YesNoNotSet.Yes;
                    }

                    this.InTransaction(resultList, adminRespService);
                    this.InTransaction(resultActionList, adminRespActionService);
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
