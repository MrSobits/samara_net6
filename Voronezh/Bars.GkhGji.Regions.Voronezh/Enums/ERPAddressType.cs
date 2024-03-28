namespace Bars.GkhGji.Regions.Voronezh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип адреса проверяемого лица
    /// </summary>
    public enum ERPAddressType
    {

        /// <summary>
        /// Место нахождения юр лица
        /// </summary>
        [Display("Место нахождения юр. лица")]
        TYPE_I = 10,

        /// <summary>
        /// Место фактического осуществления деятельности
        /// </summary>
        [Display("Место фактического осуществления деятельности")]
        TYPE_II = 20,

        /// <summary>
        /// Место нахождения опасных производственных объектов
        /// </summary>
        [Display("Место нахождения опасных производственных объектов")]
        TYPE_III = 30,

        /// <summary>
        /// Иное
        /// </summary>
        [Display("Иное")]
        TYPE_OTHER = 40

    }
}