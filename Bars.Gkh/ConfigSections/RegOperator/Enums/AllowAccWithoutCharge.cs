namespace Bars.Gkh.ConfigSections.RegOperator.Enums
{
    using B4.Utils;
    
    /// <summary>
    /// Учитывать ЛС, у которых есть задолженность, но нет начислений
    /// </summary>
    public enum AllowAccWithoutCharge
    {
        /// <summary>
        /// Не учитывать
        /// </summary>
        [Display("Не учитывать")]
        DontAllow = 0,

        /// <summary>
        /// Учитывать положительное сальдо
        /// </summary>
        [Display("Учитывать положительное сальдо")]
        WithPositiveSaldo = 1,

        /// <summary>
        /// Учитывать отрицательное сальдо
        /// </summary>
        [Display("Учитывать отрицательное сальдо")]
        WithNegativeSaldo = 2,


        /// <summary>
        /// Учитывать ненулевые сальдо
        /// </summary>
        [Display("Учитывать ненулевые сальдо")]
        WithNonZeroSaldo = 3
    }
}