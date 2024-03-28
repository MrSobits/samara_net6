namespace Bars.Gkh.RegOperator.Entities.PersonalAccount.Operations
{
    using System.Collections.Generic;

    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities.ValueObjects;
    using Bars.Gkh.RegOperator.Enums;

    /// <summary>
    /// Инициатор импорта оплат в закрытый период
    /// </summary>
    public class ClosedPeriodPaymentSource : PaymentOperationBase
    {
        private readonly IList<RecordPaymentsToClosedPeriodsImport> payments;
        
        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="period">
        /// Период
        /// </param>
        public ClosedPeriodPaymentSource(ChargePeriod period) : base(period)
        {
            base.PaymentSource = TypeTransferSource.ImportsIntoClosedPeriod;
            this.payments = new List<RecordPaymentsToClosedPeriodsImport>();
            this.OperationDate = this.FactOperationDate;
        }

        /// <summary>
        /// .ctor
        /// </summary>
        protected ClosedPeriodPaymentSource()
        {
        }

        /// <summary>
        /// Оплаты
        /// </summary>
        public virtual IEnumerable<RecordPaymentsToClosedPeriodsImport> Payments => this.payments;

        /// <summary>
        /// Создать операцию по передвижению денег
        /// </summary>
        /// <returns><see cref="MoneyOperation"/></returns>
        public override MoneyOperation CreateOperation(ChargePeriod period)
        {
            var operation = base.CreateOperation(period);
            operation.Reason = "Импорт оплат в закрытый период";

            return operation;
        }
    }
}