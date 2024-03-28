namespace Bars.GkhGji.Regions.Chelyabinsk.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип запроса в гис ерп
    /// </summary>
    public enum GisErpRequestType
    {
        /// <summary>
        /// Первичное размещение
        /// </summary>
        [Display("Первичное размещение")]
        Initialization = 1,

        /// <summary>
        /// Требует коррекции
        /// </summary>
        [Display("Требует коррекции")]
        NeedCorrection = 3,

        /// <summary>
        /// Коррекция
        /// </summary>
        [Display("Коррекция")]
        Correction = 2,

        /// <summary>
        /// Выгрузка результатов
        /// </summary>
        [Display("Выгрузка результатов")]
        CorrectionFinal = 4,

        /// <summary>
        /// Коррекция
        /// </summary>
        [Display("Выгрузка предписаний")]
        CorrectionPrescription = 5

    }
}