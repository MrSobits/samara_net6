namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип документа квалификационного аттестата
    /// </summary>
    public enum QualificationDocumentType
    {
        /// <summary>
        /// Дубликат
        /// </summary>
        [Display("Дубликат")]
        Duplicate = 10,

        /// <summary>
        /// Переоформление
        /// </summary>
        [Display("Переоформление")]
        Renew = 20
    }
}