namespace Bars.Gkh.Entities
{
    using Bars.Gkh.Enums;

    public class WorkKindCurrentRepair : BaseGkhEntity
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
        /// Единица измерения
        /// </summary>
        public virtual UnitMeasure UnitMeasure { get; set; }

        /// <summary>
        /// Тип работы
        /// </summary>
        public virtual TypeWork TypeWork { get; set; }
    }
}