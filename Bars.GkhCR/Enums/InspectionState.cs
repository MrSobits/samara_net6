namespace Bars.GkhCr.Enums
{
    using B4.Utils;

    /// <summary>
    /// Факт обследования
    /// </summary>
    public enum InspectionState
    {
        /// <summary>
        /// Не обследовано
        /// </summary>
        [Display("Не обследовано")]
        NotInspected = 0,

        /// <summary>
        /// Обследовано
        /// </summary>
        [Display("Обследовано")]
        Inspected = 10
    }
}