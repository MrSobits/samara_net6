namespace Bars.GkhGji.Regions.BaseChelyabinsk.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Статус запроса
    /// </summary>
    public enum LicenseActionType
    {
        /// <summary>
        /// Получение сведений о лицензии
        /// </summary>
        [Display("Получение сведений о конкретной лицензии")]
        GetLicenseInfo = 10,

        /// <summary>
        /// Получение сведений об аннулировании лицензии
        /// </summary>
        [Display("Прекращение действия лицензии")]
        GetLicenseCancelInfo = 20,

    }
}