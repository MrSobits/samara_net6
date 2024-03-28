namespace Bars.Gkh.Modules.GkhDi.Entities
{
    using Bars.Gkh.Entities;
    using Bars.GkhDi.Enums;

    /// <summary>
    /// Шаблонная услуга для "прочих услуг"
    /// </summary>
    public class TemplateOtherService : BaseGkhEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Характеристика
        /// </summary>
        public virtual string Characteristic { get; set; }

        /// <summary>
        /// Единица измерения
        /// </summary>
        public virtual UnitMeasure UnitMeasure { get; set; }
    }
}
