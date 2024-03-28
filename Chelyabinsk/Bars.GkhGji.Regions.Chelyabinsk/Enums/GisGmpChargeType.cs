namespace Bars.GkhGji.Regions.Chelyabinsk.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип запроса по начислению
    /// </summary>
    public enum GisGmpChargeType
    {
        /// <summary>
        /// Извещения о начислении
        /// </summary>
        [Display("Извещения о начислении")]
        First = 1,

        /// <summary>
        /// Извещение об уточнении начисления
        /// </summary>
        [Display("Извещение об уточнении начисления")]
        Refinement = 2,

        /// <summary>
        /// Аннулирование начисления
        /// </summary>
        [Display("Аннулирование начисления")]
        Cancellation = 3,

        /// <summary>
        /// Деаннулирование начисления
        /// </summary>
        [Display("Деаннулирование начисления")]
        ReCancellation = 4,

    }
}