using Bars.B4.Utils;

namespace Bars.Gkh.Modules.ClaimWork.Enums
{
    /// <summary>
    /// Тип документа искового заявления
    /// </summary>
    public enum LawsuitDocumentType
    {
        /// <summary>
        /// Не задано
        /// </summary>
        [Display("Не задано")]
        NotSet = 0,

        /// <summary>
        /// Приказ
        /// </summary>
        [Display("Судебный приказ")]
        Disposal = 10,

        /// <summary>
        /// Решение суда
        /// </summary>
        [Display("Решение суда")]
        ResolutionCourt = 20,

        /// <summary>
        /// Мировое соглашение
        /// </summary>
        [Display("Мировое соглашение")]
        AmicableAgreement = 30,

        /// <summary>
        /// Заочное решение
        /// </summary>
        [Display("Заочное решение")]
        CorrespondenceDecisiont = 40,

        /// <summary>
        /// Определение
        /// </summary>
        [Display("Определение")]
        determination = 50,

        /// <summary>
        /// Постановление
        /// </summary>
        [Display("Постановление")]
        Decree = 60

    }
}
