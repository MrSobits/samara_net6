namespace Bars.GisIntegration.Inspection.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Вид осуществления контрольной деятельности (НСИ 65)
    /// </summary>
    public enum OversightActivityType
    {
        /// <summary>
        /// Основание проверки = Проверки соискателей лицензии, то передаем CODE = 2 и GUID = 2a9cb62a-ab5e-4f59-bdae-af843bc313ce
        /// </summary>
        [Display("Лицензионный контроль")]
        LicenseControl = 10,

        /// <summary>
        /// Иначе CODE = 1 и GUID = cb8a08ad-44a7-41a3-b43c-3253e1a198da
        /// </summary>
        [Display("Государственный и муниципальный жилищный надзор (контроль)")]
        StateAndMunicipalControl = 20
    }
}

