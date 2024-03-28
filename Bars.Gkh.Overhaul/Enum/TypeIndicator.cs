namespace Bars.Gkh.Overhaul.Enum
{
    using Bars.B4.Utils;

    public enum TypeIndicator
    {
        [Display("Не задано")]
        NotSet = 10,

        [Display("Минимальный размер взноса на кап.ремонт на кв.м жилой площади (руб.)")]
        MinSizeSqMetLivinSpace = 20,

        [Display("Минимальный размер фонда на кап.ремонт (% от стоимости кап.ремонта)")]
        MinSizePercentOfCostRestoration = 30,
    }
}