namespace Bars.Gkh.ConfigSections.Overhaul.Enums
{
    using B4.Utils;

    public enum RateCalcTypeArea
    {
        [Display("Жилая площадь")]
        AreaLiving = 0,
        
        [Display("Общая площадь жилых и нежилых помещений")]
        AreaLivingNotLiving = 10,

        [Display("Общая площадь МКД")]
        AreaMkd = 20
    }
}