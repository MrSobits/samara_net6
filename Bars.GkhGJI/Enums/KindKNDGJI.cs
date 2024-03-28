namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Вид КНД
    /// </summary>
    public enum KindKNDGJI
    {
        /// <summary>
        /// Не задано
        /// </summary>
        [Display("Не задано")]
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
        /// Смешаный надзор за деятельностью регионального оператора
        /// </summary>
        [Display("Смешаный")]
        Both = 30,

        /// <summary>
        /// Смешаный надзор за деятельностью регионального оператора
        /// </summary>
        [Display("Надзор за деятельностью регионального оператора")]
        FRO = 40,


    }
}