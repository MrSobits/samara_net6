namespace Bars.Gkh.Enums
{
    using B4.Utils;

    /// <summary>
    /// Объем ремонта
    /// </summary>
    public enum VolumeRepair
    {
        [Display("Не задано")]
        NotSet = 10,

        [Display("Частично")]
        Partially = 20,

        [Display("Полностью")]
        Fully = 30
    }
}