namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.Debtor.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.Entities;

    using Castle.Windsor;

    public class DebtPeriodCalcService : IDebtPeriodCalcService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<PersonalAccountPeriodSummary> PersonalAccountPeriodSummaryDomain { get; set; }

        private Dictionary<long, List<DebtPeriodInfo>> debtCache = new Dictionary<long, List<DebtPeriodInfo>>();

        public Dictionary<long, List<DebtPeriodInfo>> DebtDict => this.debtCache;

        public void Calculate(IEnumerable<long> personalAccountIds)
        {
            this.debtCache = this.GetDebts(personalAccountIds);
        }

        private Dictionary<long, List<DebtPeriodInfo>> GetDebts(IEnumerable<long> personalAccountIds)
        {
            return this.PersonalAccountPeriodSummaryDomain.GetAll()
                .WhereContains(x => x.PersonalAccount.Id, personalAccountIds)
                .AsEnumerable()
                .GroupBy(x => x.PersonalAccount.Id,
                    x => new DebtPeriodInfo
                    {
                        AccountId = x.PersonalAccount.Id,
                        BaseTariffSum = x.BaseTariffDebt,
                        DecisionTariffSum = x.DecisionTariffDebt,
                        PenaltySum = x.PenaltyDebt,
                        Period = x.Period
                    })
                .ToDictionary(x => x.Key, x => x.ToList());
        }
    }
}