namespace Bars.Gkh.RegOperator.Domain.ReferenceCalculation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Dynamic.Core;
    
    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Config;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Dict;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;

    using Castle.Windsor;

    public abstract class ReferenceCalculationService : IReferenceCalculationService
    {
        public abstract DebtCalc DebtCalc { get; }
        public abstract PenaltyCharge PenaltyCharge { get; }

        protected IWindsorContainer Container { get; set; }

        protected IDomainService<ChargePeriod> PeriodDomain { get; set; }

        protected IDomainService<LawsuitReferenceCalculation> RefCalcDomain { get; set; }

        protected IDomainService<PaymentPenalties> PayPenDomain { get; set; }

        protected IDomainService<PaysizeRecord> PaySizeDomain { get; set; }

        protected IDomainService<MonthlyFeeAmountDecHistory> DecisionDomain { get; set; }

        protected IDomainService<EntityLogLight> EntityLogLightDomain { get; set; }

        protected IDomainService<PersonalAccountPaymentTransfer> PaymentDomain { get; set; }

        protected IDomainService<PersonalAccountChargeTransfer> ChargeDomain { get; set; }

        protected IDomainService<Lawsuit> LawsuitDomain { get; set; }

        public ReferenceCalculationService()
        {
            Container = ApplicationContext.Current.Container;
            PeriodDomain = Container.ResolveDomain<ChargePeriod>();
            RefCalcDomain = Container.ResolveDomain<LawsuitReferenceCalculation>();
            PayPenDomain = Container.ResolveDomain<PaymentPenalties>();
            PaySizeDomain = Container.ResolveDomain<PaysizeRecord>();
            DecisionDomain = Container.ResolveDomain<MonthlyFeeAmountDecHistory>();
            EntityLogLightDomain = Container.ResolveDomain<EntityLogLight>();
            PaymentDomain = Container.ResolveDomain<PersonalAccountPaymentTransfer>();
            ChargeDomain = Container.ResolveDomain<PersonalAccountChargeTransfer>();
            LawsuitDomain = Container.ResolveDomain<Lawsuit>();
        }

        public abstract IDataResult CalculateReferencePayments(Lawsuit lawsuit, List<ClaimWorkAccountDetail> claimWorkAccountDetailList, List<long> transfers);

        protected void ClearReferenceCalculation(long docId)
        {
            try
            {
                long lawsuitId = Container.Resolve<IDomainService<Lawsuit>>().Get(docId).Id;

                var currentCalc = RefCalcDomain.GetAll()
                    .Where(x => x.Lawsuit.Id == lawsuitId)
                    .Select(x => x.Id).ToList();

                foreach (long calcId in currentCalc)
                {
                    RefCalcDomain.Delete(calcId);
                }
            }
            finally
            {
                Container.Release(RefCalcDomain);
            }
        }

        protected ChargePeriod GetLastReferenceCalculationPeriod(DateTime documentDate)
        {
            bool documentDateAfter25Day = documentDate.Day > 25;

            var lastPeriod = !documentDateAfter25Day ? PeriodDomain.GetAll().FirstOrDefault(x => x.StartDate <= documentDate.AddMonths(-2).Date && x.EndDate >= documentDate.AddMonths(-2).Date)
                : PeriodDomain.GetAll().FirstOrDefault(x => x.StartDate <= documentDate.AddMonths(-1).Date && (x.EndDate ?? DateTime.MaxValue) >= documentDate.AddMonths(-1).Date);

            Container.Release(PeriodDomain);
            return lastPeriod;
        }

        protected static decimal CalculateMonthCharge(decimal tariff, decimal area, decimal share, DateTime openDate = default)
        {
            if (openDate == default)
            {
                return (area * share * tariff).RoundDecimal(2);
            }

            int daysInMonth = DateTime.DaysInMonth(openDate.Year, openDate.Month);
            int daysCounted = (new DateTime(openDate.AddMonths(1).Year, openDate.AddMonths(1).Month, 1) - openDate).Days;

            return (area * share * tariff * decimal.Divide(daysCounted, daysInMonth)).RoundDecimal(2);
        }

        protected static DateTime CalculateDebtStartDate(decimal tariff, decimal area, decimal share, decimal debt, DateTime month)
        {
            int daysInMonth = DateTime.DaysInMonth(month.Year, month.Month);
            decimal totalMonthCharge = tariff * area * share;
            decimal day;
            if (totalMonthCharge > 0)
            {
                day = (totalMonthCharge - debt) * daysInMonth / totalMonthCharge;
            }
            else
            {
                day = 0;
            }

            DateTime resutDate = new DateTime(month.Year, month.Month, 1).AddDays((int)day);
            return resutDate;
        }

        protected SumsBySummary GetSummarySums(ClaimWorkAccountDetail claimWorkAccountDetail, ChargePeriod lastPeriod)
        {
            var summuries = claimWorkAccountDetail.PersonalAccount.Summaries.Where(x => x.Period.Id >= lastPeriod.Id).ToList();

            decimal saldoIn = summuries[0].SaldoIn;
            decimal baseTarifDebt = summuries[0].BaseTariffDebt;
            decimal decisionDebt = summuries[0].DecisionTariffDebt;
            decimal penaltyDebt = summuries[0].PenaltyDebt;
            decimal paymentsBaseTarif = 0;
            decimal paymentsDecision = 0;
            decimal paymentsPenalty = 0;
            decimal recalcBaseTarif = 0;
            decimal recalcDecision = 0;
            decimal recalcPenalty = 0;

            //не ясно как поступать с изменениями сальдо в последующих периодах. Может стоит их прибавить 
            decimal saldoChanges = (decimal)summuries[0].BalanceChanges.Sum(x => x.CurrentValue - x.NewValue);
            decimal decisionChanges = (decimal)summuries[0].DecisionTariffChange;
            decimal penaltyChanges = (decimal)summuries[0].PenaltyChange;

            summuries.ForEach(x =>
            {
                paymentsBaseTarif += x.TariffPayment;
                paymentsDecision += x.TariffDecisionPayment;
                paymentsPenalty += x.PenaltyPayment;
                recalcBaseTarif += x.RecalcByBaseTariff;
                recalcPenalty += x.RecalcByPenalty;
                recalcDecision += x.RecalcByDecisionTariff;
            });

            //считаем задолженость
            decimal totalDebt = saldoIn + saldoChanges + (recalcBaseTarif + recalcDecision) + (recalcPenalty + penaltyChanges) - (paymentsBaseTarif + paymentsDecision + paymentsPenalty);

            baseTarifDebt = recalcBaseTarif + saldoChanges - paymentsBaseTarif;
            decisionDebt = recalcDecision + decisionChanges - paymentsDecision;
            penaltyDebt = recalcPenalty + penaltyChanges - paymentsPenalty;

            return new SumsBySummary(baseTarifDebt, decisionDebt, penaltyDebt);
        }

        public struct SumsBySummary
        {
            public decimal baseTarifDebtSum;
            public decimal decisionDebtSum;
            public decimal penaltyDebtSum;

            public SumsBySummary(decimal baseTarifDebtSum, decimal decisionDebtSum, decimal penaltyDebtSum)
            {
                this.baseTarifDebtSum = baseTarifDebtSum;
                this.decisionDebtSum = decisionDebtSum;
                this.penaltyDebtSum = penaltyDebtSum;
            }
        }
    }
}