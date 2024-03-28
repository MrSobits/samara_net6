namespace Bars.Gkh.RegOperator.Export.ExportToEbir
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Modules.RegOperator.Entities.RegOperator;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Enum;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Bars.Gkh.RegOperator.Enums;

    using Castle.Windsor;

    public abstract class BaseExport
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<BasePersonalAccount> BasePersonalAccountDomain { get; set; }

        public IDomainService<IndividualAccountOwner> IndividualAccountOwnerDomain { get; set; }

        public IDomainService<LegalAccountOwner> LegalAccountOwnerDomain { get; set; }

        public IDomainService<ChargePeriod> ChargePeriodDomain { get; set; }

        public IDomainService<RealityObjectDecisionProtocol> RealityObjectDecisionProtocolDomain { get; set; }

        public IDomainService<AccountOwnerDecision> AccountOwnerDecisionDomain { get; set; }

        public IDomainService<PersonalAccountPayment> PersonalAccountPayment { get; set; }

        public IDomainService<PersonalAccountPeriodSummary> PersonalAccountPeriodSummaryDomain { get; set; }

        public IDomainService<PaymentSizeMuRecord> PaymentSizeMuRecordDomain { get; set; }

        public IDomainService<MonthlyFeeAmountDecision> MonthlyFeeAmountDecisionDomain { get; set; }

        public IDomainService<CreditOrgDecision> CreditOrgDecisionDomain { get; set; }

        public IDomainService<CalcAccountRealityObject> RegopCalcAccountRealityObjectDomain { get; set; }

        public IDomainService<RegOperator> RegOperatorDomain { get; set; }

        public IDomainService<DecisionNotification> DecisionNotificationDomain { get; set; }

        public IDomainService<RentPaymentIn> RentPaymentInDomain { get; set; }

        public IDomainService<AccumulatedFunds> AccumulatedFundsDomain { get; set; }

        public IDomainService<PeriodSummaryBalanceChange> SaldoOutChangeDomain { get; set; }

        public IFileManager FileManager { get; set; }

        protected List<Record> records;

        private long periodId;

        protected abstract FileInfo SaveFile();
        public long GetExportFileId(long periodId)
        {
            this.periodId = periodId;

            this.GetData();

            var fileInfo = this.SaveFile();

            return fileInfo.Id;
        }

        private void GetData()
        {
            var period = ChargePeriodDomain.GetAll()
                .Where(x => x.IsClosed)
                .Where(x => x.Id == this.periodId)
                .Select(x => new { x.Id, x.StartDate, x.EndDate })
                .AsEnumerable()
                .Select(x => new { x.Id, x.StartDate.Month, x.StartDate.Year, x.StartDate, x.EndDate })
                .FirstOrDefault();

            if (period == null)
            {
                throw new Exception("Неверный идентификатор периода");
            }

            var accounts = BasePersonalAccountDomain.GetAll()
                .Select(x => new
                    {
                        x.Id,
                        x.PersonalAccountNum,
                        roId = (long?)x.Room.RealityObject.Id,
                        muId = (long?)x.Room.RealityObject.Municipality.Id,
                        Address = x.Room.RealityObject.Address  + ", кв. " + x.Room.RoomNum,
                        accOwnId = x.AccountOwner.Id,
                        x.AccountOwner.OwnerType,
                        x.AreaShare,
                        x.Room.Area
                    })
                .ToList();

            var jurOwnerAccounts = LegalAccountOwnerDomain.GetAll()
                .Select(x => new
                    {
                        x.Id,
                        x.Contragent.Name,
                        x.Contragent.Inn,
                        x.Contragent.Kpp
                    })
                .ToDictionary(x => x.Id);


            var indivAccounts = IndividualAccountOwnerDomain.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.Surname,
                    x.FirstName,
                    x.SecondName
                })
                .ToDictionary(x => x.Id);

            var accountsRoIdsQuery = BasePersonalAccountDomain.GetAll().Select(x => x.Room.RealityObject.Id);

            // Решение
            var accountOwnerDecision = RealityObjectDecisionProtocolDomain.GetAll()
                .Join(
                    AccountOwnerDecisionDomain.GetAll(),
                    x => x.Id,
                    y => y.Protocol.Id,
                    (x, y) => new { RealityObjectDecisionProtocol = x, AccountOwnerDecision = y })
                .Where(x => accountsRoIdsQuery.Contains(x.RealityObjectDecisionProtocol.RealityObject.Id))
                .Where(x => x.RealityObjectDecisionProtocol.State.Name == "Утверждено")
                .Select(x => new
                {
                    x.RealityObjectDecisionProtocol.ProtocolDate,
                    x.RealityObjectDecisionProtocol.RealityObject.Id,
                    x.AccountOwnerDecision.DecisionType.GetEnumMeta().Display
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, x => x.OrderByDescending(y => y.ProtocolDate).First().Display);

            // Последний платеж
            var lastAccountPaymentInPeriod = Container.Resolve<IDomainService<PersonalAccountPayment>>().GetAll()
                .Where(x => x.PaymentDate >= period.StartDate)
                .Where(x => x.PaymentDate <= period.EndDate)
                .Select(x => new { AccountId = x.BasePersonalAccount.Id, x.PaymentDate })
                .AsEnumerable()
                .GroupBy(x => x.AccountId)
                .ToDictionary(
                    x => x.Key,
                    x => x.OrderByDescending(y => y.PaymentDate).First().PaymentDate);

            // Тариф
            var feeAmountDecisions = MonthlyFeeAmountDecisionDomain.GetAll()
                .Where(x => x.Protocol.State.FinalState)
                .Where(x => accountsRoIdsQuery.Contains(x.Protocol.RealityObject.Id))
                .Select(x => new
                {
                    roId = x.Protocol.RealityObject.Id,
                    x.Decision
                })
                .ToList();

            var decisionsByRoId = feeAmountDecisions
                .Select(x => new
                {
                    x.roId,
                    PeriodMonthlyFee = x.Decision
                        .Where(y => y.From < period.EndDate)
                        .Where(y => y.To == null || y.To >= period.EndDate)
                        .OrderByDescending(y => y.From)
                        .Select(y => (decimal?)y.Value)
                        .FirstOrDefault()
                })
                .Where(x => x.PeriodMonthlyFee != null)
                .GroupBy(x => x.roId)
                .ToDictionary(x => x.Key, x => x.First().PeriodMonthlyFee.Value);

            var municipalityTarifChanges = PaymentSizeMuRecordDomain.GetAll()
                .Where(x => x.PaymentSizeCr.TypeIndicator == TypeIndicator.MinSizeSqMetLivinSpace)
                .Where(x => x.PaymentSizeCr.DateStartPeriod <= period.EndDate || period.EndDate == null || x.PaymentSizeCr.DateStartPeriod == null)
                .Select(x => new
                {
                    x.Municipality.Id,
                    x.PaymentSizeCr.DateEndPeriod,
                    x.PaymentSizeCr.PaymentSize
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(
                    x => x.Key,
                    x => x.OrderBy(z => z.DateEndPeriod.HasValue)
                        .ThenByDescending(z => z.DateEndPeriod)
                        .First().PaymentSize);

            // Операции
            var personalAccountPeriodSummaryDict = PersonalAccountPeriodSummaryDomain.GetAll()
                .Where(x => x.Period.Id == periodId)
                .Select(x => new
                {
                    accId = x.PersonalAccount.Id,
                    x.SaldoIn,
                    x.ChargedByBaseTariff,
                    x.TariffPayment,
                    x.PenaltyPayment,
                    Recalc = x.RecalcByBaseTariff, // TODO fix recalc
                    x.Penalty,
                    x.ChargeTariff,
                    AccumFunds = ((decimal?)AccumulatedFundsDomain.GetAll()
                        .Where(y => y.Account.Id == x.PersonalAccount.Id)
                        .Where(y => y.OperationDate >= x.Period.StartDate)
                        .Where(y => y.OperationDate <= x.Period.EndDate)
                        .Sum(y => y.Sum)) ?? 0,
                    RentPayment = ((decimal?)RentPaymentInDomain.GetAll()
                        .Where(y => y.Account.Id == x.PersonalAccount.Id)
                        .Where(y => y.OperationDate >= x.Period.StartDate)
                        .Where(y => y.OperationDate <= x.Period.EndDate)
                        .Sum(y => y.Sum)) ?? 0,
                    SaldoChange = ((decimal?)SaldoOutChangeDomain.GetAll()
                        .Where(y => y.PeriodSummary.PersonalAccount.Id == x.PersonalAccount.Id)
                        .Where(y => y.ObjectCreateDate >= x.Period.StartDate)
                        .Where(y => y.ObjectCreateDate <= x.Period.EndDate)
                        .Sum(y => y.NewValue - y.CurrentValue)) ?? 0,
                    periodIsClosed = x.Period.IsClosed
                })
                .AsEnumerable()
                .Select(x => new
                {
                    x.accId,
                    x.ChargeTariff,
                    x.Penalty,
                    x.PenaltyPayment,
                    x.SaldoIn,
                    x.ChargedByBaseTariff,
                    TariffPayment = x.TariffPayment
                                    + x.RentPayment
                                    + x.AccumFunds,
                    x.Recalc,
                    SaldoOut = x.periodIsClosed 
                            ? x.SaldoIn
                              + x.ChargeTariff
                              + x.Penalty
                              + x.Recalc
                              + x.SaldoChange
                              - x.RentPayment
                              - x.AccumFunds
                              - x.PenaltyPayment
                              - x.TariffPayment
                            : 0
                })
                .GroupBy(x => x.accId)
                .ToDictionary(
                    x => x.Key,
                    x => x.First());

            var activeNotifAccNum = RealityObjectDecisionProtocolDomain.GetAll()
                .Join(
                    DecisionNotificationDomain.GetAll(),
                    x => x.Id,
                    y => y.Protocol.Id,
                    (x, y) => new {RealityObjectDecisionProtocol = x, DecisionNotification = y})
                .Where(x => accountsRoIdsQuery.Contains(x.RealityObjectDecisionProtocol.RealityObject.Id))
                .Where(x => x.RealityObjectDecisionProtocol.State.Name == "Утверждено")
                .Select(x => new
                {
                    x.RealityObjectDecisionProtocol.ProtocolDate,
                    x.RealityObjectDecisionProtocol.RealityObject.Id,
                    x.DecisionNotification.AccountNum
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key,
                    z => z.OrderByDescending(y => y.ProtocolDate).Select(x => x.AccountNum).First());


            var banks = RealityObjectDecisionProtocolDomain.GetAll()
                .Join(
                    CreditOrgDecisionDomain.GetAll(),
                    x => x.Id,
                    y => y.Protocol.Id,
                    (x, y) => new {RealityObjectDecisionProtocol = x, CreditOrgDecision = y})
                .Where(x => accountsRoIdsQuery.Contains(x.RealityObjectDecisionProtocol.RealityObject.Id))
                .Where(x => x.RealityObjectDecisionProtocol.State.Name == "Утверждено")
                .Select(x => new
                {
                    x.RealityObjectDecisionProtocol.ProtocolDate,
                    x.RealityObjectDecisionProtocol.RealityObject.Id,
                    x.CreditOrgDecision.Decision.Bik,
                    x.CreditOrgDecision.Decision.Name,
                    x.CreditOrgDecision.Decision.CorrAccount
                })
                .AsEnumerable()
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, x => x.OrderByDescending(y => y.ProtocolDate)
                    .Select(y => new
                    {
                        RoId = y.Id,
                        SettlementAccount = activeNotifAccNum.Get(y.Id),
                        y.Name,
                        y.Bik
                    }).First());

            var regOperator = RegOperatorDomain.GetAll().FirstOrDefault(x => x.Contragent.ContragentState == ContragentState.Active);

            var contragentBanks = RegopCalcAccountRealityObjectDomain.GetAll()
                .Where(x => x.Account.TypeAccount == TypeCalcAccount.Regoperator)
                .Where(x => x.Account.AccountOwner.Id == regOperator.Contragent.Id &&
                        accountsRoIdsQuery.Contains(x.RealityObject.Id))
                .Select(x => new
                {
                    RoId = x.RealityObject.Id,
                    ((RegopCalcAccount) x.Account).ContragentCreditOrg.SettlementAccount,
                    Name = x.Account.CreditOrg.Name ?? ((RegopCalcAccount) x.Account).ContragentCreditOrg.Name,
                    Bik = x.Account.CreditOrg.Bik ?? ((RegopCalcAccount) x.Account).ContragentCreditOrg.Bik
                })
                .ToList();

            foreach (var bank in contragentBanks)
            {
                banks[bank.RoId] = bank;
            }

            records = accounts
                .Where(x => x.roId.HasValue)
                .Select(x =>
                    {
                        var record = new Record
                        {
                            AccountOperator = accountOwnerDecision.ContainsKey(x.roId.Value) ? accountOwnerDecision[x.roId.Value] : string.Empty,
                            AccountNum = x.PersonalAccountNum,
                            Dolya = x.AreaShare,
                            Address = x.Address,
                            Area =  x.Area,
                            TYear = period.Year,
                            TMonth = period.Month,
                            Tarif = "null"
                        };

                        if (x.OwnerType == PersonalAccountOwnerType.Individual && indivAccounts.ContainsKey(x.accOwnId))
                        {
                            var indivAcc = indivAccounts.Get(x.accOwnId);
                            record.Surname = indivAcc.Surname;
                            record.Name = indivAcc.FirstName;
                            record.Patronymic = indivAcc.SecondName;
                        }

                        if (x.OwnerType == PersonalAccountOwnerType.Legal && jurOwnerAccounts.ContainsKey(x.accOwnId))
                        {
                            var jurOwnerAcc = jurOwnerAccounts.Get(x.accOwnId);
                            record.Ulname = jurOwnerAcc.Name;
                            record.INN = !string.IsNullOrEmpty(jurOwnerAcc.Inn) ? jurOwnerAcc.Inn : "0";
                            record.KPP = !string.IsNullOrEmpty(jurOwnerAcc.Kpp) ? jurOwnerAcc.Kpp : "0";
                        }

                        if (personalAccountPeriodSummaryDict.ContainsKey(x.Id))
                        {
                            var periodSummary = personalAccountPeriodSummaryDict[x.Id];

                            record.SaldoIn = periodSummary.SaldoIn;
                            record.SaldoOut = periodSummary.SaldoOut;
                            record.ChargeSum = periodSummary.ChargeTariff + periodSummary.Recalc;
                            record.CostsSum = periodSummary.ChargeTariff;
                            record.ReChargeSum = periodSummary.Recalc;
                            record.FineSum = periodSummary.Penalty;
                            record.PaySum = periodSummary.TariffPayment;
                            record.PayFineSum = periodSummary.PenaltyPayment;
                        }

                        if (lastAccountPaymentInPeriod.ContainsKey(x.Id))
                        {
                            record.LastPayDate = lastAccountPaymentInPeriod[x.Id].ToString("dd.MM.yyyy");
                        }
                        
                        if (decisionsByRoId.ContainsKey(x.roId.Value))
                        {
                            record.Tarif = decisionsByRoId[x.roId.Value].ToString("0.00");
                        }
                        else if (x.muId.HasValue && municipalityTarifChanges.ContainsKey(x.muId.Value))
                        {
                            record.Tarif = municipalityTarifChanges[x.muId.Value].ToString("0.00");
                        }

                        if (banks.ContainsKey(x.roId.Value))
                        {
                            var bank = banks.Get(x.roId.Value);
                            record.NameKO = bank.Name;
                            record.KredOrg = bank.SettlementAccount;
                            record.BIKKO = bank.Bik;
                        }

                        return record;
                    })
                .ToList();
        }
    }
}