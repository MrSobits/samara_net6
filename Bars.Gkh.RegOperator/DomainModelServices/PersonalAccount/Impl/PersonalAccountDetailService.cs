namespace Bars.Gkh.RegOperator.DomainModelServices.PersonalAccount.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.Utils;
    using B4.Utils.Annotations;

    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Extenstions;

    using Castle.Windsor;
    using Domain;
    using Domain.Extensions;
    using Entities;
    using Entities.ValueObjects;
    using Gkh.Domain;

    /// <summary>
    /// Сервис детализации по лс
    /// </summary>
    public class PersonalAccountDetailService : IPersonalAccountDetailService
    {
        /// <summary>
        /// Конструктор сервиса
        /// </summary>
        public PersonalAccountDetailService(
            IWindsorContainer container,
            IPerformedWorkDetailService performedWorkDetailService,
            IDomainService<PersonalAccountPaymentTransfer> transferDomain,
            IDomainService<PersonalAccountChargeTransfer> chargeTransferDomain,
            IDomainService<BasePersonalAccount> accountDomain,
            IDomainService<PersonalAccountCharge> accountChargesDomain,
            IDomainService<PersonalAccountPeriodSummary> summaryDomain)
        {
            this.container = container;
            this.performedWorkDetailService = performedWorkDetailService;
            this.transferDomain = transferDomain;
            this.chargeTransferDomain = chargeTransferDomain;
            this.accountDomain = accountDomain;
            this.accountChargesDomain = accountChargesDomain;
            this.summaryDomain = summaryDomain;
        }

        private readonly IWindsorContainer container;
        private readonly IPerformedWorkDetailService performedWorkDetailService;
        private readonly IDomainService<PersonalAccountPaymentTransfer> transferDomain;
        private readonly IDomainService<PersonalAccountChargeTransfer> chargeTransferDomain;
        private readonly IDomainService<BasePersonalAccount> accountDomain;
        private readonly IDomainService<PersonalAccountCharge> accountChargesDomain;
        private readonly IDomainService<PersonalAccountPeriodSummary> summaryDomain;

        /// <summary>
        /// Детализация по периоду
        /// </summary>
        public List<PeriodDetail> GetPeriodDetail(BasePersonalAccount account)
        {
            ArgumentChecker.NotNull(account, nameof(account));

            var result = new List<PeriodDetail>();

            foreach (var summary in account.Summaries)
            {
                var chargedByBaseTariff = summary.GetTotalCharge() + summary.GetTotalChange();

                result.Add(new PeriodDetail
                {
                    Id = summary.Id,
                    Period = summary.Period.Name,
                    PeriodStart = summary.Period.StartDate,
                    SaldoIn = summary.SaldoIn,
                    SaldoOut = summary.SaldoOut,
                    ChargedByBaseTariff = chargedByBaseTariff,
                    TariffPayment = summary.GetTotalPayment(),
                    SaldoChange = chargedByBaseTariff - summary.GetTotalPayment() - summary.GetTotalPerformedWorkCharge(),
                    Recalc = summary.RecalcByBaseTariff + summary.RecalcByDecisionTariff + summary.RecalcByPenalty,
                    AccountId = summary.PersonalAccount.Id,
                    PeriodId = summary.Period.Id,
                    CurrTariffDebt = summary.BaseTariffDebt + summary.DecisionTariffDebt
                        - (summary.TariffPayment + summary.TariffDecisionPayment),
                    OverdueTariffDebt = summary.SaldoOut
                        - (summary.BaseTariffDebt + summary.DecisionTariffDebt - (summary.TariffPayment + summary.TariffDecisionPayment)),
                    PerformedWorkCharged = summary.GetTotalPerformedWorkCharge()
                });
            }

            return result;
        }

        /// <summary>
        /// Детализация по периоду
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public DataResult.ListDataResult<PeriodDetail> GetPeriodDetail(BaseParams baseParams)
        {
            var accountId = baseParams.Params.GetAsId("accountId");
            var loadParam = baseParams.GetLoadParam();

            var account = this.accountDomain.Get(accountId);

            var details = this.GetPeriodDetail(account);

            if (loadParam.Order.Length == 0)
            {
                loadParam.Order = new[] { new OrderField { Asc = true, Name = "Period" } };
            }

            this.ReplacePeriodOrder(loadParam.Order);

            return new DataResult.ListDataResult<PeriodDetail>(details.AsQueryable().Order(loadParam).Paging(loadParam).ToList(), details.Count);
        }

        /// <summary>
        /// Детализация по полям лс
        /// </summary>
        public List<FieldDetail> GetFieldDetail(BasePersonalAccount account, string fieldName)
        {
            var result = new List<FieldDetail>();

            foreach (var summary in account.Summaries)
            {
                switch (fieldName)
                {
                    case "ChargedBaseTariff":
                       // result.Add(this.GetFieldInfo(summary, x => x.GetChargedByBaseTariff() + x.BaseTariffChange - x.PerformedWorkChargedBase)); по просьбе самохвала оставляем чистые начисления
                        result.Add(this.GetFieldInfo(summary, x => x.GetChargedByBaseTariff() + x.BaseTariffChange));
                        break;
                    case "ChargedDecisionTariff":
                        result.Add(this.GetFieldInfo(summary, x => x.GetChargedByDecisionTariff() + x.DecisionTariffChange - x.PerformedWorkChargedDecision));
                        break;
                    case "ChargedPenalty":
                        result.Add(this.GetFieldInfo(summary, x => x.Penalty + x.RecalcByPenalty + x.PenaltyChange));
                        break;

                    case "PaymentBaseTariff":
                        result.Add(this.GetFieldInfo(summary, x => x.TariffPayment));
                        break;
                    case "PaymentDecisionTariff":
                        result.Add(this.GetFieldInfo(summary, x => x.TariffDecisionPayment));
                        break;
                    case "PaymentPenalty":
                        result.Add(this.GetFieldInfo(summary, x => x.PenaltyPayment));
                        break;
                    //PerfWorkCreditedBalance
                    case "PerfWorkCreditedBalance":
                        result.Add(this.GetFieldInfo(summary, x => x.PerformedWorkChargedBase));
                        break;
                    case "DebtBaseTariff":
                        result.Add(this.GetFieldInfo(summary, x => x.GetBaseTariffDebt() - x.PerformedWorkChargedBase));
                        break;
                    case "DebtDecisionTariff":
                        result.Add(this.GetFieldInfo(summary, x => x.GetDecisionTariffDebt() - x.PerformedWorkChargedDecision));
                        break;
                    case "DebtPenalty":
                        result.Add(this.GetFieldInfo(summary, x => x.GetPenaltyDebt()));
                        break;
                }
            }

            return result;
        }

        /// <summary>
        /// Детализация по полям лс
        /// </summary>
        public DataResult.ListDataResult<FieldDetail> GetFieldDetail(BaseParams baseParams)
        {
            var accountId = baseParams.Params.GetAs<long>("accId");
            var fieldName = baseParams.Params.GetAs<string>("fieldName");

            var loadParam = baseParams.GetLoadParam().ReplaceOrder("Period", "PeriodStart");

            var account = this.accountDomain.Get(accountId);

            var fieldDetail = this.GetFieldDetail(account, fieldName);

            var result = fieldDetail
                .AsQueryable()
                .OrderIf(loadParam.Order.Length == 0, true, x => x.PeriodStart)
                .Order(loadParam)
                .Paging(loadParam)
                .ToList();

            return new DataResult.ListDataResult<FieldDetail>(result, fieldDetail.Count);
        }

        /// <summary>
        /// Детализация операций по периоду
        /// </summary>
        public List<PeriodOperationDetail> GetPeriodOperationDetail(PersonalAccountPeriodSummary summary)
        {
            var walletGuids = summary.PersonalAccount.GetMainWallets().Select(x => x.WalletGuid).ToList();

            // достаём начисления открытого периода, т.к. трансферы ещё не созданы
            var charge = this.accountChargesDomain.GetAll()
                .Where(x => x.ChargePeriod.Id == summary.Period.Id && x.BasePersonalAccount.Id == summary.PersonalAccount.Id)
                .FirstOrDefault(x => !x.IsFixed && x.IsActive);

            var result = this.transferDomain.GetAll()
                .Where(x => walletGuids.Contains(x.TargetGuid) || walletGuids.Contains(x.SourceGuid))
                .Where(x => x.ChargePeriod.Id == summary.Period.Id)
                .AsEnumerable()
                .Cast<Transfer>()
                .ToList();

            result.AddRange(
                this.chargeTransferDomain.GetAll()
                    .Where(x => walletGuids.Contains(x.TargetGuid) || walletGuids.Contains(x.SourceGuid))
                    .Where(x => x.ChargePeriod.Id == summary.Period.Id));

                var data = result.Select(x => new PeriodOperationDetail
                {
                    TransferId = x.Id,
                    Date = x.PaymentDate,
                    Name = x.Reason ?? x.Operation.Reason,
                    SaldoChange =
                        x.Operation.CanceledOperation != null
                            ? -1 * x.Amount
                            : (walletGuids.Contains(x.SourceGuid)
                                /*Бля простите но уровень костылей похоже достиг своего потолка*/)
                                ? x.TargetCoef * x.Amount
                                : x.Amount,
                    Period = summary.Period.Name,
                    Document = x.Operation.Document
                })
                .ToList();

            PersonalAccountDetailService.AppendWithFakeCharges(summary, charge, data);

            if (!summary.Period.IsClosed)
            {
                this.performedWorkDetailService.GetSupposedDistributions(summary.PersonalAccount).AddTo(data);
            }

            return data;
        }
        
        /// <summary>
        /// Детализация операций по периоду
        /// </summary>
        public DataResult.ListDataResult<PeriodOperationDetail> GetPeriodOperationDetail(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var periodSummaryId = loadParams.Filter.GetAsId("periodSummaryId");

            var periodSummary = this.summaryDomain.Get(periodSummaryId);
            if (periodSummary == null)
            {
                return new DataResult.ListDataResult<PeriodOperationDetail>();
            }

            var result = this.GetPeriodOperationDetail(periodSummary)
                .AsQueryable()
                .Filter(loadParams, this.container);

            var totalCount = result.Count();

            result = result.Order(loadParams);

            return new DataResult.ListDataResult<PeriodOperationDetail>(result.ToList(), totalCount);
        }

        private FieldDetail GetFieldInfo(PersonalAccountPeriodSummary summary, Func<PersonalAccountPeriodSummary, decimal> selector)
        {
            return new FieldDetail
            {
                PeriodStart = summary.Period.StartDate,
                Period = summary.Period.Name,
                Amount = selector(summary)
            };
        }

        private void ReplacePeriodOrder(OrderField[] existOrders)
        {
            foreach (var order in existOrders)
            {
                if (order.Name == "Period")
                {
                    order.Name = "PeriodStart";
                    break;
                }
            }
        }

        private static void AppendWithFakeCharges(PersonalAccountPeriodSummary summary, PersonalAccountCharge charge, List<PeriodOperationDetail> result)
        {
            // если в текущем периоде есть незафиксированные, но активные начисления
            // то создаём фиктивные трансферы для отображения начислений
            if (charge.IsNotNull())
            {
                if (charge.ChargeTariff - charge.OverPlus != 0)
                {
                    result.Add(new PeriodOperationDetail
                    {
                        Date = charge.ChargeDate,
                        Name = "Начисление по базовому тарифу",
                        SaldoChange = charge.ChargeTariff - charge.OverPlus,
                        Period = summary.Period.Name
                    });
                }

                if (charge.RecalcByBaseTariff != 0)
                {
                    result.Add(new PeriodOperationDetail
                    {
                        Date = charge.ChargeDate,
                        Name = "Перерасчет по базовому тарифу",
                        SaldoChange = charge.RecalcByBaseTariff,
                        Period = summary.Period.Name
                    });
                }

                if (charge.OverPlus != 0)
                {
                    result.Add(new PeriodOperationDetail
                    {
                        Date = charge.ChargeDate,
                        Name = "Начисление по тарифу решения",
                        SaldoChange = charge.OverPlus,
                        Period = summary.Period.Name
                    });
                }

                if (charge.RecalcByDecisionTariff != 0)
                {
                    result.Add(new PeriodOperationDetail
                    {
                        Date = charge.ChargeDate,
                        Name = "Перерасчет по тарифу решения",
                        SaldoChange = charge.RecalcByDecisionTariff,
                        Period = summary.Period.Name
                    });
                }

                if (charge.Penalty != 0)
                {
                    result.Add(new PeriodOperationDetail
                    {
                        Date = charge.ChargeDate,
                        Name = "Начисление пени",
                        SaldoChange = charge.Penalty,
                        Period = summary.Period.Name
                    });
                }

                if (charge.RecalcPenalty != 0)
                {
                    result.Add(new PeriodOperationDetail
                    {
                        Date = charge.ChargeDate,
                        Name = "Перерасчет пени",
                        SaldoChange = charge.RecalcPenalty,
                        Period = summary.Period.Name
                    });
                }
            }
        }
    }
}