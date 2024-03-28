namespace Bars.Gkh.RegOperator.Imports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Domain.TableLocker;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums.Import;
    using Bars.Gkh.Import;
    using Bars.Gkh.RegOperator.Domain;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Enums;

    using NDbfReaderEx;

    /// <summary>
    /// Импорт начислений dbf 
    /// </summary>
    internal class PersonalAccountPaymentImport : AbstractDbfImport
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        private UnacceptedChargePacket chargePacket;

        private ChargePeriod period;

        private Dictionary<string, long> personalAccountByNumDict;

        private Dictionary<string, bool> cashPaymentCenterAccrualsDict;

        private Dictionary<string, Dictionary<string, List<long>>> personalAccountByOuterNumDict;

        private readonly List<PersonalAccountCharge> chargeList = new List<PersonalAccountCharge>();

        private readonly List<PersonalAccountPeriodSummary> summaryList = new List<PersonalAccountPeriodSummary>();

        private Dictionary<long, PersonalAccountPeriodSummary> summariesDict;

        private Dictionary<long, PersonalAccountCharge> existsChargeDict;

        public PersonalAccountPaymentImport(ILogImportManager logManager, ILogImport logImport)
            : base(logManager, logImport)
        {
            this.FieldsNames = new HashSet<string>
            {
                "LS",
                "OUTERLS",
                "EPD",
                "NAME1",
                "NAME2",
                "NAME3",
                "ADR1",
                "SUMMA",
                "ASHARE",
                "DAYCNT",
                "DEBT",
                "DEBTDAY",
                "TYPEAB",
                "DOPEN",
                "DCLOSE",
                "PERRECALC",
                "PENIASSD"
            };
        }

        /// <summary>
        /// Домен-сервис Лицевой счет
        /// </summary>
        public IDomainService<BasePersonalAccount> PersAccDomain { get; set; }

        /// <summary>
        /// Домен-сервис Лицевой счет РКЦ
        /// </summary>
        public IDomainService<CashPaymentCenterPersAcc> CashPaymentCenterPersAccDomain { get; set; }

        /// <summary>
        /// Домен-сервис РКЦ
        /// </summary>
        public IDomainService<CashPaymentCenter> CashPaymentCenterDomain { get; set; }

        /// <summary>
        /// Домен-сервис Период начислений
        /// </summary>
        public IDomainService<ChargePeriod> PeriodDomain { get; set; }

        /// <summary>
        /// Домен-сервис Пакет неподтвержденных начислений
        /// </summary>
        public IDomainService<UnacceptedChargePacket> UnacceptedChargePacketDomain { get; set; }

        /// <summary>
        /// Домен-сервис Ситуация по ЛС за период
        /// </summary>
        public IDomainService<PersonalAccountPeriodSummary> AccPeriodSummaryDomain { get; set; }

        /// <summary>
        /// Домен-сервис Начисления
        /// </summary>
        public IDomainService<PersonalAccountCharge> PersAccChargeDomain { get; set; }

        public override string Key => PersonalAccountPaymentImport.Id;

        public override string CodeImport => "PersonalAccountImport";

        public override string Name => "Импорт начислений dbf";

        public override string PermissionName => "GkhRegOp.PersonalAccount.Import.PersonalAccountPaymentImport";

        protected override Encoding Encoding => Encoding.GetEncoding(866);

        private void InitDictionaries()
        {
            var persAccList = this.PersAccDomain.GetAll()
                .Join(
                    this.CashPaymentCenterPersAccDomain.GetAll(),
                    x => x.Id,
                    y => y.PersonalAccount.Id,
                    (x, y) => new
                    {
                        x.Id,
                        x.PersonalAccountNum,
                        x.PersAccNumExternalSystems,
                        CodeErc = y.CashPaymentCenter.Identifier,
                        y.CashPaymentCenter.ConductsAccrual
                    }).ToList();

            this.cashPaymentCenterAccrualsDict = this.CashPaymentCenterDomain.GetAll()
                .Where(x => x.Identifier != string.Empty && x.Identifier.Length > 0)
                .Select(
                    x => new
                    {
                        x.ConductsAccrual,
                        x.Identifier
                    }).AsEnumerable()
                .GroupBy(x => x.Identifier)
                .ToDictionary(x => x.Key, y => y.First().ConductsAccrual);

            this.personalAccountByNumDict = persAccList.GroupBy(x => x.PersonalAccountNum).AsEnumerable()
                .ToDictionary(x => x.Key, y => y.First().Id);

            this.personalAccountByOuterNumDict =
                persAccList
                    .Where(x => !string.IsNullOrEmpty(x.PersAccNumExternalSystems))
                    .GroupBy(x => x.PersAccNumExternalSystems)
                    .AsEnumerable()
                    .ToDictionary(
                        x => x.Key,
                        y => y.GroupBy(x => !string.IsNullOrEmpty(x.CodeErc) ? x.CodeErc : "-")
                            .ToDictionary(p => p.Key, q => q.Select(z => z.Id).ToList()));

            this.summariesDict = this.AccPeriodSummaryDomain.GetAll()
                .Where(x => x.Period.Id == this.period.Id)
                .GroupBy(x => x.PersonalAccount.Id)
                .AsEnumerable()
                .ToDictionary(x => x.Key, y => y.First());

            this.chargePacket = new UnacceptedChargePacket
            {
                CreateDate = DateTime.Now,
                PacketState = PaymentOrChargePacketState.Accepted,
                UserName = this.Container.Resolve<IUserIdentity>().Name
            };

            this.existsChargeDict = this.PersAccChargeDomain.GetAll()
                .Where(x => x.ChargePeriod.Id == this.period.Id)
                .Where(x => x.IsActive)
                .GroupBy(x => x.BasePersonalAccount.Id)
                .AsEnumerable()
                .ToDictionary(x => x.Key, y => y.First());

            this.Container.Resolve<ISessionProvider>().GetCurrentSession().Clear();
        }

        private void SaveData()
        {
            this.chargePacket.Description = string.Format("Импорт начислений из dbf по {0} л.c. за {1}", this.summaryList.Count(), this.period.Name);

            this.UnacceptedChargePacketDomain.Save(this.chargePacket);

            TransactionHelper.InsertInManyTransactions(this.Container, this.chargeList, 1000, false, true);
            TransactionHelper.InsertInManyTransactions(this.Container, this.summaryList, 1000, false, true);
        }

        protected override void ProcessLine(DbfRow row, int rowNumber)
        {
            var accIds = this.GetPersonalAccountId(row, rowNumber);

            if (!accIds.Any())
            {
                this.LogImport.Error("Лицевой счет", string.Format("{0} не найден, строка {1}", this.GetValueOrDefault(row, "LS"), rowNumber));

                // не нужно грузить эти данные
                return;
            }

            if (accIds.Count() == 1)
            {
                this.ApplyPayment(row, accIds.First());
            }
            else
            {
                this.AddUnacceptedCharge(row, accIds);
            }
        }

        private string GetValueOrDefault(DbfRow row, string column)
        {
            return this.HeadersIndexes.ContainsKey(column) ? row.GetValue(column).ToString() : string.Empty;
        }

        private void AddUnacceptedCharge(DbfRow row, List<long> accIds)
        {
            var charge = new PersonalAccountCharge
            {
                Packet = this.chargePacket,
                Guid = Guid.NewGuid().ToString(),
                ChargePeriod = this.period,
                ChargeDate = this.period.GetCurrentInPeriodDate()
            };

            if (this.HeadersIndexes.ContainsKey("SUMMA"))
            {
                charge.ChargeTariff = row.GetDecimal("SUMMA");
            }

            if (this.HeadersIndexes.ContainsKey("PERRECALC"))
            {
                charge.RecalcByBaseTariff = row.GetDecimal("PERRECALC");
            }

            if (this.HeadersIndexes.ContainsKey("PENIASSD"))
            {
                charge.Penalty = row.GetDecimal("PENIASSD");
            }

            charge.Charge = charge.ChargeTariff + charge.RecalcByBaseTariff + charge.Penalty;

            if (accIds.Count() == 1)
            {
                charge.BasePersonalAccount = new BasePersonalAccount {Id = accIds.First()};

                charge.Packet.Description =
                    string.Format(
                        "В систему ранее были внесены данные по начислениям л.с. {0}, при повторной загрузке данные будут заменены",
                        this.GetValueOrDefault(row, "LS"));
            }
            else if (accIds.Count() > 1)
            {
                charge.Packet.Description = string.Format(
                    "Загружены начисления по л.с. с одинаковыми номерами. номер лс: '{0}', внешний номер лс:'{1}'",
                    this.GetValueOrDefault(row, "LS"),
                    this.GetValueOrDefault(row, "OUTERLS"));
            }

            var existCharge = this.existsChargeDict.Get(accIds.First());
            var summary = this.summariesDict.Get(accIds.First());

            if (existCharge != null)
            {
                existCharge.IsActive = false;
                this.chargeList.Add(existCharge);

                summary.UndoCharge(existCharge); // отменяем последнее активное начисление
            }

            this.chargeList.Add(charge);


            summary.ApplyCharge(charge.ChargeTariff, 0, charge.Penalty, 0, charge.RecalcByBaseTariff, 0);

            this.summaryList.Add(summary);

            this.chargeList.Add(charge);

            this.LogImport.CountAddedRows++;
        }

        private void ApplyPayment(DbfRow row, long accId)
        {
            if (this.summariesDict.ContainsKey(accId))
            {
                this.AddUnacceptedCharge(row, new List<long> {accId});
            }
        }

        private List<long> GetPersonalAccountId(DbfRow row, int rowNumber)
        {
            if (this.HeadersIndexes.ContainsKey("LS"))
            {
                var accNum = row.GetValue("LS").ToString();

                if (this.personalAccountByNumDict.ContainsKey(accNum))
                {
                    return new List<long> {this.personalAccountByNumDict[accNum]};
                }
            }

            if (this.HeadersIndexes.ContainsKey("OUTERLS"))
            {
                var accOuterNum = row.GetValue("OUTERLS").ToString();

                if (this.personalAccountByOuterNumDict.ContainsKey(accOuterNum))
                {
                    var rkc = this.personalAccountByOuterNumDict[accOuterNum];

                    var rkcNum = this.HeadersIndexes.ContainsKey("EPD") ? row.GetValue("EPD").ToString() : string.Empty;

                    if (!string.IsNullOrEmpty(rkcNum))
                    {
                        if (rkc.ContainsKey(rkcNum))
                        {
                            var accIds = rkc[rkcNum];

                            if (this.cashPaymentCenterAccrualsDict.ContainsKey(rkcNum) && this.cashPaymentCenterAccrualsDict[rkcNum])
                            {
                                return accIds;
                            }
                            else
                            {
                                // в системе на найден РКЦ «значение поля EPD» 
                                this.LogImport.Error(
                                    "У РКЦ не установлен признак «проводит начисления»",
                                    string.Format("У РКЦ '{0}' не установлен признак «проводит начисления» ", rkcNum));
                                return null;
                            }
                        }
                        else
                        {
                            // в системе на найден РКЦ «значение поля EPD» 
                            this.LogImport.Error("В заданном РКЦ {0} не найден ЛС, строка {1}".FormatUsing(rkcNum, rowNumber), accOuterNum);
                            return null;
                        }
                    }
                    else
                    {
                        var result = new List<long>();
                        rkc.Values.ForEach(result.AddRange);

                        return result;
                    }
                }
                else
                {
                    this.LogImport.Error("Не найден номер лицевого счета в заданном РКЦ", accOuterNum);
                    return new List<long>();
                }
            }

            return new List<long>();
        }

        public override ImportResult Import(BaseParams baseParams)
        {
            var file = baseParams.Files["FileImport"];

            try
            {
                this.InitLog(file.FileName);

                this.GetChargePeriod(file.FileName);

                if (this.period != null)
                {
                    this.InitDictionaries();

                    this.ProcessData(file.Data);

                    this.SaveData();
                }
            }
            catch (ImportException e)
            {
                return new ImportResult(StatusImport.CompletedWithError, e.Message);
            }

            this.ReleaseLog(file);

            var message = this.LogImportManager.GetInfo();
            var status = this.LogImportManager.CountError > 0
                ? StatusImport.CompletedWithError
                : (this.LogImportManager.CountWarning > 0 ? StatusImport.CompletedWithWarning : StatusImport.CompletedWithoutError);
            return new ImportResult(status, message, string.Empty, this.LogImportManager.LogFileId);
        }

        private void GetChargePeriod(string filename)
        {
            const string nameError = @"Имя файла должно быть вида YYMM.dbf
                    YY – номер года (две цифры) периода начисления.
                    MM - номер месяца (две цифры) периода начисления";

            if (filename.Length < 4)
            {
                this.LogImport.Error("Неверное имя файла", nameError);
                return;
            }

            var year = filename.Substring(0, 2).ToInt();
            var month = filename.Substring(2, 2).ToInt();

            if (year == 0 || month == 0)
            {
                this.LogImport.Error("Неверное имя файла", nameError);
                return;
            }

            year += 2000;

            this.period = this.PeriodDomain.GetAll().FirstOrDefault(x => x.StartDate.Year == year && x.StartDate.Month == month);

            if (this.period == null)
            {
                this.LogImport.Error("Не удалось определить период начислений по имени файла", nameError);
            }
        }

        public override bool Validate(BaseParams baseParams, out string message)
        {
            message = null;

            var tableLocker = this.Container.Resolve<ITableLocker>();
            try
            {
                if (tableLocker.CheckLocked<BasePersonalAccount>("INSERT"))
                {
                    message = TableLockedException.StandardMessage;
                    return false;
                }
            }
            finally
            {
                this.Container.Release(tableLocker);
            }

            return true;
        }
    }
}