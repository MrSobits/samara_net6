namespace Bars.GkhGji.Regions.BaseChelyabinsk.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип  запроса ЕРУЛ
    /// </summary>
    public enum ERULRequestType
    {

        /// <summary>
        /// Запрос номера лицензии
        /// </summary>
        [Display("Запрос номера лицензии")]
        GetLicNumber = 10,

        /// <summary>
        /// Внесении изменений в лицензию
        /// </summary>
        [Display("Внесении изменений в лицензию")]
        Changes = 20,

        /// <summary>
        /// Направление дополнительной информации
        /// </summary>
        [Display("Направление дополнительной информации")]
        AdditionalInfo = 30,

        /// <summary>
        /// Прекращение действия лицензии
        /// </summary>
        [Display("Прекращение действия лицензии")]
        Ternmination = 40,
    }
}