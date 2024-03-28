using Bars.B4.Utils;

namespace Bars.Gkh.Modules.ClaimWork.Enums
{
    /// <summary>
    /// Причина остановки взыскания
    /// </summary>
    public enum LawsuitCollectionDebtReasonStoppedType
    {
        [Display("Не задано")]
        NotSet = 0,

        [Display("Фактическое исполнение")]
        FactPerformed = 10,

        [Display("Невозможность взыскание")]
        Impossible = 20,

        [Display("Отсутствие должника")]
        DebtorNotAvailable = 30,
        
        [Display("Банкротство должника")]
        DebtorBankrot = 40,
        
        [Display("Утрата дееспособности должника")]
        DebtorNotСapacity = 50,
        
        [Display("Смерть должника")]
        DebtorDie = 60,
        
        [Display("Отмена судебного акта")]
        DecisionCanel = 70,
        
        [Display("Отказ")]
        Failture= 80,

        [Display("Иная причина")]
        OtherReason = 100
    }
}
