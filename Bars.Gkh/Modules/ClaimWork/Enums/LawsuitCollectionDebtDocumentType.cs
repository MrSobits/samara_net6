namespace Bars.Gkh.Modules.ClaimWork.Enums
{
    using System;

    using Bars.B4.Utils;

    /// <summary>
    /// Тип документа взыскания задолженности
    /// </summary>
    public enum LawsuitCollectionDebtDocumentType
    {
        /// <summary>
        /// Не задано
        /// </summary>
        [Display("Не задано")]
        NotSet = 0,

        /// <summary>
        /// Документ реструктуризации
        /// </summary>
        [Display("Документ реструктуризации")]
        [Obsolete]
        DocumentRestructuring = 10,

        /// <summary>
        /// Исполнительный лист
        /// </summary>
        [Display("Исполнительный лист")]
        WritOfExecution = 20,

        /// <summary>
        /// Судебный приказ
        /// </summary>
        [Display("Судебный приказ")]
        CourtOrder = 30
    }
}