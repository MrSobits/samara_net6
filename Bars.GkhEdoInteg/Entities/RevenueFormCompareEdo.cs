namespace Bars.GkhEdoInteg.Entities
{
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Сопоставление формы поступления с кодом Эдо
    /// </summary>
    public class RevenueFormCompareEdo : BaseGkhEntity
    {
        /// <summary>
        /// Форма поступления
        /// </summary>
        public virtual RevenueFormGji RevenueForm { get; set; }

        /// <summary>
        /// Код эдо
        /// </summary>
        public virtual int CodeEdo { get; set; }
    }
}
