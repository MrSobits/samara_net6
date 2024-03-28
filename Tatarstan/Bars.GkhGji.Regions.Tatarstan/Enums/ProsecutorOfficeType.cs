namespace Bars.GkhGji.Regions.Tatarstan.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип прокуратуры
    /// </summary>
    public enum ProsecutorOfficeType
    {
        /// <summary>
        /// Генеральная прокуратура РФ
        /// </summary>
        [Display ("Генеральная прокуратура РФ")]
        General = 1,

        /// <summary>
        /// Прокуратура уровня Федерального округа РФ
        /// </summary>
        [Display("Прокуратура уровня Федерального округа РФ")]
        FederalDistrict = 2,
            
        /// <summary>
        /// Прокуратура уровня Федерального центра РФ
        /// </summary>
        [Display("Прокуратура уровня Федерального центра РФ")]
        FederalCenter = 3,
        
        /// <summary>
        /// Прокуратура уровня района
        /// </summary>
        [Display("Прокуратура уровня района")]
        District = 4
    }
}