namespace Bars.Gkh.Regions.Tatarstan.Entities.Dicts
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Regions.Tatarstan.Enums;

    /// <summary>
    /// Элемент двора. Справочник
    /// </summary>
    public class ElementOutdoor : BaseEntity
    {
        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Группа
        /// </summary>
        public virtual ElementOutdoorGroup? ElementGroup { get; set; }

        /// <summary>
        /// Единица измерения
        /// </summary>
        public virtual UnitMeasure UnitMeasure { get; set; }
    }
}