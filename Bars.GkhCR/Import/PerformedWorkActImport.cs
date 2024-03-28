namespace Bars.GkhCr.Import
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums.Import;
    using Bars.Gkh.Import;
    using Bars.GkhCr.Entities;
    using Bars.GkhExcel;

    using Castle.Windsor;
    using Gkh.Import.Impl;

    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Импорт актов выполненных работ
    /// </summary>
    public class PerformedWorkActImport : GkhImportBase
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public virtual IWindsorContainer Container { get; set; }

        public override string Key
        {
            get { return Id; }
        }

        public override string CodeImport
        {
            get { return "PerformedWorkAct"; }
        }

        public override string Name
        {
            get { return "Импорт актов выполненных работ"; }
        }

        public override string PossibleFileExtensions
        {
            get { return "xls"; }
        }

        public override string PermissionName
        {
            get { return "Import.PerformedWorkAct.View"; }
        }

        public PerformedWorkActImport(ILogImportManager logImportManager, ILogImport logImport)
        {
            this.LogImportManager = logImportManager;
            this.LogImport = logImport;
        }

        public override ImportResult Import(BaseParams baseParams)
        {
            var message = string.Empty;

            var fileData = baseParams.Files["FileImport"];
            var performedWorkActId = baseParams.Params["PerformedWorkActId"].ToLong();

            InitLog(fileData.FileName);

            var performedWorkAct = Container.Resolve<IDomainService<PerformedWorkAct>>().Get(performedWorkActId);
            if (performedWorkAct == null)
            {
                LogImport.Error(Name, "Не найден акт выполненных работ");
                LogImport.IsImported = false;

                this.LogImportManager.Add(fileData, LogImport);
                this.LogImportManager.Save();
                message = "Не найден акт выполненных работ";
                return new ImportResult(StatusImport.CompletedWithError, message, string.Empty, this.LogImportManager.LogFileId);
            }

            var excel = this.Container.Resolve<IGkhExcelProvider>("ExcelEngineProvider");
            using (this.Container.Using(excel))
            {
                if (excel == null)
                {
                    throw new Exception("Не найдена реализация интерфейса IGkhExcelProvider");
                }

                using (var memoryStreamFile = new MemoryStream(fileData.Data))
                {
                    excel.Open(memoryStreamFile);

                    var transaction = this.Container.Resolve<IDataTransaction>();
                    using (this.Container.Using(transaction))
                    {
                        var servPerformedWorkActRecord = Container.Resolve<IDomainService<PerformedWorkActRecord>>();
                        try
                        {
                            // Перед загрузкой удаляем показатели
                            var records = servPerformedWorkActRecord.GetAll()
                                                         .Where(x => x.PerformedWorkAct.Id == performedWorkAct.Id)
                                                         .Select(x => (object)x.Id)
                                                         .ToArray();

                            foreach (var rec in records)
                            {
                                servPerformedWorkActRecord.Delete(rec);
                            }

                            var start = false;
                            foreach (var row in excel.GetRows(0, 0))
                            {
                                // Считаем что достигли конца файла
                                if (start && row[0].IsMerged && row[0].Value != null
                                    && row[0].Value.ToLower().StartsWith("итого"))
                                {
                                    break;
                                }

                                // Ищем начало импортируемого блока данных
                                if (!start && row[0].Value == "1")
                                {
                                    start = true;
                                    continue;
                                }

                                // Пропускаем объединенные строки
                                if (row[0].IsMerged)
                                {
                                    continue;
                                }

                                if (!start)
                                {
                                    continue;
                                }

                                // Пропускаем
                                var number = row[0].Value != null ? row[0].Value.Trim() : string.Empty;
                                if (number.ToLower().Contains("уд"))
                                {
                                    continue;
                                }

                                if (row.Length < 14)
                                {
                                    continue;
                                }

                                ImportRow(row, performedWorkAct, number, servPerformedWorkActRecord);
                            }

                            transaction.Commit();
                            LogImport.IsImported = true;
                        }
                        catch (Exception exc)
                        {
                            try
                            {
                                LogImport.IsImported = false;
                                Container.Resolve<ILogger>().LogError(exc, "Импорт");
                                message = "Произошла неизвестная ошибка";
                                LogImport.Error(Name, "Произошла неизвестная ошибка. Обратитесь к администратору");
                                transaction.Rollback();
                            }
                            catch (Exception ex)
                            {
                                throw new Exception(ex.Message, exc);
                            }
                        }
                        finally
                        {
                            Container.Release(servPerformedWorkActRecord);
                        }
                    }
                }
            }

            if (LogImport.IsImported && LogImport.CountImportedRows == 0)
            {
                LogImport.IsImported = false;
                LogImport.Info(Name, "Не удалось обнаружить записи для импорта");
                message = "Не удалось обнаружить записи для импорта";
            }

            this.LogImportManager.Add(fileData, LogImport);
            message += this.LogImportManager.GetInfo();
            this.LogImportManager.Save();

            var status = !LogImport.IsImported ? StatusImport.CompletedWithError : (LogImport.CountWarning > 0 ? StatusImport.CompletedWithWarning : StatusImport.CompletedWithoutError);
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

                var excel = this.Container.Resolve<IGkhExcelProvider>("ExcelEngineProvider");
                using (this.Container.Using(excel))
                {
                    if (excel == null)
                    {
                        throw new Exception("Не найдена реализация интерфейса IGkhExcelProvider");
                    }

                    using (var memoryStreamFile = new MemoryStream(bytes))
                    {
                        excel.Open(memoryStreamFile);

                        if (excel.IsEmpty(0, 0))
                        {
                            message = string.Format("Не удалось обнаружить записи в файле: {0}", PossibleFileExtensions);
                            return false;
                        }

                        var isNotValid = true;
                        foreach (var row in excel.GetRows(0, 0))
                        {
                            if (row[0].Value.Trim() == "1" || row[1].Value.Trim() == "2"
                                || row[2].Value.Trim() == "3" || row[3].Value.Trim() == "4"
                                || row[4].Value.Trim() == "5" || row[5].Value.Trim() == "6"
                                || row[4].Value.Trim() == "7" || row[5].Value.Trim() == "8"
                                || row[4].Value.Trim() == "9" || row[5].Value.Trim() == "10"
                                || row[4].Value.Trim() == "11" || row[5].Value.Trim() == "12"
                                || row[4].Value.Trim() == "13" || row[5].Value.Trim() == "14")
                            {
                                isNotValid = false;
                                break;
                            }
                        }

                        if (isNotValid)
                        {
                            message = "Файл не соответствует шаблону";
                            return false;
                        }

                        return true;
                    }
                }
            }
            catch (Exception exp)
            {
                Container.Resolve<ILogger>().LogError(exp, "Валидация файла импорта");
                message = "Произошла неизвестная ошибка при проверки формата файла";
                return false;
            }
        }

        private static string RemoveLineBreak(string value)
        {
            if (!string.IsNullOrEmpty(value) && value.Contains("\n"))
            {
                value = value.Remove(value.IndexOf("\n", StringComparison.Ordinal));
            }

            if (!string.IsNullOrEmpty(value) && value.Trim() == "-")
            {
                value = "0";
            }

            return value;
        }

        private void InitLog(string fileName)
        {
            if (this.LogImportManager == null)
            {
                throw new Exception("Не найдена реализация интерфейса ILogImportManager");
            }

            this.LogImportManager.FileNameWithoutExtention = fileName;
            this.LogImportManager.UploadDate = DateTime.Now;

            if (LogImport == null)
            {
                throw new Exception("Не найдена реализация интерфейса ILogImport");
            }

            this.LogImport.SetFileName(fileName);
            this.LogImport.ImportKey = Key;
        }

        private void ImportRow(
            GkhExcelCell[] row,
            PerformedWorkAct performedWorkAct,
            string number,
            IDomainService<PerformedWorkActRecord> repPerformedWorkActRecord)
        {
            var importUnitMeasure = RemoveLineBreak(row[3].Value.Trim());

            if (number.Length > 50)
            {
                number = number.Substring(0, 50);
            }

            var name = row[2].Value.Trim();
            if (name.Length > 300)
            {
                name = name.Substring(0, 50);
            }

            var reason = row[1].Value;
            if (!string.IsNullOrEmpty(reason) && "0123456789".Contains(reason.Substring(0, 1)) && reason.Contains("."))
            {
                reason = reason.Substring(reason.IndexOf(".", 1, StringComparison.Ordinal) + 1).Trim();
            }

            if (reason.Length > 300)
            {
                reason = reason.Substring(0, 50);
            }
    
            var performedWorkActRecord = new PerformedWorkActRecord
                {
                    PerformedWorkAct = performedWorkAct,
                    Number = number,
                    Name = name,
                    UnitMeasure = importUnitMeasure,
                    OnUnitCount = RemoveLineBreak(row[4].Value).To<decimal?>(),
                    TotalCount = RemoveLineBreak(row[5].Value).To<decimal?>(),
                    OnUnitCost = RemoveLineBreak(row[6].Value).To<decimal?>(),
                    TotalCost = RemoveLineBreak(row[7].Value).To<decimal?>(),
                    BaseSalary = RemoveLineBreak(row[8].Value).To<decimal?>(),
                    MachineOperatingCost = RemoveLineBreak(row[9].Value).To<decimal?>(),
                    MechanicSalary = RemoveLineBreak(row[10].Value).To<decimal?>(),
                    MaterialCost = RemoveLineBreak(row[11].Value).To<decimal?>(),
                    BaseWork = RemoveLineBreak(row[12].Value).To<decimal?>(),
                    MechanicWork = RemoveLineBreak(row[13].Value).To<decimal?>(),
                    Reason = reason
                };

            repPerformedWorkActRecord.Save(performedWorkActRecord);
            LogImport.CountAddedRows++;
        }
    }
}
