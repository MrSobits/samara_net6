namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Код раздела
    /// </summary>
    public enum TypeMunicipality
    {
        /// <summary>
        /// Сельское поселение
        /// </summary>
        [Display("Сельское поселение")]
        Settlement = 10,

        /// <summary>
        /// Городское поселение
        /// </summary>
        [Display("Городское поселение")]
        UrbanSettlement = 20,

        /// <summary>
        /// Муниципальный район
        /// </summary>
        [Display("Муниципальный район")] 
        MunicipalArea = 30,

        /// <summary>
        /// Городской округ
        /// </summary>
        [Display("Городской округ")] 
        UrbanArea = 40,

        /// <summary>
        /// Внутригородская территория города федерального значения
        /// </summary>
        [Display("Внутригородская территория города федерального значения")] 
        InterCityArea = 50
    }
}