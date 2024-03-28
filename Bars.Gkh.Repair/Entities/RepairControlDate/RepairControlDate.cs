namespace Bars.Gkh.Repair.Entities.RepairControlDate
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Контрольные сроки работ по текущему ремонту
    /// </summary>
    public class RepairControlDate : BaseEntity
    {
        /// <summary>
        /// Программа текущего ремонта
        /// </summary>
        public virtual RepairProgram RepairProgram { get; set; }

        /// <summary>
        /// Вид работы
        /// </summary>
        public virtual WorkKindCurrentRepair Work { get; set; }

        /// <summary>
        /// Контрольный срок
        /// </summary>
        public virtual DateTime? Date { get; set; }
    }
}
