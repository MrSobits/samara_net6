namespace Bars.Gkh.RegOperator.Imports
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Modules.Security;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums.Import;
    using Bars.Gkh.Import;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Dict;
    using Bars.Gkh.RegOperator.Imports.BenefitsCategory;
    using Bars.Gkh.RegOperator.Utils;

    using Castle.Windsor;

    using DbfDataReader;

    using Import.Impl;
    
    // TODO: Проверить работу после смены библиотеки

    /// <summary>
    /// Импорт сведений по льготным категориям.
    /// </summary>
    class BenefitsCategoryImport : GkhImportBase
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        /// <summary>
        /// The fields.
        /// </summary>
        private readonly HashSet<string> fields = new HashSet<string>
        {
            "PKU",
            "KOD_POST",
            "POST",
            "FAMIL",
            "IMJA",
            "OTCH",
            "ID_DOMA",
            "MU",
            "TYPE_CITY",
            "CITY",
            "TP_STREET",
            "STREET",
            "HOUSE_NUM",
            "LITER",
            "KORPUS",
            "BUILDING",
            "FLAT_NUM",
            "DATS",
            "LIVE_AREA",
            "SHARE",
            "KOD_LG",
            "DATE_LS",
            "SUMMA_LG",
            "SUML1",
            "LSA",
            "DOLG"
        };

        /// <summary>
        /// The headers dictionary.
        /// </summary>
        private readonly Dictionary<string, int> headersDict = new Dictionary<string, int>();

        /// <summary>
        /// Импортируемые записи.
        /// </summary>
        private readonly List<Record> records = new List<Record>();

        /// <summary>
        /// Заменить данные.
        /// </summary>
        private bool isOverwriteData;
        
        /// <summary>
        /// Список колонок таблицы DBF
        /// </summary>
        private List<string> columnNames;

        /// <summary>
        /// The existing realty object id list.
        /// </summary>
        private List<long> existingRealtyObjectIdList;

        /// <summary>
        /// 3 - уровневневый словарь
        /// Список существующих домов сгруппированных по улице
        /// => сгруппированных по населенному пункту
        /// => сгруппированных по муниципальному образованию (первого уровня)
        /// </summary>
        private Dictionary<string, Dictionary<string, Dictionary<string, List<RealtyObjectInStreet>>>> realtyObjectsByAddressDict;

        public BenefitsCategoryImport(ILogImportManager logManager, ILogImport logImport)
        {
            this.LogImportManager = logManager;
            this.LogImport = logImport;
        }

        /// <summary>
        /// Gets the key.
        /// </summary>
        public override string Key
        {
            get
            {
                return Id;
            }
        }

        /// <summary>
        /// Gets the code import.
        /// </summary>
        public override string CodeImport
        {
            get
            {
                return "PersonalAccountImport";
            }
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public override string Name
        {
            get
            {
                return "Импорт сведений по льготным категориям граждан";
            }
        }

        /// <summary>
        /// Gets the possible file extensions.
        /// </summary>
        public override string PossibleFileExtensions
        {
            get
            {
                return "dbf";
            }
        }

        /// <summary>
        /// Gets the permission name.
        /// </summary>
        public override string PermissionName
        {
            get
            {
                return "Import.BenefitsCategoryImport";
            }
        }

        /// <summary>
        /// Gets or sets the container.
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Gets or sets the period domain.
        /// </summary>
        public IDomainService<ChargePeriod> PeriodDomain { get; set; }

        /// <summary>
        /// Gets or sets the privileged category domain.
        /// </summary>
        public IDomainService<PrivilegedCategory> PrivilegedCategoryDomain { get; set; }

        /// <summary>
        /// Gets or sets the base per acc domain.
        /// </summary>
        public IDomainService<BasePersonalAccount> BasePerAccDomain { get; set; }

        /// <summary>
        /// Gets or sets the per acc owner domain.
        /// </summary>
        public IDomainService<PersonalAccountOwner> PerAccOwnerDomain { get; set; }

        /// <summary>
        /// Gets or sets the entity log domain.
        /// </summary>
        public IDomainService<EntityLogLight> EntityLogDomain { get; set; } 

        /// <summary>
        /// Gets or sets the reality object repository.
        /// </summary>
        public IRepository<RealityObject> RealityObjectRepository { get; set; }

        /// <summary>
        /// Gets or sets the user repo.
        /// </summary>
        public IRepository<User> UserRepo { get; set; }

        /// <summary>
        /// Gets or sets the identity.
        /// </summary>
        public IUserIdentity Identity { get; set; }

        /// <summary>
        /// Метод импорта.
        /// </summary>
        /// <param name="baseParams">
        /// The base params.
        /// </param>
        /// <returns>
        /// The <see cref="ImportResult"/>.
        /// </returns>
        public override ImportResult Import(BaseParams baseParams)
        {
            var file = baseParams.Files["FileImport"];
            this.isOverwriteData = baseParams.Params.GetAs<bool>("replaceData");

            try
            {
                this.InitLog(file.FileName);

                this.InitDictionaries();

                this.ProcessData(file.Data);
            }
            catch (ImportException e)
            {
                return new ImportResult(StatusImport.CompletedWithError, e.Message);
            }

            this.LogImportManager.FileNameWithoutExtention = file.FileName;
            this.LogImportManager.UploadDate = DateTime.Now;

            this.LogImport.SetFileName(file.FileName);
            this.LogImport.ImportKey = this.Key;

            this.LogImportManager.Add(file, this.LogImport);
            this.LogImportManager.Save();

            var message = this.LogImportManager.GetInfo();
            var status = this.LogImportManager.CountError > 0
                ? StatusImport.CompletedWithError
                : (this.LogImportManager.CountWarning > 0
                    ? StatusImport.CompletedWithWarning
                    : StatusImport.CompletedWithoutError);
        
            return new ImportResult(status, message, string.Empty, this.LogImportManager.LogFileId);
        }

        /// <summary>
        /// Валидация импорта.
        /// </summary>
        /// <param name="baseParams">
        /// The base params.
        /// </param>
        /// <param name="message">
        /// The message.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public override bool Validate(BaseParams baseParams, out string message)
        {
            message = null;

            if (!baseParams.Files.ContainsKey("FileImport"))
            {
                message = "Не выбран файл для импорта";
                return false;
            }

            var extention = baseParams.Files["FileImport"].Extention.Trim().ToLower();

            var fileExtentions = this.PossibleFileExtensions.Contains(",") 
                ? this.PossibleFileExtensions.Split(',') 
                : new[] { this.PossibleFileExtensions };

            if (fileExtentions.All(x => x != extention))
            {
                message = string.Format("Необходимо выбрать файл с допустимым расширением: {0}", this.PossibleFileExtensions);
                return false;
            }

            var fileData = baseParams.Files["FileImport"].Data;

            try
            {
                using (var stream = new MemoryStream(fileData))
                {
                    using (var table = new DbfTable(stream, Encoding.GetEncoding(866)))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                message = "Файл не является корректным .dbf файлом";
                return false;
            }
        }

        /// <summary>
        /// Инициализировать лог импорта.
        /// </summary>
        /// <param name="fileName">
        /// Имя файла.
        /// </param>
        /// <exception cref="ImportException">
        /// Исключение импорта
        /// </exception>
        private void InitLog(string fileName)
        {
            if (!this.Container.Kernel.HasComponent(typeof(ILogImportManager)))
            {
                throw new ImportException("Не найдена реализация интерфейса ILogImportManager");
            }

            this.LogImportManager.FileNameWithoutExtention = fileName;
            this.LogImportManager.UploadDate = DateTime.Now;

            if (this.LogImport == null)
            {
                throw new ImportException("Не найдена реализация интерфейса ILogImport");
            }

            this.LogImport.SetFileName(fileName);
            this.LogImport.ImportKey = this.Key;
        }

        /// <summary>
        /// Получить период начислений.
        /// </summary>
        /// <param name="date">
        /// Дата.
        /// </param>
        /// <returns>
        /// The <see cref="ChargePeriod"/>.
        /// </returns>
        private ChargePeriod GetChargePeriod(string date)
        {
            var datetime = date.ToDateTime();

            var year = datetime.Year;
            var month = datetime.Month;

            var period = this.PeriodDomain.GetAll().FirstOrDefault(x => x.StartDate.Year == year && x.StartDate.Month == month);

            if (period == null)
            {
                this.LogImport.Error("Не удалось определить период начислений", datetime.ToShortDateString());
            }

            return period;
        }

        /// <summary>
        /// Обработать данные.
        /// </summary>
        /// <param name="fileData">
        /// Массив байтов данных.
        /// </param>
        private void ProcessData(byte[] fileData)
        {
            using (var stream = new MemoryStream(fileData))
            {
                using (var table = new DbfTable(stream, Encoding.GetEncoding(866)))
                {
                    this.FillHeader(table);

                    var dbfRecord = new DbfRecord(table);
                    columnNames = table.Columns.Select(x => x.ColumnName).ToList();
                    int index = 0;
                    while (table.Read(dbfRecord))
                    {
                        var record = this.ReadLine(dbfRecord, index);
                        
                        if (record.IsValidRecord)
                        {
                            record.Period = this.GetChargePeriod(record.DateLs);
                            this.records.Add(record);
                        }

                        index++;
                    }
                }
            }

            var roIds = this.records.Select(x => x.RealtyObjectId).ToList();
            var names = this.records.Select(x => x.Name).ToList();
            
            // Из базы получаем только тех абонентов
            // 1. Имена и ИД дома совпадает с записями импортируемого файла
            // 2. Льготная категория которых не пустая
            // Только в этих случаях надо будет или установить или удалить льготную категорию.
            var basePerAccDict = this.BasePerAccDomain.GetAll()
                .Where(x => (roIds.Contains(x.Room.RealityObject.Id) && names.Contains(x.AccountOwner.Name)) || x.AccountOwner.PrivilegedCategory != null)
                .Select(x => new { x.Id, RoId = x.Room.RealityObject.Id, x.AccountOwner })
                .AsEnumerable()
                .GroupBy(x => x.RoId)
                .ToDictionary(
                    x => x.Key, 
                    x => new BasePerAccProxy 
                        {
                            BasePerAccId = x.Select(y => y.Id).FirstOrDefault(),
                            Owners = x.Select(z => z.AccountOwner).ToList()
                        });

            var entityLogListToSave = new List<EntityLogLight>();

            foreach (var basePerAccByRo in basePerAccDict)
            {
                foreach (var basePerAcc in basePerAccByRo.Value.Owners)
                {
                    const string EntityLogPropertyName = "Privileged";
                    var login = this.UserRepo.Get(this.Identity.UserId).Return(u => u.Login);

                    if (login.IsEmpty())
                    {
                        login = "anonymous";
                    }

                    var record = this.records
                        .Where(x => x.RealtyObjectId == basePerAccByRo.Key)
                        .Where(x => x.Name == basePerAcc.Name).ToList();

                    var logLight = this.EntityLogDomain.GetAll()
                        .Where(x => x.EntityId == basePerAccByRo.Value.BasePerAccId)
                        .Where(x => x.ClassName == typeof(BasePersonalAccount).Name)
                        .Where(x => x.PropertyName == EntityLogPropertyName)
                        .FirstOrDefault(x => x.DateActualChange == record.First().Period.StartDate);

                    // Если абонента нет в импортируемых данных и стоит галочка "Заменить данные", то удаляем льготную категорию
                    if (!record.Any() && this.isOverwriteData)
                    {
                        // Eсли период закрытый, то изменяем категорию в истории изменений
                        if (record.First().Period.IsClosed)
                        {
                            if (logLight != null)
                            {
                                logLight.PropertyValue = "Нет";
                                entityLogListToSave.Add(logLight);
                            }
                        }
                        else
                        {
                            basePerAcc.PrivilegedCategory = null;
                        }

                        this.LogImport.CountChangedRows++;
                    }
                    else if (record.Count() == 1)
                    {
                        var catergory = this.PrivilegedCategoryDomain.GetAll().FirstOrDefault(x => x.Code == record.First().PrivilegedCategoryCode);

                        if (catergory == null)
                        {
                            this.LogImport.Error("Не найдена льготная категория", record.First().PrivilegedCategoryCode);
                            continue;
                        }

                        // Eсли период закрытый, то сажаем в историю изменений
                        if (record.First().Period.IsClosed)
                        {
                            if (logLight != null)
                            {
                                if (this.isOverwriteData)
                                {
                                    logLight.PropertyValue = catergory.Name;
                                    logLight.DateApplied = DateTime.UtcNow;
                                    logLight.ObjectEditDate = DateTime.Now;

                                    entityLogListToSave.Add(logLight);
                                    this.LogImport.CountChangedRows++;
                                }
                            }
                            else
                            {
                                var log = new EntityLogLight
                                {
                                    EntityId = basePerAccByRo.Value.BasePerAccId,
                                    ClassName = typeof(BasePersonalAccount).Name,
                                    PropertyName = EntityLogPropertyName,
                                    PropertyValue = catergory.Name,
                                    ParameterName = "Льготная категория",
                                    DateApplied = DateTime.UtcNow,
                                    DateActualChange = record.First().Period.StartDate,
                                    User = login,
                                    ObjectCreateDate = DateTime.Now,
                                    ObjectEditDate = DateTime.Now
                                };

                                entityLogListToSave.Add(log);
                                this.LogImport.CountAddedRows++;
                            }
                        }
                        else
                        {
                            if (basePerAcc.PrivilegedCategory == null)
                            {
                                // Если категории нет, то назначаем ее
                                basePerAcc.PrivilegedCategory = catergory;
                                this.LogImport.CountAddedRows++;
                            }
                            else if (basePerAcc.PrivilegedCategory != null && this.isOverwriteData)
                            {
                                // Если категория уже есть и выбрали "Заменять данные"
                                basePerAcc.PrivilegedCategory = catergory;
                                this.LogImport.CountChangedRows++;
                            }
                        }
                    }
                    else
                    {
                        this.LogImport.Error(
                            "Данные по абоненту не однозначны", 
                            string.Format("Строки: {0}", string.Join(", ", record.Select(x => x.RowNumber).ToArray())));

                        continue;
                    }

                    this.PerAccOwnerDomain.Update(basePerAcc);
                }
            }

            TransactionHelper.InsertInManyTransactions(this.Container, entityLogListToSave, 1000, true, true);
        }

        /// <summary>
        /// Заполнить словарь заголовков столбцов.
        /// </summary>
        /// <param name="table">
        /// The table.
        /// </param>
        private void FillHeader(DbfTable table)
        {
            var index = 0;
            foreach (var header in table.Columns.Select(col => col.ColumnName))
            {
                if (this.fields.Contains(header))
                {
                    this.headersDict[header] = index;
                }
                else
                {
                    this.LogImport.Error("Неизвестное название колонки", string.Format("Данные в колонке '{0}' не будут импортированы", header));
                }

                index++;
            }
        }

        /// <summary>
        /// Получить значение из строки dbf по имени столбца.
        /// </summary>
        /// <param name="row">
        /// Строка dbf.
        /// </param>
        /// <param name="column">
        /// Имя столбца.
        /// </param>
        /// <returns>
        /// The <see cref="string"/>.
        /// </returns>
        private string GetValueOrDefault(DbfRecord row, string column)
        {
            return this.headersDict.ContainsKey(column) ? row.GetValueOrDefault(column, columnNames).ToString() : string.Empty;
        }

        /// <summary>
        /// Инициализировать переменные.
        /// </summary>
        private void InitDictionaries()
        {
            this.existingRealtyObjectIdList = this.RealityObjectRepository.GetAll().Select(x => x.Id).ToList();

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
                .Where(x => x.child.ActStatus == FiasActualStatusEnum.Actual)
                .Where(x => x.child.AOLevel == FiasLevelEnum.Street)
                .Select(x => new
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
                                        v => v.Select(y => new 
                                                {
                                                    y.RealityObjectId,
                                                    y.House,
                                                    y.Letter,
                                                    y.Housing,
                                                    y.Building
                                                })
                                            .Distinct()
                                            .Select(u => new RealtyObjectInStreet
                                                {
                                                    RoId = u.RealityObjectId,
                                                    House = u.House,
                                                    Letter = u.Letter,
                                                    Housing = u.Housing,
                                                    Building = u.Building
                                                })
                                            .ToList())));
        }

        /// <summary>
        /// Прочитать данные из строки dbf
        /// </summary>
        /// <param name="row">
        /// The row.
        /// </param>
        /// <returns>
        /// The <see cref="Record"/>.
        /// </returns>
        private Record ReadLine(DbfRecord row, int rowNumber)
        {
            var record = new Record
                             {
                                 IsValidRecord = false,
                                 RowNumber = rowNumber,
                                 ImportRealtyObjectId = this.GetValueOrDefault(row, "ID_DOMA").ToInt()
                             };

            if (record.ImportRealtyObjectId > 0 && this.existingRealtyObjectIdList.Contains(record.ImportRealtyObjectId))
            {
                record.RealtyObjectId = record.ImportRealtyObjectId;
            }

            if (record.RealtyObjectId == 0)
            {
                record.Letter = this.GetValueOrDefault(row, "LITER");
                record.Housing = this.GetValueOrDefault(row, "HOUSING");
                record.Building = this.GetValueOrDefault(row, "BUILDING");
                record.MunicipalityName = this.GetValueOrDefault(row, "MU");

                if (string.IsNullOrWhiteSpace(record.MunicipalityName))
                {
                    this.LogImport.Error(record.RowNumber.ToString(), "Поиск жилого дома: не задано муниципальное образование.");
                    return record;
                }

                record.LocalityName = this.GetValueOrDefault(row, "TYPE_CITY") + " " + this.GetValueOrDefault(row, "CITY");

                if (string.IsNullOrWhiteSpace(record.LocalityName))
                {
                    this.LogImport.Error(record.RowNumber.ToString(), "Поиск жилого дома: не задан населенный пункт дома.");
                    return record;
                }

                record.StreetName = this.GetValueOrDefault(row, "TP_STREET") + " " + this.GetValueOrDefault(row, "STREET");

                if (string.IsNullOrWhiteSpace(record.StreetName))
                {
                    this.LogImport.Error(record.RowNumber.ToString(), "Поиск жилого дома: не задана улица дома.");
                    return record;
                }

                if (!this.realtyObjectsByAddressDict.ContainsKey(record.MunicipalityName.ToLower()))
                {
                    this.LogImport.Error(record.RowNumber.ToString(), "Поиск жилого дома: не найдены дома в муниципальном образовании: " + record.MunicipalityName);
                    return record;
                }

                var municipalityRealtyObjectsDict = this.realtyObjectsByAddressDict[record.MunicipalityName.ToLower()];

                if (!municipalityRealtyObjectsDict.ContainsKey(record.LocalityName.ToLower()))
                {
                    this.LogImport.Error(record.RowNumber.ToString(), "Поиск жилого дома: в указанном МО не найдены дома в населенном пунтке: " + record.LocalityName);
                    return record;
                }

                var localityRealtyObjectsDict = municipalityRealtyObjectsDict[record.LocalityName.ToLower()];

                if (!localityRealtyObjectsDict.ContainsKey(record.StreetName.ToLower()))
                {
                    this.LogImport.Error(record.RowNumber.ToString(), "Поиск жилого дома: в указанном населенном пункте не найдены дома на улице: " + record.StreetName);
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
                    this.LogImport.Error(record.RowNumber.ToString(), string.Format("Поиск жилого дома: На улице '{0}' не найден дом", record.StreetName));
                    return record;
                }

                if (realtyObjectsOnStreet.Count > 1)
                {
                    this.LogImport.Error(record.RowNumber.ToString(), "Поиск жилого дома: неоднозначный дом. Соответствующих данному набору адресных параметров домов найдено: " + realtyObjectsOnStreet.Count);
                    return record;
                }

                record.RealtyObjectId = realtyObjectsOnStreet.First().RoId;
            }

            record.Name = string.Format(
                    "{0} {1} {2}",
                    this.GetValueOrDefault(row, "FAMIL"),
                    this.GetValueOrDefault(row, "IMJA"),
                    this.GetValueOrDefault(row, "OTCH"));

            if (string.IsNullOrWhiteSpace(record.Name))
            {
                this.LogImport.Error(record.RowNumber.ToString(), "Не указано ФИО абонента");
                return record;
            }

            record.FlatNum = this.GetValueOrDefault(row, "FLAT_NUM");
            record.PrivilegedCategoryCode = this.GetValueOrDefault(row, "KOD_LG");
            record.DateLs = this.GetValueOrDefault(row, "DATE_LS");

            record.IsValidRecord = true;

            return record;
        }

        private class RealtyObjectInStreet
        {
            /// <summary>
            /// Gets or sets the ro id.
            /// </summary>
            public long RoId { get; set; }

            /// <summary>
            /// Gets or sets the house.
            /// </summary>
            public string House { get; set; }

            /// <summary>
            /// Gets or sets the letter.
            /// </summary>
            public string Letter { get; set; }

            /// <summary>
            /// Gets or sets the housing.
            /// </summary>
            public string Housing { get; set; }

            /// <summary>
            /// Gets or sets the building.
            /// </summary>
            public string Building { get; set; }
        }

        private class BasePerAccProxy
        {
            /// <summary>
            /// Gets or sets the base personal account id.
            /// </summary>
            public long BasePerAccId { get; set; }

            /// <summary>
            /// Gets or sets the account owners.
            /// </summary>
            public List<PersonalAccountOwner> Owners { get; set; } 
        }
    }
}
