namespace Bars.Gkh.ConfigSections.Overhaul.Enums
{
    using B4.Utils;

    /// <summary>
    /// Определение расценки по работам при расчете ДПКР (с учетом группы капитальности или без учета)
    /// </summary>
    public enum WorkPriceDetermineType
    {
        [Display("Без учета типа дома и группы капитальности")]
        WithoutCapitalGroup = 0,

        [Display("С учетом группы капитальности")]
        WithCapitalGroup = 10,

        [Display("С учетом типа дома")]
        WithRealEstType = 20
    }
}