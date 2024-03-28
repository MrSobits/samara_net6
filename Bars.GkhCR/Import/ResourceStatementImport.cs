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
    using Bars.Gkh.Enums;
    using Bars.Gkh.Enums.Import;
    using Bars.Gkh.Import;
    using Bars.GkhCr.Entities;
    using Bars.GkhExcel;

    using Castle.Windsor;
    using Gkh.Import.Impl;

    using Microsoft.Extensions.Logging;

    public class ResourceStatementImport : GkhImportBase
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        private ILogImport logImport;

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
            get { return "Импорт ведомостей ресурсов"; }
        }

        public override string PossibleFileExtensions
        {
            get { return "xls"; }
        }

        public override string PermissionName
        {
            get { return "Import.ResourceStatement.View"; }
        }

        public ResourceStatementImport(ILogImportManager logImportManager, ILogImport logImport)
        {
            this.LogImportManager = logImportManager;
            this.LogImport = logImport;
        }

        public override ImportResult Import(BaseParams baseParams)
        {
            var message = string.Empty;
            var fileData = baseParams.Files["FileImport"];

            var estimateCalculationId = baseParams.Params["EstimateCalculationId"].ToLong();
            var repResourceStatement = Container.Resolve<IDomainService<ResourceStatement>>();

            InitLog(fileData.FileName);

            var transaction = this.Container.Resolve<IDataTransaction>();
            using (this.Container.Using(transaction))
            {
                try
                {
                    var estimateCalculation = Container.Resolve<IDomainService<EstimateCalculation>>().Get(estimateCalculationId);
                    if (estimateCalculation == null)
                    {
                        LogImport.Error(Name, "Не удалось обнаружить сметный расчет по работе");
                        return new ImportResult(StatusImport.CompletedWithError, "Не удалось обнаружить сметный расчет по работе", string.Empty);
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

                            // Перед загрузкой удаляем показатели
                            var records = repResourceStatement.GetAll()
                                .Where(x => x.EstimateCalculation.Id == estimateCalculation.Id)
                                .Select(x => (object)x.Id)
                                .ToArray();

                            foreach (var rec in records)
                            {
                                repResourceStatement.Delete(rec);
                            }

                            var start = false;

                            foreach (var row in excel.GetRows(0, 0))
                            {
                                // Проверка на кол-во столбцов в файле (должно быть 7)
                                if (row.Length != 7)
                                {
                                    message = "Ошибка импорта - неверное количество столбцов. Ожидаемое количество столбцов - 7. ";
                                    LogImport.IsImported = false;
                                    LogImport.Error(Name, message);
                                    break;
                                }

                                // Считаем что достигли конца файла
                                if (start && string.IsNullOrEmpty(row[0].Value)
                                    && string.IsNullOrEmpty(row[1].Value)
                                    && string.IsNullOrEmpty(row[2].Value)
                                    && string.IsNullOrEmpty(row[3].Value)
                                    && string.IsNullOrEmpty(row[4].Value)
                                    && string.IsNullOrEmpty(row[5].Value)
                                    && string.IsNullOrEmpty(row[6].Value))
                                {
                                    break;
                                }

                                // Пропустим ненужные строки
                                if (string.IsNullOrEmpty(row[1].Value))
                                {
                                    continue;
                                }

                                // Ищем начало импортируемого блока данных
                                if (!start && row[0].Value == "1" && row[1].Value == "2" && row[2].Value == "3" && row[3].Value == "4"
                                    && row[4].Value == "5" && row[5].Value == "6" && row[6].Value == "7")
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

                                ImportRow(row, estimateCalculation, repResourceStatement);
                            }

                            memoryStreamFile.Close();

                            if (LogImport.CountImportedRows > 0)
                            {
                                LogImport.IsImported = true;
                                transaction.Commit();
                            }
                            else
                            {
                                LogImport.IsImported = false;
                                transaction.Rollback();
                            }
                        }
                    }
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
            }

            if (LogImport.IsImported && LogImport.CountImportedRows == 0)
            {
                LogImport.Info(Name, "Не удалось обнаружить записи для импорта");
                message = "Не удалось обнаружить записи для импорта";
            }

            LogImportManager.Add(fileData, LogImport);
            message += LogImportManager.GetInfo();
            LogImportManager.Save();
            
            var status = !LogImport.IsImported ? StatusImport.CompletedWithError : (LogImport.CountWarning > 0 ? StatusImport.CompletedWithWarning : StatusImport.CompletedWithoutError);
            return new ImportResult(status, message, string.Empty, LogImportManager.LogFileId);
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
                                || row[6].Value.Trim() == "7")
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

            if (string.IsNullOrEmpty(value) || (!string.IsNullOrEmpty(value) && value.Trim() == "-"))
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

            if (this.LogImport == null)
            {
                throw new Exception("Не найдена реализация интерфейса ILogImport");
            }

            this.LogImport.SetFileName(fileName);
            this.LogImport.ImportKey = Key;
        }

        private void ImportRow(
    GkhExcelCell[] row,
    EstimateCalculation estimateCalculation,
    IDomainService<ResourceStatement> repResourceStatement)
        {
            var number = row[0].Value != null ? row[0].Value.Trim() : string.Empty;
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

            var resourceStatement = new ResourceStatement
            {
                EstimateCalculation = estimateCalculation,
                Number = number,
                Name = name,
                UnitMeasure = importUnitMeasure,
                TotalCount = RemoveLineBreak(row[4].Value).To<decimal?>(),
                OnUnitCost = RemoveLineBreak(row[5].Value).To<decimal?>(),
                TotalCost = RemoveLineBreak(row[6].Value).To<decimal?>(),
                Reason = reason
            };

            repResourceStatement.Save(resourceStatement);
            LogImport.Info(Name, string.Format("Добавлена ведомость ресурсов {0}", resourceStatement.Name), LogTypeChanged.Added);
        }
    }
}
