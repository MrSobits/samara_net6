namespace Bars.GkhCr.Services.Impl
{
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Services.DataContracts;
    using Bars.GkhCr.Services.DataContracts.GetCrDocuments;

    public partial class Service
    {


        /// <summary>
        /// Получить документы объекта КР
        /// </summary>
        /// <param name="houseId"></param>
        /// <param name="programId"></param>
        /// <returns></returns>
        public CrDocumentsResponse GetCrDocuments(string houseId, string programId)
        {
            var roId = houseId.ToLong();
            var prId = programId.ToLong();

            var objectCrDomain = this.Container.ResolveDomain<ObjectCr>();
            var protocolCrDomain = this.Container.ResolveDomain<ProtocolCr>();
            var defectListDomain = this.Container.ResolveDomain<DefectList>();
            var desingAssignmentDomain = this.Container.ResolveDomain<DesignAssignment>();
            var estimateCalculationDomain = this.Container.ResolveDomain<EstimateCalculation>();
            var buildContractDomain = this.Container.ResolveDomain<BuildContract>();
            var contractCrDomain = this.Container.ResolveDomain<ContractCr>();
            var documentWorkCrDomain = this.Container.ResolveDomain<DocumentWorkCr>();
            var performedActWorkActDomain = this.Container.ResolveDomain<PerformedWorkAct>();
            var fileManager = this.Container.Resolve<IFileManager>();

            List<Section> sections = new List<Section>();

            try
            {
                var crObjId = objectCrDomain.GetAll()
                    .Where(x => x.RealityObject.Id == roId)
                    .Where(x => x.ProgramCr.Id == prId)
                    .Where(x => x.ProgramCr.UsedInExport)
                    .Select(x => x.Id)
                    .FirstOrDefault();

                if (crObjId == 0)
                {
                    return new CrDocumentsResponse { Result = Result.DataNotFound };
                }

                // Протоколы, акты
                var protocolCrFiles = protocolCrDomain.GetAll()
                    .Where(x => x.ObjectCr.Id == crObjId)
                    .Where(x => x.UsedInExport == YesNo.Yes)
                    .Where(x => x.File != null)
                    .Select(
                        x => new Document
                        {
                            DocumentId = x.Id,
                            TypeWorkCrId = 0,
                            Id = x.File.Id,
                            File = fileManager.GetBase64String(x.File),
                            Name = x.File.Name,
                            Extention = x.File.Extention
                        })
                        .AsEnumerable()
                        .Where(x => IsValidFileExtension(x.Extention))
                        .ToArray();

                if (protocolCrFiles.Length != 0)
                {
                    sections.Add(
                    new Section
                    {
                        Id = 1,
                        Name = "Протоколы, акты",
                        Documents = protocolCrFiles
                    });
                }

                // Дефектные ведомости
                var defectListFiles = defectListDomain.GetAll()
                    .Where(x => x.ObjectCr.Id == crObjId)
                    .Where(x => x.State.FinalState)
                    .Where(x => x.UsedInExport == YesNo.Yes)
                    .Where(x => x.File != null)
                    .Select(
                        x => new Document
                        {
                            DocumentId = x.Id,
                            TypeWorkCrId = x.TypeWork.Id,
                            Id = x.File.Id,
                            File = fileManager.GetBase64String(x.File),
                            Name = x.File.Name,
                            Extention = x.File.Extention
                        }).AsEnumerable()
                        .Where(x => IsValidFileExtension(x.Extention))
                        .ToArray();

                if (defectListFiles.Length != 0)
                {
                    sections.Add(new Section
                    {
                        Id = 2,
                        Name = "Дефектные ведомости",
                        Documents = defectListFiles
                    });
                }

                // Задание на проектирование
                var desingAssignmentFiles = desingAssignmentDomain.GetAll()
                    .Where(x => x.ObjectCr.Id == crObjId)
                    .Where(x => x.UsedInExport == YesNo.Yes)
                    .Where(x => x.State.FinalState)
                    .Where(x => x.DocumentFile != null)
                    .Select(
                        x => new Document
                        {
                            DocumentId = x.Id,
                            Id = x.DocumentFile.Id,
                            File = fileManager.GetBase64String(x.DocumentFile),
                            Name = x.DocumentFile.Name,
                            Extention = x.DocumentFile.Extention
                        }).AsEnumerable()
                        .Where(x => IsValidFileExtension(x.Extention))
                        .ToArray();

                if (desingAssignmentFiles.Length != 0)
                {
                    sections.Add(new Section
                    {
                        Id = 3,
                        Name = "Задание на проектирование",
                        Documents = desingAssignmentFiles
                    });
                }

                // Сметный расчет по работе
                var estimateCalculationAllFiles = estimateCalculationDomain.GetAll()
                    .Where(x => x.ObjectCr.Id == crObjId)
                    .Where(x => x.State.FinalState)
                    .Where(x => x.UsedInExport == YesNo.Yes)
                    .Select(
                        x => new
                        {
                            DocumentId = x.Id,
                            TypeWorkCrId = x.TypeWorkCr.Id,
                            x.ResourceStatmentFile,
                            x.EstimateFile,
                            x.FileEstimateFile
                        }).ToList();

                List<Document> estimateCalculationFiles = new List<Document>();

                foreach (var file in estimateCalculationAllFiles)
                {
                    if (file.ResourceStatmentFile.IsNotNull() && IsValidFileExtension(file.ResourceStatmentFile.Extention))
                    {
                        estimateCalculationFiles.Add(
                            new Document
                            {
                                DocumentId = file.DocumentId,
                                TypeWorkCrId = file.TypeWorkCrId,
                                Id = file.ResourceStatmentFile.Id,
                                File = fileManager.GetBase64String(file.ResourceStatmentFile),
                                Name = file.ResourceStatmentFile.Name,
                                Extention = file.ResourceStatmentFile.Extention
                            });
                    }

                    if (file.EstimateFile.IsNotNull() && IsValidFileExtension(file.EstimateFile.Extention))
                    {
                        estimateCalculationFiles.Add(new Document
                        {
                            DocumentId = file.DocumentId,
                            TypeWorkCrId = file.TypeWorkCrId,
                            Id = file.EstimateFile.Id,
                            File = fileManager.GetBase64String(file.EstimateFile),
                            Name = file.EstimateFile.Name,
                            Extention = file.EstimateFile.Extention
                        });
                    }

                    if (file.FileEstimateFile.IsNotNull() && IsValidFileExtension(file.FileEstimateFile.Extention))
                    {
                        estimateCalculationFiles.Add(new Document
                        {
                            DocumentId = file.DocumentId,
                            TypeWorkCrId = file.TypeWorkCrId,
                            Id = file.FileEstimateFile.Id,
                            File = fileManager.GetBase64String(file.FileEstimateFile),
                            Name = file.FileEstimateFile.Name,
                            Extention = file.FileEstimateFile.Extention
                        });
                    }
                }

                if (estimateCalculationFiles.Count != 0)
                {
                    sections.Add(new Section
                    {
                        Id = 4,
                        Name = "Сметный расчет по работе",
                        Documents = estimateCalculationFiles.ToArray()
                    });
                }

                // Договоры подряда
                var buildContractAllFiles = buildContractDomain.GetAll()
                    .Where(x => x.ObjectCr.Id == crObjId)
                    .Where(x => x.State.FinalState)
                    .Where(x => x.UsedInExport == YesNo.Yes)
                    .Select(
                        x => new
                        {
                            DocumentId = x.Id,
                            TypeWorkCrId = x.TypeWork.Id,
                            x.DocumentFile,
                            x.ProtocolFile
                        }).ToList();

                List<Document> buildContractFiles = new List<Document>();

                foreach (var file in buildContractAllFiles)
                {
                    if (file.DocumentFile.IsNotNull() && IsValidFileExtension(file.DocumentFile.Extention))
                    {
                        buildContractFiles.Add(new Document
                        {
                            DocumentId = file.DocumentId,
                            TypeWorkCrId = file.TypeWorkCrId,
                            Id = file.DocumentFile.Id,
                            File = fileManager.GetBase64String(file.DocumentFile),
                            Name = file.DocumentFile.Name,
                            Extention = file.DocumentFile.Extention
                        });
                    }

                    if (file.ProtocolFile.IsNotNull() && IsValidFileExtension(file.ProtocolFile.Extention))
                    {
                        buildContractFiles.Add(new Document
                        {
                            DocumentId = file.DocumentId,
                            TypeWorkCrId = file.TypeWorkCrId,
                            Id = file.ProtocolFile.Id,
                            File = fileManager.GetBase64String(file.ProtocolFile),
                            Name = file.ProtocolFile.Name,
                            Extention = file.ProtocolFile.Extention
                        });
                    }
                }

                if (buildContractFiles.Count != 0)
                {
                    sections.Add(new Section
                    {
                        Id = 5,
                        Name = "Договор подряда",
                        Documents = buildContractFiles.ToArray()
                    });
                }

                // Договор на услуги
                var contractCrFiles = contractCrDomain.GetAll()
                    .Where(x => x.ObjectCr.Id == crObjId)
                    .Where(x => x.UsedInExport == YesNo.Yes)
                    .Where(x => x.State.FinalState)
                    .Where(x => x.File != null)
                    .Select(
                        x => new Document
                        {
                            DocumentId = x.Id,
                            TypeWorkCrId = x.TypeWork.Id,
                            Id = x.File.Id,
                            File = fileManager.GetBase64String(x.File),
                            Name = x.File.Name,
                            Extention = x.File.Extention
                        }).AsEnumerable()
                        .Where(x => IsValidFileExtension(x.Extention))
                        .ToArray();

                if (contractCrFiles.Length != 0)
                {
                    sections.Add(new Section
                    {
                        Id = 6,
                        Name = "Договор на услуги",
                        Documents = contractCrFiles
                    });
                }

                // Документы (Мониторинг СМР)
                var documentWorkCrFiles = documentWorkCrDomain.GetAll()
                    .Where(x => x.ObjectCr.Id == crObjId)
                    .Where(x => x.UsedInExport == YesNo.Yes)
                    .Where(x => x.File != null)
                    .Select(
                        x => new Document
                        {
                            DocumentId = x.Id,
                            TypeWorkCrId = x.TypeWork.Id,
                            Id = x.File.Id,
                            File = fileManager.GetBase64String(x.File),
                            Name = x.File.Name,
                            Extention = x.File.Extention
                        }).AsEnumerable()
                        .Where(x => IsValidFileExtension(x.Extention))
                        .ToArray();

                if (documentWorkCrFiles.Length != 0)
                {
                    sections.Add(new Section
                    {
                        Id = 7,
                        Name = "Документы (Мониторинг СМР)",
                        Documents = documentWorkCrFiles
                    });
                }

                // Акты выполненных работ
                var performedActWorkActAllFiles = performedActWorkActDomain.GetAll()
                    .Where(x => x.ObjectCr.Id == crObjId)
                    .Where(x => x.State.FinalState)
                    .Where(x => x.UsedInExport == YesNo.Yes)
                    .Select(
                        x => new
                        {
                            DocumentId = x.Id,
                            TypeWorkCrId = x.TypeWorkCr.Id,
                            x.DocumentFile,
                            x.CostFile,
                            x.AdditionFile
                        }).ToList();

                List<Document> performedActWorkActFiles = new List<Document>();

                foreach (var file in performedActWorkActAllFiles)
                {
                    if (file.DocumentFile.IsNotNull())
                    {
                        performedActWorkActFiles.Add(
                            new Document
                            {
                                DocumentId = file.DocumentId,
                                TypeWorkCrId = file.TypeWorkCrId,
                                DocumentName = "DocumentFile",
                                Id = file.DocumentFile.Id,
                                File = fileManager.GetBase64String(file.DocumentFile),
                                Name = file.DocumentFile.Name,
                                Extention = file.DocumentFile.Extention
                            });
                    }

                    if (file.CostFile.IsNotNull())
                    {
                        performedActWorkActFiles.Add(
                            new Document
                            {
                                DocumentId = file.DocumentId,
                                TypeWorkCrId = file.TypeWorkCrId,
                                DocumentName = "CostFile",
                                Id = file.CostFile.Id,
                                File = fileManager.GetBase64String(file.CostFile),
                                Name = file.CostFile.Name,
                                Extention = file.CostFile.Extention
                            });
                    }

                    if (file.AdditionFile.IsNotNull())
                    {
                        performedActWorkActFiles.Add(
                            new Document
                            {
                                DocumentId = file.DocumentId,
                                TypeWorkCrId = file.TypeWorkCrId,
                                DocumentName = "AdditionFile",
                                Id = file.AdditionFile.Id,
                                File = fileManager.GetBase64String(file.AdditionFile),
                                Name = file.AdditionFile.Name,
                                Extention = file.AdditionFile.Extention
                            });
                    }
                }

                if (performedActWorkActFiles.Count != 0)
                {
                    sections.Add(new Section
                    {
                        Id = 8,
                        Name = "Акты выполненных работ",
                        Documents = performedActWorkActFiles.ToArray()
                    });
                }

                return sections.Count > 0
                    ? new CrDocumentsResponse { Sections = sections.ToArray(), Result = Result.NoErrors }
                    : new CrDocumentsResponse { Result = Result.DataNotFound };
            }
            finally
            {
                this.Container.Release(objectCrDomain);
                this.Container.Release(protocolCrDomain);
                this.Container.Release(defectListDomain);
                this.Container.Release(desingAssignmentDomain);
                this.Container.Release(estimateCalculationDomain);
                this.Container.Release(buildContractDomain);
                this.Container.Release(contractCrDomain);
                this.Container.Release(documentWorkCrDomain);
                this.Container.Release(performedActWorkActDomain);
                this.Container.Release(fileManager);
            }
        }

        /// <summary>
        /// Проверка расширения файла
        /// </summary>
        /// <param name="extension"></param>
        private static bool IsValidFileExtension(string extension)
        {
            string[] format = new[]
            {"bmp",
            "cpt",
            "gif",
            "hdr",
            "jpeg",
            "jpg",
            "jpe",
            "jp2",
            "pcx",
            "pdf",
            "pdn",
            "png",
            "psd",
            "raw",
            "tga",
            "tpic",
            "tiff",
            "tif",
            "wdp",
            "hdp",
            "xpm"
            };
            return format.Any(ext => extension.ToLower() == ext);
        }
    }
}