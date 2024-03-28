namespace Bars.Gkh.Enums
{
    using B4.Utils;

    /// <summary>
    /// Стоимость 
    /// </summary>
    public enum PriceCalculateBy
    {
        [Display("По объему")]
        Volume = 0,

        [Display("Жилая площадь")]
        LivingArea = 10,

        [Display("Общая площадь")]
        TotalArea = 20,

        [Display("Общая площадь жилых и нежилых помещений в МКД")]
        AreaLivingNotLivingMkd = 30
    }
}