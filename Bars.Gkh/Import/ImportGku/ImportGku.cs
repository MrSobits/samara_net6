namespace Bars.Gkh.Import
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using B4;
    using B4.Utils;
    using Entities;
    using Entities.Hcs;
    using Enums.Import;
    using Impl;
    using Ionic.Zip;
    using Castle.Windsor;
    using B4.DataAccess;

    public class ImportGku : GkhImportBase
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        #region Properties

        public override string Key
        {
            get { return Id; }
        }

        public override string CodeImport
        {
            get { return "ImportGku"; }
        }

        public override string Name
        {
            get { return "Импорт сведений по ЖКУ"; }
        }

        public override string PossibleFileExtensions
        {
            get { return "zip"; }
        }

        public override string PermissionName
        {
            get { return "Import.Gku.View"; }
        }

        public IWindsorContainer Container { get; set; }

        public ILogImportManager LogManager { get; set; }

        public ILogImport LogImport { get; set; }

        public ILogImportManager LogImportManager { get; set; }

        public IRepository<RealityObject> RepRobject { get; set; }

        public IRepository<HouseInfoOverview> RepHouseInfo { get; set; }

        public IRepository<HouseAccount> RepHouseAccount { get; set; }

        public IRepository<HouseAccountCharge> RepHouseAccountCharge { get; set; }

        public IRepository<MeterReading> RepMeterReading { get; set; }

        public IRepository<HouseMeterReading> RepHouseMeterReading { get; set; }

        public IRepository<HouseOverallBalance> RepHouseOverallBalance { get; set; }

        #endregion Properties

        #region Fields

        //словарь код ЕРЦ - []{Id, Address} жилого дома
        private Dictionary<string, RobjectProxy[]> dictRealityObject;

        //словарь платежный код - идентификатор счета
        private Dictionary<string, HouseAccountProxy[]> dictHouseAccount;

        #endregion Fields

        public override bool Validate(BaseParams baseParams, out string message)
        {
            message = null;
            if (!baseParams.Files.ContainsKey("FileImport"))
            {
                message = "Не выбран файл для импорта";
                return false;
            }

            var fileData = baseParams.Files["FileImport"];
            var extention = fileData.Extention;

            var fileExtentions = PossibleFileExtensions.Contains(",")
                ? PossibleFileExtensions.Split(',')
                : new[] {PossibleFileExtensions};
            if (fileExtentions.All(x => x != extention))
            {
                message = string.Format("Необходимо выбрать файл с допустимым расширением: {0}", PossibleFileExtensions);
                return false;
            }

            return true;
        }

        public override ImportResult Import(BaseParams baseParams)
        {
            var sw = new Stopwatch();
            sw.Start();

            var file = baseParams.Files["FileImport"];

            using (var zipfileMemoryStream = new MemoryStream(file.Data))
            {
                using (var zipFile = ZipFile.Read(zipfileMemoryStream))
                {
                    var zipEntries = zipFile.Where(x => x.FileName.EndsWith(".csv")).ToArray();

                    if (zipEntries.Length < 1)
                    {
                        LogImport.Error("Ошибка", "Отсутствуют файлы для импорта");
                        return new ImportResult(StatusImport.CompletedWithError, "Отсутствуют файлы для импорта");
                    }

                    this.ImportInternal(zipEntries, zipFile.Name);
                }
            }

            LogImport.SetFileName(file.FileName);
            LogImport.ImportKey = this.CodeImport;

            LogImportManager.FileNameWithoutExtention = file.FileName;
            LogImportManager.Add(file, LogImport);
            LogImportManager.Save();

            var statusImport = LogImport.CountError > 0
                ? StatusImport.CompletedWithError
                : LogImport.CountWarning > 0
                    ? StatusImport.CompletedWithWarning
                    : StatusImport.CompletedWithoutError;

            sw.Stop();

            return new ImportResult(statusImport, string.Format("time elapsed: {0} ms;", sw.ElapsedMilliseconds));
        }

        private void ImportInternal(ZipEntry[] zipEntries, string archiveName)
        {
            dictRealityObject = RepRobject.GetAll()
                .Where(x => x.CodeErc != null)
                .Select(x => new
                {
                    x.Id,
                    x.Address,
                    x.CodeErc
                })
                .AsEnumerable()
                .GroupBy(x => x.CodeErc)
                .ToDictionary(x => x.Key,
                    y => y.Select(x => new RobjectProxy {Id = x.Id, Address = x.Address}).ToArray());

            var domFile = zipEntries.FirstOrDefault(x => x.FileName.Contains("dom.csv"));

            if (domFile != null)
            {
                using (var ms = new MemoryStream())
                {
                    domFile.Extract(ms);
                    ms.Seek(0, SeekOrigin.Begin);
                    this.ImportHouseInfo(ms, domFile.FileName);
                }
            }
            else
            {
                LogImport.Info(archiveName, "Отсутствует файл dom.csv");
            }

            var schetFile = zipEntries.FirstOrDefault(x => x.FileName.Contains("l_schet.csv"));

            if (schetFile != null)
            {
                using (var ms = new MemoryStream())
                {
                    schetFile.Extract(ms);
                    ms.Seek(0, SeekOrigin.Begin);
                    this.ImportAccount(ms, schetFile.FileName);
                }
            }
            else
            {
                LogImport.Info(archiveName, "Отсутствует файл l_schet.csv");
            }

            dictHouseAccount = RepHouseAccount.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.PaymentCode,
                    RoId = x.RealityObject.Id,
                    x.RealityObject.Address
                })
                .AsEnumerable()
                .GroupBy(x => x.PaymentCode)
                .ToDictionary(x => x.Key, y => y.Select(x => new HouseAccountProxy { Id = x.Id, RoId = x.RoId, Address = x.Address }).ToArray());

            var filesCharge = zipEntries.Where(x => x.FileName.Contains("charge_")).ToArray();

            if (filesCharge.Length > 0)
            {
                this.ImportCharges(filesCharge);
            }

            var meterDeviceHouseFile = zipEntries.FirstOrDefault(x => x.FileName.Contains("pribor_ucheta_dom.csv"));

            if (meterDeviceHouseFile != null)
            {
                using (var ms = new MemoryStream())
                {
                    meterDeviceHouseFile.Extract(ms);
                    ms.Seek(0, SeekOrigin.Begin);
                    this.ImportRobjectMeteringDevice(ms, meterDeviceHouseFile.FileName);
                }
            }

            var meterDeviceFile = zipEntries.FirstOrDefault(x => x.FileName.Contains("pribor_ucheta.csv"));

            if (meterDeviceFile != null)
            {
                using (var ms = new MemoryStream())
                {
                    meterDeviceFile.Extract(ms);
                    ms.Seek(0, SeekOrigin.Begin);
                    this.ImportMeteringDevice(ms, meterDeviceFile.FileName);
                }
            }

            var filesOdn = zipEntries.Where(x => x.FileName.Contains("odn_")).ToArray();

            if (filesOdn.Length > 0)
            {
                this.ImportOdn(filesOdn);
            }
                    }

        private void ImportHouseInfo(Stream ms, string fileName)
        {
            var dictHeaders = new Dictionary<string, int>
            {
                {"town_rajon", -1},
                {"nas_punkt", -1},
                {"ulica", -1},
                {"ndom", -1},
                {"nkor", -1},
                {"dom_code", -1},
                {"count_ls", -1},
                {"count_fl", -1},
                {"count_fl_nanim", -1},
                {"count_fl_sobst", -1},
                {"count_jur", -1},
                {"count_jur_nanim", -1},
                {"count_jur_sobst", -1}
            };

            var existRecs = RepHouseInfo.GetAll()
                .Where(x => x.RealityObject.CodeErc != null)
                .Select(x => new
                    {
                        x.Id,
                        RoId = x.RealityObject.Id
                    })
                .AsEnumerable()
                .ToDictionary(x => x.RoId, y => y.Id);

            var records = new List<HouseInfoOverview>();
                using (var sr = new StreamReader(ms, Encoding.GetEncoding(1251)))
                {
                    var headersLine = sr.ReadLine();

                    if (!ReadHeaders(fileName, headersLine, dictHeaders))
                    {
                        return;
                    }

                    if (dictHeaders["dom_code"] == -1)
                    {
                        LogImport.Error(fileName, "Отсутствует столбец dom_code, загрузка не выполнена");
                        return;
                    }

                    int rowNumber = 0;

                    while (!sr.EndOfStream)
                    {
                        rowNumber++;
                        var line = sr.ReadLine();

                        if (string.IsNullOrWhiteSpace(line))
                        {
                            continue;
                        }

                        var row = line.Split(';').Select(x => (x ?? "").Trim('"', ' ')).ToArray();

                        var domCode = GetValue("dom_code", row, dictHeaders);

                        if (!CheckHouse(domCode, fileName, rowNumber))
                        {
                            continue;
                        }

                        var countFl = GetValue("count_fl", row, dictHeaders).ToInt();
                        var countFlSobst = GetValue("count_fl_sobst", row, dictHeaders).ToInt();
                        var countFlNanim = GetValue("count_fl_nanim", row, dictHeaders).ToInt();

                        var countJur = GetValue("count_jur", row, dictHeaders).ToInt();
                        var countJurSobst = GetValue("count_jur_sobst", row, dictHeaders).ToInt();
                        var countJurNanim = GetValue("count_jur_nanim", row, dictHeaders).ToInt();

                        var roId = dictRealityObject[domCode][0].Id;

                        HouseInfoOverview rec;

                        if (existRecs.ContainsKey(roId))
                        {
                            LogImport.CountChangedRows++;
                            rec = RepHouseInfo.Load(existRecs[roId]);
                        }
                        else
                        {
                            LogImport.CountAddedRows++;
                            rec = new HouseInfoOverview
                            {
                                RealityObject = this.RepRobject.Load(roId)
                            };
                        }

                        rec.IndividualAccountsCount = countFl;
                        rec.IndividualOwnerAccountsCount = countFlSobst;
                        rec.IndividualTenantAccountsCount = countFlNanim;

                        rec.LegalAccountsCount = countJur;
                        rec.LegalOwnerAccountsCount = countJurSobst;
                        rec.LegalTenantAccountsCount = countJurNanim;

                        records.Add(rec);
                    }
                }

                this.SaveOrUpdate(records, RepHouseInfo);
                records.Clear();
                existRecs.Clear();
            }

        private void ImportAccount(Stream ms, string fileName)
        {
            var dictHeaders = new Dictionary<string, int>
            {
                {"dom_code", -1},
                {"pkod", -1},
                {"fio", -1},
                {"nkvar", -1},
                {"progiv_gil", -1},
                {"propis_gil", -1},
                {"all_pl", -1},
                {"gil_pl", -1},
                {"kol_komnat", -1},
                {"sost_sceta", -1},
                {"privatiz", -1},
                {"vremenno_ubiv", -1},
                {"gil_negil", -1}
            };

            var existRecs = RepHouseAccount.GetAll()
                .Where(x => x.RealityObject.CodeErc != null)
                .Select(x => new
                {
                    RoId = x.RealityObject.Id,
                    x.Id,
                    x.PaymentCode
                })
                .AsEnumerable()
                .GroupBy(x => x.RoId)
                .ToDictionary(x => x.Key,
                    y => y.GroupBy(x => x.PaymentCode)
                        .ToDictionary(x => x.Key, z => z.Select(x => x.Id).ToArray()));

            var records = new List<HouseAccount>();
                using (var sr = new StreamReader(ms, Encoding.GetEncoding(1251)))
                {
                    var headersLine = sr.ReadLine();

                    if (!ReadHeaders(fileName, headersLine, dictHeaders))
                    {
                        return;
                    }

                    int rowNumber = 0;

                    while (!sr.EndOfStream)
                    {
                        rowNumber++;

                        var line = sr.ReadLine();

                        if (string.IsNullOrWhiteSpace(line))
                        {
                            continue;
                        }

                        var row = line.Split(';').Select(x => (x ?? "").Trim('"', ' ')).ToArray();

                        var domCode = GetValue("dom_code", row, dictHeaders);

                        if (!CheckHouse(domCode, fileName, rowNumber))
                        {
                            continue;
                        }

                        var roId = dictRealityObject[domCode][0].Id;

                        var pkod = GetValue("pkod", row, dictHeaders).ToStr();

                        HouseAccount rec;

                        if (existRecs.ContainsKey(roId) && existRecs[roId].ContainsKey(pkod))
                        {
                            LogImport.CountChangedRows++;
                            rec = RepHouseAccount.Load(existRecs[roId][pkod][0]);
                        }
                        else
                        {
                            LogImport.CountAddedRows++;
                            rec = new HouseAccount
                            {
                                RealityObject = RepRobject.Load(roId),
                                PaymentCode = pkod.ToStr()
                            };
                        }

                        var nkvar = GetValue("nkvar", row, dictHeaders).ToInt();

                        if (nkvar == 0)
                        {
                            LogImport.Warn(fileName, string.Format("Неверный номер квартиры, строка {0}", rowNumber));
                        }
                        else
                        {
                            rec.Apartment = nkvar;
                        }

                        rec.ResidentsCount = GetValue("propis_gil", row, dictHeaders).ToInt();
                        rec.ApartmentArea = GetValue("all_pl", row, dictHeaders).ToDecimal();
                        rec.LivingArea = GetValue("gil_pl", row, dictHeaders).ToDecimal();
                        rec.RoomsCount = GetValue("kol_komnat", row, dictHeaders).ToInt();
                        rec.AccountState = GetValue("sost_sceta", row, dictHeaders);
                        rec.Privatizied = GetValue("privatiz", row, dictHeaders).ToLower() == "да";
                        rec.TemporaryGoneCount = GetValue("vremenno_ubiv", row, dictHeaders).ToInt();
                        rec.Living = GetValue("gil_negil", row, dictHeaders).ToInt() == 0;

                        records.Add(rec);
                    }
                }

                this.SaveOrUpdate(records, RepHouseAccount);
                records.Clear();
                existRecs.Clear();
        }

        private void ImportCharges(IEnumerable<ZipEntry> entries)
        {
            var existRec = RepHouseAccountCharge.GetAll()
                .Where(x => x.CompositeKey != null)
                .GroupBy(x => x.CompositeKey)
                .ToDictionary(x => x.Key, y => 0);

            var dictHeaders = new Dictionary<string, int>
            {
                {"pkod", -1},
                {"charge_date", -1},
                {"service_name", -1},
                {"(sum)", -1},
                {"rashod", -1},
                {"full_rascet", -1},
                {"nedop", -1},
                {"nachisleno", -1},
                {"pereraschet", -1},
                {"vhod_saldo", -1},
                {"izmenen", -1},
                {"oplata", -1},
                {"k_oplate", -1},
                {"vihod_saldo", -1}
            };

            var records = new List<HouseAccountCharge>();

                foreach (var zipEntry in entries)
                {
                    using (var ms = new MemoryStream())
                    {
                        zipEntry.Extract(ms);
                        ms.Seek(0, SeekOrigin.Begin);

                        var fileName = zipEntry.FileName;

                        using (var sr = new StreamReader(ms, Encoding.GetEncoding(1251)))
                        {
                            var headersLine = sr.ReadLine();

                            if (!ReadHeaders(fileName, headersLine, dictHeaders))
                            {
                                continue;
                            }

                            int rowNumber = 0;

                            while (!sr.EndOfStream)
                            {
                                rowNumber++;

                                var line = sr.ReadLine();

                                if (string.IsNullOrWhiteSpace(line))
                                {
                                    continue;
                                }

                                var row = line.Split(';').Select(x => x.Trim('"', ' ')).ToArray();

                                var pkod = GetValue("pkod", row, dictHeaders).ToStr();

                                if (!CheckHouseAccount(pkod, fileName, rowNumber))
                                {
                                    continue;
                                }

                                var chargeDate = GetValue("charge_date", row, dictHeaders).ToDateTime();
                                var serviceName = GetValue("service_name", row, dictHeaders).ToLower().Replace(" ", "");
                                var compositeKey = GetCompositeKey(pkod, chargeDate.ToString("yy-MM-dd"), serviceName);

                                if (existRec.ContainsKey(compositeKey))
                                {
                                    continue;
                                }

                                LogImport.CountAddedRows++;

                                var houseAccount = dictHouseAccount[pkod][0];

                                var rec = new HouseAccountCharge
                                {
                                    CompositeKey = compositeKey,
                                    DateCharging = chargeDate,
                                    Service = GetValue("service_name", row, dictHeaders),
                                    Tariff = GetValue("(sum)", row, dictHeaders).ToDecimal(),
                                    Expense = GetValue("rashod", row, dictHeaders).ToDecimal(),
                                    CompleteCalc = GetValue("full_rascet", row, dictHeaders).ToDecimal(),
                                    Underdelivery = GetValue("nedop", row, dictHeaders).ToDecimal(),
                                    Charged = GetValue("nachisleno", row, dictHeaders).ToDecimal(),
                                    Recalc = GetValue("pereraschet", row, dictHeaders).ToDecimal(),
                                    InnerBalance = GetValue("vhod_saldo", row, dictHeaders).ToDecimal(),
                                    Changed = GetValue("izmenen", row, dictHeaders).ToDecimal(),
                                    Payment = GetValue("oplata", row, dictHeaders).ToDecimal(),
                                    ChargedPayment = GetValue("k_oplate", row, dictHeaders).ToDecimal(),
                                    OuterBalance = GetValue("vihod_saldo", row, dictHeaders).ToDecimal(),
                                    Account = RepHouseAccount.Load(houseAccount.Id),
                                    RealityObject = RepRobject.Load(houseAccount.RoId)
                                };

                                records.Add(rec);

                                existRec.Add(compositeKey, 0);
                            }
                        }
                    }
                }

                this.SaveOrUpdate(records, Container.Resolve<IRepository<HouseAccountCharge>>());
                records.Clear();
                existRec.Clear();
        }

        private void ImportMeteringDevice(Stream ms, string fileName)
        {
            var dictHeaders = new Dictionary<string, int>
            {
                {"pkod", -1},
                {"service_name", -1},
                {"pribor_number", -1},
                {"pribor_type", -1},
                {"data_snyatiya_tek", -1},
                {"data_snyatiya_pred", -1},
                {"pokazanie_pred", -1},
                {"pokazanie_tek", -1},
                {"rashod", -1},
                {"plan_rashod", -1}
            };

            var existRecs = RepMeterReading.GetAll()
                .Where(x => x.CompositeKey != null)
                .GroupBy(x => x.CompositeKey)
                .ToDictionary(x => x.Key, y => 0);

            var records = new List<MeterReading>();

                using (var sr = new StreamReader(ms, Encoding.GetEncoding(1251)))
                {
                    var headersLine = sr.ReadLine();

                    if (!ReadHeaders(fileName, headersLine, dictHeaders))
                    {
                        return;
                    }

                    int rowNumber = 0;

                    while (!sr.EndOfStream)
                    {
                        rowNumber++;

                        var line = sr.ReadLine();

                        if (string.IsNullOrWhiteSpace(line))
                        {
                            continue;
                        }

                        var row = line.Split(';').Select(x => x.Trim('"', ' ')).ToArray();

                        var pkod = GetValue("pkod", row, dictHeaders).ToStr();

                        if (!CheckHouseAccount(pkod, fileName, rowNumber))
                        {
                            continue;
                        }

                        var serviceName = GetValue("service_name", row, dictHeaders).ToLower().Replace(" ", "");
                        var priborNumber = GetValue("pribor_number", row, dictHeaders).ToLower().Replace(" ", "");
                        var compositeKey = GetCompositeKey(pkod, priborNumber, serviceName);

                        if (existRecs.ContainsKey(compositeKey))
                        {
                            continue;
                        }

                        LogImport.CountAddedRows++;

                        var houseAccount = dictHouseAccount[pkod][0];

                        var rec = new MeterReading
                        {
                            CompositeKey = compositeKey,
                            MeterSerial = GetValue("pribor_number", row, dictHeaders),
                            Service = GetValue("service_name", row, dictHeaders),
                            MeterType = GetValue("pribor_type", row, dictHeaders),
                            CurrentReadingDate = GetValue("data_snyatiya_tek", row, dictHeaders).ToDateTime(),
                            PrevReadingDate = GetValue("data_snyatiya_pred", row, dictHeaders).ToDateTime(),
                            CurrentReading = GetValue("pokazanie_tek", row, dictHeaders).ToDecimal(),
                            PrevReading = GetValue("pokazanie_pred", row, dictHeaders).ToDecimal(),
                            Expense = GetValue("rashod", row, dictHeaders).ToDecimal(),
                            PlannedExpense = GetValue("plan_rashod", row, dictHeaders).ToDecimal(),
                            RealityObject = RepRobject.Load(houseAccount.RoId),
                            Account = RepHouseAccount.Load(houseAccount.Id)
                        };

                        records.Add(rec);
                    }
                }

                this.SaveOrUpdate(records, RepMeterReading);
                records.Clear();
                existRecs.Clear();
        }

        private void ImportRobjectMeteringDevice(Stream ms, string fileName)
        {
            var existRecs = RepHouseMeterReading.GetAll()
                .Where(x => x.CompositeKey != null)
                .GroupBy(x => x.CompositeKey)
                .ToDictionary(x => x.Key, y => 0);

            var records = new List<HouseMeterReading>();

                var dictHeaders = new Dictionary<string, int>
                {
                    {"dom_code", -1},
                    {"service_name", -1},
                    {"pribor_number", -1},
                    {"pribor_type", -1},
                    {"data_snyatiya_tek", -1},
                    {"data_snyatiya_pred", -1},
                    {"pokazanie_pred", -1},
                    {"pokazanie_tek", -1},
                    {"rashod", -1},
                    {"rashod_po_negilim", -1},
                    {"plan_rashod", -1}
                };

                using (var sr = new StreamReader(ms, Encoding.GetEncoding(1251)))
                {
                    var headersLine = sr.ReadLine();

                    if (!ReadHeaders(fileName, headersLine, dictHeaders))
                    {
                        return;
                    }

                    int rowNumber = 0;

                    while (!sr.EndOfStream)
                    {
                        rowNumber++;

                        var line = sr.ReadLine();

                        if (string.IsNullOrWhiteSpace(line))
                        {
                            continue;
                        }

                        var row = line.Split(';').Select(x => x.Trim('"', ' ')).ToArray();

                        var domCode = GetValue("dom_code", row, dictHeaders);

                        if (!this.CheckHouse(domCode, fileName, rowNumber))
                        {
                            continue;
                        }

                        var serviceName = GetValue("service_name", row, dictHeaders).ToLower().Replace(" ", "");
                        var priborNumber = GetValue("pribor_number", row, dictHeaders).ToLower().Replace(" ", "");
                        var compositeKey = GetCompositeKey(domCode, priborNumber, serviceName);

                        if (existRecs.ContainsKey(compositeKey))
                        {
                            continue;
                        }

                        LogImport.CountAddedRows++;

                        var rec = new HouseMeterReading
                        {
                            CompositeKey = compositeKey,
                            MeterSerial = GetValue("pribor_number", row, dictHeaders),
                            Service = GetValue("service_name", row, dictHeaders),
                            MeterType = GetValue("pribor_type", row, dictHeaders),
                            CurrentReadingDate = GetValue("data_snyatiya_tek", row, dictHeaders).ToDateTime(),
                            PrevReadingDate = GetValue("data_snyatiya_pred", row, dictHeaders).ToDateTime(),
                            CurrentReading = GetValue("pokazanie_tek", row, dictHeaders).ToDecimal(),
                            PrevReading = GetValue("pokazanie_pred", row, dictHeaders).ToDecimal(),
                            Expense = GetValue("rashod", row, dictHeaders).ToDecimal(),
                            NonLivingExpense = GetValue("rashod_po_negilim", row, dictHeaders).ToDecimal(),
                            RealityObject = RepRobject.Load(dictRealityObject[domCode][0].Id)
                        };

                        records.Add(rec);
                    }
                }

                this.SaveOrUpdate(records, RepHouseMeterReading);
                records.Clear();
                existRecs.Clear();
        }

        private void ImportOdn(IEnumerable<ZipEntry> entries)
        {
            var dictHeaders = new Dictionary<string, int>
            {
                {"dom_code", -1},
                {"charge_date", -1},
                {"service_name", -1},
                {"vhod_saldo", -1},
                {"nachisleno", -1},
                {"k_oplate", -1},
                {"oplata", -1},
                {"vihod_saldo", -1},
                {"koef_korr", -1},
                {"rash_dom", -1},
                {"rash_ls", -1}
            };

            var existRecs = RepHouseOverallBalance.GetAll()
                .Where(x => x.CompositeKey != null)
                .GroupBy(x => x.CompositeKey)
                .ToDictionary(x => x.Key, y => 0);

            var records = new List<HouseOverallBalance>();

                foreach (var zipEntry in entries)
                {
                    using (var ms = new MemoryStream())
                    {
                        zipEntry.Extract(ms);
                        ms.Seek(0, SeekOrigin.Begin);

                        var fileName = zipEntry.FileName;

                        using (var sr = new StreamReader(ms, Encoding.GetEncoding(1251)))
                        {
                            var headersLine = sr.ReadLine();

                            if (!ReadHeaders(fileName, headersLine, dictHeaders))
                            {
                                return;
                            }

                            int rowNumber = 0;

                            while (!sr.EndOfStream)
                            {
                                rowNumber++;

                                var line = sr.ReadLine();

                                if (string.IsNullOrWhiteSpace(line))
                                {
                                    continue;
                                }

                                var row = line.Split(';').Select(x => (x ?? "").Trim('"', ' ')).ToArray();

                                var domCode = GetValue("dom_code", row, dictHeaders);

                                if (!this.CheckHouse(domCode, fileName, rowNumber))
                                {
                                    continue;
                                }

                                var chargeDate = GetValue("charge_date", row, dictHeaders).ToDateTime();
                                var serviceName = GetValue("service_name", row, dictHeaders).ToLower().Replace(" ", "");

                                var compositeKey = GetCompositeKey(domCode, chargeDate.ToString("yy-MM-dd"), serviceName);

                                if (existRecs.ContainsKey(compositeKey))
                                {
                                    continue;
                                }

                                LogImport.CountAddedRows++;

                                var rec = new HouseOverallBalance
                                {
                                    CompositeKey = compositeKey,
                                    DateCharging = chargeDate,
                                    Service = GetValue("service_name", row, dictHeaders),
                                    MonthCharge = GetValue("nachisleno", row, dictHeaders).ToDecimal(),
                                    Payment = GetValue("k_oplate", row, dictHeaders).ToDecimal(),
                                    Paid = GetValue("oplata", row, dictHeaders).ToDecimal(),
                                    InnerBalance = GetValue("vhod_saldo", row, dictHeaders).ToDecimal(),
                                    OuterBalance = GetValue("vihod_saldo", row, dictHeaders).ToDecimal(),
                                    CorrectionCoef = GetValue("koef_korr", row, dictHeaders).ToDecimal(),
                                    HouseExpense = GetValue("rash_dom", row, dictHeaders).ToDecimal(),
                                    AccountsExpense = GetValue("rash_ls", row, dictHeaders).ToDecimal(),
                                    RealityObject = RepRobject.Load(dictRealityObject[domCode][0].Id)
                                };

                                records.Add(rec);

                                existRecs.Add(compositeKey, 0);
                            }
                        }
                    }
                }

                this.SaveOrUpdate(records, RepHouseOverallBalance);
                records.Clear();
                existRecs.Clear();
        }

        #region Utils
        
        private bool CheckHouseAccount(string paymentCode, string fileName, int rowNumber)
        {
            if (!dictHouseAccount.ContainsKey(paymentCode))
            {
                LogImport.Warn(fileName,
                    string.Format("Не найден лицевой счет с номером {0}, строка {1}",
                        paymentCode,
                        rowNumber));

                return false;
            }

            /*проверяем количество домов*/
            if (dictHouseAccount[paymentCode].Length > 1)
            {
                var strAddrs =
                    dictHouseAccount[paymentCode]
                        .Aggregate("", (x, y) => x + (string.IsNullOrEmpty(x) ? ", " + y.Address : y.Address));

                LogImport.Warn(fileName, string.Format("По лицевому счету {0} найдены дома {1}, строка {2}", paymentCode, strAddrs, rowNumber));
                return false;
            }

            return true;
        }

        private bool CheckHouse(string codeErc, string fileName, int rowNumber)
        {
            /*проверка наличия дома с таким кодом ЕРЦ*/
            if (!dictRealityObject.ContainsKey(codeErc))
            {
                LogImport.Warn(fileName, string.Format("Не найден дом с кодом ЕРЦ {0}, строка {1}", codeErc, rowNumber));
                return false;
            }

            /*проверяем количество домов*/
            if (dictRealityObject[codeErc].Length > 1)
            {
                var strAddrs =
                    dictRealityObject[codeErc]
                        .Aggregate("", (x, y) => x + (string.IsNullOrEmpty(x) ? ", " + y.Address : y.Address));

                LogImport.Warn(fileName, string.Format("По коду {0} найдены дома {1}, строка {2}", codeErc, strAddrs, rowNumber));
                return false;
            }

            return true;
        }

        private bool ReadHeaders(string fileName, string headersLine, Dictionary<string, int> dictHeaders)
        {
            if (string.IsNullOrWhiteSpace(headersLine))
            {
                LogImport.Error(fileName, "Отсутствует строка заголовков, загрузка не выполнена");
                return false;
            }

            var headers = headersLine.Split(';').Select(x => x.Trim(new[] { '"', ' ' })).ToArray();

            for (int i = 0; i < headers.Length; i++)
            {
                if (dictHeaders.ContainsKey(headers[i]))
                {
                    dictHeaders[headers[i]] = i;
                }
            }

            return true;
        }

        private static string GetValue(string key, string[] row, Dictionary<string, int> dictHeaders)
        {
            return dictHeaders[key] > -1 ? row[dictHeaders[key]] : "";
        }

        private static string GetCompositeKey(object arg1, object arg2, object arg3)
        {
            return string.Format("{0}#{1}#{2}", arg1, arg2, arg3);
        }

        private void SaveOrUpdate(IEnumerable<IEntity> entities, IRepository repository)
        {
            var listTransaction = new List<IEntity>();

            var i = 0;

            entities = entities.ToArray();

            var count = entities.Count();

            foreach (var entity in entities)
            {
                i++;
                listTransaction.Add(entity);

                if (i%1000 == 0 || i == count)
                {
                    this.InTransaction(() =>
                    {
                        foreach (var entity1 in listTransaction)
                        {
                            if ((int) entity1.Id > 0)
                                repository.Update(entity1);
                            else
                                repository.Save(entity1);
                        }
                    });

                    listTransaction.Clear();
                }
            }
        }

        private void InTransaction(Action action)
        {
            using (var transaction = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    action();

                    transaction.Commit();
                }
                catch (Exception exc)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (TransactionRollbackException ex)
                    {
                        throw new DataAccessException(ex.Message, exc);
                    }
                    catch (Exception e)
                    {
                        throw new DataAccessException(
                            string.Format(
                                "Произошла неизвестная ошибка при откате транзакции: \r\nMessage: {0}; \r\nStackTrace:{1};",
                                e.Message,
                                e.StackTrace),
                            exc);
                    }

                    throw;
                }
            }
        }

        #endregion Utils

        private struct RobjectProxy
        {
            public long Id { get; set; }
            public string Address { get; set; }
        }

        private struct HouseAccountProxy
        {
            public long Id { get; set; }
            public string Address { get; set; }
            public long RoId { get; set; }
        }
    }
}