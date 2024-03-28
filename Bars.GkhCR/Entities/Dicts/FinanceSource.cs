namespace Bars.GkhCr.Entities
{
    using Bars.Gkh.Entities;
    using Bars.GkhCr.Enums;

    /// <summary>
    /// Разрез финансирования по КР
    /// </summary>
    public class FinanceSource : BaseGkhEntity
    {
        /// <summary>
        /// Группа финансирования
        /// </summary>
        public virtual TypeFinanceGroup TypeFinanceGroup { get; set; }

        /// <summary>
        /// Тип разреза
        /// </summary>
        public virtual TypeFinance TypeFinance { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Наимеонвание
        /// </summary>
        public virtual string Name { get; set; }
    }
}
