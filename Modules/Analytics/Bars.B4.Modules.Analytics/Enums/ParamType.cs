namespace Bars.B4.Modules.Analytics.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип параметра.
    /// </summary>
    public enum ParamType
    {
        /// <summary>
        /// Число
        /// </summary>
        [Display("Число")]
        Number,
        
        /// <summary>
        /// Текст
        /// </summary>
        [Display("Текст")]
        Text,
        
        /// <summary>
        /// Выбор из справочника
        /// </summary>
        [Display("Выбор из справочника")]
        Catalog,
        
        /// <summary>
        /// Дата
        /// </summary>
        [Display("Дата")]
        Date,
        
        /// <summary>
        /// Перечисление
        /// </summary>
        [Display("Перечисление")]
        Enum,

        /// <summary>
        /// Логическое
        /// </summary>
        [Display("Логическое")]
        Bool,

        /// <summary>
        /// Из SQL запроса
        /// </summary>
        [Display("Из SQL запроса")]
        SqlQuery,

        /// <summary>
        /// Месяц, год
        /// </summary>
        [Display("Месяц, год")]
        MonthYear
    }
}