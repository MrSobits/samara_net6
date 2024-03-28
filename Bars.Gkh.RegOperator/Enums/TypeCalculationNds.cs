namespace Bars.Gkh.RegOperator.Enums
{
    using B4.Utils;

    /// <summary>
    /// Вид расчета НДС
    /// </summary>
    public enum TypeCalculationNds
    {
        /// <summary>
        /// Без НДС
        /// </summary>
        [Display("Без НДС")]
        WithoutNds = 0,

        /// <summary>
        /// 10%
        /// </summary>
        [Display("10%")]
        TenPercent = 10,

        /// <summary>
        /// 18%
        /// </summary>
        [Display("18%")]
        EighteenPercent = 20
    }
}