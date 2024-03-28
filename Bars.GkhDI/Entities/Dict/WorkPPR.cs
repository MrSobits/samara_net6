namespace Bars.GkhDi.Entities
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Работы по ППР
    /// </summary>
    public class WorkPpr : BaseImportableEntity
    {
        /// <summary>
        /// Группа работ по ППР
        /// </summary>
        public virtual GroupWorkPpr GroupWorkPpr { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Единица измерения
        /// </summary>
        public virtual UnitMeasure Measure { get; set; }
    }
}
