namespace Bars.GkhEdoInteg.Entities
{
    using Gkh.Entities;

    /// <summary>
    /// Сопоставление инспекторов с кодом Эдо
    /// </summary>
    public class InspectorCompareEdo : BaseGkhEntity
    {
        /// <summary>
        /// Инспектор
        /// </summary>
        public virtual Inspector Inspector { get; set; }

        /// <summary>
        /// Код эдо
        /// </summary>
        public virtual int CodeEdo { get; set; }
    }
}
