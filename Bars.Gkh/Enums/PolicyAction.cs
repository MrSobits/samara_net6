namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Действие страхового полиса
    /// </summary>
    public enum PolicyAction
    {
        [Display("Действует")]
        Active = 10,

        [Display("Приостановлен")]
        Paused = 20,

        [Display("Расторгнут")]
        Terminated = 30
    }
}
