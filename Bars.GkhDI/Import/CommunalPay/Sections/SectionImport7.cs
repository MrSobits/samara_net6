namespace Bars.GkhDi.Import.Sections
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.GkhDi.Entities;
    using Bars.GkhDi.Enums;

    using Castle.Windsor;

    using FileInfo = Bars.B4.Modules.FileStorage.FileInfo;

    public class SectionImport7 : ISectionImport
    {
        public string Name => "Импорт из комплат секция #7";

        public IWindsorContainer Container { get; set; }

        public void ImportSection(ImportParams importParams)
        {
            var listDocuments = new List<Documents>();
            var listFinActivityDocs = new List<FinActivityDocs>();
            var listFinActivityDocByYear = new List<FinActivityDocByYear>();
            var listFinActivityAudit = new List<FinActivityAudit>();
            var listDocumentsRealityObjProtocol = new List<DocumentsRealityObjProtocol>();

            var inn = importParams.Inn;
            var sectionsData = importParams.SectionData;

            if (sectionsData.Section7.Count == 0)
            {
                return;
            }

            var logImport = importParams.LogImport;
            var realityObjIds = importParams.RealityObjectIds;

            var fileinfoService = this.Container.Resolve<IDomainService<FileInfo>>();
            var fileManager = this.Container.Resolve<IFileManager>();
            var documentsService = this.Container.Resolve<IDomainService<Documents>>();
            var finDocByYearService = this.Container.Resolve<IDomainService<FinActivityDocByYear>>();
            var finAuditService = this.Container.Resolve<IDomainService<FinActivityAudit>>();
            var finDocsService = this.Container.Resolve<IDomainService<FinActivityDocs>>();
            var disclosureInfoService = this.Container.Resolve<IDomainService<DisclosureInfo>>();
            var periodDiService = this.Container.Resolve<IDomainService<PeriodDi>>();
            var documentsRoService = this.Container.Resolve<IDomainService<DocumentsRealityObjProtocol>>();

            using (this.Container.Using(fileinfoService,
                fileManager,
                documentsService,
                finDocByYearService,
                finAuditService,
                finDocsService,
                disclosureInfoService,
                periodDiService,
                documentsRoService))
            {
                var periodDi = periodDiService.Get(importParams.PeriodDiId);
                if (periodDi == null)
                {
                    logImport.Error(Name, "Не найден период");
                    return;
                }

                var disclosureInfo = disclosureInfoService
                    .GetAll()
                    .FirstOrDefault(x => x.ManagingOrganization.Contragent.Inn == inn && x.PeriodDi.Id == importParams.PeriodDiId);

                var currentYear = periodDi.DateStart.Value.Year;

                if (disclosureInfo == null)
                {
                    logImport.Warn(this.Name, string.Format("Не удалось получить сведения об УО с ИНН {0} в выбраном периоде", inn));
                }
                else
                {
                    var document = documentsService.GetAll().FirstOrDefault(x => x.DisclosureInfo.Id == disclosureInfo.Id)
                        ?? new Documents { DisclosureInfo = disclosureInfo };

                    var finDoc = finDocsService.GetAll().FirstOrDefault(x => x.DisclosureInfo.Id == disclosureInfo.Id)
                        ?? new FinActivityDocs { DisclosureInfo = disclosureInfo };

                    var finDocsByYear = finDocByYearService.GetAll()
                        .Where(x => x.ManagingOrganization.Id == disclosureInfo.ManagingOrganization.Id)
                        .ToArray();

                    var finAuditDocs = finAuditService
                        .GetAll()
                        .Where(x => x.ManagingOrganization.Id == disclosureInfo.ManagingOrganization.Id)
                        .ToArray();

                    foreach (var section7Record in sectionsData.Section7)
                    {
                        switch (section7Record.DocumentType)
                        {
                            case "Проект договора управления":
                            {
                                document.FileProjectContract = FileCreater.Create(importParams.zipName, section7Record.File, fileinfoService, fileManager);

                                if (document.Id > 0)
                                {
                                    logImport.CountChangedRows++;
                                }
                                else
                                {
                                    logImport.CountAddedRows++;
                                }

                                listDocuments.Add(document);

                                break;
                            }

                            case "Перечень и качество коммунальных услуг":
                            {
                                document.FileCommunalService = FileCreater.Create(importParams.zipName, section7Record.File, fileinfoService, fileManager);

                                if (document.Id > 0)
                                {
                                    logImport.CountChangedRows++;
                                }
                                else
                                {
                                    logImport.CountAddedRows++;
                                }

                                listDocuments.Add(document);

                                break;
                            }

                            case "Базовый перечень показателей качества содержания, эксплуатации и технического обслуживания жилых зданий":
                            {
                                document.FileServiceApartment =
                                    FileCreater.Create(importParams.zipName, section7Record.File, fileinfoService, fileManager);

                                if (document.Id > 0)
                                {
                                    logImport.CountChangedRows++;
                                }
                                else
                                {
                                    logImport.CountAddedRows++;
                                }

                                listDocuments.Add(document);

                                break;
                            }

                            case "Бухгалтерский баланс":
                            {
                                finDoc.BookkepingBalance = FileCreater.Create(importParams.zipName, section7Record.File, fileinfoService, fileManager);

                                if (finDoc.Id > 0)
                                {
                                    logImport.CountChangedRows++;
                                }
                                else
                                {
                                    logImport.CountAddedRows++;
                                }

                                listFinActivityDocs.Add(finDoc);

                                break;
                            }

                            case "Приложение к бухгалтерскому балансу":
                            {
                                finDoc.BookkepingBalanceAnnex =
                                    FileCreater.Create(importParams.zipName, section7Record.File, fileinfoService, fileManager);

                                if (finDoc.Id > 0)
                                {
                                    logImport.CountChangedRows++;
                                }
                                else
                                {
                                    logImport.CountAddedRows++;
                                }

                                listFinActivityDocs.Add(finDoc);

                                break;
                            }

                            case "Сметы доходов и расходов на текущий год":
                            {
                                var finDocByYear = finDocsByYear
                                    .FirstOrDefault(
                                        x => x.Year == currentYear
                                            && x.TypeDocByYearDi == TypeDocByYearDi.EstimateIncome);

                                if (finDocByYear == null)
                                {
                                    finDocByYear = new FinActivityDocByYear
                                    {
                                        ManagingOrganization = disclosureInfo.ManagingOrganization,
                                        Year = currentYear,
                                        TypeDocByYearDi = TypeDocByYearDi.EstimateIncome
                                    };
                                }

                                finDocByYear.File = FileCreater.Create(importParams.zipName, section7Record.File, fileinfoService, fileManager);

                                if (finDocByYear.Id > 0)
                                {
                                    logImport.CountChangedRows++;
                                }
                                else
                                {
                                    logImport.CountAddedRows++;
                                }

                                listFinActivityDocByYear.Add(finDocByYear);

                                break;
                            }

                            case "Сметы доходов и расходов за предыдущий год":
                            {
                                var finDocByYear = finDocsByYear
                                    .FirstOrDefault(x => x.Year == currentYear - 1
                                        && x.TypeDocByYearDi == TypeDocByYearDi.EstimateIncome);

                                if (finDocByYear == null)
                                {
                                    finDocByYear = new FinActivityDocByYear
                                    {
                                        ManagingOrganization = disclosureInfo.ManagingOrganization,
                                        Year = currentYear - 1,
                                        TypeDocByYearDi = TypeDocByYearDi.EstimateIncome
                                    };
                                }

                                finDocByYear.File = FileCreater.Create(importParams.zipName, section7Record.File, fileinfoService, fileManager);

                                if (finDocByYear.Id > 0)
                                {
                                    logImport.CountChangedRows++;
                                }
                                else
                                {
                                    logImport.CountAddedRows++;
                                }

                                listFinActivityDocByYear.Add(finDocByYear);

                                break;
                            }

                            case "Отчёт о выполнении сметы доходов и расходов за предыдущий год":
                            {
                                var finDocByYear = finDocsByYear.FirstOrDefault(
                                    x => x.Year == currentYear - 1
                                        && x.TypeDocByYearDi == TypeDocByYearDi.ReportEstimateIncome);

                                if (finDocByYear == null)
                                {
                                    finDocByYear = new FinActivityDocByYear
                                    {
                                        ManagingOrganization =
                                            disclosureInfo.ManagingOrganization,
                                        Year = currentYear - 1,
                                        TypeDocByYearDi =
                                            TypeDocByYearDi.ReportEstimateIncome
                                    };
                                }

                                finDocByYear.File = FileCreater.Create(importParams.zipName, section7Record.File, fileinfoService, fileManager);

                                if (finDocByYear.Id > 0)
                                {
                                    logImport.CountChangedRows++;
                                }
                                else
                                {
                                    logImport.CountAddedRows++;
                                }

                                listFinActivityDocByYear.Add(finDocByYear);

                                break;
                            }

                            case "Заключение ревизионной комиссии за текущий год":
                            {
                                var finDocByYear = finDocsByYear.FirstOrDefault(
                                    x => x.Year == currentYear
                                        && x.TypeDocByYearDi == TypeDocByYearDi.ConclusionRevisory);

                                if (finDocByYear == null)
                                {
                                    finDocByYear = new FinActivityDocByYear
                                    {
                                        ManagingOrganization =
                                            disclosureInfo.ManagingOrganization,
                                        Year = currentYear,
                                        TypeDocByYearDi =
                                            TypeDocByYearDi.ConclusionRevisory
                                    };
                                }

                                finDocByYear.File = FileCreater.Create(importParams.zipName, section7Record.File, fileinfoService, fileManager);

                                if (finDocByYear.Id > 0)
                                {
                                    logImport.CountChangedRows++;
                                }
                                else
                                {
                                    logImport.CountAddedRows++;
                                }

                                listFinActivityDocByYear.Add(finDocByYear);

                                break;
                            }

                            case "Заключение ревизионной комиссии за год предшествующий текущему году":
                            {
                                var finDocByYear = finDocsByYear
                                    .FirstOrDefault(x => x.Year == currentYear - 1
                                        && x.TypeDocByYearDi == TypeDocByYearDi.ConclusionRevisory);

                                if (finDocByYear == null)
                                {
                                    finDocByYear = new FinActivityDocByYear
                                    {
                                        ManagingOrganization = disclosureInfo.ManagingOrganization,
                                        Year = currentYear - 1,
                                        TypeDocByYearDi = TypeDocByYearDi.ConclusionRevisory
                                    };
                                }

                                finDocByYear.File = FileCreater.Create(importParams.zipName, section7Record.File, fileinfoService, fileManager);

                                if (finDocByYear.Id > 0)
                                {
                                    logImport.CountChangedRows++;
                                }
                                else
                                {
                                    logImport.CountAddedRows++;
                                }

                                listFinActivityDocByYear.Add(finDocByYear);

                                break;
                            }

                            case "Заключение ревизионной комиссии за 2 года предшествующих текущему году":
                            {
                                var finDocByYear = finDocsByYear
                                    .FirstOrDefault(x => x.Year == currentYear - 2
                                        && x.TypeDocByYearDi == TypeDocByYearDi.ConclusionRevisory);

                                if (finDocByYear == null)
                                {
                                    finDocByYear = new FinActivityDocByYear
                                    {
                                        ManagingOrganization = disclosureInfo.ManagingOrganization,
                                        Year = currentYear - 2,
                                        TypeDocByYearDi = TypeDocByYearDi.ConclusionRevisory
                                    };
                                }

                                finDocByYear.File = FileCreater.Create(importParams.zipName, section7Record.File, fileinfoService, fileManager);

                                if (finDocByYear.Id > 0)
                                {
                                    logImport.CountChangedRows++;
                                }
                                else
                                {
                                    logImport.CountAddedRows++;
                                }

                                listFinActivityDocByYear.Add(finDocByYear);

                                break;
                            }

                            case "Заключение аудиторской проверки за текущий год":
                            {
                                var finAudit =
                                    finAuditService.GetAll()
                                        .FirstOrDefault(
                                            x =>
                                                x.ManagingOrganization.Id
                                                == disclosureInfo.ManagingOrganization.Id
                                                && x.Year == currentYear);

                                if (finAudit == null)
                                {
                                    finAudit = new FinActivityAudit
                                    {
                                        ManagingOrganization =
                                            disclosureInfo.ManagingOrganization,
                                        Year = currentYear,
                                        TypeAuditStateDi = TypeAuditStateDi.NotSet
                                    };
                                }

                                finAudit.File = FileCreater.Create(importParams.zipName, section7Record.File, fileinfoService, fileManager);

                                if (finAudit.Id > 0)
                                {
                                    logImport.CountChangedRows++;
                                }
                                else
                                {
                                    logImport.CountAddedRows++;
                                }

                                listFinActivityAudit.Add(finAudit);

                                break;
                            }

                            case "Заключение аудиторской проверки за год предшествующий текущему":
                            {
                                var finAudit = finAuditDocs.FirstOrDefault(x => x.Year == currentYear - 1);

                                if (finAudit == null)
                                {
                                    finAudit = new FinActivityAudit
                                    {
                                        ManagingOrganization = disclosureInfo.ManagingOrganization,
                                        Year = currentYear - 1,
                                        TypeAuditStateDi = TypeAuditStateDi.NotSet
                                    };
                                }

                                finAudit.File = FileCreater.Create(importParams.zipName, section7Record.File, fileinfoService, fileManager);

                                if (finAudit.Id > 0)
                                {
                                    logImport.CountChangedRows++;
                                }
                                else
                                {
                                    logImport.CountAddedRows++;
                                }

                                listFinActivityAudit.Add(finAudit);

                                break;
                            }

                            case "Заключение аудиторской проверки за 2 года предшествующих текущему году":
                            {
                                var finAudit = finAuditDocs.FirstOrDefault(x => x.Year == currentYear - 2);

                                if (finAudit == null)
                                {
                                    finAudit = new FinActivityAudit
                                    {
                                        ManagingOrganization = disclosureInfo.ManagingOrganization,
                                        Year = currentYear - 2,
                                        TypeAuditStateDi = TypeAuditStateDi.NotSet
                                    };
                                }

                                finAudit.File = FileCreater.Create(importParams.zipName, section7Record.File, fileinfoService, fileManager);

                                if (finAudit.Id > 0)
                                {
                                    logImport.CountChangedRows++;
                                }
                                else
                                {
                                    logImport.CountAddedRows++;
                                }

                                listFinActivityAudit.Add(finAudit);

                                break;
                            }
                        }
                    }
                }

                var documentsRoServiceDict =
                    documentsRoService.GetAll()
                        .Where(x => realityObjIds.Contains(x.RealityObject.Id))
                        .GroupBy(x => x.RealityObject.Id)
                        .ToDictionary(x => x.Key);

                foreach (var section7Record in sectionsData.Section7)
                {
                    foreach (var realityObjId in realityObjIds)
                    {
                        switch (section7Record.DocumentType)
                        {
                            case "Протокол собрания № 1":
                            case "Протокол собрания № 2":
                            case "Протокол собрания № 3":
                            case "Протокол собрания № 4":
                            {
                                var documentsRo = documentsRoServiceDict.ContainsKey(realityObjId)
                                    ? documentsRoServiceDict[realityObjId].FirstOrDefault(x => x.Year == currentYear - 1)
                                    : null;

                                if (documentsRo == null)
                                {
                                    documentsRo = new DocumentsRealityObjProtocol
                                    {
                                        RealityObject = new RealityObject { Id = realityObjId },
                                        Year = currentYear - 1
                                    };

                                    documentsRoService.Save(documentsRo);
                                }

                                documentsRo.File = FileCreater.Create(importParams.zipName, section7Record.File, fileinfoService, fileManager);
                                if (documentsRo.Id > 0)
                                {
                                    logImport.CountChangedRows++;
                                }
                                else
                                {
                                    logImport.CountAddedRows++;
                                }

                                listDocumentsRealityObjProtocol.Add(documentsRo);

                                break;
                            }

                            case "Протокол собрания № 5":
                            case "Протокол собрания № 6":
                            case "Протокол собрания № 7":
                            case "Протокол собрания № 8":
                            {
                                var documentsRo = documentsRoServiceDict.ContainsKey(realityObjId)
                                    ? documentsRoServiceDict[realityObjId].FirstOrDefault(x => x.Year == currentYear)
                                    : null;

                                if (documentsRo == null)
                                {
                                    documentsRo = new DocumentsRealityObjProtocol
                                    {
                                        RealityObject = new RealityObject { Id = realityObjId },
                                        Year = currentYear
                                    };

                                    documentsRoService.Save(documentsRo);
                                }

                                documentsRo.File = FileCreater.Create(importParams.zipName, section7Record.File, fileinfoService, fileManager);
                                if (documentsRo.Id > 0)
                                {
                                    logImport.CountChangedRows++;
                                }
                                else
                                {
                                    logImport.CountAddedRows++;
                                }

                                documentsRoService.Update(documentsRo);

                                break;
                            }
                        }
                    }
                }

                this.InTransaction(listDocuments, documentsService);
                this.InTransaction(listFinActivityDocs, finDocsService);
                this.InTransaction(listFinActivityAudit, finAuditService);
                this.InTransaction(listFinActivityDocByYear, finDocByYearService);
                this.InTransaction(listDocumentsRealityObjProtocol, documentsRoService);

                listDocuments.Clear();
                listFinActivityDocs.Clear();
                listFinActivityAudit.Clear();
                listFinActivityDocByYear.Clear();
                listDocumentsRealityObjProtocol.Clear();
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
