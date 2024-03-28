namespace Bars.Gkh.RegOperator.DataProviders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.IoC;
    using B4.Modules.Analytics.Data;
    using B4.Utils;
    using Castle.Windsor;
    using DomainModelServices;
    using Entities;
    using Entities.PersonalAccount;
    using Entities.ValueObjects;
    using Gkh.Domain;
    using Meta;
    
    /// <summary>
    /// Провайдер для получения данных для Отчета по Лицевому счету для данных по операциям
    /// </summary>
    class PersonalAccountOperationDataProvider : BaseCollectionDataProvider<PersonalAccountOperationInfo>
    {
        private long _accountId;

        /// <summary>
        /// Создает новый экземпляр.
        /// </summary>
        public PersonalAccountOperationDataProvider(IWindsorContainer container, long accountId) : base(container)
        {
            _accountId = accountId;
        }

        /// <summary>
        /// Метод получения данных. Реализуется в наследниках, 
        /// которые должны возвращать <see cref="IQueryable{T}"/>.
        /// К этим данным будут применены фильтры.
        /// </summary>
        /// <returns></returns>
        protected override IQueryable<PersonalAccountOperationInfo> GetDataInternal(BaseParams baseParams)
        {
            if (baseParams.Params.ContainsKey("accountId"))
            {
                _accountId = baseParams.Params.GetAsId("accountId");
            }

            var accountDomain = Container.ResolveDomain<BasePersonalAccount>();
            var summaryDomain = Container.ResolveDomain<PersonalAccountPeriodSummary>();
            var chargeDomain = Container.ResolveDomain<PersonalAccountCharge>();
            var balanceChangeDomain = Container.ResolveDomain<PeriodSummaryBalanceChange>();
            var chargePeriodDomain = Container.ResolveDomain<ChargePeriod>();
            var transferDomain = Container.ResolveDomain<Transfer>();
            var detailService = Container.Resolve<IPersonalAccountDetailService>();

            List<PersonalAccountOperationInfo> data = new List<PersonalAccountOperationInfo>();

            using (Container.Using(summaryDomain, chargeDomain, balanceChangeDomain, chargePeriodDomain, transferDomain, detailService))
            {
                var account = accountDomain.Get(_accountId);

                var periods = chargePeriodDomain.GetAll()
                    .Select(x => new { x.Id, x.StartDate, x.EndDate })
                    .OrderBy(x => x.StartDate)
                    .ToList();

                Func<DateTime, long> getPeriodId = x => periods
                    .Where(y => y.StartDate <= x)
                    .Where(y => y.EndDate == null || y.EndDate >= x)
                    .Select(y => y.Id)
                    .FirstOrDefault();

                var chargesByPeriodDict = chargeDomain.GetAll()
                    .Where(x => x.IsFixed)
                    .Where(x => x.BasePersonalAccount.Id == _accountId)
                    .Select(x => new
                    {
                        x.ChargeDate,
                        x.ChargeTariff,
                        Recalc = x.RecalcByBaseTariff + x.RecalcByDecisionTariff,
                        x.Penalty
                    })
                    .AsEnumerable()
                    .GroupBy(x => getPeriodId(x.ChargeDate))
                    .ToDictionary(x => x.Key, x =>
                        new
                        {
                            ChargeTariff = x.Sum(y => y.ChargeTariff),
                            Recalc = x.Sum(y => y.Recalc),
                            Penalty = x.Sum(y => y.Penalty)
                        });

                var tarifWalletGuids = new List<string>
                {
                    account.BaseTariffWallet.WalletGuid,
                    account.DecisionTariffWallet.WalletGuid
                };
                
                var penaltyWalletGuid = account.PenaltyWallet.WalletGuid;

                var penaltyTransfers = transferDomain.GetAll()
                    .Where(x => penaltyWalletGuid == x.TargetGuid || (penaltyWalletGuid == x.SourceGuid && !x.IsCopyForSource))
                    .Select(x => new
                    {
                        x.OperationDate,
                        Sum = x.Operation.CanceledOperation != null
                            ? -1 * x.Amount : (tarifWalletGuids.Contains(x.SourceGuid))
                            ? x.TargetCoef * x.Amount : x.Amount
                    })
                    .ToArray()
                    .GroupBy(x => getPeriodId(x.OperationDate))
                    .ToDictionary(x => x.Key, x => x.Sum(y => y.Sum));

                var saldoChangeByPeriod = balanceChangeDomain.GetAll()
                    .Where(x => x.PeriodSummary.PersonalAccount.Id == _accountId)
                    .Select(x => new
                    {
                        x.NewValue,
                        x.CurrentValue,
                        x.ObjectCreateDate
                    })
                    .AsEnumerable()
                    .GroupBy(x => getPeriodId(x.ObjectCreateDate))
                    .ToDictionary(x => x.Key, x => x.Sum(y => y.NewValue - y.CurrentValue));

                var summaries = account.Summaries.OrderBy(x => x.Period.StartDate).ToArray();

                var dict = new Dictionary<long, string>();
                foreach (var period in summaries)
                {
                    var details = detailService.GetPeriodOperationDetail(period);
                    var dates = new List<DateTime>();
                    foreach (var detail in details)
                    {
                        if (detail.Name.ToLower() == "оплата по тарифу решения" || detail.Name.ToLower() == "оплата по базовому тарифу")
                        {
                            dates.Add(detail.Date);
                        }
                    }
                    dates.Sort();
                    dict.Add(period.Id, (dates.Count > 0) ? dates.First().ToShortDateString() : "-");
                    dates.Clear();
                }

                foreach (var summary in summaries)
                {
                    var chargedByBaseTariff = summary.GetTotalCharge();

                    data.Add(new PersonalAccountOperationInfo
                    {
                        Период = summary.Period.Name,
                        ВходящееСальдо = summary.SaldoIn.RoundDecimal(2),
                        НачисленоВзносов = chargedByBaseTariff.RoundDecimal(2),
                        НачисленоПени = chargesByPeriodDict.Get(summary.Id).Return(z => z.Penalty).RoundDecimal(2),
                        Перерасчет = (summary.RecalcByBaseTariff + summary.RecalcByDecisionTariff).RoundDecimal(2),
                        УплаченоВзносов = summary.GetTotalPayment().RoundDecimal(2),
                        ДатаОплаты = dict.ContainsKey(summary.Id) ? dict[summary.Id] : "-",
                        УплаченоПени = penaltyTransfers.Get(summary.Id).RoundDecimal(2),
                        УстановкаИзменениеСальдо = saldoChangeByPeriod.Get(summary.Id).RoundDecimal(2),
                        ИсходящееСальдо = summary.SaldoOut.RoundDecimal(2)

                        /*
                        Оставлю это здесь, чтобы видеть, как берутся значения для представления в ui
                        Id = summary.Id,
                        Period = summary.Period.Name,
                        PeriodStart = summary.Period.StartDate,
                        SaldoIn = summary.SaldoIn,
                        SaldoOut = summary.SaldoOut,
                        ChargedByBaseTariff = chargedByBaseTariff,
                        TariffPayment = summary.GetTotalPayment(),
                        SaldoChange = chargedByBaseTariff - summary.GetTotalPayment(),
                        Recalc = summary.RecalcByBaseTariff + summary.RecalcByDecisionTariff,
                        AccountId = summary.PersonalAccount.Id,
                        PeriodId = summary.Period.Id,
                        CurrTariffDebt = summary.BaseTariffDebt + summary.DecisionTariffDebt
                            - (summary.TariffPayment + summary.TariffDecisionPayment),
                        OverdueTariffDebt = summary.SaldoOut
                            - (summary.BaseTariffDebt + summary.DecisionTariffDebt - (summary.TariffPayment + summary.TariffDecisionPayment))*/
                    });
                }
            }

            return data.AsQueryable();
        }

        /// <summary>
        /// Наименование провайдера  Отчета по ЛС по операциям
        /// </summary>
        public override string Name
        {
            get { return null; }
        }

        /// <summary>
        /// Описание провайдера  Отчета по ЛС по операциям
        /// </summary>
        public override string Description
        {
            get { return null; }
        }
    }
}