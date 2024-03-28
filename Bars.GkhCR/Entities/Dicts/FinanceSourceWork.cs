namespace Bars.GkhCr.Entities
{
    using Gkh.Entities.Dicts;

    using Gkh.Entities;

    /// <summary>
    /// Работы источника финансирования по КР
    /// </summary>
    public class FinanceSourceWork : BaseGkhEntity
    {
        /// <summary>
        /// Работа
        /// </summary>
        public virtual Work Work { get; set; }

        /// <summary>
        /// Разрез финансирования
        /// </summary>
        public virtual FinanceSource FinanceSource { get; set; }
    }
}
