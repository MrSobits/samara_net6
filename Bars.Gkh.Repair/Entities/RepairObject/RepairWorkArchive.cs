namespace Bars.Gkh.Repair.Entities
{
    using System;

    using Bars.B4.DataAccess;

    /// <summary>
    /// Архив значений видов работы текущего ремонта
    /// </summary>
    public class RepairWorkArchive : BaseEntity
    {
        /// <summary>
        /// Вид работы
        /// </summary>
        public virtual RepairWork RepairWork { get; set; }

        /// <summary>
        /// Объем выполнения
        /// </summary>
        public virtual decimal? VolumeOfCompletion { get; set; }

        /// <summary>
        /// Процент выполнения
        /// </summary>
        public virtual decimal? PercentOfCompletion { get; set; }

        /// <summary>
        /// Сумма расходов
        /// </summary>
        public virtual decimal? CostSum { get; set; }

        /// <summary>
        /// Дата изменения записи
        /// </summary>
        public virtual DateTime? DateChangeRec { get; set; }
    }
}
