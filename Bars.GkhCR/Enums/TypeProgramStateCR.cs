namespace Bars.GkhCr.Enums
{
    using B4.Utils;

    /// <summary>
    /// Тип состояния программы капитального ремонта
    /// </summary>
    public enum TypeProgramStateCr
    {
        [Display("Новая")]
        New = 10,

        [Display("Открыта")]
        Open = 20,

        [Display("Активна")]
        Active = 30,

        [Display("Закрыта")]
        Close = 40,

        [Display("Закрыта на корректировку")]
        CloseOnCorrect = 50,

        [Display("Завершена КР")]
        Complete = 60,
    }
}
