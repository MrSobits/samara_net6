namespace Bars.Gkh.RegOperator.Domain.AggregationRoots
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using Bars.Gkh.RegOperator.Entities.ValueObjects;

    using Entities;

    /// <summary>
    /// Корень операций на доме
    /// </summary>
    public class RealtyObjectPaymentRoot
    {
        private readonly IList<Transfer> transfers;

        /// <summary>
        /// Счет оплат
        /// </summary>
        public RealityObjectPaymentAccount PaymentAccount { get; protected set; }

        /// <summary>
        /// Счёт начислений
        /// </summary>
        public RealityObjectChargeAccount ChargeAccount { get; protected set; }

        /// <summary>
        /// Трансфер
        /// </summary>
        public IReadOnlyList<Transfer> Transfers => new ReadOnlyCollection<Transfer>(this.transfers);

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="paymentAccount">Счет оплат</param>
        /// <param name="chargeAccount">Счёт начислений</param>
        public RealtyObjectPaymentRoot(RealityObjectPaymentAccount paymentAccount, RealityObjectChargeAccount chargeAccount)
        {
            this.PaymentAccount = paymentAccount;
            this.ChargeAccount = chargeAccount;
            this.transfers = new List<Transfer>();
        }

        /// <summary>
        /// Добавить трансфер для сохранения
        /// </summary>
        /// <param name="transfer">Трансфер</param>
        public void AddTransfer(Transfer transfer)
        {
            this.transfers.Add(transfer);
        }

        /// <summary>
        /// Очистить трансферы
        /// </summary>
        public void ClearTransfers()
        {
            this.transfers.Clear();
        }
    }
}