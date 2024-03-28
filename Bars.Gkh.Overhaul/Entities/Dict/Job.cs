namespace Bars.Gkh.Overhaul.Entities
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Dicts;

    /// <summary>
    /// Работа
    /// </summary>
    public class Job : BaseImportableEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Вид работ
        /// </summary>
        public virtual Work Work { get; set; }

        /// <summary>
        /// Ед. измерения
        /// </summary>
        public virtual UnitMeasure UnitMeasure { get; set; }
    }
}
