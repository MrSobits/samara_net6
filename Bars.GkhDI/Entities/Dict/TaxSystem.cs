namespace Bars.GkhDi.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Система налогооблажения
    /// </summary>
    public class TaxSystem : BaseGkhEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Сокращенное наименование
        /// </summary>
        public virtual string ShortName { get; set; }
    }
}
