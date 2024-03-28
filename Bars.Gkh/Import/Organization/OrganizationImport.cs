namespace Bars.Gkh.Import.Organization
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Reflection;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Enums.Import;
    using Bars.Gkh.Import;
    using Bars.GkhExcel;
    using Bars.Gkh.Import.Organization.Impl;

    using Castle.Windsor;

    using Import.Impl;

    public class OrganizationImport : GkhImportBase
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        #region Properties

        public virtual IWindsorContainer Container { get; set; }

        private bool updatePeriodsManOrgs;

        public override string Key
        {
            get { return Id; }
        }

        public override string CodeImport
        {
            get { return "OrganizationImport"; }
        }

        public override string Name
        {
            get { return "Импорт организаций"; }
        }

        public override string PossibleFileExtensions
        {
            get { return "xls"; }
        }

        public override string PermissionName
        {
            get { return "Import.OrganizationImport"; }
        }

        public ISessionProvider SessionProvider { get; set; }

        public IRepository<RealityObject> RealityObjectRepository { get; set; }

        public IOrganizationImportCommonHelper OrganizationImportCommonHelper { get; set; }

        #endregion Properties

        #region Dictionaries

        class RealtyObjectInStreet
        {
            public long roId { get; set; }

            public string House { get; set; }

            public string Letter { get; set; }

            public string Housing { get; set; }

            public string Building { get; set; }
        }


        // 3 - уровневневый словарь
        // Список существующих домов сгруппированных по улице
        // => сгруппированных по населенному пункту
        // => сгруппированных по муниципальному образованию (первого уровня)
        private Dictionary<string, Dictionary<string, Dictionary<string, List<RealtyObjectInStreet>>>> realtyObjectsByAddressDict;

        private Dictionary<string, List<RealtyObjectInStreet>> realtyObjectsByKladrCodeDict;

        private List<long> existingRealtyObjectIdList;

        private readonly Dictionary<string, long> headersDict = new Dictionary<string, long>();

        private Dictionary<string, IOrganizationImportHelper> helpersDict = new Dictionary<string, IOrganizationImportHelper>();

        #endregion Dictionaries

        public OrganizationImport(ILogImportManager logImportManager, ILogImport logImport)
        {
            this.LogImportManager = logImportManager;
            this.LogImport = logImport;
        }

        protected virtual void InitDictionaries()
        {
            var fiasService = this.Container.Resolve<IDomainService<Fias>>().GetAll();

            var realtyObjects = fiasService
                .Join(
                    fiasService,
                    x => x.AOGuid,
                    x => x.ParentGuid,
                    (a, b) => new { parent = a, child = b })
                .Join(
                    this.RealityObjectRepository.GetAll(),
                    x => x.child.AOGuid,
                    y => y.FiasAddress.StreetGuidId,
                    (c, d) => new { c.parent, c.child, realityObject = d })

                //.Where(x => x.parent.ActStatus == FiasActualStatusEnum.Actual) 
                .Where(x => x.child.ActStatus == FiasActualStatusEnum.Actual)
                .Where(x => x.child.AOLevel == FiasLevelEnum.Street)
                .Select(
                    x => new
                    {
                        localityName = x.parent.OffName,
                        localityShortname = x.parent.ShortName,
                        streetName = x.child.OffName,
                        streetShortname = x.child.ShortName,
                        x.realityObject.FiasAddress.House,
                        x.realityObject.FiasAddress.Letter,
                        x.realityObject.FiasAddress.Housing,
                        x.realityObject.FiasAddress.Building,
                        RealityObjectId = x.realityObject.Id,
                        MunicipalityName = x.realityObject.Municipality.Name,
                        x.child.KladrCode
                    })
                .ToArray();

            this.existingRealtyObjectIdList = this.RealityObjectRepository.GetAll().Select(x => x.Id).ToList();

            // 3 - уровневневый словарь
            // Список существующих домов сгруппированных по улице
            // => сгруппированных по населенному пункту
            // => сгруппированных по муниципальному образованию (первого уровня)
            this.realtyObjectsByAddressDict = realtyObjects
                .Where(x => !string.IsNullOrWhiteSpace(x.localityName))
                .Where(x => !string.IsNullOrWhiteSpace(x.streetName))
                .GroupBy(x => (x.MunicipalityName ?? string.Empty).Trim().ToLower())
                .ToDictionary(
                    x => x.Key,
                    x => x.GroupBy(z => (z.localityName + " " + (z.localityShortname ?? string.Empty)).Trim().ToLower())
                        .ToDictionary(
                            z => z.Key,
                            z => z.GroupBy(v => (v.streetName + " " + (v.streetShortname ?? string.Empty)).Trim().ToLower())
                                .ToDictionary(
                                    v => v.Key,
                                    v => v.Select(
                                            y => new
                                            {
                                                y.RealityObjectId,
                                                y.House,
                                                y.Letter,
                                                y.Housing,
                                                y.Building
                                            })
                                        .Distinct()
                                        .Select(
                                            u => new RealtyObjectInStreet
                                            {
                                                roId = u.RealityObjectId,
                                                House = u.House,
                                                Letter = u.Letter,
                                                Housing = u.Housing,
                                                Building = u.Building
                                            })
                                        .ToList())));

            this.realtyObjectsByKladrCodeDict = realtyObjects
                .Where(x => !string.IsNullOrWhiteSpace(x.KladrCode))
                .GroupBy(x => x.KladrCode)
                .ToDictionary(
                    x => x.Key,
                    x => x.Select(y => new 
                            {
                                y.RealityObjectId,
                                y.House,
                                y.Letter,
                                y.Housing,
                                y.Building
                            })
                        .Distinct()
                        .Select(y => new RealtyObjectInStreet
                            {
                                roId = y.RealityObjectId,
                                House = y.House,
                                Letter = y.Letter,
                                Housing = y.Housing,
                                Building = y.Building
                            })
                          .ToList());
        }

        private IDataResult InitHeader(GkhExcelCell[] data)
        {
            this.headersDict["ID_DOMA"] = -1;
            this.headersDict["MU"] = -1;
            this.headersDict["TYPE_CITY"] = -1;
            this.headersDict["CITY"] = -1;
            this.headersDict["TYPE_STREET"] = -1;
            this.headersDict["KLADR"] = -1;
            this.headersDict["STREET"] = -1;
            this.headersDict["HOUSE_NUM"] = -1;
            this.headersDict["LITER"] = -1;
            this.headersDict["KORPUS"] = -1;
            this.headersDict["BUILDING"] = -1;

            this.headersDict["ID_COM"] = -1;
            this.headersDict["TYPE_COM"] = -1;
            this.headersDict["NAME_COM"] = -1;
            this.headersDict["INN"] = -1;
            this.headersDict["KPP"] = -1;
            this.headersDict["OGRN"] = -1;
            this.headersDict["DATE_REG"] = -1;
            this.headersDict["TYPE_LAW_FORM"] = -1;
            this.headersDict["MR_COM"] = -1;
            this.headersDict["MU_COM"] = -1;
            this.headersDict["TYPE_CITY_COM"] = -1;
            this.headersDict["CITY_COM"] = -1;
            this.headersDict["TYPE_STREET_COM"] = -1;
            this.headersDict["STREET_COM"] = -1;
            this.headersDict["KLADR_COM"] = -1;
            this.headersDict["HOUSE_NUM_COM"] = -1;
            this.headersDict["LITER_COM"] = -1;
            this.headersDict["KORPUS_COM"] = -1;
            this.headersDict["BUILDING_COM"] = -1;

            this.headersDict["DATE_START_CON"] = -1;
            this.headersDict["TYPE_CON"] = -1;
            this.headersDict["NUM_DOG"] = -1;
            this.headersDict["DATE_DOG"] = -1;

            for (var index = 0; index < data.Length; ++index)
            {
                var header = data[index].Value.ToStr().Trim().ToUpper();
                if (this.headersDict.ContainsKey(header))
                {
                    this.headersDict[header] = index;
                }
            }

            var requiredFields = new List<string> { "TYPE_COM" };

            var absentColumns = requiredFields.Where(x => !this.headersDict.ContainsKey(x) || this.headersDict[x] == -1).ToList();

            if (absentColumns.Any())
            {
                var msg = "Отсутствуют обязательные столбцы: " + string.Join(", ", absentColumns);
                return BaseDataResult.Error(msg);
            }

            return new BaseDataResult();
        }

        public void InitLog(string fileName)
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
            this.LogImport.ImportKey = this.Key;
        }

        private void InitHelpers()
        {
            this.OrganizationImportCommonHelper.Init();

            var helpers = this.Container.ResolveAll<IOrganizationImportHelper>(new[]
            {
                new KeyValuePair<string, object>("commonHelper", this.OrganizationImportCommonHelper),
                new KeyValuePair<string, object>("headersDict", this.headersDict),
                new KeyValuePair<string, object>("updatePeriodsManOrgs", this.updatePeriodsManOrgs),
                new KeyValuePair<string, object>("LogImport", this.LogImport),
            });

            if (helpers.GroupBy(x => x.OrganizationType).Any(x => x.Count() > 1))
            {
                throw new Exception("Дублирующий IOrganizationImportHelper");
            }

            this.helpersDict = helpers.ToDictionary(x => x.OrganizationType);
        }

        private string GetValue(GkhExcelCell[] data, string field)
        {
            var result = string.Empty;

            if (this.headersDict.ContainsKey(field))
            {
                var index = this.headersDict[field];
                if (data.Length > index && index > -1)
                {
                    result = data[index].Value;
                }
            }

            return result.Trim();
        }
        
        public override ImportResult Import(BaseParams baseParams)
        {
            var file = baseParams.Files["FileImport"];
           
            this.updatePeriodsManOrgs = baseParams.Params.GetAs<bool>("updatePeriodsManOrgs");

            this.InitLog(file.FileName);

            this.InitDictionaries();

            var result = this.ProcessData(file.Data, this.updatePeriodsManOrgs);

            if (!result.Success)
            {
                return new ImportResult(StatusImport.CompletedWithError, result.Message);
            }
        
            this.SaveData();

            this.LogImportManager.Add(file, this.LogImport);
            this.LogImportManager.Save();

            result.Message += this.LogImportManager.GetInfo();
            var status = this.LogImportManager.CountError > 0
                ? StatusImport.CompletedWithError
                : (this.LogImportManager.CountWarning > 0 ? StatusImport.CompletedWithWarning : StatusImport.CompletedWithoutError);
            return new ImportResult(status, result.Message, string.Empty, this.LogImportManager.LogFileId);
        }

        private IDataResult ProcessData(byte[] fileData, bool updatePeriods)
        {
            var excel = this.Container.Resolve<IGkhExcelProvider>("ExcelEngineProvider");
            using (this.Container.Using(excel))
            {
                if (excel == null)
                {
                    throw new Exception("Не найдена реализация интерфейса IGkhExcelProvider");
                }

                using (var memoryStreamFile = new MemoryStream(fileData))
                {
                    memoryStreamFile.Seek(0, SeekOrigin.Begin);

                    excel.Open(memoryStreamFile);

                    var rows = excel.GetRows(0, 0);

                    var result = this.InitHeader(rows.First());

                    if (!result.Success)
                    {
                        return result;
                    }

                    this.InitHelpers();

                    for (var i = 1; i < rows.Count; ++i)
                    {
                        var record = this.ProcessLine(rows[i], i + 1);

                        if (record.isValidRecord)
                        {
                            record.ImportHelper.CreateContract(record, updatePeriods);
                            this.AddLog(record.RowNumber, "Успешно загружено", LogMessageType.Info);
                            this.LogImport.CountAddedRows++;
                        }
                    }
                }
            }

            return new BaseDataResult();
        }

        /// <summary>
        /// Обработка строки импорта
        /// </summary>
        private Record ProcessLine(GkhExcelCell[] data, int rowNumber)
        {
            var record = new Record { isValidRecord = false, RowNumber = rowNumber };

            if (data.Length <= 1)
            {
                return record;
            }

            var organizationType = this.GetValue(data, "TYPE_COM");
            if (string.IsNullOrEmpty(organizationType))
            {
                this.AddLog(record.RowNumber, "Не задан тип организации.", LogMessageType.Error);
                return record;
            }

            record.ImportHelper = this.GetOrganizationImportHelper(organizationType);

            //в ManagingOrganizationImportHelper есть проверка по этому свойству
            //не совсем понятно, зачем она там нужна, но чтобы не поломать пока делаю так
            if (record.ImportHelper is ManagingOrganizationImportHelper)
            {
                record.OrganizationType = OrgType.ManagingOrganization;
            }

            if (record.ImportHelper == null)
            {
                this.AddLog(record.RowNumber, "Неизвестный тип организации: " + organizationType, LogMessageType.Error);
                return record;
            }

            var result = record.ImportHelper.ProcessLine(data, record);

            if (!result.Success)
            {
                return record;
            }
            
            var accountCreateDate = this.GetValue(data, "DATE_START_CON");

            if (!string.IsNullOrEmpty(accountCreateDate))
            {
                accountCreateDate = accountCreateDate.Trim();
            }

            if (string.IsNullOrWhiteSpace(accountCreateDate))
            {
                this.AddLog(record.RowNumber, "Не задана Дата начала управления/обслуживания домом (а).", LogMessageType.Error);
                return record;
            }
            else
            {
                DateTime date;

                accountCreateDate = accountCreateDate.Length > 10
                    ? accountCreateDate.Substring(0, 10)
                    : accountCreateDate;

                if (DateTime.TryParseExact(accountCreateDate, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                {
                    record.ContractStartDate = date;
                }
                else
                {
                    this.AddLog(
                        record.RowNumber,
                        "Некорректная дата в поле 'DATE_START_CON': " + accountCreateDate + "'. Дата ожидается в формате 'дд.мм.гггг'",
                        LogMessageType.Error);
                    return record;
                }
            }

            record.DocumentNumber = this.GetValue(data, "NUM_DOG");

            var documentDate = this.GetValue(data, "DATE_DOG");

            if (!string.IsNullOrEmpty(documentDate))
            {
                documentDate = documentDate.Trim();
            }

            if (string.IsNullOrWhiteSpace(documentDate))
            {
                this.AddLog(record.RowNumber, "Не задана Дата договора управления/обслуживания.", LogMessageType.Error);
                return record;
            }
            else
            {
                DateTime date;

                documentDate = documentDate.Length > 10
                    ? documentDate.Substring(0, 10)
                    : documentDate;

                if (DateTime.TryParseExact(documentDate, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out date))
                {
                    record.DocumentDate = date;
                }
                else
                {
                    this.AddLog(
                        record.RowNumber,
                        "Некорректная дата в поле 'DATE_DOG': " + accountCreateDate + "'. Дата ожидается в формате 'дд.мм.гггг'",
                        LogMessageType.Error);
                    return record;
                }
            }
            
            //// 1. Если указан идентификатор дома, пытаемся найти его среди существующих
            record.ImportRealtyObjectId = this.GetValue(data, "ID_DOMA").ToInt();
            if (record.ImportRealtyObjectId > 0 && this.existingRealtyObjectIdList.Contains(record.ImportRealtyObjectId))
            {
                record.RealtyObjectId = record.ImportRealtyObjectId;
            }

            //// 2. Если дом еще не найден, ищем по связке Код КЛАДР + номер дома + литер + корпус + строение
            if (record.RealtyObjectId == 0)
            {
                record.House = this.GetValue(data, "HOUSE_NUM");

                if (string.IsNullOrWhiteSpace(record.House))
                {
                    this.AddLog(record.RowNumber, "Поиск жилого дома: Не задан номер дома.", LogMessageType.Error);
                    return record;
                }

                record.Letter = this.GetValue(data, "LITER");
                record.Housing = this.GetValue(data, "HOUSING");
                record.Building = this.GetValue(data, "BUILDING");
                record.StreetKladrCode = this.GetValue(data, "KLADR");

                if (!string.IsNullOrWhiteSpace(record.StreetKladrCode))
                {
                    if (!this.realtyObjectsByKladrCodeDict.ContainsKey(record.StreetKladrCode))
                    {
                        this.AddLog(
                            record.RowNumber,
                            "Поиск жилого дома по кладр: в системе не найдена улица дома жил.фонда с кодом кладр: " + record.StreetKladrCode,
                            LogMessageType.Error);
                        return record;
                    }

                    var realtyObjectsOnStreet = this.realtyObjectsByKladrCodeDict[record.StreetKladrCode].Where(x => x.House == record.House).ToList();

                    realtyObjectsOnStreet = string.IsNullOrWhiteSpace(record.Letter)
                        ? realtyObjectsOnStreet.Where(x => string.IsNullOrWhiteSpace(x.Letter)).ToList()
                        : realtyObjectsOnStreet.Where(x => x.Letter == record.Letter).ToList();

                    realtyObjectsOnStreet = string.IsNullOrWhiteSpace(record.Housing)
                        ? realtyObjectsOnStreet.Where(x => string.IsNullOrWhiteSpace(x.Housing)).ToList()
                        : realtyObjectsOnStreet.Where(x => x.Housing == record.Housing).ToList();

                    realtyObjectsOnStreet = string.IsNullOrWhiteSpace(record.Building)
                        ? realtyObjectsOnStreet.Where(x => string.IsNullOrWhiteSpace(x.Building)).ToList()
                        : realtyObjectsOnStreet.Where(x => x.Building == record.Building).ToList();

                    if (realtyObjectsOnStreet.Count > 1)
                    {
                        this.AddLog(
                            record.RowNumber,
                            "Поиск жилого дома по кладр: неоднозначный дом. Соответствующих данному набору адресных параметров домов найдено: "
                            + realtyObjectsOnStreet.Count,
                            LogMessageType.Error);
                        return record;
                    }
                    
                    if (realtyObjectsOnStreet.Count == 1)
                    {
                        record.RealtyObjectId = realtyObjectsOnStreet.First().roId;
                    }
                }
            }

            //// 3. Если дом все еще не найден, ищем по текстовым значениям: МО + Нас.пункт + улица + номер дома + литер + корпус + строение
            if (record.RealtyObjectId == 0)
            {
                record.MunicipalityName = this.GetValue(data, "MU");
                if (string.IsNullOrWhiteSpace(record.MunicipalityName))
                {
                    this.AddLog(record.RowNumber, "Поиск жилого дома: не задано муниципальное образование.", LogMessageType.Error);
                    return record;
                }

                record.LocalityName = Simplified(this.GetValue(data, "CITY") + " " + this.GetValue(data, "TYPE_CITY"));

                if (string.IsNullOrWhiteSpace(record.LocalityName))
                {
                    this.AddLog(record.RowNumber, "Поиск жилого дома: не задан населенный пункт дома.", LogMessageType.Error);
                    return record;
                }

                record.StreetName = Simplified(this.GetValue(data, "STREET") + " " + this.GetValue(data, "TYPE_STREET"));

                if (string.IsNullOrWhiteSpace(record.StreetName))
                {
                    this.AddLog(record.RowNumber, "Поиск жилого дома: не задана улица дома.", LogMessageType.Error);
                    return record;
                }
                
                if (!this.realtyObjectsByAddressDict.ContainsKey(record.MunicipalityName.ToLower()))
                {
                    this.AddLog(
                        record.RowNumber,
                        "Поиск жилого дома: не найдены дома в муниципальном образовании: " + record.MunicipalityName,
                        LogMessageType.Error);
                    return record;
                }

                var municipalityRealtyObjectsDict = this.realtyObjectsByAddressDict[record.MunicipalityName.ToLower()];

                if (!municipalityRealtyObjectsDict.ContainsKey(record.LocalityName.ToLower()))
                {
                    this.AddLog(
                        record.RowNumber,
                        "Поиск жилого дома: в указанном МО не найдены дома в населенном пунтке: " + record.LocalityName,
                        LogMessageType.Error);
                    return record;
                }

                var localityRealtyObjectsDict = municipalityRealtyObjectsDict[record.LocalityName.ToLower()];

                if (!localityRealtyObjectsDict.ContainsKey(record.StreetName.ToLower()))
                {
                    this.AddLog(
                        record.RowNumber,
                        "Поиск жилого дома: в указанном населенном пункте не найдены дома на улице: " + record.StreetName,
                        LogMessageType.Error);
                    return record;
                }

                var realtyObjectsOnStreet = localityRealtyObjectsDict[record.StreetName.ToLower()].Where(x => x.House == record.House).ToList();

                realtyObjectsOnStreet = string.IsNullOrWhiteSpace(record.Letter)
                    ? realtyObjectsOnStreet.Where(x => string.IsNullOrWhiteSpace(x.Letter)).ToList()
                    : realtyObjectsOnStreet.Where(x => x.Letter == record.Letter).ToList();

                realtyObjectsOnStreet = string.IsNullOrWhiteSpace(record.Housing)
                    ? realtyObjectsOnStreet.Where(x => string.IsNullOrWhiteSpace(x.Housing)).ToList()
                    : realtyObjectsOnStreet.Where(x => x.Housing == record.Housing).ToList();

                realtyObjectsOnStreet = string.IsNullOrWhiteSpace(record.Building)
                    ? realtyObjectsOnStreet.Where(x => string.IsNullOrWhiteSpace(x.Building)).ToList()
                    : realtyObjectsOnStreet.Where(x => x.Building == record.Building).ToList();

                if (realtyObjectsOnStreet.Count == 0)
                {
                    this.AddLog(record.RowNumber, string.Format("Поиск жилого дома: На улице '{0}' не найден дом", record.StreetName), LogMessageType.Error);
                    return record;
                }

                if (realtyObjectsOnStreet.Count > 1)
                {
                    this.AddLog(
                        record.RowNumber,
                        "Поиск жилого дома: неоднозначный дом. Соответствующих данному набору адресных параметров домов найдено: "
                        + realtyObjectsOnStreet.Count,
                        LogMessageType.Error);
                    return record;
                }

                record.RealtyObjectId = realtyObjectsOnStreet.First().roId;
            }

            record.isValidRecord = true;

            return record;
        }

        private void AddLog(int rowNum, string message, LogMessageType type)
        {
            var rowNumber = $"Строка {rowNum}";
            switch (type)
            {
                case LogMessageType.Debug:
                    this.LogImport.Debug(rowNumber, message);
                    break;
                case LogMessageType.Info:
                    this.LogImport.Info(rowNumber, message);
                    break;
                case LogMessageType.Warning:
                    this.LogImport.Warn(rowNumber, message);
                    break;
                case LogMessageType.Error:
                    this.LogImport.Error(rowNumber, message);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }

        private IOrganizationImportHelper GetOrganizationImportHelper(string orgType)
        {
            return this.helpersDict.ContainsKey(orgType.ToLower()) ? this.helpersDict[orgType.ToLower()] : null;
        }

        private void SaveData()
        {
            this.SessionProvider.CloseCurrentSession();

            using (var session = this.SessionProvider.OpenStatelessSession())
            {
                session.SetBatchSize(1000);

                using (var tr = session.BeginTransaction())
                {
                    this.OrganizationImportCommonHelper.SaveData(session);

                    this.helpersDict.Values.ForEach(x => x.SaveData(session));

                    tr.Commit();
                }
            }
        }

        /// <summary>
        /// Returns a string that has space removed from the start and the end, and that has each sequence of internal space replaced with a single space.
        /// </summary>
        /// <param name="initialString"></param>
        /// <returns></returns>
        private static string Simplified(string initialString)
        {
            if (string.IsNullOrEmpty(initialString))
            {
                return initialString;
            }

            var trimmed = initialString.Trim();

            if (!trimmed.Contains(" "))
            {
                return trimmed;
            }

            var result = string.Join(" ", trimmed.Split(' ').Where(x => !string.IsNullOrWhiteSpace(x)));

            return result;
        }

    }
}