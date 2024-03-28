namespace Bars.GisIntegration.Base.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип выгружаемых показаний ПУ
    /// </summary>
    public enum MeteringDeviceValueType
    {
        /// <summary>
        /// Контрольное показание ПУ
        /// </summary>
        [Display("Контрольное показание ПУ")]
        ControlValue = 10,

        /// <summary>
        /// Текущие показания ПУ
        /// </summary>
        [Display("Текущие показания ПУ")]
        CurrentValue = 20,

        /// <summary>
        /// Показание поверки ПУ
        /// </summary>
        [Display("Показание поверки ПУ")]
        VerificationValue = 30
    }
}