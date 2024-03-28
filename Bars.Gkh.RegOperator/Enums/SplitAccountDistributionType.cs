namespace Bars.Gkh.RegOperator.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Вид расппределения задолженности
    /// </summary>
    public enum SplitAccountDistributionType
    {
        /// <summary>
        /// Вручную
        /// </summary>
        [Display("Вручную")]
        Manual,

        /// <summary>
        /// Равномерное
        /// </summary>
        [Display("Равномерное")]
        Uniform,

        /// <summary>
        /// Пропорционально долям
        /// </summary>
        [Display("Пропорционально долям")]
        ProportionalAreaShare
    }
}