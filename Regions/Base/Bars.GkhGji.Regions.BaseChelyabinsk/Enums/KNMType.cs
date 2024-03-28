namespace Bars.GkhGji.Regions.BaseChelyabinsk.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип КНМ
    /// </summary>
    public enum KNMType
    {

        /// <summary>
        /// Плановая проверка
        /// </summary>
        [Display("Плановая проверка")]
        Planned = 0,

        /// <summary>
        /// Плановая проверка
        /// </summary>
        [Display("Внеплановая проверка")]
        NotPlanned = 1,

        /// <summary>
        /// Контрольная закупка
        /// </summary>
        [Display("Контрольная закупка")]
        Purchase = 2
    }
}