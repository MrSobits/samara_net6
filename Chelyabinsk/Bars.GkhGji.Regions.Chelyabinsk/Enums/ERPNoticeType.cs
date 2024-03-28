namespace Bars.GkhGji.Regions.Chelyabinsk.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    ///Простой тип - коды способов уведомления
    /// </summary>
    public enum ERPNoticeType
    {

        /// <summary>
        /// Лично
        /// </summary>
        [Display("Лично")]
        TYPE_I = 10,

        /// <summary>
        /// Нарочно
        /// </summary>
        [Display("Нарочно")]
        TYPE_II = 20,

        /// <summary>
        /// Повестка
        /// </summary>
        [Display("Повестка")]
        TYPE_III = 30,

        /// <summary>
        ///Иное
        /// </summary>
        [Display("Иное")]
        TYPE_OTHER = 40

    }
}