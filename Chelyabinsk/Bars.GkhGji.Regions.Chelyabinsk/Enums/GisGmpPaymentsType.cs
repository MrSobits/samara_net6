namespace Bars.GkhGji.Regions.Chelyabinsk.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип запроса по оплате
    /// </summary>
    public enum GisGmpPaymentsType
    {
        /// <summary>
        /// Оплата текущего начисления
        /// </summary>
        [Display("Оплата текущего начисления")]
        Current = 1,

        /// <summary>
        /// Все оплаты плательщика
        /// </summary>
        [Display("Все оплаты плательщика")]
        All = 2,

        /// <summary>
        /// Все оплаты за период
        /// </summary>
        [Display("Все оплаты за период")]
        AllInTime = 3,


    }
}