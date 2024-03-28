namespace Bars.GkhGji.Regions.Tatarstan.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип отправки начисления
    /// </summary>
    public enum ChargeStatus
    {
        /// <summary>
        /// Отправка начисления
        /// </summary>
        [Display ("Отправка начисления")]
        Send = 1,

        /// <summary>
        /// Изменение начисления
        /// </summary>
        [Display("Изменение начисления")]
        Change = 2,

        /// <summary>
        /// Аннулирование начисления
        /// </summary>
        [Display("Аннулирование начисления")]
        Annul = 3
    }
}