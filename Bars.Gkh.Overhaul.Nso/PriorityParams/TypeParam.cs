namespace Bars.Gkh.Overhaul.Nso.PriorityParams
{
    using B4.Utils;

    public enum TypeParam
    {
        /// <summary>
        /// Количественный
        /// </summary>
        [Display("Количественный")]
        Quant = 10,

        /// <summary>
        /// Качественный
        /// </summary>
        [Display("Качественный")]
        Qualit = 20,

        /// <summary>
        /// Множественный
        /// </summary>
        [Display("Множественный")]
        Multi = 30
    }
}