namespace Bars.Gkh.Regions.Tatarstan.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Причина прекращения исполнительного производства
    /// </summary>
    public enum TerminationReasonType
    {
        [Display("Не задано")]
        NotSet = 0,

        [Display("Фактическое исполнение")]
        FactPerformed = 10,

        [Display("Добровольное исполнение")]
        VoluntarilyPerformed = 20, 

        [Display("Невозможность взыскания")]
        Impossible = 30,

        [Display("Отсутствие должника")]
        DebtorNotAvailable = 40,

        [Display("Иная причина")]
        OtherReason = 100
    }
}