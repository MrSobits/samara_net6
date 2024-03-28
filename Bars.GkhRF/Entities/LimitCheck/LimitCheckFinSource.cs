namespace Bars.GkhRf.Entities
{
    using Bars.Gkh.Entities;

    using GkhCr.Entities;

    /// <summary>
    /// Разрез финансирования проверки лимитов по заявке
    /// </summary>
    public class LimitCheckFinSource : BaseImportableEntity
    {
        /// <summary>
        /// Разрез финансирования
        /// </summary>
        public virtual FinanceSource FinanceSource { get; set; }

        /// <summary>
        /// Проверка лимитов
        /// </summary>
        public virtual LimitCheck LimitCheck { get; set; }
    }
}