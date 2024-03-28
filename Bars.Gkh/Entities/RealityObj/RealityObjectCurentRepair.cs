namespace Bars.Gkh.Entities
{
    using System;

    /// <summary>
    /// Текущий ремонт жилого дома
    /// </summary>
    public class RealityObjectCurentRepair : BaseGkhEntity
    {
        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// ед. измерения
        /// </summary>
        public virtual string UnitMeasure { get; set; }

        /// <summary>
        /// Вид работы
        /// </summary>
        public virtual WorkKindCurrentRepair WorkKind { get; set; }

        /// <summary>
        /// План дата
        /// </summary>
        public virtual DateTime? PlanDate { get; set; }

        /// <summary>
        /// План на сумму
        /// </summary>
        public virtual decimal? PlanSum { get; set; }

        /// <summary>
        /// План объем работ
        /// </summary>
        public virtual decimal? PlanWork { get; set; }

        /// <summary>
        /// Факт дата
        /// </summary>
        public virtual DateTime? FactDate { get; set; }

        /// <summary>
        /// факт на сумму
        /// </summary>
        public virtual decimal? FactSum { get; set; }

        /// <summary>
        /// факт объем работ
        /// </summary>
        public virtual decimal? FactWork { get; set; }

        /// <summary>
        /// Подрядчик
        /// </summary>
        public virtual Builder Builder { get; set; }
    }
}
