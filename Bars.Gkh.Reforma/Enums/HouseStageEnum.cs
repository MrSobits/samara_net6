namespace Bars.Gkh.Reforma.Enums
{
    using Bars.B4.Utils;

    public enum HouseStageEnum {
        /// <summary>
        /// Эксплуатируемый
        /// </summary>
        [Display("Эксплуатируемый")]
        exploited = 1,

        /// <summary>
        /// Выведенный из эксплуатации
        /// </summary>
        [Display("Выведенный из эксплуатации")]
        decommissioned = 2,

        /// <summary>
        /// Снесенный
        /// </summary>
        [Display("Снесенный")]
        drifting = 3
    }
}