namespace Bars.GkhGji.Regions.BaseChelyabinsk.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип КНМ
    /// </summary>
    public enum IdentityDocumentType
    {

        /// <summary>
        /// ностранный паспор
        /// </summary>
        [Display("Иностранный паспорт")]
        foreignPassport = 0,

        /// <summary>
        /// Загранпаспорт РФ
        /// </summary>
        [Display("Загранпаспорт РФ")]
        internationalPassportRf = 1,

        /// <summary>
        /// Паспорт РФ
        /// </summary>
        [Display("Паспорт РФ")]
        passportRf = 2
    }
}