namespace Bars.GkhEdoInteg.Entities
{
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Сопоставление источника поступления с кодом Эдо
    /// </summary>
    public class RevenueSourceCompareEdo : BaseGkhEntity
    {
        /// <summary>
        /// Источник поступления
        /// </summary>
        public virtual RevenueSourceGji RevenueSource { get; set; }

        /// <summary>
        /// Код эдо
        /// </summary>
        public virtual int CodeEdo { get; set; }
    }
}
