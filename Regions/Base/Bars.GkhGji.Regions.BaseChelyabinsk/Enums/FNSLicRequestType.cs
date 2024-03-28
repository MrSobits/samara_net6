namespace Bars.GkhGji.Regions.BaseChelyabinsk.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Статус запроса
    /// </summary>
    public enum FNSLicRequestType
    {

        /// <summary>
        /// Добавление
        /// </summary>
        [Display("Добавить в реестр ФНС")]
        Add = 10,

        /// <summary>
        /// Удаление
        /// </summary>
        [Display("Исключить из реестра ФНС")]
        Delete = 20,
    }
}