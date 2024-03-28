namespace Bars.GkhEdoInteg.Entities
{
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Сопоставление вида обращения с кодом Эдо
    /// </summary>
    public class KindStatementCompareEdo : BaseGkhEntity
    {
        /// <summary>
        /// Вид обращения
        /// </summary>
        public virtual KindStatementGji KindStatement { get; set; }

        /// <summary>
        /// Код эдо
        /// </summary>
        public virtual int CodeEdo { get; set; }
    }
}
