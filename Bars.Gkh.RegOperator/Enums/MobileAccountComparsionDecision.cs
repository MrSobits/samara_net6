namespace Bars.Gkh.RegOperator.Enums
{
    using Bars.B4.Utils;

    public enum MobileAccountComparsionDecision
    {
        [Display("Не задано")]
        NotSet = 1,

        [Display("ФИО в систме скорректировано")]
        FioAdjusted = 2,

        [Display("ФИО в системе коректно")]
        FioCorrect = 3,

        [Display("Оставленно без рассмотрения")]
        NotViewed = 4
    }
}
