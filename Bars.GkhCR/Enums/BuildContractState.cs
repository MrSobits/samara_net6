namespace Bars.GkhCr.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Состояние договора
    /// </summary>
    public enum BuildContractState
    {
        /// <summary>
        /// Действует
        /// </summary>
        [Display("Действует")]
        Active = 10,

        /// <summary>
        /// Аннулирован
        /// </summary>
        [Display("Аннулирован")]
        Cancelled = 20,

        /// <summary>
        /// Расторгнут
        /// </summary>
        [Display("Расторгнут")]
        Terminated = 30
    }
}