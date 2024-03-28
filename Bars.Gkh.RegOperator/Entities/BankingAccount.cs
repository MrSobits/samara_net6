namespace Bars.Gkh.RegOperator.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Расчетный счет
    /// </summary>
    public class BankingAccount : BaseImportableEntity
    {
        /// <summary>
        /// Номер счета
        /// </summary>
        public virtual string BankAccountNum { get; set; }

        /// <summary>
        /// Состояние счета
        /// </summary>
        public virtual decimal CurrentBalance { get; set; }
    }
}