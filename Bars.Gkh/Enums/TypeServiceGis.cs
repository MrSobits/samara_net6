namespace Bars.Gkh.Enums
{
    using B4.Utils;

    /// <summary>
    /// Тип услуги
    /// </summary>
    public enum TypeServiceGis
    {
        /// <summary>
        /// Не задан
        /// </summary>
        [Display("Не задан")]
        NotSet = 0,

        /// <summary>
        /// Коммунальная
        /// </summary>
        [Display("Коммунальная")]
        Communal = 10,

        /// <summary>
        /// Жилищная
        /// </summary>
        [Display("Жилищная")]
        Housing = 20,

        /// <summary>
        /// Прочая
        /// </summary>
        [Display("Прочая")]
        Other = 30
    }
}