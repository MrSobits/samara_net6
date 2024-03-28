namespace Bars.Gkh.RegOperator.Enums
{
    using B4.Utils;

    /// <summary>
    /// Состояние распределения
    /// </summary>
    public enum DistributionState
    {
        /// <summary>
        /// Распределена
        /// </summary>
        [Display("Распределена")]
        Distributed = 10,

        /// <summary>
        /// Не распределена
        /// </summary>
        [Display("Не распределена")]
        NotDistributed = 20,

        /// <summary>
        /// Отменен
        /// </summary>
        [Display("Отменен")]
        Canceled = 30,

        /// <summary>
        /// Удалена
        /// </summary>
        [Display("Удалена")]
        Deleted = 40,

        /// <summary>
        /// Частично распределена
        /// </summary>
        [Display("Частично распределена")]
        PartiallyDistributed = 50,

        /// <summary>
        /// Ожидание распределения
        /// </summary>
        [Display("Ожидание распределения")]
        WaitingDistribution = 60,

        /// <summary>
        /// Ожидание отмены распределения
        /// </summary>
        [Display("Ожидание отмены распределения")]
        WaitingCancellation = 70,
    }
}