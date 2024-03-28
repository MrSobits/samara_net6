namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип решения об отмене
    /// </summary>
    public enum TypePrescriptionCancel
    {
        /// <summary>
        /// Отменено полностью.
        /// </summary>
        [Display("Отменено полностью")]
        CompletelyCancel = 10,

        /// <summary>
        /// Отменено частично.
        /// </summary>
        [Display("Отменено частично")]
        PartiallyCancel = 20,

        /// <summary>
        /// Оставлено в силе.
        /// </summary>
        [Display("Оставлено в силе")]
        Upheld = 30,

        /// <summary>
        /// Отказ в продлении срока исполнения предписания.
        /// </summary>
        [Display("Отказ в продлении срока исполнения предписания")]
        RefusExtend = 40,

        /// <summary>
        /// Продление срока исполнения предписания.
        /// </summary>
        [Display("Продление срока исполнения предписания")]
        Prolongation = 50
    }
}
