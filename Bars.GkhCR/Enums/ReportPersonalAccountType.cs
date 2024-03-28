namespace Bars.GkhCr.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип абонента
    /// </summary>
    public enum ReportPersonalAccountType
    {
        /// <summary>
        /// Физ.лицо
        /// </summary>
        [Display("Физ.лицо")]
        Individual = 0,

        /// <summary>
        /// Юр.лицо
        /// </summary>
        [Display("Юр.лицо")]
        Legal = 1
    }
}
