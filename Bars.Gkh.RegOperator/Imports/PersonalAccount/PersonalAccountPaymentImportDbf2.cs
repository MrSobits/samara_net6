namespace Bars.Gkh.RegOperator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using B4;
    using B4.DataAccess;
    using B4.IoC;
    using B4.Utils;

    using Bars.Gkh.Entities;

    using Gkh.Domain;
    using Gkh.Enums.Import;
    using Import;
    using Domain;
    using Entities;
    using Enums;
    using Imports;
    using NDbfReaderEx;

    public class PersonalAccountPaymentImportDbf2 : AbstractDbfImport
    {
        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public override string Key
        {
            get { return Id; }
        }

        public override string CodeImport
        {
            get { return "PersonalAccountImport"; }
        }

        public override string Name
        {
            get { return "Импорт начислений dbf 2"; }
        }

        public override string PermissionName
        {
            get { return "GkhRegOp.PersonalAccount.Import.PersonalAccountPaymentImportDbf2"; }
        }

        public PersonalAccountPaymentImportDbf2(ILogImportManager logManager, ILogImport logImport)
            : base(logManager, logImport)
        {
            FieldsNames = new HashSet<string>
            {
                "PERIOD",
                "NAME_RN",
                "UL",
                "DOM",
                "KODD",
                "KOR",
                "NKV",
                "KKC",
                "OSQ",
                "SLD_N",
                "NACH",
                "LGT",
                "RAZV",
                "NACH_T",
                "PLAT_KR",
                "SLD_K",
                "PENY",
                "PLAT_PR"
            };

        }

        private readonly List<PaymentRow> _paymentRows = new List<PaymentRow>();

        private List<ChargePeriod> _chargePeriods;
        private Dictionary<string, List<long>> _personalAccountByExterNumDict;
        private Dictionary<long, List<CashPaymentCenterProxy>> _cashPaymentCenetrsByAcc;


        private void InitCache()
        {
            var chargePeriodDomain = Container.ResolveDomain<ChargePeriod>();
            var personalAccountDomain = Container.ResolveDomain<BasePersonalAccount>();
            var cashPaymentCenterPersAccDomain = Container.ResolveDomain<CashPaymentCenterPersAcc>();
            var cashPaymentCenterDomain = Container.ResolveDomain<CashPaymentCenter>();

            using (Container.Using(chargePeriodDomain, personalAccountDomain,
                cashPaymentCenterDomain, cashPaymentCenterPersAccDomain))
            {
                _chargePeriods = chargePeriodDomain.GetAll().ToList();

                var persAccList = personalAccountDomain.GetAll()
                    .Join(cashPaymentCenterPersAccDomain.GetAll(),
                        x => x.Id,
                        y => y.PersonalAccount.Id,
                        (x, y) => new
                        {
                            x.Id,
                            x.PersonalAccountNum,
                            x.PersAccNumExternalSystems,
                            y.DateStart,
                            y.DateEnd,
                            CashPaymentCenterId = y.CashPaymentCenter.Id,
                            y.CashPaymentCenter.ConductsAccrual,
                            ContragentName = y.CashPaymentCenter.Contragent.Name
                        }).ToList();


                _personalAccountByExterNumDict =
                    personalAccountDomain.GetAll()
                        .Where(x => x.PersAccNumExternalSystems != null && x.PersAccNumExternalSystems != string.Empty)
                        .GroupBy(x => x.PersAccNumExternalSystems)
                        .ToDictionary(x => x.Key, x => x.Select(y => y.Id).ToList());

                _cashPaymentCenetrsByAcc = persAccList.GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, x => x.Select(y => new CashPaymentCenterProxy
                    {
                        ConductsAccrual = y.ConductsAccrual,
                        Id = y.CashPaymentCenterId,
                        DateEnd = y.DateEnd,
                        DateStart = y.DateStart,
                        ContragentName = y.ContragentName
                    }).ToList());
            }

        }

        public override ImportResult Import(BaseParams baseParams)
        {
            var file = baseParams.Files["FileImport"];

            try
            {
                InitLog(file.FileName);

                InitCache();

                ProcessData(file.Data);

                SaveData(_paymentRows);

            }
            catch (ImportException e)
            {
                return new ImportResult(StatusImport.CompletedWithError, e.Message);
            }

            ReleaseLog(file);

            var message = this.LogImportManager.GetInfo();
            var status = this.LogImportManager.CountError > 0
                ? StatusImport.CompletedWithError
                : (this.LogImportManager.CountWarning > 0
                    ? StatusImport.CompletedWithWarning
                    : StatusImport.CompletedWithoutError);
            return new ImportResult(status, message, string.Empty, this.LogImportManager.LogFileId);
        }

        protected override Encoding Encoding
        {
            get { return Encoding.GetEncoding(1251); }
        }

        private void SaveData(IEnumerable<PaymentRow> paymentRows)
        {
            var paymentRowsByPeriods = paymentRows.GroupBy(x => x.Period).ToDictionary(x => x.Key);

            foreach (var keyVal in paymentRowsByPeriods)
            {
                var result = GetChargePeriod(keyVal.Key);

                if (result.Success)
                {
                    var chargePacket = new UnacceptedChargePacket
                    {
                        CreateDate = DateTime.Now,
                        PacketState = PaymentOrChargePacketState.Pending,
                        UserName = Container.Resolve<IUserIdentity>().Name
                    };

                    var unacceptedCharges = new List<UnacceptedCharge>();

                    foreach (var paymentRow in keyVal.Value)
                    {
                        var unacceptedCharge = CreateUnacceptedCharge(paymentRow, chargePacket, result.Data);
                        if (unacceptedCharge != null)
                        {
                            unacceptedCharges.Add(unacceptedCharge);
                            LogImport.CountAddedRows++;
                        }
                    }

                    chargePacket.Description = string.Format("Начисление по {0} лицевым счетам за {1}",
                        unacceptedCharges.Count, result.Data.Name);

                    SaveCharges(chargePacket, unacceptedCharges, result.Data);
                }
                else
                {
                    LogImport.Error("Не найден период",
                        string.Format("Не найден период {0}, данные по {1} записям не будут загружены. Ошибка: {2}",
                            keyVal.Key, keyVal.Value.Count(), result.Message));
                }
            }

        }

        private void SaveCharges(UnacceptedChargePacket chargePacket, List<UnacceptedCharge> unacceptedCharges,
            ChargePeriod period)
        {
            chargePacket.Description = string.Format("Импорт начислений из dbf по {0} л.c. за период {1}",
                unacceptedCharges.Count, period.Name);

            Container.ResolveDomain<UnacceptedChargePacket>().Save(chargePacket);

            TransactionHelper.InsertInManyTransactions(Container, unacceptedCharges, 1000, false, true);
        }

        protected override void ProcessLine(DbfRow row, int rowNumber)
        {
            _paymentRows.Add(new PaymentRow(row, rowNumber));
        }

        private BasePersonalAccount GetPersonalAccount(PaymentRow paymentRow)
        {
            if (_personalAccountByExterNumDict.ContainsKey(paymentRow.ExternalAccountNumber))
            {
                var accIds = _personalAccountByExterNumDict.Get(paymentRow.ExternalAccountNumber);
                if (accIds.Count == 1)
                {
                    return new BasePersonalAccount { Id = accIds.First() };
                }

                if (accIds.Count > 1)
                {
                    LogImport.Error("Ошибка при поиске лицевого счета",
                        string.Format(
                            "Найдены несколько лицевых счетов, соответствующих внешенему номеру ЛС {0}, строка {1}",
                            paymentRow.ExternalAccountNumber, paymentRow.RowNum));
                }
            }
            else
            {
                LogImport.Error("Ошибка при поиске лицевого счета",
                    string.Format("Не найден лицевой счет, соответствующий внешенему номеру ЛС {0}, строка {1}",
                        paymentRow.ExternalAccountNumber, paymentRow.RowNum));
            }
            return null;
        }

        private IDataResult<ChargePeriod> GetChargePeriod(string mmyyyy)
        {
            const string nameError = @"Название периода должно быть вида mmyyyy
                    yyyy – год периода начисления.
                    mm - номер месяца (две цифры) периода начисления";

            if (mmyyyy.Length != 6)
            {
                return new DataResultWrapper<ChargePeriod>(null, false, nameError);
            }

            var year = mmyyyy.Substring(2).ToInt();
            var month = mmyyyy.Substring(0, 2).ToInt();

            if (year == 0 || month == 0)
            {
                return new DataResultWrapper<ChargePeriod>(null, false, nameError);
            }

            var period = _chargePeriods.SingleOrDefault(x => x.StartDate.Year == year && x.StartDate.Month == month);

            return new DataResultWrapper<ChargePeriod>(period, period != null,
                period != null ? string.Empty : "Не найден соответсвующий период.");
        }

        private UnacceptedCharge CreateUnacceptedCharge(PaymentRow paymentRow, UnacceptedChargePacket chargePacket, ChargePeriod period)
        {
            var personalAccount = GetPersonalAccount(paymentRow);

            if (personalAccount == null)
            {
                return null;
            }

            if (!_cashPaymentCenetrsByAcc.ContainsKey(personalAccount.Id))
            {
                LogImport.Error("Нет привязки к ЕРЦ", string.Format("Лицевой счет {0} не имеет привязки к ЕРЦ в период {1}",
                    personalAccount.PersonalAccountNum, period.Name));
                return null;
            }

            var cashPaymentCenter = _cashPaymentCenetrsByAcc.Get(personalAccount.Id)
                .SingleOrDefault(x => x.DateStart <= period.StartDate && (!x.DateEnd.HasValue || period.EndDate.Value <= x.DateEnd.Value));

            if (cashPaymentCenter == null)
            {
                LogImport.Error("Нет привязки к ЕРЦ", string.Format("Лицевой счет {0} не имеет привязки к ЕРЦ в период {1}",
                    personalAccount.PersonalAccountNum, period.Name));
                return null;
            }

            if (!cashPaymentCenter.ConductsAccrual)
            {
                LogImport.Error("ЕРЦ не проводит начислений",
                    string.Format("У ЕРЦ {0}, к которому привязан ЛС {1} в период {2}, не выставлен признак проводит начисления",
                    cashPaymentCenter.ContragentName, personalAccount.PersonalAccountNum, period.Name));
                return null;
            }

            return new UnacceptedCharge
            {
                Packet = chargePacket,
                Penalty = paymentRow.Penalty,
                ChargeTariff = paymentRow.Charged,
                PersonalAccount = personalAccount,
                RecalcByBaseTariff = paymentRow.Recalc,
                Accepted = false
            };
        }


        private class PaymentRow
        {
            public PaymentRow(DbfRow row, int rowNum)
            {
                Period = row.GetString("PERIOD");
                ExternalAccountNumber = row.GetString("KKC");
                SaldoIn = row.GetDecimal("SLD_N");
                Charged = row.GetDecimal("NACH");
                Recalc = row.GetDecimal("RAZV");
                SaldoOut = row.GetDecimal("SLD_K");
                Penalty = row.GetDecimal("PENY");
                RowNum = rowNum;
            }

            public string Period { get; private set; }

            public string ExternalAccountNumber { get; private set; }

            public decimal SaldoOut { get; private set; }

            public decimal Charged { get; private set; }

            public decimal SaldoIn { get; private set; }

            public decimal Penalty { get; private set; }

            public decimal Recalc { get; private set; }

            public int RowNum { get; private set; }
        }

        private class CashPaymentCenterProxy
        {
            public long Id { get; set; }
            public bool ConductsAccrual { get; set; }

            public DateTime DateStart { get; set; }

            public DateTime? DateEnd { get; set; }

            public string ContragentName { get; set; }
        }

    }
}
