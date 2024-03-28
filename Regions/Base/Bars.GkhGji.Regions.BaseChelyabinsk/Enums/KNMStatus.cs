namespace Bars.GkhGji.Regions.BaseChelyabinsk.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Статус КНМ
    /// </summary>
    public enum KNMStatus
    {

        /// <summary>
        /// Новая
        /// </summary>
        [Display("Новая")]
        New = 1,

        /// <summary>
        /// Ожидает согласовани
        /// </summary>
        [Display("Ожидает согласования")]
        AwaitAproove = 2,

        /// <summary>
        /// Требует доработки
        /// </summary>
        [Display("Требует доработки")]
        NeedCorrection = 3,

        /// <summary>
        /// Отклонена
        /// </summary>
        [Display("Отклонена")]
        Rejected = 4,

        /// <summary>
        /// Ожидает проведения
        /// </summary>
        [Display("Ожидает проведения")]
        WaitForProcess = 5,

        /// <summary>
        /// Ожидает завершения
        /// </summary>
        [Display("Ожидает завершения")]
        PendingCompletion = 6,

        /// <summary>
        /// Завершена
        /// </summary>
        [Display("Завершена")]
        Completed = 7,

        /// <summary>
        /// Не может быть проведена
        /// </summary>
        [Display("Не может быть проведена")]
        Impossible = 8,

        /// <summary>
        /// Обжалована
        /// </summary>
        [Display("Обжалована")]
        Appealed = 9,
    }
}