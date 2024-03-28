namespace Bars.Gkh.ClaimWork.Entities
{
    using B4.DataAccess;

    /// <summary>
    /// Детализация графика реструктуризации
    /// </summary>
    public class RestructDebtScheduleDetail : PersistentObject
    {
        /// <summary>
        /// Запись графика реструктуризации
        /// </summary>
        public virtual RestructDebtSchedule ScheduleRecord { get; set; }

        /// <summary>
        /// Идентификатор трансфера
        /// </summary>
        public virtual long TransferId { get; set; }

        /// <summary>
        /// Сумма оплаты трансфера
        /// </summary>
        public virtual decimal Sum { get; set; }
    }
}