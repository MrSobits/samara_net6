namespace Bars.Gkh.Modules.Gkh1468.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип системы теплоснабжения
    /// </summary>
    public enum HeatingSystemType
    {
        /// <summary>
        /// Открытого типа
        /// </summary>
        [Display("Открытая")]
        Opened = 10,

        /// <summary>
        /// Закрытого типа
        /// </summary>
        [Display("Закрытая")]
        Closed = 20
    }
}
