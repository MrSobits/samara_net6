namespace Bars.GkhGji.Regions.Habarovsk.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Вид КНД
    /// </summary>
    public enum KindKND
    {
        /// <summary>
        /// Не указан
        /// </summary>
        [Display("Не указан")]
        NotSet = 0,

        /// <summary>
        /// Жилищный надзор
        /// </summary>
        [Display("Жилищный надзор")]
        HousingSupervision = 10,

        /// <summary>
        /// Лицензионный контроль
        /// </summary>
        [Display("Лицензионный контроль")]
        LicenseControl = 20,

        /// <summary>
        /// Лицензионный контроль
        /// </summary>
        [Display("Контроль алкоголя")]
        AlcoholControl = 30,

        /// <summary>
        /// Лицензионный контроль
        /// </summary>
        [Display("Экологический надзор")]
        EcologicControl = 40,

        /// <summary>
        /// Лицензионный контроль
        /// </summary>
        [Display("Геологический надзор")]
        GeologicControl = 50,

        /// <summary>
        /// Смешаный надзор за деятельностью регионального оператора
        /// </summary>
        [Display("Надзор за деятельностью регионального оператора")]
        FRO = 60,

    }
}