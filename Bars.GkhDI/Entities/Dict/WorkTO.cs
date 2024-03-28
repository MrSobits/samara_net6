namespace Bars.GkhDi.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Работы по ТО
    /// </summary>
    public class WorkTo : BaseImportableEntity
    {
        /// <summary>
        /// Группа работ по ТО
        /// </summary>
        public virtual GroupWorkTo GroupWorkTo { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Работа не актуальна
        /// </summary>
        public virtual bool IsNotActual { get; set; }
    }
}
