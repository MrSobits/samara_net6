namespace Bars.GkhCr.Import
{
    using System;
    using System.Collections.Generic;
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
    using Bars.Gkh.ConfigSections.Cr;
    using Bars.Gkh.ConfigSections.Cr.Enums;
    using Bars.Gkh.Utils;
    
    using Gkh.Entities;
    using Gkh.Entities.Dicts;
    using Gkh.Import.Impl;

    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Импорт средств источников финансирования
    /// </summary>
    public class FinanceSourceImport : GkhImportBase
    {
        /// <summary>
        /// Идентификатор импорта
        /// </summary>
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;
        
        /// <summary>
        /// Ключ импорта
        /// </summary>
        public override string Key
        {
            get { return FinanceSourceImport.Id; }
        }

        /// <summary>
        /// Код импорта
        /// </summary>
        public override string CodeImport
        {
            get { return "ObjectCr"; }
        }

        /// <summary>
        /// Наименование импорта
        /// </summary>
        public override string Name
        {
            get { return "Импорт средств источников финансирования"; }
        }

        /// <summary>
        /// Доступные расширения файла
        /// </summary>
        public override string PossibleFileExtensions
        {
            get { return "xls"; }
        }

        /// <summary>
        /// Наименование разрешения
        /// </summary>
        public override string PermissionName
        {
            get { return "Import.FinanceSource.View"; }
        }

        /// <summary>
        /// Домен объектов КР
        /// </summary>
        public IDomainService<ObjectCr> ObjectCrDomain { get; set; }

        /// <summary>
        /// Домен разрезов финансирования программы КР
        /// </summary>
        public IDomainService<ProgramCrFinSource> ProgramCrFinSourceDomain { get; set; }

        /// <summary>
        /// Домен разрезов финансирования по КР
        /// </summary>
        public IDomainService<FinanceSource> FinanceSourceDomain { get; set; }

        /// <summary>
        /// Домен средств источника финансирования
        /// </summary>
        public IDomainService<FinanceSourceResource> NsoFinanceSourceResourceDomain { get; set; }

        /// <summary>
        /// Домен работ
        /// </summary>
        public IDomainService<Work> WorkDomain { get; set; }

        /// <summary>
        /// Домен типов работ КР
        /// </summary>
        public IDomainService<TypeWorkCr> TypeWorkCrDomain { get; set; }

        /// <summary>
        /// Репозиторий муниципальных образований
        /// </summary>
        public IRepository<Municipality> MunicipalityDomain { get; set; }

        private List<Municipality> municipalities;
        private Dictionary<long, Dictionary<string, ObjectCr>> objCrDict;
        private Dictionary<string, FinanceSource> finSourceDict;
        private Dictionary<long, List<FinanceSourceResource>> finSourceResDict;
        private Dictionary<string, Work> workDict;
        private Dictionary<long, List<TypeWorkCr>> typeWorkByRoDict;

        /// <summary>
        /// Импорт данных
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат импорта</returns>
        public override ImportResult Import(BaseParams baseParams)
        {
            var message = string.Empty;
            var fileData = baseParams.Files["FileImport"];
            var programId = baseParams.Params["ProgramCr"].ToInt();
            var formFinanceSource = this.Container.GetGkhConfig<GkhCrConfig>().General.FormFinanceSource;

            this.InitLog(fileData.FileName);
            this.InitDictionaries(programId);

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
                    using (var transaction = this.Container.Resolve<IDataTransaction>())
                    {
                        try
                        {
                            // если "Без учета вида работ" то в файле должно быть 7 столбцов(нет столбца вид работы)
                            var countCells = formFinanceSource == FormFinanceSource.WithTypeWork ? 8 : 7;

                            var data = excel.GetRows(0, 0)
                                .Where(x => x.Length >= countCells && !x[0].FontBold && !string.IsNullOrEmpty(x[1].Value))
                                .ToArray();
                            foreach (var row in data)
                            {
                                this.ImportRow(row, formFinanceSource);
                            }

                            transaction.Commit();
                            this.LogImport.IsImported = true;
                        }
                        catch (Exception exp)
                        {
                            try
                            {
                                this.LogImport.IsImported = false;
                                this.Container.Resolve<ILogger>().LogError(exp, "Импорт");
                                message = "Произошла неизвестная ошибка.";
                                this.LogImport.Error(this.Name, "Произошла неизвестная ошибка. Обратитесь к администратору");
                                transaction.Rollback();
                            }
                            catch (Exception ex)
                            {
                                throw new Exception(ex.Message, exp);
                            }
                        }
                    }
                }
            }

            this.LogImportManager.Add(fileData, this.LogImport);
            message += this.LogImportManager.GetInfo();
            this.LogImportManager.Save();

            var status = !this.LogImport.IsImported
                ? StatusImport.CompletedWithError
                : (this.LogImport.CountWarning > 0 ? StatusImport.CompletedWithWarning : StatusImport.CompletedWithoutError);

            return new ImportResult(status, message, string.Empty, this.LogImportManager.LogFileId);
        }

        /// <summary>
        /// Валидация входного файла
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <param name="message">Сообщение об ошибке</param>
        /// <returns>Признак успеха валидации</returns>
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

                var fileExtentions = this.PossibleFileExtensions.Contains(",")
                    ? this.PossibleFileExtensions.Split(',')
                    : new[] {this.PossibleFileExtensions};
                if (fileExtentions.All(x => x != extention))
                {
                    message = string.Format("Необходимо выбрать файл с допустимым расширением: {0}", this.PossibleFileExtensions);
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
                            message = string.Format("Не удалось обнаружить записи в файле: {0}", this.PossibleFileExtensions);
                            return false;
                        }
                        
                        var formFinanceSource = this.Container.GetGkhConfig<GkhCrConfig>().General.FormFinanceSource;
                        var expectedColumns = new List<string>();

                        expectedColumns.Add("Район");
                        expectedColumns.Add("Адрес");

                        if (formFinanceSource == FormFinanceSource.WithTypeWork)
                        {
                            expectedColumns.Add("Вид работ");
                            expectedColumns.Add("Год");
                        }
                        else
                        {
                            expectedColumns.Add("Источник финансирования");
                        }

                        expectedColumns.Add("Средства фонда");
                        expectedColumns.Add("Средства субъекта");
                        expectedColumns.Add("Средства МО");
                        expectedColumns.Add("Средства собственников");

                        var title = excel.GetRow(0, 0, 0);

                        if (title.Length < expectedColumns.Count)
                        {
                            message =
                                "Количество столбцов в файле не совпадает с шаблоном. "
                                + "Ожидаемое количество столбцов: " + expectedColumns.Count;

                            return false;
                        }

                        for (int i = 0; i < title.Length; ++i)
                        {
                            if (title[i].Value.Trim() != expectedColumns[i])
                            {
                                message =
                                    "Нет в наличии столбца \"" + expectedColumns[i] + "\". "
                                    + "Порядковый номер столбца: " + (i + 1);

                                break;
                            }
                        }

                        if (!string.IsNullOrEmpty(message))
                        {
                            return false;
                        }
                    }
                }

                return true;
            }
            catch (Exception exp)
            {
                this.Container.Resolve<ILogger>().LogError(exp, "Валидация файла импорта");
                message = "Произошла неизвестная ошибка при проверке формата файла";
                return false;
            }
        }

        private void ImportRow(
            GkhExcelCell[] row,
            FormFinanceSource formFinanceSource)
        {
            var region = row[0].Value;
            var address = row[1].Value;

            var workName = string.Empty;
            var year = 0;
            var sourceFinancing = string.Empty;
            decimal? fundResource = null;
            decimal? budgetSubject = null;
            decimal? budgetMu = null;
            decimal? ownerResource = null;

            if (formFinanceSource == FormFinanceSource.WithTypeWork)
            {
                workName = row[2].Value;
                year = row[3].Value.Trim().ToInt();
                fundResource = row[4].Value.To<decimal?>();
                budgetSubject = row[5].Value.To<decimal?>();
                budgetMu = row[6].Value.To<decimal?>();
                ownerResource = row[7].Value.To<decimal?>();
            }
            else
            {
                sourceFinancing = row[2].Value;
                fundResource = row[3].Value.To<decimal?>();
                budgetSubject = row[4].Value.To<decimal?>();
                budgetMu = row[5].Value.To<decimal?>();
                ownerResource = row[6].Value.To<decimal?>();
            }

            var mu = this.municipalities.FirstOrDefault(x => x.Name == region);

            if (mu == null)
            {
                this.LogImport.Warn(this.Name, string.Format("Не найдено муниципальное образование {0}", region));
                return;
            }

            ObjectCr objectCr = null;
            if (this.objCrDict.ContainsKey(mu.Id))
            {
                var adr = address.Replace(" ", string.Empty).ToUpper();
                if (this.objCrDict.Get(mu.Id).ContainsKey(adr))
                {
                    objectCr = this.objCrDict.Get(mu.Id).Get(adr);
                }
            }

            var work = this.workDict.Get(workName.ToUpper());

            if (formFinanceSource == FormFinanceSource.WithTypeWork)
            {
                if (work == null)
                {
                    this.LogImport.Warn(this.Name, string.Format("Не найден вид работ: {0}", workName));
                    return;
                }
            }

            if (objectCr != null)
            {
                var realtyObjName = objectCr.RealityObject.Address;
                var existFinSources = this.finSourceResDict.Get(objectCr.Id) ?? new List<FinanceSourceResource>();

                var financeSourceResource = formFinanceSource == FormFinanceSource.WithTypeWork ?
                    existFinSources.FirstOrDefault(x => x.TypeWorkCr.Work.Id == work.Id && x.Year == year) : 
                    existFinSources.FirstOrDefault(x => x.ObjectCr.Id == objectCr.Id && x.FinanceSource?.Name == sourceFinancing);

                if (financeSourceResource != null)
                {
                    financeSourceResource.FundResource = fundResource;
                    financeSourceResource.BudgetSubject = budgetSubject;
                    financeSourceResource.BudgetMu = budgetMu;
                    financeSourceResource.OwnerResource = ownerResource;
                    financeSourceResource.ObjectCr = objectCr;
                    this.NsoFinanceSourceResourceDomain.Update(financeSourceResource);

                    var msg =
                        string.Format("Изменение записи {0} в объекте капитального ремонта. Вид работ: {1}. Год: {2}",
                        realtyObjName, workName, year);

                    this.LogImport.Info(this.Name, msg, LogTypeChanged.Changed);
                }
                else
                {
                    var existTypeWorkCr = this.typeWorkByRoDict.Get(objectCr.Id) ?? new List<TypeWorkCr>();
                    var typeWorkCr = work != null ? existTypeWorkCr.FirstOrDefault(x => x.Work.Id == work.Id) : null;

                    if (typeWorkCr == null && formFinanceSource == FormFinanceSource.WithTypeWork)
                    {
                        this.LogImport.Warn(this.Name, string.Format("Вид работ '{0}' не добавлен в дом: {1}", workName, address));
                        return;
                    }

                    var finSource = this.finSourceDict.Get(sourceFinancing);
                    if (finSource == null)
                    {
                        this.LogImport.Warn(this.Name, $"Изменение записи {realtyObjName} в объекте капитального ремонта не произведено. " +
                            "Значение источника финансирования не уникально или не существует");
                        return;
                    }

                    financeSourceResource = new FinanceSourceResource
                    {
                        TypeWorkCr = typeWorkCr,
                        Year = year,
                        FundResource = fundResource,
                        BudgetSubject = budgetSubject,
                        BudgetMu = budgetMu,
                        ObjectCr = objectCr,
                        OwnerResource = ownerResource,
                        FinanceSource = finSource
                    };
                    this.NsoFinanceSourceResourceDomain.Save(financeSourceResource);

                    var msg =
                        string.Format("Изменение записи {0} в объекте капитального ремонта. Вид работ: {1}. Год: {2}",
                            realtyObjName, workName, year);
                    this.LogImport.Info(this.Name, msg, LogTypeChanged.Added);
                }
            }
            else
            {
                this.LogImport.Warn(this.Name,
                    string.Format(
                        "Адрес дома {0}  введен неправильно или дом не попадает в выбранную программу капитального ремонта",
                        address));
            }
        }

        private void InitDictionaries(long programCrId)
        {
            this.municipalities = this.MunicipalityDomain.GetAll().ToList();

            this.objCrDict = this.ObjectCrDomain.GetAll()
                .Where(x => x.ProgramCr.Id == programCrId)
                .Select(x => new
                {
                    ObjectCr = x,
                    MuId = x.RealityObject.Municipality.Id,
                })
                .AsEnumerable()
                .GroupBy(x => x.MuId)
                .ToDictionary(x => x.Key,
                    y => y.GroupBy(x => x.ObjectCr.RealityObject.Address.ToStr().Replace(" ", string.Empty).ToUpper())
                        .ToDictionary(x => x.Key, z => z.Select(x => x.ObjectCr).First()));

            this.finSourceDict = this.FinanceSourceDomain.GetAll()
                .AsEnumerable()
                .GroupBy(x=> x.Name)
                .ToDictionary(x => x.Key, y=> y.Count() == 1 ? y.First() : null);

            this.finSourceResDict = this.NsoFinanceSourceResourceDomain.GetAll()
                .Where(x => x.ObjectCr.ProgramCr.Id == programCrId)
                .Select(x => new
                {
                    ObjectId = x.ObjectCr.Id,
                    FinSourceRes = x
                })
                .AsEnumerable()
                .GroupBy(x => x.ObjectId)
                .ToDictionary(x => x.Key, y => y.Select(x => x.FinSourceRes).ToList());

            this.workDict = this.WorkDomain.GetAll()
                .AsEnumerable()
                .GroupBy(x => x.Name.Trim().ToUpper())
                .ToDictionary(x => x.Key, y => y.First());

            this.typeWorkByRoDict = this.TypeWorkCrDomain.GetAll()
                .Where(x => x.ObjectCr.ProgramCr.Id == programCrId)
                .Select(x => new
                {
                    ObjectId = x.ObjectCr.Id,
                    Work = x
                })
                .AsEnumerable()
                .GroupBy(x => x.ObjectId)
                .ToDictionary(x => x.Key, y => y.Select(x => x.Work).ToList());
        }
    }
}
