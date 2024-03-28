namespace Bars.Gkh.RegOperator.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Сущность связи между счетом нвс и кредитом
    /// </summary>
    public class SuspenseAccountCreditPayment : BaseImportableEntity
    {
        /// <summary>
        /// Счет нвс
        /// </summary>
        public virtual SuspenseAccount Account { get; set; }

        /// <summary>
        /// Кредит
        /// </summary>
        public virtual CalcAccountCredit Credit { get; set; }

        public virtual decimal PercentPayment { get; set; }

        public virtual decimal CreditPayment { get; set; }
    }
}