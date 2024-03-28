namespace Bars.Gkh.RegOperator.Domain.ValueObjects
{
    using System.Collections.Generic;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;

    public class PersonalAccountRefundResult
    {
        private readonly List<Transfer> _transfers;

        public PersonalAccountRefundResult(decimal refundFromBaseTariff, decimal refundFromDecisionTariff,
            decimal refundFromPenalty, decimal refundFromSocSupp)
        {
            _transfers = new List<Transfer>();
            RefundFromDecisionTariffWallet = refundFromDecisionTariff;
            RefundFromPenaltyWallet = refundFromPenalty;
            RefundFromBaseTariffWallet = refundFromBaseTariff;
            RefundFromSocSuppWallet = refundFromSocSupp;
        }
        public PersonalAccountRefundResult(decimal refundFromBaseTariff, decimal refundFromDecisionTariff,
            decimal refundFromPenalty, decimal refundFromSocSupp, IEnumerable<Transfer> transfers)
        {
            _transfers = new List<Transfer>(transfers);
            RefundFromDecisionTariffWallet = refundFromDecisionTariff;
            RefundFromPenaltyWallet = refundFromPenalty;
            RefundFromBaseTariffWallet = refundFromBaseTariff;
            RefundFromSocSuppWallet = refundFromSocSupp;
        }

        public decimal RefundFromBaseTariffWallet { get; protected set; }

        public decimal RefundFromDecisionTariffWallet { get; protected set; }

        public decimal RefundFromPenaltyWallet { get; protected set; }

        public decimal RefundFromSocSuppWallet { get; protected set; }

        public IEnumerable<Transfer> Transfers { get { return _transfers; } }

        public void AddTransfer(Transfer transfer)
        {
            _transfers.Add(transfer);
        }
    }
}