namespace Bars.Gkh.Enums.ClaimWork
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип основания
    /// </summary>
    [Display("Тип основания")]
    public enum ReasonType
    {
        /// <summary>
        /// Не задано
        /// </summary>
        [Display("Не задано")]
        NotSet,

        /// <summary>
        /// Реестр неплательщиков
        /// </summary>
        [Display("Реестр неплательщиков")]
        Debtor = 10,

        /// <summary>
        /// Подрядчики, нарушившие условия договора
        /// </summary>
        [Display("Подрядчики, нарушившие условия договора")]
        Builder = 20
    }
}