namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип учета прибора учета
    /// </summary>
    public enum TypeAccounting
    {
        /// <summary>
        /// Индивидуальный
        /// </summary>
        [Display("Индивидуальный")]
        Individual = 10,

        /// <summary>
        /// Общедомовой
        /// </summary>
        [Display("Общедомовой")]
        Social = 20,

        /// <summary>
        /// Квартирный
        /// </summary>
        [Display("Квартирный")]
        Apartment = 30,

        /// <summary>
        /// Комнатный
        /// </summary>
        [Display("Комнатный")]
        Room = 40
    }
}
