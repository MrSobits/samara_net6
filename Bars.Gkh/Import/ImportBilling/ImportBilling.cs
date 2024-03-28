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

    using Bars.Gkh.Enums;

    using Entities;
    using Entities.Hcs;
    using Enums.Import;
    using Castle.Windsor;
    using B4.DataAccess;
    using Impl;
    using NHibernate;

    public class ImportBilling : GkhImportBase
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        #region Properties

        public override string Key
        {
            get { return Id; }
        }

        public override string CodeImport
        {
            get { return "ImportBilling"; }
        }

        public override string Name
        {
            get { return "Импорт данных по начислениям лицевых счетов из Биллинга"; }
        }

        public override string PossibleFileExtensions
        {
            get { return "csv"; }
        }

        public override string PermissionName
        {
            get { return "Import.Billing.View"; }
        }

        public IWindsorContainer Container { get; set; }

        public ILogImportManager LogManager { get; set; }

        public ILogImport LogImport { get; set; }

        public ILogImportManager LogImportManager { get; set; }

        public IDomainService<RealityObject> RepRobject { get; set; }

        public IDomainService<HouseAccount> RepHouseAccount { get; set; }

        public IDomainService<HouseAccountCharge> RepHouseAccountCharge { get; set; }

        #endregion Properties

        #region Fields

        //словарь код ЕРЦ - []{Id, Address} жилого дома
        private Dictionary<string, RobjectProxy> dictRealityObject;

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

            var fileData = baseParams.Files["FileImport"];

            this.dictRealityObject = this.RepRobject.GetAll()
                .Where(x => x.CodeErc != null && x.CodeErc != string.Empty)
                .Select(x => new
                {
                    x.Id,
                    x.Address,
                    x.CodeErc
                })
                .AsEnumerable()
                .GroupBy(x => x.CodeErc)
                .ToDictionary(x => x.Key, y => y.Select(x => new RobjectProxy { Id = x.Id, Address = x.Address }).FirstOrDefault());


            var dictCurrentAccount =
                this.RepHouseAccount.GetAll()
                    .Where(
                        x =>
                        x.RealityObject != null && x.RealityObject.CodeErc != null
                        && x.RealityObject.CodeErc != string.Empty)
                    .Where(x => x.PaymentCode != null && x.HouseAccountNumber != null)
                    .ToDictionary(x => x.Id);

            var dictRealityObjectAccount = this.RepHouseAccount.GetAll()
                .Where(x => x.RealityObject != null && x.RealityObject.CodeErc != null && x.RealityObject.CodeErc != string.Empty)
                .Where(x => x.PaymentCode != null && x.HouseAccountNumber != null)
                .Select(x => new
                {
                    HscAccString = x.RealityObject.CodeErc + "_" + x.HouseAccountNumber + "_" + x.PaymentCode,
                    accId = x.Id
                })
                .AsEnumerable()
                .GroupBy(x => x.HscAccString)
                .ToDictionary(
                x => x.Key,
                x => x.Select(y => y.accId).FirstOrDefault());

            var dictCurrentAccountCharge =
                this.RepHouseAccountCharge.GetAll()
                    .Where(x => x.Account != null)
                    .Where(x => x.Account.RealityObject != null && x.Account.RealityObject.CodeErc != null)
                    .Where(x => x.Account.PaymentCode != null && x.Account.HouseAccountNumber != null)
                    .ToDictionary(x => x.Id);

            var dictAccountCharge =
                this.RepHouseAccountCharge.GetAll()
                    .Where(x => x.Account != null)
                    .Where(x => x.Account.RealityObject != null && x.Account.RealityObject.CodeErc != null)
                    .Where(x => x.Account.PaymentCode != null && x.Account.HouseAccountNumber != null)
                    .Select(x => new
                    {
                        accountId = x.Account.Id,
                        accChargeId = x.Id,
                        dateStr = string.Format("{0}.{1}", x.DateCharging.Month, x.DateCharging.Year)
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.accountId)
                    .ToDictionary(
                        x => x.Key,
                        x =>
                        x.GroupBy(y => y.dateStr)
                         .ToDictionary(y => y.Key, y => y.Select(z => z.accChargeId).FirstOrDefault()));

            var memoryStreamFile = new MemoryStream(fileData.Data);

            var data = new List<HouseAccountChargeProxy>();
                
            using (var sr = new StreamReader(memoryStreamFile, Encoding.GetEncoding(1251)))
            {
                int rowNum = 0;

                while (!sr.EndOfStream)
                {
                    var line = sr.ReadLine();

                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    var row = line.Split(';').Select(x => (x ?? "").Trim('"', ' ')).ToArray();

                    var houseAccountChargeEntity = this.ParseStrArray(row, fileData.FileName);

                    if (dictRealityObject.ContainsKey(houseAccountChargeEntity.CodeErc))
                    {
                        data.Add(houseAccountChargeEntity);
                    }

                    rowNum++;
                }
            }

            var dataQuery = data
                .Where(x => x.CodeErc != string.Empty && x.ServiceCode != 0 && x.PaymentCode != string.Empty && x.HouseAccountNumber != null && x.Month != 0 && x.Year != 0)
                .Where(x => x.ServiceCode == 500 || x.ServiceCode == 206);

            var paymentDataDict = dataQuery.GroupBy(x => x.CodeErc + "_" + x.HouseAccountNumber + "_" + x.PaymentCode + "_" + x.ServiceCode)
                    .ToDictionary(x => x.Key, x => x.FirstOrDefault());

            var dataDict = data
                    .GroupBy(x => x.HscAccString)
                    .ToDictionary(
                    x => x.Key,
                    x => 
                    {
                        var result = new HouseAccountChargeInfoProxy();

                        var keyPayment = x.Key + "_206";
                        var keyPenalty = x.Key + "_500";

                        var payment = paymentDataDict.ContainsKey(keyPayment) ? paymentDataDict[keyPayment] : null;
                        var penalty = paymentDataDict.ContainsKey(keyPenalty) ? paymentDataDict[keyPenalty] : null;
                        var defaultRec = payment ?? (penalty ?? null);

                        if (defaultRec != null)
                        {
                            result.DateStr = string.Format("{0}.{1}", defaultRec.Month, defaultRec.Year);
                            result.DateCharging = new DateTime(defaultRec.Year, defaultRec.Month, 01);
                            result.ApartmentArea = defaultRec.ApartmentArea;
                            result.HouseAccountNumber = defaultRec.HouseAccountNumber;
                            result.PaymentCode = defaultRec.PaymentCode;
                            result.CodeErc = defaultRec.CodeErc;
                            result.HscAccString = defaultRec.HscAccString ?? string.Empty;
                            result.OwnerName = defaultRec.OwnerName;
                            result.OwnerType = defaultRec.OwnerType == 2 ? OwnerType.JurPerson : OwnerType.Individual;
                        }

                        if (payment != null)
                        {
                            result.PaymentChargeAll = payment.ChargeAll;
                            result.PaymentChargeMonth = payment.ChargeMonth;
                            result.PaymentPaidAll = payment.PaidAll;
                        }

                        if (penalty != null)
                        {
                            result.PenaltiesChargeAll = penalty.ChargeAll;
                            result.PenaltiesChargeMonth = penalty.ChargeMonth;
                            result.PenaltiesPaidAll = penalty.PaidAll;
                        }

                        return result;
                    });

            if (dataDict.Count == 0)
            {
                throw new Exception("Не найдены записи");
            }

            var houseAccountRecordsList = new List<HouseAccount>();
            var houseAccountChargeRecordsList = new List<HouseAccountCharge>();

            var rowNumber = 1;
            foreach (var dataByRo in dataDict)
            {
                if (this.CheckHouse(dataByRo.Value.CodeErc, fileData.FileName, rowNumber))
                {
                    var curAccCharge = dataByRo.Value;
                    var realtyObjId = this.dictRealityObject[curAccCharge.CodeErc].Id;
                    var accountStr = dataByRo.Value.HscAccString;

                    var realtyObjAccountId = dictRealityObjectAccount.ContainsKey(accountStr) ? dictRealityObjectAccount[accountStr] : 0;
                    
                    var realtyObj = new RealityObject { Id = realtyObjId };
                    HouseAccount account = null;
                    HouseAccountCharge accountChargeRecord = null;
                    long accChargeId = 0;

                    if (realtyObjAccountId > 0 && dictCurrentAccount.ContainsKey(realtyObjAccountId))
                    {
                        account = dictCurrentAccount[realtyObjAccountId];
                        this.LogImport.Warn(fileData.FileName, string.Format("Найден лицевой счет дома {0}", account.HouseAccountNumber));

                        if (dictAccountCharge.ContainsKey(account.Id) && dictAccountCharge[account.Id].ContainsKey(curAccCharge.DateStr))
                        {
                            accChargeId = dictAccountCharge[account.Id][curAccCharge.DateStr];
                        }
                    }
                    else
                    {
                        account = new HouseAccount();
                        account.RealityObject = realtyObj;
                        account.HouseAccountNumber = curAccCharge.HouseAccountNumber;
                        account.PaymentCode = curAccCharge.PaymentCode;
                    }

                    account.ApartmentArea = curAccCharge.ApartmentArea;
                    account.OwnerName = curAccCharge.OwnerName;
                    account.OwnerType = curAccCharge.OwnerType;
                    houseAccountRecordsList.Add(account);

                    if (accChargeId > 0 && dictCurrentAccountCharge.ContainsKey(accChargeId))
                    {
                        accountChargeRecord = dictCurrentAccountCharge[accChargeId];
                    }
                    else
                    {
                        accountChargeRecord = new HouseAccountCharge();
                        accountChargeRecord.RealityObject = realtyObj;
                        accountChargeRecord.Account = account;
                    }

                    this.FillRecord(accountChargeRecord, curAccCharge);

                    houseAccountChargeRecordsList.Add(accountChargeRecord);
                }

                rowNumber++;
            }

            var session = Container.Resolve<ISessionProvider>().GetCurrentSession();
            var oldFlushMode = session.FlushMode;
            session.FlushMode = FlushMode.Commit;
            
            try
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        houseAccountRecordsList.ForEach(session.SaveOrUpdate);

                        houseAccountChargeRecordsList.ForEach(session.SaveOrUpdate);

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
            finally
            {
                session.FlushMode = oldFlushMode;
                this.Container.Resolve<ISessionProvider>().CloseCurrentSession();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            LogImport.SetFileName(fileData.FileName);
            LogImport.ImportKey = this.CodeImport;

            LogImportManager.FileNameWithoutExtention = fileData.FileName;
            LogImportManager.Add(fileData, LogImport);
            LogImportManager.Save();

            var statusImport = LogImport.CountError > 0
                        ? StatusImport.CompletedWithError
                        : LogImport.CountWarning > 0
                            ? StatusImport.CompletedWithWarning
                            : StatusImport.CompletedWithoutError;

            sw.Stop();

            return new ImportResult(statusImport, string.Format("time elapsed: {0} ms;", sw.ElapsedMilliseconds));
        }

        private void FillRecord(HouseAccountCharge accountChargeRecord, HouseAccountChargeInfoProxy curAccCharge)
        {
            accountChargeRecord.Service = "Капитальный ремонт жилых зданий";
            accountChargeRecord.DateCharging = curAccCharge.DateCharging;
            accountChargeRecord.PaymentChargeAll = curAccCharge.PaymentChargeAll;
            accountChargeRecord.PaymentChargeMonth = curAccCharge.PaymentChargeMonth;
            accountChargeRecord.PaymentPaidAll = curAccCharge.PaymentPaidAll;
            accountChargeRecord.PenaltiesChargeAll = curAccCharge.PenaltiesChargeAll;
            accountChargeRecord.PenaltiesChargeMonth = curAccCharge.PenaltiesChargeMonth;
            accountChargeRecord.PenaltiesPaidAll = curAccCharge.PenaltiesPaidAll;
        }

        private HouseAccountChargeProxy ParseStrArray(string[] array, string fileName)
        {
            var res = new HouseAccountChargeProxy();

            for (var i = 0; i <= 23; i++)
            {
                var cellValueStr = array[i].Trim().Replace('.', ',');
                int intValue = 0;
                decimal decimalValue = 0M;

                switch (i)
                {
                    case 0:
                        if (!string.IsNullOrEmpty(cellValueStr))
                        {
                            res.HouseAccountNumber = cellValueStr;
                        }
                        else
                        {
                            LogImport.Warn(fileName, string.Format("В строке {0} не указан код ЛС", i));
                        }

                        break;
                    case 1:

                        if (cellValueStr != string.Empty)
                        {
                            res.PaymentCode = cellValueStr;
                        }
                        else
                        {
                            LogImport.Warn(fileName, string.Format("В строке {0} не указан платежный код", i));
                        }

                        break;
                    case 3:
                        if (int.TryParse(cellValueStr, out intValue))
                        {
                            res.Month = intValue;
                        }
                        else
                        {
                            LogImport.Warn(fileName, string.Format("В строке {0} не указан месяц", i));
                        }

                        break;
                    case 4:
                        if (int.TryParse(cellValueStr, out intValue))
                        {
                            res.Year = intValue;
                        }
                        else
                        {
                            LogImport.Warn(fileName, string.Format("В строке {0} не указан год", i));
                        }

                        break;
                    case 5:
                        if (decimal.TryParse(cellValueStr, out decimalValue))
                        {
                            res.ApartmentArea = decimalValue;
                        }
                        else
                        {
                            LogImport.Warn(fileName, string.Format("В строке {0} не указана площадь", i));
                        }
                          
                        break;
                    case 6:
                        if (decimal.TryParse(cellValueStr, out decimalValue))
                        {
                            res.ChargeAll = decimalValue;
                        }
                        else
                        {
                            LogImport.Warn(fileName, string.Format("В строке {0} не указано начисление всего", i));
                        }

                        break;
                    case 7:
                        if (decimal.TryParse(cellValueStr, out decimalValue))
                        {
                            res.ChargeMonth = decimalValue;
                        }
                        else
                        {
                            LogImport.Warn(fileName, string.Format("В строке {0} не указано начисление за месяц", i));
                        }

                        break;
                    case 10:
                        if (decimal.TryParse(cellValueStr, out decimalValue))
                        {
                            res.PaidAll = decimalValue;
                        }
                        else
                        {
                            LogImport.Warn(fileName, string.Format("В строке {0} не указано оплачено всего", i));
                        }

                        break;
                    case 13:
                        if (int.TryParse(cellValueStr, out intValue))
                        {
                            res.ServiceCode = intValue;
                        }
                        else
                        {
                            LogImport.Warn(fileName, string.Format("В строке {0} не указан код услуги", i));
                        }

                        break;
                    case 18:
                        if (cellValueStr != string.Empty)
                        {
                            res.CodeErc = cellValueStr;
                        }
                        else
                        {
                            LogImport.Warn(fileName, string.Format("В строке {0} не указан код ЕРЦ дома", i));
                        }
                        break;
                    case 22:
                        if (cellValueStr != string.Empty)
                        {
                            res.OwnerName = cellValueStr;
                        }
                        else
                        {
                            LogImport.Warn(fileName, string.Format("В строке {0} не указано имя владельца", i));
                        }

                        break;
                    case 23:
                        if (int.TryParse(cellValueStr, out intValue))
                        {
                            res.OwnerType = intValue;
                        }
                        else
                        {
                            LogImport.Warn(fileName, string.Format("В строке {0} не тип владельца", i));
                        }

                        break;
                }
            }

            res.HscAccString = res.CodeErc + "_" + res.HouseAccountNumber + "_" + res.PaymentCode;
            return res;
        }
       

        #region Utils
        
        private bool CheckHouse(string codeErc, string fileName, int rowNumber)
        {
            /*проверка наличия дома с таким кодом ЕРЦ*/
            if (!dictRealityObject.ContainsKey(codeErc))
            {
                LogImport.Warn(fileName, string.Format("Не найден дом с кодом ЕРЦ {0}, строка {1}", codeErc, rowNumber));
                return false;
            }

            return true;
        }

        #endregion Utils

        private struct RobjectProxy
        {
            public long Id { get; set; }
            public string Address { get; set; }
        }

        private class HouseAccountChargeProxy
        {
            public string CodeErc { get; set; }
            public string HouseAccountNumber { get; set; }
            public string PaymentCode { get; set; }
            public int Year { get; set; }
            public int Month { get; set; }
            public decimal ApartmentArea { get; set; }
            public decimal ChargeAll { get; set; }
            public decimal ChargeMonth { get; set; }
            public decimal PaidAll { get; set; }
            public int ServiceCode { get; set; }
            public string HscAccString { get; set; }
            public string OwnerName { get; set; }
            public int OwnerType { get; set; }
        }

        private struct HouseAccountChargeInfoProxy
        {
            public decimal ApartmentArea { get; set; }
            public DateTime DateCharging { get; set; }
            public decimal PaymentChargeAll { get; set; }
            public decimal PaymentChargeMonth { get; set; }
            public decimal PaymentPaidAll { get; set; }
            public decimal PenaltiesChargeAll { get; set; }
            public decimal PenaltiesChargeMonth { get; set; }
            public decimal PenaltiesPaidAll { get; set; }
            public string DateStr { get; set; }
            public string HscAccString { get; set; }
            public string HouseAccountNumber { get; set; }
            public string PaymentCode { get; set; }
            public string CodeErc { get; set; }
            public string OwnerName { get; set; }
            public OwnerType OwnerType { get; set; }
        }
    }
}