namespace Bars.Gkh.Regions.Tatarstan.Enums
{
    using Bars.B4.Utils;

    public enum ConditionElementOutdoor
    {
        [Display("Хорошее")]
        Good = 10,

        [Display("Удовлетворительное")]
        Satisfactory = 20,

        [Display("Плохое(требует ремонта)")]
        Bad = 30
    }
}