namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Состояние программы переселения
    /// </summary>
    public enum StateResettlementProgram
    {
        [Display("Новая")]
        New = 10,

        [Display("Открыта")]
        Open = 20,

        [Display("Активна")]
        Active = 30,

        [Display("Закрыта")]
        Closed = 40,

        [Display("Закрыта на корректировку")]
        ClosedForCorrections = 50,

        [Display("Завершена КР")]
        FinishedKR = 60
    }
}
