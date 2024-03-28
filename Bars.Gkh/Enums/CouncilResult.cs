namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Результат собрания
    /// </summary>
    public enum CouncilResult
    {
        [Display("Не задано")]
        NotSet = 10,

        [Display("Очное собрание созвано - кворум отсутствовал")]
        NoQuorum = 20,

        [Display("Очное собрание созвано - кворум имеется - совет МКД не выбран")]
        CouncilNotCreated = 30,

        [Display("Создан совет мкд")]
        CouncilCreated = 40,

        [Display("Заочное собрание созвано - кворум имеется - совет МКД не выбран")]
        AbsencteeMeetingCouncilNotCreated = 50,
    }
}

