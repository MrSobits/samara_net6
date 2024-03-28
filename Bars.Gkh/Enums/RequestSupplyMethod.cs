namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Способ подачи заявления
    /// </summary>
    public enum RequestSupplyMethod
    {
        /// <summary>
        /// Не задано
        /// </summary>
        [Display("Не задано")]
        NotSet = 0,

        /// <summary>
        /// Личный кабинет
        /// </summary>
        [Display("Личный кабинет")]
        PrivateOffice = 1,

        /// <summary>
        /// Ресепшн
        /// </summary>
        [Display("Ресепшн")]
        Receptionist = 2,

        /// <summary>
        /// В электронном виде
        /// </summary>
        [Display("В электронном виде")]
        ElectronicForm = 3,

        /// <summary>
        /// Подписанное ЭЦП
        /// </summary>
        [Display("Подписанное ЭЦП")]
        DigitalSignature = 4
    }
}
