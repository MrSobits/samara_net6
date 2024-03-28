namespace Bars.GkhGji.Regions.Voronezh.Controllers
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.Import;
    using Gkh.Helpers;
    using System.IO;
    using Bars.GkhExcel;
    using System.Collections.Generic;
    using Bars.GkhGji.Entities;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Enums.Import;
    using Bars.GkhGji.InspectionRules;
    using Bars.GkhGji.Enums;
    using Bars.Gkh.Enums;

    public class AmirsImportController : BaseController
    {

        public IDomainService<Protocol> ProtocolDomain { get; set; }

        public IDomainService<DocumentGji> DocumentGjiDomain { get; set; }

        public IDomainService<Resolution> ResolutionDomain { get; set; }

        public IDomainService<InspectionGjiStage> InspectionStageDomain { get; set; }

        public IDomainService<DocumentGjiChildren> ChildrenDomain { get; set; }



        public ILogImport LogImport { get; set; }

        public ILogImportManager LogManager { get; set; }

        public ILogImportManager LogImportManager { get; set; }

        public string CodeImport
        {
            get { return "AmirsImport"; }
        }

        public string Name
        {
            get { return "Импорт выписок из Росреестра"; }
        }

        public string PossibleFileExtensions
        {
            get { return "xksx"; }
        }

        private void InitLog(string fileName)
        {
            LogManager = Container.Resolve<ILogImportManager>();
            if (LogManager == null)
            {
                throw new Exception("Не найдена реализация интерфейса ILogImportManager");
            }
            LogImport.ImportKey = this.Name;
            LogManager.FileNameWithoutExtention = fileName;
            LogManager.UploadDate = DateTime.Now;
        }

        /// <summary>
        /// Метод для дебага импортов, просто в js правим урл импорта
        /// </summary>
        public ActionResult ImportNow(BaseParams baseParams)
        {
            var file = baseParams.Files["FileImport"];
           
            using (var excel = Container.Resolve<IGkhExcelProvider>("ExcelEngineProvider"))
            {
                if (excel == null)
                {
                    throw new Exception("Не найдена реализация интерфейса IGkhExcelProvider");
                }

                excel.UseVersionXlsx();

                using (var xlsMemoryStream = new MemoryStream(file.Data))
                {
                    excel.Open(xlsMemoryStream);
                    var data = excel.GetRows(0, 0);
                    var header = data[0].Select(x => x.Value.ToLower()).ToArray();
                    var dict = new Dictionary<string, string>();
                    for (var i = 1; i < data.Count(); i++)
                    {
                        var row = data[i];
                        dict.Clear();
                        for (var j = 0; j < row.Length; j++)
                        {
                            dict.Add(header[j], row[j].Value);
                        }

                        if (!dict.ContainsKey("prot_date"))
                        {
                            LogImport.Error(
                                "Постановление не добавлено",
                                string.Format("Для постановления в строке {0} не найдена дата протокола", i + 1));
                            continue;
                        }

                        if (!dict.ContainsKey("prot_num"))
                        {
                            LogImport.Error(
                                 "Постановление не добавлено",
                                string.Format("Для постановления в строке {0} не найден номер протокола", i + 1));
                            continue;
                        }

                        if (!dict.ContainsKey("resolution_type"))
                        {
                            LogImport.Error(
                                 "Постановление не добавлено",
                                string.Format("Для постановления в строке {0} не найден тип постановления", i + 1));
                            continue;
                        }

                        if (!dict.ContainsKey("resolution_date"))
                        {
                            LogImport.Error(
                                 "Постановление не добавлено",
                                string.Format("Для постановления в строке {0} не найдена дата постановления", i + 1));
                            continue;
                        }

                        if (!dict.ContainsKey("resolution_num"))
                        {
                            LogImport.Error(
                                 "Постановление не добавлено",
                                string.Format("Для постановления в строке {0} не найден номер постановления", i + 1));
                            continue;
                        }

                        string prot_num = dict["prot_num"];
                        DateTime prot_date = Convert.ToDateTime(dict["prot_date"]);

                        var currentProtocol = ProtocolDomain.GetAll()
                            .Where(x=> x.DocumentNumber == prot_num && x.DocumentDate.HasValue && x.DocumentDate.Value == prot_date).FirstOrDefault();

                        if (currentProtocol == null)
                        {
                            LogImport.Error(
                                 "Постановление не добавлено",
                                string.Format("Для постановления в строке {0} не найден протокол в системе", i + 1));
                            continue;
                        }
                        else
                        {
                           
                            //var ServiceProvider = Container.Resolve<IDocumentGjiRule >();                         
                            IDataResult createRez = this.CreateDocument(currentProtocol, dict);
                            if (createRez.Success == true)
                            {
                                LogImport.CountAddedRows += 1;
                            }
                            else
                            {
                                LogImport.Error(
                                "Постановление не добавлено",
                               string.Format("Для постановления в строке {0} импорт не удался " + createRez.Message, i + 1));
                                continue;
                            }
                            //ProtocolToResolutionRule protToRez = new ProtocolToResolutionRule();
                            //IDataResult createRez = protToRez.CreateDocument((DocumentGji)currentProtocol);
                        

                        }


                    }
                }
            }

            IDataTransaction transaction = null;


            var statusImport = LogImport.CountError > 0
                ? StatusImport.CompletedWithError
                : LogImport.CountWarning > 0
                    ? StatusImport.CompletedWithWarning
                    : StatusImport.CompletedWithoutError;
            LogImport.ImportKey = CodeImport;
            LogImportManager.FileNameWithoutExtention = file.FileName;
            LogImportManager.Add(file, LogImport);
            LogImportManager.Save();

            return new JsonNetResult( new
            {
                success = true,
                message = string.Format("Импортировано {0} записей", LogImport.CountAddedRows),
                title = string.Empty
            });
            //
            //var  actionResult = new JsonNetResult(new
            //{
            //    success = true,
            //    title = string.Empty,
            //    message = string.Format("Импорт завершился {0}", "1")
            //});
                  

            //    return actionResult;
    
        }

        public IDataResult CreateDocument(Protocol protocol, Dictionary<string, string> dict)
        {
            #region Формируем постановление
            SanctionGji sanction = this.Container.Resolve<IDomainService<SanctionGji>>().GetAll()
                 .Where(x => x.Name == dict["sanction"]).FirstOrDefault();

            if (protocol == null)
            {
                throw new Exception("Неудалось получить протокол");
            }

            var resolution = new Resolution()
            {
                Inspection = protocol.Inspection,
                TypeDocumentGji = TypeDocumentGji.Resolution,
                Contragent = protocol.Contragent,
                Executant = protocol.Executant,
                DocumentNumber = dict["resolution_num"],
                DocumentDate = Convert.ToDateTime(dict["resolution_date"]),
                PhysicalPerson = protocol.PhysicalPerson,
                PhysicalPersonInfo = protocol.PhysicalPersonInfo,
                SectorNumber = dict["judical_office"],
                TypeInitiativeOrg = TypeInitiativeOrgGji.Court,
                Sanction = sanction,
                PenaltyAmount = dict["penalty"] != ""? Convert.ToDecimal(dict["penalty"]):0,

                Paided = YesNoNotSet.NotSet,
                Description = protocol.Description
            };
            #endregion

            #region Формируем этап проверки
            // Если у родительского документа есть этап у которого есть родитель
            // тогда берем именно родительский Этап (Просто для красоты в дереве, чтобы не плодить дочерние узлы)
            var parentStage = protocol.Stage;
            if (parentStage != null && parentStage.Parent != null)
            {
                parentStage = parentStage.Parent;
            }

            InspectionGjiStage newStage = null;

            var currentStage = InspectionStageDomain.GetAll().FirstOrDefault(x => x.Parent == parentStage && x.TypeStage == TypeStage.Resolution);

            if (currentStage == null)
            {
                // Если этап ненайден, то создаем новый этап
                currentStage = new InspectionGjiStage
                {
                    Inspection = protocol.Inspection,
                    TypeStage = TypeStage.Resolution,
                    Parent = parentStage,
                    Position = 1
                };
                var stageMaxPosition = InspectionStageDomain.GetAll().Where(x => x.Inspection.Id == protocol.Inspection.Id)
                                     .OrderByDescending(x => x.Position).FirstOrDefault();

                if (stageMaxPosition != null)
                {
                    currentStage.Position = stageMaxPosition.Position + 1;
                }

                // Фиксируем новый этап чтобы потом незабыть сохранить 
                newStage = currentStage;
            }

            resolution.Stage = currentStage;
            #endregion

            #region формируем связь с родителем
            var parentChildren = new DocumentGjiChildren
            {
                Parent = protocol,
                Children = resolution
            };
            #endregion

            #region Сохранение
            using (var tr = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    if (newStage != null)
                    {
                        this.InspectionStageDomain.Save(newStage);
                    }

                    this.ResolutionDomain.Save(resolution);

                    this.ChildrenDomain.Save(parentChildren);

                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                    return new BaseDataResult(false, "ошибка транзакции БД");
                }
            }
            #endregion

            return new BaseDataResult(true, "создано постановление");
        }
    }
}
