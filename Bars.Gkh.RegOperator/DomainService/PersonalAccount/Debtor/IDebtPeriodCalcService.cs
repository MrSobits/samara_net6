namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.Debtor
{
    using System;
    using System.Collections.Generic;

    using Bars.Gkh.Entities;

    /// <summary>
    /// Сервис расчета задолженности за период
    /// </summary>
    public interface IDebtPeriodCalcService
    {
        Dictionary<long, List<DebtPeriodInfo>> DebtDict { get; }

        void Calculate(IEnumerable<long> personalAccountIds);
    }

    public struct DebtPeriodInfo
    {
        public long AccountId { get; set; }
        public decimal BaseTariffSum { get; set; }
        public decimal DecisionTariffSum { get; set; }
        public decimal PenaltySum { get; set; }
        public ChargePeriod Period { get; set; }
    }
}