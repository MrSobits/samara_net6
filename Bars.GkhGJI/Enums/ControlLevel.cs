namespace Bars.GkhGji.Regions.Tatarstan.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Уровень контроля (надзора).
    /// </summary>
    public enum ControlLevel
    {
        [Display("Федеральный")]
        Federal = 10,

        [Display("Региональный")]
        Regional = 20,

        [Display("Муниципальный")]
        Municipal = 30
    }
}
