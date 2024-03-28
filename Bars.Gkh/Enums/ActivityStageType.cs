namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип стадии деятельности
    /// </summary>
    public enum ActivityStageType
    {
        /// <summary>
        /// Действует
        /// </summary>
        [Display("Действует")]
        Active = 10,

        /// <summary>
        /// Не предоставляет услуги управления
        /// </summary>
        [Display("Не предоставляет услуги управления")]
        NotManagementService = 20,

        /// <summary>
        /// Банкрот
        /// </summary>
        [Display("Банкротство")]
        Bankrupt = 30,

        /// <summary>
        /// Ликвидирован
        /// </summary>
        [Display("Ликвидация")]
        Liquidated = 40,

        /// <summary>
        /// Реорганизация
        /// </summary>
        [Display("Реорганизация")]
        Reorganized = 50,

        /// <summary>
        /// Платежеспособен
        /// </summary>
        [Display("Платежеспособен")]
        Affordable = 60
    }
}