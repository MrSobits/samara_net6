namespace Bars.Gkh.RegOperator.Imports.Room
{
    using B4.Modules.FIAS;
    using B4.Utils;
    using Bars.B4.IoC;
    using Enums;
    using Gkh.Enums;
    using GkhExcel;
    using Import;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;

    partial class RoomImport
    {
        private const string HeaderIdDoma = "ID_DOMA";


        /// <summary>
        /// Обработка файла импорта
        /// </summary>
        /// <param name="fileData">Файл</param>
        /// <param name="fileExtention">Расширения файла</param>
        private void ProcessData(byte[] fileData, string fileExtention)
        {
            this.records.Clear();

            try
            {
                var excel = this.Container.Resolve<IGkhExcelProvider>("ExcelEngineProvider");
                using (this.Container.Using(excel))
                {
                    if (excel == null)
                    {
                        throw new ImportException("Не найдена реализация интерфейса IGkhExcelProvider");
                    }

                    if (fileExtention == "xlsx")
                    {
                        excel.UseVersionXlsx();
                    }

                    using (var memoryStreamFile = new MemoryStream(fileData))
                    {
                        memoryStreamFile.Seek(0, SeekOrigin.Begin);

                        excel.Open(memoryStreamFile);

                        var rows = excel.GetRows(0, 0);

                        // получаем строку с идентификаторами заголовков
                        var headerIdsRow = this.GetHeaderIdsRow(rows);

                        this.InitHeader(headerIdsRow);

                        // начинаем цикл с i = номер строки после строки с идентификаторами
                        var startIndex = rows.IndexOf(headerIdsRow) + 1;

                        for (var i = startIndex; i < rows.Count; ++i)
                        {
                            var record = this.ProcessLine(rows[i], i + 1);


                            if (this.records.Any() && record.AccountNumber.IsNotEmpty() &&
                                this.records.Select(x => x.AccountNumber).Contains(record.AccountNumber))
                            {
                                this.AddLog(record.RowNumber, string.Format("Дубль ЛС {0}", record.AccountNumber), false);
                                continue;
                            }

                            if (record.isValidRecord)
                            {
                                this.records.Add(record);
                            }
                        }

                        this.records = this.records
                            .GroupBy(x => new
                            {
                                x.RealtyObjectId,
                                x.Apartment,
                                x.Area,
                                x.LivingArea,
                                x.RoomType,
                                x.OwnershipType,
                                x.OwnerType,
                                x.AreaShare,
                                x.PersAccNumExternalSystems,
                                x.OwnerPhysSurname,
                                x.OwnerPhysFirstName,
                                x.OwnerPhysSecondName,
                                x.OwnerJurName,
                                x.OwnerJurInn,
                                x.OwnerJurKpp
                            })
                            .Select(x =>
                            {
                                var first = x.First();
                                if (x.Count() > 1)
                                {
                                    var errMsg = string.Format("Дубль строки {0}", first.RowNumber);
                                    x.Where(y => y != first).ForEach(y => this.AddLog(y.RowNumber, errMsg, false));
                                }

                                return first;
                            })
                            .ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Некорректный файл", ex);
            }
        }

        /// <summary>
        /// Обработка строки импорта
        /// </summary>
        /// <param name="data">Файл</param>
        /// <param name="rowNumber">Номер строки</param>
        /// <returns>Record</returns>
        private Record ProcessLine(GkhExcelCell[] data, int rowNumber)
        {
            var record = new Record { isValidRecord = false, RowNumber = rowNumber };

            if (data.Length <= 1)
            {
                return record;
            }

            if (!this.TryExtractRealityObject(record, data) || !this.TryExtractRoomInfo(record, data))
            {
                return record;
            }

            record.isValidRecord = true;
            record.CreateOwner = this.TryExtractOwner(record, data) && this.TryExtractPersonalAccount(record, data);

            return record;
        }

        /// <summary>
        /// Проверка дома
        /// </summary>
        /// <param name="record">Запись</param>
        /// <param name="data">Файл импорта</param>
        /// <returns>Bool</returns>
        private bool TryExtractRealityObject(Record record, GkhExcelCell[] data)
        {
            record.ImportRealtyObjectId = this.GetValue(data, "ID_DOMA").ToLong();

            if (record.ImportRealtyObjectId > 0 && this.existRobjectIds.Contains(record.ImportRealtyObjectId))
            {
                record.RealtyObjectId = record.ImportRealtyObjectId;
            }
            else
            {
                record.House = this.GetValue(data, "HOUSE_NUM");

                if (string.IsNullOrWhiteSpace(record.House))
                {
                    this.AddLog(record.RowNumber, "Не задан номер дома.", false);
                    return false;
                }

                record.Letter = this.GetValue(data, "LITER");
                record.Housing = this.GetValue(data, "KORPUS");
                record.Building = this.GetValue(data, "BUILDING");

                record.Kladr = this.GetValue(data, "KLADRCODE");

                // Если задан код КЛАДР, то адрес формируется исходя из этого кода
                if (!string.IsNullOrWhiteSpace(record.Kladr))
                {
                    var fiasRecord = this.fiasRecords.FirstOrDefault(x => x.CodeRecord == record.Kladr);

                    if (fiasRecord == null)
                    {
                        this.AddLog(record.RowNumber, "По коду КЛАДР не найден адрес", false);
                        return false;
                    }

                    var roId = this.FindRealityObject(record, fiasRecord);

                    if (roId > 0)
                    {
                        record.RealtyObjectId = roId;
                    }
                }

                if (record.RealtyObjectId > 0)
                {
                    // Если нашли дом через адрес, то выходим
                    return true;
                }

                record.MunicipalityName = this.GetValue(data, "MU");
                if (string.IsNullOrWhiteSpace(record.MunicipalityName))
                {
                    this.AddLog(record.RowNumber, "Не задано муниципальное образование.", false);
                    return false;
                }

                record.LocalityName = Simplified(this.GetValue(data, "CITY") + " " + this.GetValue(data, "TYPE_CITY"));

                if (string.IsNullOrWhiteSpace(record.LocalityName))
                {
                    this.AddLog(record.RowNumber, "Не задан населенный пункт.", false);
                    return false;
                }

                record.StreetName = Simplified(this.GetValue(data, "STREET") + " " + this.GetValue(data, "TYPE_STREET"));

                //if (string.IsNullOrWhiteSpace(record.StreetName))
                //{
                //    AddLog(record.RowNumber, "Не задана улица.", false);  бывают дома без заданной улицы
                //    return false;
                //}

                if (!this.realtyObjectsByAddressDict.ContainsKey(record.MunicipalityName.ToLower()))
                {
                    this.AddLog(record.RowNumber,
                        "Не найдены дома в муниципальном образовании: " + record.MunicipalityName, false);
                    return false;
                }

                var municipalityRealtyObjectsDict = this.realtyObjectsByAddressDict[record.MunicipalityName.ToLower()];

                if (!municipalityRealtyObjectsDict.ContainsKey(record.LocalityName.ToLower()))
                {
                    this.AddLog(record.RowNumber,
                        "В указанном МО не найдены дома в населенном пунтке: " + record.LocalityName, false);
                    return false;
                }

                var localityRealtyObjectsDict = municipalityRealtyObjectsDict[record.LocalityName.ToLower()];

                if (!localityRealtyObjectsDict.ContainsKey(record.StreetName.ToLower()))
                {
                    this.AddLog(record.RowNumber,
                        "В указанном населенном пункте не найдены дома на улице: " + record.StreetName, false);
                    return false;
                }

                var houseLetter = string.Format("{0}{1}{2}{3}", record.House, record.Housing, record.Letter, record.Building)
                    .ToLower();

                var realtyObjectsOnStreet =
                    localityRealtyObjectsDict[record.StreetName.ToLower()].Where(x => x.HouseLetter == houseLetter)
                        .ToList();

                if (realtyObjectsOnStreet.Count > 1)
                {
                    realtyObjectsOnStreet = !string.IsNullOrWhiteSpace(record.Building) ||
                                            !string.IsNullOrEmpty(record.Housing) ||
                                            !string.IsNullOrEmpty(record.Letter)
                        ? realtyObjectsOnStreet.Where(
                            x =>
                                !string.IsNullOrWhiteSpace(x.Building) ||
                                !string.IsNullOrEmpty(x.Housing) ||
                                !string.IsNullOrEmpty(x.Letter)).ToList()
                        : realtyObjectsOnStreet.Where(
                            x =>
                                string.IsNullOrWhiteSpace(x.Building) &&
                                string.IsNullOrEmpty(x.Housing) &&
                                string.IsNullOrEmpty(x.Letter)).ToList();
                }

                if (realtyObjectsOnStreet.Count > 1)
                {
                    realtyObjectsOnStreet = realtyObjectsOnStreet.Where(x => x.House == record.House).ToList();
                }

                if (realtyObjectsOnStreet.Count == 0)
                {
                    this.AddLog(record.RowNumber, "Дом не найден в системе", false);
                    return false;
                }

                if (realtyObjectsOnStreet.Count > 1)
                {
                    this.AddLog(record.RowNumber,
                        "Неоднозначный дом. Соответствующих данному набору адресных параметров домов найдено: " +
                        realtyObjectsOnStreet.Count, false);
                    return false;
                }

                record.RealtyObjectId = realtyObjectsOnStreet.First().RoId;
            }

            return true;
        }

        /// <summary>
        /// Проверка помещения
        /// </summary>
        /// <param name="record">Запись</param>
        /// <param name="data">Файл импорта</param>
        /// <returns>Bool</returns>
        private bool TryExtractRoomInfo(Record record, GkhExcelCell[] data)
        {
            record.Apartment = this.GetValue(data, "FLAT_PLACE_NUM");
            record.CadastralNumber = this.GetValue(data, "CADASTRAL_NUM");

            if (string.IsNullOrWhiteSpace(record.Apartment))
            {
                this.AddLog(record.RowNumber, "Не задан номер квартиры", false);
                return false;
            }

            var area = this.GetValue(data, "TOTAL_AREA");

            if (string.IsNullOrWhiteSpace(area))
            {
                this.AddLog(record.RowNumber, "Не задана общая площадь квартиры", false);
                return false;
            }

            if (decimal.TryParse(area.Replace('.', ','), NumberStyles.Number, this.culture, out decimal value) && value > 0)
            {
                record.Area = value;
            }
            else
            {
                this.AddLog(record.RowNumber, "Некорректное число в поле 'TOTAL_AREA': " + area, false);
                return false;
            }

            var roomType = this.GetValue(data, "FLAT_PLACE_TYPE");
            if (string.IsNullOrEmpty(roomType))
            {
                this.AddLog(record.RowNumber, "Не задан тип помещения", false);
                return false;
            }
            switch (roomType.ToLower())
            {
                case "жилое":
                    record.RoomType = RoomType.Living;
                    break;

                case "нежилое":
                    record.RoomType = RoomType.NonLiving;
                    break;

                default:
                    this.AddLog(record.RowNumber, "Неизвестный тип помещения: " + roomType, false);
                    return false;
            }

            var ownershipType = this.GetValue(data, "PROPERTY_TYPE");
            if (string.IsNullOrEmpty(ownershipType))
            {
                this.AddLog(record.RowNumber, "Не задан тип собственности", false);
                return false;
            }
            switch (ownershipType.ToLower())
            {
                case "частная":
                    record.OwnershipType = RoomOwnershipType.Private;
                    break;

                case "муниципальная":
                    record.OwnershipType = RoomOwnershipType.Municipal;
                    break;

                case "государственная":
                    record.OwnershipType = RoomOwnershipType.Goverenment;
                    break;

                case "коммерческая":
                    record.OwnershipType = RoomOwnershipType.Commerse;
                    break;

                case "федеральная":
                    record.OwnershipType = RoomOwnershipType.Federal;
                    break;

                case "областная":
                    record.OwnershipType = RoomOwnershipType.Regional;
                    break;

                case "смешанная":
                    record.OwnershipType = RoomOwnershipType.Mixed;
                    break;

                default:
                    this.AddLog(record.RowNumber, "Неизвестный тип собственности: " + ownershipType, false);
                    return false;
            }

            var livingArea = this.GetValue(data, "LIVE_AREA");

            if (!string.IsNullOrWhiteSpace(livingArea))
            {
                if (decimal.TryParse(livingArea.Replace('.', ','), NumberStyles.Number, this.culture, out value) &&
                    value > 0)
                {
                    record.LivingArea = value;
                }
                else
                {
                    this.AddLog(record.RowNumber, "Некорректное число в поле 'LIVE_AREA': " + livingArea, false);
                }
            }

            this.AddLog(record.RowNumber, "Определена информация о помещении");
            return true;
        }

        /// <summary>
        /// Проверка ЛС
        /// </summary>
        /// <param name="record">Запись</param>
        /// <param name="data">Файл импорта</param>
        /// <returns>Bool</returns>
        private bool TryExtractPersonalAccount(Record record, GkhExcelCell[] data)
        {
            record.AccountNumber = this.GetValue(data, "LS_NUM");

            var areaShare = this.GetValue(data, "SHARE");
            if (string.IsNullOrWhiteSpace(areaShare))
            {
                this.AddLog(record.RowNumber, "Не задана доля собственника", false);
                return false;
            }
            if (decimal.TryParse(areaShare.Replace('.', ','), NumberStyles.Number, this.culture, out decimal areaShareValue))
            {
                record.AreaShare = areaShareValue;
            }
            else
            {
                this.AddLog(record.RowNumber, "Некорректное число в поле 'SHARE': " + areaShare, false);
                return false;
            }

            var persAccNumExternalSystems = this.GetValue(data, "RKC_LS_NUM");
            if (string.IsNullOrWhiteSpace(persAccNumExternalSystems))
            {
                this.AddLog(record.RowNumber, "Не задан номер ЛС в РКЦ", false);
            }
            else
            {
                record.PersAccNumExternalSystems = persAccNumExternalSystems;
            }

            record.RkcIdentifier = this.GetValue(data, "RKC_NUM");

            try
            {
                record.RkcDateStart = this.ToDateTimeNullable(this.GetValue(data, "RKC_START_DATE"));
            }
            catch (FormatException)
            {
                this.AddLog(record.RowNumber, "Неверный формат даты для начала действия договора с РКЦ [RKC_START_DATE]");
            }
            try
            {
                record.RkcDateEnd = this.ToDateTimeNullable(this.GetValue(data, "RKC_END_DATE"));
            }
            catch (FormatException)
            {
                this.AddLog(record.RowNumber, "Неверный формат даты для конца действия договора с РКЦ [RKC_END_DATE]");
            }

            var accountCreateDate = this.GetValue(data, "LS_DATE");

            if (string.IsNullOrWhiteSpace(accountCreateDate))
            {
                this.AddLog(record.RowNumber, "Не указана Дата открытия лицевого счета, присвоена текущая дата", false);
                record.AccountCreateDate = DateTime.Today;
            }
            else
            {

                if (accountCreateDate.Length > 10)
                {
                    accountCreateDate = accountCreateDate.Substring(0, 10);
                }

                if (DateTime.TryParseExact(accountCreateDate, "dd.MM.yyyy", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out DateTime date))
                {
                    record.AccountCreateDate = date;
                }
                else
                {
                    this.AddLog(record.RowNumber,
                        "Некорректная дата в поле 'LS_DATE': " + accountCreateDate +
                        "'. Дата ожидается в формате 'дд.мм.гггг'. Присвоена текущая дата", false);
                    record.AccountCreateDate = DateTime.Today;
                }
            }

            this.AddLog(record.RowNumber, "Определена информация о лицевом счете");
            return true;
        }

        private DateTime? ToDateTimeNullable(string obj, DateTime? defaultValue = null, string format = "dd.MM.yyyy")
        {
            if (obj.Length > 10)
            {
                obj = obj.Substring(0, 10);
            }

            return string.IsNullOrEmpty(obj) ? defaultValue : DateTime.ParseExact(obj, format, CultureInfo.InvariantCulture, DateTimeStyles.None);
        }

        private bool TryExtractOwner(Record record, GkhExcelCell[] data)
        {
            var ownerType = this.GetValue(data, "BILL_TYPE");

            if (string.IsNullOrEmpty(ownerType))
            {
                this.AddLog(record.RowNumber, "Не задан тип собственника", false);
                return false;
            }

            switch (ownerType.ToLower())
            {
                case "счет физ.лица":
                    record.OwnerType = PersonalAccountOwnerType.Individual;
                    break;

                case "счет юр.лица":
                    record.OwnerType = PersonalAccountOwnerType.Legal;
                    break;

                default:
                    this.AddLog(record.RowNumber, "Неизвестный тип собственника: " + ownerType, false);
                    return false;
            }

            if (record.OwnerType == PersonalAccountOwnerType.Individual)
            {
                record.OwnerPhysSurname = this.GetValue(data, "SURNAME");
                record.OwnerPhysFirstName = this.GetValue(data, "NAME");
                record.OwnerPhysSecondName = this.GetValue(data, "LASTNAME");

                var fio =
                    string.Format("{0} {1} {2}", record.OwnerPhysSurname, record.OwnerPhysFirstName,
                        record.OwnerPhysSecondName).Trim();

                if (string.IsNullOrWhiteSpace(fio))
                {
                    this.AddLog(record.RowNumber, "Не задан собственник-физ.лицо", false);
                    return false;
                }

                if (record.OwnerPhysSurname != null && record.OwnerPhysSurname.Length > 100)
                {
                    this.AddLog(record.RowNumber,
                        string.Format("Слишком длинная фамилия абонента (лимит - 100):{0}", record.OwnerPhysSurname),
                        false);
                    return false;
                }

                if (record.OwnerPhysFirstName != null && record.OwnerPhysFirstName.Length > 100)
                {
                    this.AddLog(record.RowNumber,
                        string.Format("Слишком длинное имя абонента (лимит - 100):{0}", record.OwnerPhysFirstName),
                        false);
                    return false;
                }

                if (record.OwnerPhysSecondName != null && record.OwnerPhysSecondName.Length > 100)
                {
                    this.AddLog(record.RowNumber,
                        string.Format("Слишком длинное отчество абонента (лимит - 100):{0}", record.OwnerPhysSecondName),
                        false);
                    return false;
                }
            }
            else
            {
                record.OwnerJurInn = this.GetValue(data, "INN");

                if (string.IsNullOrEmpty(record.OwnerJurInn))
                {
                    this.AddLog(record.RowNumber, "Не задан ИНН.", false);
                    return false;
                }

                record.OwnerJurKpp = this.GetValue(data, "KPP");
                record.OwnerJurName = this.GetValue(data, "RENTER_NAME");

                if (string.IsNullOrEmpty(record.OwnerJurName))
                {
                    this.AddLog(record.RowNumber, "Не задан собственник-юр.лицо", false);
                    return false;
                }
            }

            this.AddLog(record.RowNumber, "Определена информация о собственнике");
            return true;
        }

        private GkhExcelCell[] GetHeaderIdsRow(IEnumerable<GkhExcelCell[]> rows)
        {
            try
            {
                return rows.First(row => row.FirstOrDefault(cell => cell.Value == HeaderIdDoma) != null);
            }
            catch (Exception)
            {
                throw new Exception("Импортируемый файл не соответствует формату");
            }
        }

        private long FindRealityObject(Record record, FiasProxy fiasRecord)
        {
            List<FiasAddressProxy> fiasAddress;

            if (fiasRecord.AOLevel == FiasLevelEnum.Street)
            {
                fiasAddress = this.fiasAddresList.Where(x => x.StreetGuidId == fiasRecord.AOGuid).ToList();
            }
            else
            {
                fiasAddress = this.fiasAddresList.Where(x => x.PlaceGuidId == fiasRecord.AOGuid).ToList();
            }

            var houseLetter = string.Format("{0}{1}{2}{3}", record.House, record.Housing, record.Letter, record.Building);

            fiasAddress = fiasAddress.Where(x => x.HouseLetter == houseLetter).ToList();

            if (fiasAddress.Count == 0)
            {
                this.AddLog(record.RowNumber, "Дом не найден в системе", false);
                return 0L;
            }

            if (fiasAddress.Count > 1)
            {
                this.AddLog(record.RowNumber,
                    "Неоднозначный дом. Соответствующих данному набору адресных параметров домов найдено: " +
                    fiasAddress.Count, false);
                return 0L;
            }

            var fiasAddressId = fiasAddress.Select(x => x.Id).FirstOrDefault();

            var ro = this.RealityObjectDomain.GetAll()
                .Where(x => x.FiasAddress.Id == fiasAddressId)
                .FirstOrDefault();

            if (ro != null)
            {
                return ro.Id;
            }

            return 0L;
        }
    }
}