namespace Bars.GkhCr.Import
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Enums.Import;
    using Bars.Gkh.Import;
    using Bars.GkhCr.Entities;

    using Castle.Windsor;
    using Gkh.Import.Impl;

    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Импорт смет в формате АРПС 1.10
    /// </summary>
    public class EstimateImportFromArps : GkhImportBase
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        private const string Numbers = "0123456789";

        public virtual IWindsorContainer Container { get; set; }

        public override string Key
        {
            get { return Id; }
        }

        public override string CodeImport
        {
            get { return "EstimateObjectCr"; }
        }

        public override string Name
        {
            get { return "Импорт смет в формате АРПС 1.10"; }
        }

        public override string PossibleFileExtensions
        {
            get { return "txt,arp"; }
        }

        public override string PermissionName
        {
            get { return "Import.EstimateARPS.View"; }
        }

        public EstimateImportFromArps(ILogImportManager logManager, ILogImport logImport)
        {
            this.LogImportManager = logManager;
            this.LogImport = logImport;
        }

        public override ImportResult Import(BaseParams baseParams)
        {
            var message = string.Empty;

            var fileData = baseParams.Files["FileImport"];
            var estimateCalculationId = baseParams.Params["EstimateCalculationId"].ToLong();
            var repEstimate = Container.Resolve<IDomainService<Estimate>>();
            var repEstimateCalculation = Container.Resolve<IDomainService<EstimateCalculation>>();
            var repResourceStatement = Container.Resolve<IDomainService<ResourceStatement>>();

            InitLog(fileData.FileName);

            try
            {
                var estimateCalculation = Container.Resolve<IDomainService<EstimateCalculation>>().Get(estimateCalculationId);
                if (estimateCalculation == null)
                {
                    this.LogImport.Error(Name, "Не найден сметный расчет по работе");
                    this.LogImportManager.Add(fileData, this.LogImport);
                    this.LogImportManager.Save();
                    return new ImportResult(StatusImport.CompletedWithError, "Не найден сметный расчет по работе");
                }

                using (var transaction = Container.Resolve<IDataTransaction>())
                {
                    try
                    {
                        // Перед загрузкой удаляем сметный расчет и ведомости ресурсов
                        var recsEstimate = repEstimate.GetAll()
                                                         .Where(x => x.EstimateCalculation.Id == estimateCalculation.Id)
                                                         .Select(x => (object)x.Id)
                                                         .ToArray();

                        var recsResourceStatement = repResourceStatement.GetAll()
                                                          .Where(x => x.EstimateCalculation.Id == estimateCalculation.Id)
                                                          .Select(x => (object)x.Id)
                                                          .ToArray();

                        foreach (var rec in recsEstimate)
                        {
                            repEstimate.Delete(rec);
                        }

                        foreach (var rec in recsResourceStatement)
                        {
                            repResourceStatement.Delete(rec);
                        }

                        var sections = FillSection(fileData.Data);
                        estimateCalculation.TotalEstimate = sections[3][0].Data[13].ToDecimal();
                        repEstimateCalculation.Update(estimateCalculation);

                        var countImportedSmeta = 0;
                        
                        // смета
                        foreach (var sect in sections[20])
                        {
                            var reason = sect.Data[2];
                            if (!string.IsNullOrEmpty(reason) && Numbers.Contains(reason.Substring(0, 1)) && reason.Contains("."))
                            {
                                reason = reason.Substring(reason.IndexOf(".", 1, StringComparison.Ordinal) + 1).Trim();
                            }

                            var importUnitMeasure = sect.Data[3];
                            var estimate = new Estimate
                                {
                                    Number = sect.Data[1],
                                    Name = sect.Data[4],
                                    OnUnitCost = sect.Data[15].ToDecimal(),
                                    BaseSalary = sect.Data[16].ToDecimal(),
                                    MachineOperatingCost = sect.Data[17].ToDecimal(),
                                    MechanicSalary = sect.Data[18].ToDecimal(),
                                    MaterialCost = sect.Data[19].ToDecimal(),
                                    BaseWork = sect.Data[23].ToDecimal(),
                                    MechanicWork = sect.Data[24].ToDecimal(),
                                    TotalCount = sect.Data[26].ToDecimal(),
                                    TotalCost = sect.Data[15].ToDecimal() * sect.Data[26].ToDecimal(),  // Количество всего * Стоимость на ед.
                                    EstimateCalculation = estimateCalculation,
                                    Reason = reason,
                                    UnitMeasure = importUnitMeasure
                                };

                            repEstimate.Save(estimate);

                            this.LogImport.CountAddedRows++;
                            countImportedSmeta++;
                        }

                        var countImportedResourceStatement = 0;
                        
                        // ведомости ресурсов
                        foreach (var sect in sections[30])
                        {
                            var importUnitMeasure = sect.Data[2];

                            var reason = sect.Data[1];
                            if (!string.IsNullOrEmpty(reason) && Numbers.Contains(reason.Substring(0, 1)) && reason.Contains("."))
                            {
                                reason = reason.Substring(reason.IndexOf(".", 1, StringComparison.Ordinal) + 1).Trim();
                            }

                            // ищем 20 секцию, которая  идет перед 30 секцией, чтобы получить Номер
                            var numberRowSmeta = sections[20].Where(x => x.Number < sect.Number).Max(x => x.Number);
                            var smeta = sections[20].FirstOrDefault(x => x.Number == numberRowSmeta);
                            if (smeta == null) continue;

                            var resourceStatement = new ResourceStatement
                                {
                                    EstimateCalculation = estimateCalculation,
                                    Number = smeta.Data[1],
                                    Name = sect.Data[3],
                                    UnitMeasure = importUnitMeasure,
                                    TotalCount = sect.Data[5].ToDecimal() * smeta.Data[26].ToDecimal(),
                                    OnUnitCost = sect.Data[7].ToDecimal(),
                                    TotalCost = (sect.Data[5].ToDecimal() * smeta.Data[26].ToDecimal()) * sect.Data[7].ToDecimal(),  // (Количество всего) * Стоимость на ед.
                                    Reason = reason
                                };

                            repResourceStatement.Save(resourceStatement);
                            countImportedResourceStatement++;
                            this.LogImport.CountAddedRows++;
                        }

                        transaction.Commit();

                        this.LogImport.Info(Name, string.Format("Добавлен итог по смете {0}", estimateCalculation.TotalEstimate), LogTypeChanged.Changed);
                        this.LogImport.Info(Name, string.Format("Количество добавленых позиций ведомости ресурсов={0}", countImportedResourceStatement));
                        this.LogImport.Info(Name, string.Format("Количество добавленых позиций сметы={0}", countImportedSmeta));
                        this.LogImport.IsImported = true;
                    }
                    catch (Exception exc)
                    {
                        try
                        {
                            this.LogImport.IsImported = false;
                            Container.Resolve<ILogger>().LogError(exc, "Импорт");
                            message = "Произошла неизвестная ошибка. Обратитесь к администратору.";
                            this.LogImport.Error(Name, "Произошла неизвестная ошибка при импорте файла. Обратитесь к администратору");

                            transaction.Rollback();
                        }
                        catch (Exception ex)
                        {
                            throw new Exception(ex.Message, exc);
                        }
                    }
                }

                if (this.LogImport.CountImportedRows == 0 && this.LogImport.CountError == 0)
                {
                    this.LogImport.IsImported = false;
                    this.LogImport.Warn(Name, "Не удалось обнаружить записи для импорта");
                    message = "Не удалось обнаружить записи для импорта";
                }
            }
            catch (Exception exp)
            {
                Container.Resolve<ILogger>().LogError(exp, "Импорт");

                this.LogImportManager.Add(fileData, this.LogImport);
                this.LogImportManager.Save();
                return new ImportResult(StatusImport.CompletedWithError, "Произошла неизвестная ошибка. Обратитесь к администратору.", string.Empty, this.LogImportManager.LogFileId);
            }

            this.LogImportManager.Add(fileData, this.LogImport);
            message += this.LogImportManager.GetInfo();
            this.LogImportManager.Save();

            var status = !this.LogImport.IsImported ? StatusImport.CompletedWithError : (LogImport.CountWarning > 0 ? StatusImport.CompletedWithWarning : StatusImport.CompletedWithoutError);
            return new ImportResult(status, message, string.Empty, this.LogImportManager.LogFileId);
        }

        public override bool Validate(BaseParams baseParams, out string message)
        {
            try
            {
                message = null;
                if (!baseParams.Files.ContainsKey("FileImport"))
                {
                    message = "Не выбран файл для импорта";
                    return false;
                }

                var bytes = baseParams.Files["FileImport"].Data;
                var extention = baseParams.Files["FileImport"].Extention;

                var fileExtentions = PossibleFileExtensions.Contains(",") ? PossibleFileExtensions.Split(',') : new[] { PossibleFileExtensions };
                if (fileExtentions.All(x => x != extention))
                {
                    message = string.Format("Необходимо выбрать файл с допустимым расширением: {0}", PossibleFileExtensions);
                    return false;
                }

                var sections = FillSection(bytes);
                if (!sections.ContainsKey(1) || (sections.ContainsKey(1) && sections[1].Count > 1))
                {
                    message = "Секция 1 имеет неверный формат";
                    return false;
                }

                if (!sections.ContainsKey(3) || (sections.ContainsKey(3) && sections[3].Count > 1))
                {
                    message = "Секция 3 имеет неверный формат";
                    return false;
                }

                if (!sections.ContainsKey(20))
                {
                    message = "Нет данных для импортирования";
                    return false;
                }

                return true;
            }
            catch (Exception exp)
            {
                Container.Resolve<ILogger>().LogError(exp, "Валидация файла импорта");
                message = "Произошла неизвестная ошибка при проверки формата файла";
                return false;
            }
        }

        private void InitLog(string fileName)
        {
            if (this.LogImportManager == null)
            {
                throw new Exception("Не найдена реализация интерфейса ILogImportManager");
            }

            this.LogImportManager.FileNameWithoutExtention = fileName;
            this.LogImportManager.UploadDate = DateTime.Now;

            if (this.LogImport == null)
            {
                throw new Exception("Не найдена реализация интерфейса ILogImport");
            }

            this.LogImport.SetFileName(fileName);
            this.LogImport.ImportKey = Key;
        }

        private static Dictionary<int, List<Section>> FillSection(byte[] file)
        {
            Dictionary<int, List<Section>> sections;
            using (var memStrFile = new MemoryStream(file))
            {
                sections = new Dictionary<int, List<Section>>();
                var numberRow = 0;
                using (var sr = new StreamReader(memStrFile, Encoding.GetEncoding(866)))
                {
                    while (true)
                    {
                        var row = sr.ReadLine();
                        numberRow++;

                        if (string.IsNullOrEmpty(row) || row.Length == 0 || !row.Contains("#")) break;
 
                        var section = new Section(row, numberRow);
                        if (sections.ContainsKey(section.Id))
                        {
                            sections[section.Id].Add(section);
                        }
                        else
                        {
                            sections.Add(section.Id, new List<Section> { section });
                        }
                    }
                }
            }

            return sections;
        }

        public sealed class Section
        {
            public Section(string row, int number)
            {
                var data = row.Split('#').Select(x => x.Trim()).ToArray();
                Id = data[0].ToInt();
                Data = data;

                switch (Id)
                {
                    case 0:
                        Valid = true;
                        break;
                    case 1:
                        this.Valid = this.Data.Length == 3 && this.Data[1] == "АРПС 1.10";
                        break;
                    case 3:
                        Valid = Data.Length == 20;
                        break;
                    case 20:
                        Valid = Data.Length == 29;
                        break;
                }

                Number = number;
            }

            public int Id { get; set; }

            public string[] Data { get; set; }

            public bool Valid { get; set; }

            public int Number { get; set; }
        }
    }
}
