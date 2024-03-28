namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Да/Нет/Не применимо
    /// </summary>
    public enum YesNoNotApplicable
    {
        [Display("Ответ отсутствует")]
        NotSet = 0,

        [Display("Да")]
        Yes = 10,

        [Display("Нет")]
        No = 20,

        [Display("Не применимо")]
        NotApplicable = 30
    }
}
