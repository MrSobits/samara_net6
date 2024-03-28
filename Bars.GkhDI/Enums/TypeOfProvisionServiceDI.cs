namespace Bars.GkhDi.Enums
{
    using B4.Utils;

    /// <summary>
    /// Тип оказания услуг
    /// </summary>
    public enum TypeOfProvisionServiceDi
    {
        /// <summary>
        /// Услуга предоставляется через УО
        /// </summary>
        [Display("Услуга предоставляется через УО")]
        ServiceProvidedMo = 10,

        /// <summary>
        /// Услуга предоставляется без участия УО
        /// </summary>
        [Display("Услуга предоставляется без участия УО")]
        ServiceProvidedWithoutMo  = 20,

        /// <summary>
        /// Услуга не предоставляется
        /// </summary>
        [Display("Услуга не предоставляется")]
        ServiceNotAvailable  = 30,

        /// <summary>
        /// Собственники отказались от предоставления услуги
        /// </summary>
        [Display("Собственники отказались от предоставления услуги")]
        OwnersRefused = 40
    }
}
