namespace Bars.Gkh.Overhaul.Enum
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип источника финансирования
    /// </summary>
    public enum TypeFinSource
    {
        [Display("Бюджет фонда")]
        Fund = 10,

        [Display("Бюджет региона")]
        Region = 20,

        [Display("Бюджет МО")]
        Municipality = 30,

        [Display("Средства собственника")]
        Owners = 40,

        [Display("Иные источники")]
        Other = 50
    }
}