namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Статус ознакомления с результатами проверки
    /// </summary>
    public enum AcquaintState
    {
        /// <summary>
        /// Не задано
        /// </summary>
        [Display("Не задано")]
        NotSet = 0,

        /// <summary>
        /// Не ознакомлен
        /// </summary>
        [Display("Не ознакомлен")]
        NotAcquainted = 10,

        /// <summary>
        /// Отказ от ознакомления
        /// </summary>
        [Display("Отказ от ознакомления")]
        RefuseToAcquaint = 20,

        /// <summary>
        /// Ознакомлен
        /// </summary>
        [Display("Ознакомлен")]
        Acquainted = 30
    }
}