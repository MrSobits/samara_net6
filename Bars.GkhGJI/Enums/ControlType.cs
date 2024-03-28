namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Вид контроля
    /// </summary>
    public enum ControlType
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
        LicensedControl = 20,

        /// <summary>
        /// Не задано
        /// </summary>
        [Display("Смешанный")]
        Both = 30,
    }
}