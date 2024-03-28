namespace Bars.Gkh.RegOperator.Entities.PersonalAccount.Operations
{
    using System.Collections.Generic;

    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// Импорт оплат
    /// </summary>
    public class PaymentImportSource : PaymentOperationBase
    {
        private readonly IList<PaymentImport> payments;

        /// <summary>
        /// .ctor
        /// </summary>
        public PaymentImportSource()
        {
        }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="period">
        /// Период
        /// </param>
        public PaymentImportSource(ChargePeriod period) : base(period)
        {
            base.PaymentSource = TypeTransferSource.ImportPayment;
            this.payments = new List<PaymentImport>();
            this.OperationDate = this.FactOperationDate;
        }

        /// <summary>
        /// Оплаты
        /// </summary>
        public virtual IEnumerable<PaymentImport> Payments => this.payments;

        /// <summary>
        /// Создать операцию по передвижению денег
        /// </summary>
        /// <returns><see cref="MoneyOperation"/></returns>
        public override MoneyOperation CreateOperation(ChargePeriod period)
        {
            var operation = base.CreateOperation(period);
            operation.Reason = "Импорт оплат от ЧЭС";

            return operation;
        }
    }
}