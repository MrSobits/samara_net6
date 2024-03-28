namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Результат рассмотрения заявки о внесении изменений в реестр лицензий
    /// </summary>
    public enum LicStatementResult
    {
        /// <summary>
        /// Не задано
        /// </summary>
        [Display("Не задано")]
        NotSet = 0,

        /// <summary>
        /// Удовлетворено
        /// </summary>
        [Display("Удовлетворено")]
        Allowed = 10,

        /// <summary>
        /// Удовлетворено в части
        /// </summary>
        [Display("Удовлетворено в части")]
        PartiallyAllowed = 20,

        /// <summary>
        /// Отказано
        /// </summary>
        [Display("Отказано")]
        Denied = 30,

        /// <summary>
        /// Рассмотрение прекращено
        /// </summary>
        [Display("Рассмотрение прекращено")]
        Stopped = 40
    }
}