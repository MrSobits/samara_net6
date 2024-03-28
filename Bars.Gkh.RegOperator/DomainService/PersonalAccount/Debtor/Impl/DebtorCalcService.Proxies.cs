namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.Impl
{
    using System;

    using Bars.Gkh.Entities;
    using Enums;

    public partial class DebtorCalcService
    {
        private class SumData
        {
            public SumData(decimal tariff, decimal penalty)
            {
                this.Tariff = tariff;
                this.Penalty = penalty;
            }

            public SumData(decimal tariff, decimal baseTariff, decimal decisionTariff, decimal penalty)
            {
                this.Tariff = tariff;
                this.BaseTariff = baseTariff;
                this.DecisionTariff = decisionTariff;
                this.Penalty = penalty;
            }

            public readonly decimal Tariff;
            public readonly decimal Penalty;
            public readonly decimal BaseTariff;
            public readonly decimal DecisionTariff;
        }

        private class RecalcHistoryProxy
        {
            public decimal RecalcSum;
            public long AccountId;
            public ChargePeriod RecalcPeriod;
        }

        private class PeriodSummaryDto
        {
            public long AccountId;
            public decimal Charge;
            public decimal Debt;
            public decimal DebtBaseTariff;
            public decimal DebtDecisionTariff;
            public decimal PenaltyDebt;
            public decimal Change;
            public decimal Payment;
            public decimal TariffPayment;
            public decimal TariffDecisionPayment;
            public decimal PenaltyPayment;
            public decimal Recalc;
            public decimal RecalcByBaseTariff;
            public decimal RecalcByDecisionTariff;
            public decimal RecalcByPenalty;
            public decimal BaseTariffChange;
            public decimal DecisionTariffChange;
            public decimal PenaltyChange;
            public decimal ChargedByBaseTariff;
            public decimal Penalty;
            public ChargePeriod Period;
        }

        public class BasePersonalAccountDto
        {
            public long Id { get; internal set; }
            public long OwnerId { get; set; }
            public string OwnerName { get; set; }
            public long RealityObjectId { get; set; }
            public long MunicipalityId { get; set; }
            public long MoSettlementId { get; set; }
            public string PersonalAccountNum { get; set; }
            public PersonalAccountOwnerType OwnerType { get; set; }
            public string BaseWalletGuid { get; internal set; }
            public string DecisionWalletGuid { get; internal set; }
            public string RestructAmicAgrWalletGuid { get; set; }
            public string PenaltyWalletGuid { get; set; }
            public decimal Balance { get; internal set; }
        }
    }
}