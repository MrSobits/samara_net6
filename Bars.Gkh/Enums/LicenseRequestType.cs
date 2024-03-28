namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип обращения
    /// </summary>
    public enum LicenseRequestType
    {
        /// <summary>
        /// Предоставление лицензии 
        /// </summary>
        [Display("Предоставление лицензии ")]
        GrantingLicense = 10,

        /// <summary>
        /// Переоформление лицензии 
        /// </summary>
        [Display("Переоформление лицензии ")]
        RenewalLicense = 20,

        /// <summary>
        /// Выдача дубликата лицензии 
        /// </summary>
        [Display("Выдача дубликата лицензии")]
        IssuingDuplicateLicense = 30,

        /// <summary>
        /// Прекращение деятельности
        /// </summary>
        [Display("Прекращение деятельности")]
        TerminationActivities = 40,

        /// <summary>
        /// Предоставление копии лицензии
        /// </summary>
        [Display("Предоставление копии лицензии")]
        ProvisionCopiesLicense = 50,

        /// <summary>
        /// Выписка из реестра лицензий
        /// </summary>
        [Display("Выписка из реестра лицензий")]
        ExtractFromRegisterLicense = 60
    }
}