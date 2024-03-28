namespace Bars.Gkh.Regions.Tatarstan.Entities.Dicts
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Regions.Tatarstan.Enums;

    /// <summary>
    /// Вид работы двора
    /// </summary>
    public class WorkRealityObjectOutdoor : BaseEntity
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
        /// Тип работы
        /// </summary>
        public virtual KindWorkOutdoor TypeWork { get; set; }

        /// <summary>
        /// Единица измерения
        /// </summary>
        public virtual UnitMeasure UnitMeasure { get; set; }

        /// <summary>
        /// Актуальность
        /// </summary>
        public virtual bool IsActual { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }
    }
}