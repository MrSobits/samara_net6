namespace Bars.B4.Modules.FIAS
{
    using Bars.B4.Utils;

    /// <summary>
    /// Статус центра
    /// </summary>
    public enum FiasCenterStatusEnum : byte
    {
        [Display("Не является центром")]
        NotCenter = 0,

        [Display("Является центром района")]
        CenterRain = 1,

        [Display("Является центром региона")]
        CenterRegion = 2,

        [Display("Является и центром региона и центром района")]
        CenterRegionRaion = 3
    }
}