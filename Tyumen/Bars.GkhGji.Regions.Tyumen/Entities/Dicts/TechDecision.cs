namespace Bars.GkhGji.Regions.Tyumen.Entities.Dicts
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Техническое решение
    /// </summary>
    public class TechDecision : BaseEntity
    {
        /// <summary>
        /// Наименование.
        /// </summary>
        public virtual string Name { get; set; }
    }
}
