namespace Bars.Gkh.RegOperator.DomainModelServices.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4.Utils;
    using Domain;
    using Domain.Interfaces;
    using Domain.ParametersVersioning;
    using Domain.ProxyEntity;
    using Entities;
    using Entities.Dict;
    using Entities.PersonalAccount;
    using Enums;
    using Gkh.Domain.CollectionExtensions;
    using Gkh.Entities;
    using Gkh.Enums.Decisions;
    using Overhaul.Utils;

    public class NoRecalcPenaltyCalculator
    {
        public NoRecalcPenaltyCalculator(IParameterTracker parameterTracker, ICalculationGlobalCache globalCache)
        {
            _parameterTracker = parameterTracker;
            _globalCache = globalCache;
        }

        public CalculationResult<Dictionary<IPeriod, decimal>> Calculate(
            BasePersonalAccount account,
            IPeriod period,
            UnacceptedCharge unAccepted)
        {
            var result = new CalculationResult<Dictionary<IPeriod, decimal>>();

            if (account.OpenDate > period.StartDate)
            {
                return result;
            }

            var calculation = new Dictionary<IPeriod, decimal>();

            var penaltyParamList = GetPenaltyParams();

            var allChanges = _parameterTracker.GetChanges(account, period).ToList();

            allChanges.ForEach(x => result.AddParam(new PersonalAccountCalcParam_tmp
            {
                LoggedEntity = new EntityLogLight { Id = x.LogId },
                PersonalAccount = account
            }));

            var needToRecalcPeriods = _globalCache.GetClosedPeriods().Cast<IPeriod>().ToList();

            needToRecalcPeriods = needToRecalcPeriods
                .Where(x => allChanges.Any(y => y.DateActualChange <= x.EndDate && x.EndDate <= period.StartDate))
                .OrderBy(x => x.StartDate)
                .ToList();

            needToRecalcPeriods = needToRecalcPeriods.OrderBy(x => x.StartDate).ToList();

            if (needToRecalcPeriods.All(x => x.Id != period.Id))
            {
                needToRecalcPeriods.Add(period);
            }

            var allPayments = _globalCache.GetPaymentTransfers(account);

            var roFundFormationType = _globalCache.GetRoFundFormationType(account).ToList();

            var paymentsInRange = allPayments
                .GroupBy(x => x.PaymentDate.Date)
                .Select(x => new
                {
                    PaymentDate = x.Key,
                    Amount = x.Sum(z => z.Amount)
                })
                .ToList();

            var accountSummaries = _globalCache.GetAllSummaries(account).ToList();

            foreach (IPeriod penaltyRecalcPeriod in needToRecalcPeriods)
            {
                var prevPeriodSummary =
                    accountSummaries
                        .OrderByDescending(x => x.Period.StartDate)
                        .FirstOrDefault(x => x.Period.StartDate < penaltyRecalcPeriod.StartDate);

                var periodStartDate = penaltyRecalcPeriod.StartDate;
                var periodEndDate = penaltyRecalcPeriod.GetEndDate();

                var roFundFormation =
                    roFundFormationType
                        .FirstOrDefault(x => x.Item1 <= periodStartDate)
                        .Return(x => x.Item2, CrFundFormationDecisionType.Unknown);

                var currentAccountSummary =
                    accountSummaries
                        .FirstOrDefault(x => x.Period.Id == penaltyRecalcPeriod.Id);

                var currentSaldo = currentAccountSummary.Return(x => x.SaldoIn);

                // Пени для того, чтобы их вычесть
                var prevPeriodPenalties =
                    accountSummaries
                        .Where(x => x.Period.StartDate < periodStartDate)
                        .Select(x => x.Penalty - x.PenaltyPayment)
                        .SafeSum(x => x);

                var currentBalance =
                    currentSaldo
                    - prevPeriodSummary.Return(x => x.ChargeTariff)
                    - prevPeriodPenalties;

                var currentPenaltyParameter =
                    penaltyParamList
                        .OrderByDescending(x => x.DateStart)
                        .Where(x => x.DecisionType == roFundFormation)
                        .FirstOrDefault(x => x.DateStart <= penaltyRecalcPeriod.StartDate);

                var days = currentPenaltyParameter.Return(x => x.Days);
                var percent = currentPenaltyParameter.Return(x => x.Percentage);

                var recalcPeriod = new RecalcPeriodProxy(periodStartDate, periodEndDate, currentBalance, days, percent);

                var startDay = periodStartDate.AddDays(days);

                // Платежи, полученные до начала расчета пеней
                paymentsInRange
                    .Where(x => x.PaymentDate >= periodStartDate)
                    .Where(x => x.PaymentDate < startDay)
                    .ForEach(x => recalcPeriod.ApplySum(x.PaymentDate, (-1) * x.Amount));

                //добавляем начисления за текущий период
                recalcPeriod.ApplySum(prevPeriodSummary.Return(x => x.ChargeTariff));

                // Платежи, полученные после начала расчета пеней
                paymentsInRange
                    .Where(x => x.PaymentDate >= startDay)
                    .Where(x => x.PaymentDate <= periodEndDate)
                    .ForEach(x => recalcPeriod.ApplySum(x.PaymentDate, (-1) * x.Amount));

                calculation.Add(penaltyRecalcPeriod, recalcPeriod.GetSumPenalties());

                recalcPeriod.Penalties
                    .Where(x => x.Balance > 0)
                    .ForEach(x => result.AddTrace(new CalculationParameterTrace
                    {
                        ParameterValues = new Dictionary<string, object>
                        {
                            {"payment_penalty_percentage", percent},
                            {"penalty_debt", x.Balance}
                        },
                        DateStart = x.Start,
                        DateEnd = x.End ?? periodEndDate,
                        CalculationType = CalculationTraceType.Penalty,
                        CalculationGuid = unAccepted.Guid,
                        ChargePeriod = (ChargePeriod)penaltyRecalcPeriod
                    }));
            }

            result.Result = calculation;

            return result;
        }

        private List<PaymentPenalties> GetPenaltyParams()
        {
            return _globalCache.GetPenaltyParams().ToList();
        }

        private readonly IParameterTracker _parameterTracker;
        private readonly ICalculationGlobalCache _globalCache;
        private static decimal PENALTY_CONSTANT = (1M / 300);

        private class RecalcPeriodProxy
        {
            public RecalcPeriodProxy(
                DateTime periodStart,
                DateTime periodEnd,
                decimal currBalance,
                int days,
                decimal percent)
            {
                _periodStart = periodStart;
                _periodEnd = periodEnd;
                _currBalance = currBalance;
                _days = days;
                _percent = percent;
                Penalties = new List<PenaltyPeriod>();
            }

            private DateTime _periodStart;

            private DateTime _periodEnd;

            private decimal _currBalance;

            private readonly int _days;

            private readonly decimal _percent;

            public readonly List<PenaltyPeriod> Penalties;

            public void ApplySum(decimal sum = 0)
            {
                ApplySum(_periodStart.AddDays(_days - 1), sum);
            }

            //если sum > 0 то начисление, иначе оплата
            public void ApplySum(DateTime date, decimal sum = 0)
            {
                var penalty = GetCurrent();

                //закрываем предыдущий период
                penalty.Balance = _currBalance;
                penalty.End = date;

                _currBalance += sum;

                var newPenalty = new PenaltyPeriod
                {
                    Start = date.AddDays(1),
                    Balance = _currBalance
                };
                Penalties.Add(newPenalty);
            }

            private PenaltyPeriod GetCurrent()
            {
                var penalty = Penalties.FirstOrDefault(x => !x.End.HasValue);

                if (penalty == null)
                {
                    penalty = new PenaltyPeriod { Start = _periodStart };
                    Penalties.Add(penalty);
                }

                return penalty;
            }

            public decimal GetSumPenalties()
            {
                return Penalties
                    .Where(x => x.Balance > 0)
                    .SafeSum(x => (((x.End ?? _periodEnd) - x.Start).Days + 1) * x.Balance)
                       * PENALTY_CONSTANT
                       * _percent.ToDivisional();
            }
        }

        private class PenaltyPeriod
        {
            public DateTime Start { get; set; }

            public DateTime? End { get; set; }

            public decimal Balance { get; set; }
        }
    }
}
