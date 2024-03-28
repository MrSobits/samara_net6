namespace Bars.Gkh.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип дома
    /// </summary>
    public enum TypeHouse
    {
        /// <summary>
        /// Не задано
        /// </summary>
        [Display("Не задано")]
        NotSet = 0,

        /// <summary>
        /// Блокированной застройки
        /// </summary>
        [Display("Блокированной застройки")]
        BlockedBuilding = 10,

        /// <summary>
        /// Индивидуальный
        /// </summary>
        [Display("Индивидуальный")]
        Individual = 20,

        /// <summary>
        /// Многоквартирный
        /// </summary>
        [Display("Многоквартирный")]
        ManyApartments = 30,

        /// <summary>
        /// Общежитие
        /// </summary>
        [Display("Общежитие")]
        SocialBehavior = 40
    }
}