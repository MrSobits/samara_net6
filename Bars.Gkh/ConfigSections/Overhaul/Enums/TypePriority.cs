namespace Bars.Gkh.ConfigSections.Overhaul.Enums
{
    using B4.Utils;

    /// <summary>
    /// Тип применяемой очередности
    /// </summary>
    public enum TypePriority
    {
        [Display("Расчет по критериям")]
        Criteria = 10,

        [Display("Расчет по баллам")]
        Points = 20,

        [Display("Расчет по критериям и баллам")]
        CriteriaAndPoins = 30
    }
}