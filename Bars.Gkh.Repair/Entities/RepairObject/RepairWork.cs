namespace Bars.Gkh.Repair.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Работа объекта текущего ремонта
    /// </summary>
    public class RepairWork : BaseEntity
    {
        /// <summary>
        /// Объект текущего ремонта
        /// </summary>
        public virtual RepairObject RepairObject { get; set; }

        /// <summary>
        /// Вид работы
        /// </summary>
        public virtual WorkKindCurrentRepair Work { get; set; }

        /// <summary>
        /// Объем (плановый)
        /// </summary>
        public virtual decimal? Volume { get; set; }

        /// <summary>
        /// Сумма (плановая)
        /// </summary>
        public virtual decimal? Sum { get; set; }

        /// <summary>
        /// Дата начала работ
        /// </summary>
        public virtual DateTime? DateStart { get; set; }

        /// <summary>
        /// Дата окончания работ
        /// </summary>
        public virtual DateTime? DateEnd { get; set; }

        /// <summary>
        /// Объем (фактический)
        /// </summary>
        public virtual decimal? VolumeOfCompletion { get; set; }

        /// <summary>
        /// Процент выполнения
        /// </summary>
        public virtual decimal? PercentOfCompletion { get; set; }

        /// <summary>
        /// Сумма (фактическая)
        /// </summary>
        public virtual decimal? CostSum { get; set; }

        /// <summary>
        /// Подрядчик
        /// </summary>
        public virtual string Builder { get; set; }

        /// <summary>
        /// Доп. срок
        /// </summary>
        public virtual DateTime? AdditionalDate { get; set; }
    }
}