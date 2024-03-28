namespace Bars.GkhCr.Entities
{ 
    using Gkh.Entities;

    /// <summary>
    /// Архив значений в мониторинге СМР
    /// </summary>
    public class ArchiveMultiplyContragentSmr : BaseGkhEntity
    {
        /// <summary>
        /// Вид работы
        /// </summary>
        public virtual TypeWorkCr TypeWorkCr { get; set; }

        /// <summary>
        /// Объем выполнения
        /// </summary>
        public virtual decimal VolumeOfCompletion { get; set; }

        /// <summary>
        /// Контрагент
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        /// Процент выполнения
        /// </summary>
        public virtual decimal PercentOfCompletion { get; set; }

        /// <summary>
        /// Сумма расходов
        /// </summary>
        public virtual decimal CostSum { get; set; }
    }
}
