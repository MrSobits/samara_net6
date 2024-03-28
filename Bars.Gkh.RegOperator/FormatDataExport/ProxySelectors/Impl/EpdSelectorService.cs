namespace Bars.Gkh.RegOperator.FormatDataExport.ProxySelectors.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.FormatDataExport.ProxySelectors;
    using Bars.Gkh.RegOperator.DataProviders.Meta;
    using Bars.Gkh.RegOperator.DomainModelServices;
    using Bars.Gkh.RegOperator.DomainService;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.Repositories.ChargePeriod;
    using Bars.Gkh.Utils;

    using Newtonsoft.Json;

    /// <summary>
    /// Селектор для Платежный документ
    /// </summary>
    public class EpdSelectorService : BaseProxySelectorService<EpdProxy>
    {
        /// <inheritdoc />
        protected override ICollection<EpdProxy> GetAdditionalCache()
        {
            var accountPaymentInfoSnapshotRepository = this.Container.ResolveRepository<AccountPaymentInfoSnapshot>();
            using (this.Container.Using(accountPaymentInfoSnapshotRepository))
            {
                return this.GetProxies(accountPaymentInfoSnapshotRepository.GetAll().WhereContainsBulked(x => x.Id, this.AdditionalIds));
            }
        }

        /// <inheritdoc />
        protected override IDictionary<long, EpdProxy> GetCache()
        {
            return this.GetProxies(this.FilterService.GetFiltredQuery<AccountPaymentInfoSnapshot>())
                .ToDictionary(x => x.Id);
        }

        protected ICollection<EpdProxy> GetProxies(IQueryable<AccountPaymentInfoSnapshot> query)
        {
            var calcAccountRepository = this.Container.ResolveRepository<CalcAccount>();
            var cashPaymentCenterPersAccRepository = this.Container.ResolveRepository<CashPaymentCenterPersAcc>();
            var basePersonalAccountRepository = this.Container.ResolveRepository<BasePersonalAccount>();
            var chargePeriodRepository = this.Container.Resolve<IChargePeriodRepository>();
            var regopPersAccService = this.Container.Resolve<IRegopCalcAccountService>();
            var personalAccountService = this.Container.Resolve<IPersonalAccountService>();
            var personalAccountPeriodSummaryDomain = this.Container.ResolveDomain<PersonalAccountPeriodSummary>();

            using (this.Container.Using(calcAccountRepository, 
                cashPaymentCenterPersAccRepository, 
                chargePeriodRepository, 
                basePersonalAccountRepository,
                regopPersAccService,
                personalAccountService,
                personalAccountPeriodSummaryDomain))
            {
                var accounts = this.ProxySelectorFactory.GetSelector<KvarProxy>().GetProxyList();

                var chargePeriod = chargePeriodRepository.Get(this.FilterService.PeriodId);
                var exportByDate = this.FilterService.ExportDate;

                var contragentRschetDict = new Dictionary<string, long?>();

                var contragentRschetGroup = calcAccountRepository.GetAll()
                    .WhereNotEmptyString(x => x.AccountNumber)
                    .Select(x => new
                    {
                        x.Id,
                        AccountOwner = (long?) x.AccountOwner.ExportId,
                        x.AccountNumber
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.AccountNumber);
                try
                {
                    contragentRschetDict = contragentRschetGroup.ToDictionary(x => x.Key,
                        x => x.Select(y => (long?) y.Id)
                            .DistinctValues()
                            .SingleOrDefault());
                }
                catch (InvalidOperationException e)
                {
                    var errorContragents = contragentRschetGroup.Where(x => x.Count() > 1)
                        .Select(x => $"{x.Key}: {x.Select(y => y.AccountOwner.ToStr()).AggregateWithSeparator(", ")}")
                        .AggregateWithSeparator("; ");

                    throw new Exception($"Для одного расчетного счета привязано более одного контрагента: {errorContragents}", e);
                }

                var transferQuery = this.FilterService.GetFiltredQuery<PersonalAccountPaymentTransfer>();
                var bankDocumentImportQuery = this.FilterService.GetFiltredQuery<BankDocumentImport>();
                var bankAccountStatementQuery = this.FilterService.GetFiltredQuery<BankAccountStatement>();
                var paymentSnapshotQuery = this.FilterService.GetFiltredQuery<AccountPaymentInfoSnapshot>();

                var lastPaymentDict = transferQuery
                    .Where(x => bankDocumentImportQuery.Any(y => y.TransferGuid == x.Operation.OriginatorGuid)
                        || bankAccountStatementQuery.Any(y => y.TransferGuid == x.Operation.OriginatorGuid))
                    .Select(x => new
                    {
                        x.Owner.Id,
                        x.PaymentDate
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id, x => x.PaymentDate)
                    .ToDictionary(x => x.Key, x => (DateTime?)x.OrderByDescending(y => y).FirstOrDefault());

                var accountList = paymentSnapshotQuery
                    .Select(x => new
                    {
                        x.Id,
                        x.Snapshot.Period.StartDate,
                        x.ObjectCreateDate,
                        x.AccountId,
                        x.Snapshot.PaymentReceiverAccount,
                        x.Snapshot.TotalCharge,
                        x.Snapshot.AccountCount,
                        x.Snapshot.Data,
                    })
                    .AsEnumerable()
                    .Select(x => new
                    {
                        x.Id,
                        x.StartDate,
                        x.ObjectCreateDate,
                        x.AccountId,
                        x.PaymentReceiverAccount,
                        x.TotalCharge,
                        x.AccountCount,
                        Data = !string.IsNullOrWhiteSpace(x.Data)
                            ? JsonConvert.DeserializeObject<InvoiceInfo>(x.Data)
                            : null,
                    })
                    .Where(x => x.Data != null)
                    .Select(x =>
                    {
                        var debt = x.Data.ДолгНаНачало;
                        var summaryDebt = (x.Data.ДолгБазовыйНаНачало ?? 0) + (x.Data.ДолгТарифРешенияНаНачало ?? 0)
                            + (x.Data.ДолгПениНаНачало ?? 0);
                        var previousPeriodDebtDebt = debt ?? (summaryDebt > 0 ? summaryDebt : (decimal?) null);
                        var overpayments = x.Data.ПереплатаНаНачало ?? (summaryDebt < 0 ? summaryDebt : (decimal?) null);
                        var totalPayment = previousPeriodDebtDebt.HasValue || overpayments.HasValue
                            ? x.TotalCharge + (previousPeriodDebtDebt ?? 0) + (overpayments ?? 0)
                            : default(decimal?);
                        var kvarInfo = accounts.Get(x.AccountId);
                        var state = (x.Data.НачисленоБазовый ?? 0) + (x.Data.НачисленоТарифРешения ?? 0) + (x.Data.НачисленоПени ?? 0)
                            + (x.Data.КорректировкаБазовый ?? 0) + (x.Data.КорректировкаТарифРешения ?? 0) + (x.Data.КорректировкаПени ?? 0)
                            - ((x.Data.ОтменыКорректировкаБазовый ?? 0) + (x.Data.ОтменыКорректировкаТарифРешения ?? 0) 
                                + (x.Data.ОтменыКорректировкаПени ?? 0))
                            == (x.Data.ОплаченоБазовый ?? 0) + (x.Data.ОплаченоТарифРешения ?? 0) + (x.Data.ОплаченоПени ?? 0);
                        var charge = (x.Data.НачисленоБазовый ?? 0) + (x.Data.НачисленоТарифРешения ?? 0) + (x.Data.НачисленоПени ?? 0);
                        return new EpdProxy
                        {
                            Id = x.Id,
                            ReportPeriod = x.StartDate,
                            DocNumber = x.Data?.НомерДокумента,
                            Date = x.ObjectCreateDate,
                            AccountId = x.AccountId,
                            ContragentRschetId = contragentRschetDict.Get(x.PaymentReceiverAccount),
                            ResidentCount = kvarInfo?.ResidentCount ?? x.AccountCount,
                            TotalArea = x.Data?.ОбщаяПлощадь,
                            LivingArea = kvarInfo?.LivingArea,
                            PreviousPeriodDebtDebt = this.NonZero(previousPeriodDebtDebt),
                            Overpayment = this.NonZero(overpayments),
                            TotalPayment = totalPayment,
                            AllTotalPayment = x.TotalCharge,
                            Paid = x.Data.ОплаченоБазовый + x.Data.ОплаченоТарифРешения + x.Data.ОплаченоПени ?? 0,
                            LastPaymentDate = lastPaymentDict.Get(x.AccountId),
                            ContragentId = kvarInfo?.CashPaymentCenterContragentId,

                            Tariff = x.Data.Тариф,
                            Correction = x.Data.КорректировкаБазовый + x.Data.КорректировкаТарифРешения + x.Data.КорректировкаПени,
                            Recalc = x.Data.ПерерасчетБазовый + x.Data.ПерерасчетТарифРешения + x.Data.ПерерасчетПени,
                            Charge = x.Data.НачисленоБазовый + x.Data.НачисленоТарифРешения + x.Data.НачисленоПени,
                            Benefit = x.Data.Льгота ?? 0,
                            SaldoOut = x.Data.ДолгБазовыйНаКонец + x.Data.ДолгТарифРешенияНаКонец + x.Data.ДолгПениНаКонец,

                            KvisolResult = state ? 1 : 2,
                            KvisolSum = charge * 100
                        };
                    });

                var chargePeriodStartDate = chargePeriod?.StartDate.Date;

                var cashPaymentCenterQuery = cashPaymentCenterPersAccRepository.GetAll()
                    .Where(x => x.DateStart <= exportByDate && (!x.DateEnd.HasValue || exportByDate <= x.DateEnd))
                    .Where(x => x.CashPaymentCenter.ConductsAccrual);

                var persAccPeriodQuery = personalAccountPeriodSummaryDomain.GetAll()
                    .Where(x => cashPaymentCenterQuery.Any(y => y.PersonalAccount.Id == x.PersonalAccount.Id))
                    .WhereIfContainsBulked(this.FilterService.PersAccIds.Count > 0, x => x.PersonalAccount.Id, this.FilterService.PersAccIds);

                var summariesDict = persAccPeriodQuery
                    .Where(x => x.Period.StartDate.Date == chargePeriodStartDate)
                    .Select(x => new
                    {
                        x.Id,
                        PersAccId = x.PersonalAccount.Id,
                        PeriodId = x.Period.Id,
                        x.Period.StartDate,
                        x.SaldoOut,
                        x.SaldoIn,
                        Paid = x.TariffPayment + x.TariffDecisionPayment,
                        TotalPayment = x.SaldoOut + x.SaldoIn,
                        ChargedByBaseTariff = (x.ChargeTariff + x.Penalty + x.RecalcByBaseTariff + x.RecalcByDecisionTariff + x.RecalcByPenalty) 
                        + (x.BaseTariffChange + x.DecisionTariffChange + x.PenaltyChange),
                        Recalc = x.RecalcByBaseTariff + x.RecalcByDecisionTariff + x.RecalcByPenalty
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.PersAccId)
                    .ToDictionary(x => x.Key, x => x.FirstOrDefault());

                var prevPeriodSummariesDict = persAccPeriodQuery
                    .Where(y => y.Period.StartDate.Date < chargePeriodStartDate)
                    .Select(x => new
                    {
                        PersAccId = x.PersonalAccount.Id,
                        TariffDebt = x.BaseTariffDebt + x.DecisionTariffDebt
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.PersAccId)
                    .ToDictionary(x => x.Key, x => x.Select(y => y.TariffDebt).FirstOrDefault());

                var accountByPaymentCenterList = basePersonalAccountRepository.GetAll()
                    .Where(x => cashPaymentCenterQuery.Any(y => y.PersonalAccount.Id == x.Id))
                    .WhereIfContainsBulked(this.FilterService.PersAccIds.Count > 0, x => x.Id, this.FilterService.PersAccIds)
                    .AsEnumerable()
                    .Select(x =>
                    {
                        var periodSummary = summariesDict.Get(x.Id);
                        var prevTariffDebt = prevPeriodSummariesDict.Get(x.Id);
                        var totalPayment = periodSummary.TotalPayment + prevTariffDebt;
                        
                        var kvarInfo = accounts.Get(x.Id);
                        var settlementAccount = regopPersAccService.GetRegopAccount(x.Room.RealityObject).ContragentCreditOrg.SettlementAccount;

                        var tariff = personalAccountService.GetTariff(x, DateTime.Today);

                        return new EpdProxy
                        {
                            Id = x.Id,
                            ReportPeriod = chargePeriodStartDate,

                            Date = periodSummary.StartDate,
                            AccountId = x.Id,
                            ContragentRschetId = contragentRschetDict.Get(settlementAccount),
                            ResidentCount = kvarInfo?.ResidentCount,
                            LivingArea = x.Room.LivingArea,
                            TotalArea = x.Room.Area,
                            PreviousPeriodDebtDebt = this.NonZero(periodSummary?.SaldoIn),
                            Overpayment = this.NonZero(prevTariffDebt),
                            TotalPayment = totalPayment,
                            AllTotalPayment = totalPayment,
                            Paid = periodSummary.Paid,
                            LastPaymentDate = lastPaymentDict.Get(x.Id),

                            //EPDCAPITAL
                            Tariff = tariff,
                            Charge = periodSummary.ChargedByBaseTariff,
                            Correction = periodSummary.Recalc,
                            Benefit = 0,
                            SaldoOut = periodSummary.SaldoOut,
                            ContragentId = kvarInfo?.CashPaymentCenterContragentId
                        };
                    });

                return accountList.Union(accountByPaymentCenterList).ToList();
            }
        }
    }
}