using Bars.B4.Utils;

namespace Bars.Gkh.Modules.ClaimWork.Enums
{
    /// <summary>
    /// Результат рассмотрения
    /// </summary>
    public enum LawsuitResultConsideration
    {
        /// <summary>
        /// Отказано
        /// </summary>
        [Display("Отказано")]
        Denied = 0,

        /// <summary>
        /// Удовлетворено
        /// </summary>
        [Display("Удовлетворено")]
        Satisfied = 10,

        /// <summary>
        /// Частично удовлетворено
        /// </summary>
        [Display("Частично удовлетворено")]
        PartiallySatisfied = 20,

        /// <summary>
        /// Не задано
        /// </summary>
        [Display("Не задано")]
        NotSet = 30,

        /// <summary>
        /// Вынесен судебный приказ
        /// </summary>
        [Display("Вынесен судебный приказ")]
        CourtOrderIssued = 40,
        
        /// <summary>
        /// Вынесен судебный приказ
        /// </summary>
        [Display("Вынесено решение")]
        DecisionIssued = 45,


        /// <summary>
        /// Производство прекращено
        /// </summary>
        [Display("Производство прекращено")]
        ProductionDiscontinued = 50,


        /// <summary>
        /// Направлено по подсудности
        /// </summary>
        [Display("Направлено по подсудности")]
        SentByJurisdiction = 60,

        /// <summary>
        /// Оставлено без рассмотрения
        /// </summary>
        [Display("Оставлено без рассмотрения")]
        LeftUunaddressed = 70,
        
        /// <summary>
        /// Отказано в приеме
        /// </summary>
        [Display("Отказано в приеме")]
        DeniedRecive = 80
        
    }
}
