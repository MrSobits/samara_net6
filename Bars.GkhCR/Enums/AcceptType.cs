namespace Bars.GkhCr.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Статус приемки 
    /// </summary>
    public enum AcceptType
    {
        /// <summary>
        /// Принят
        /// </summary>
        [Display("Принят")]
        Accepted = 1,

        /// <summary>
        /// Не принят
        /// </summary>
        [Display("Не принят")]
        NotAccepted = 2
    }
}