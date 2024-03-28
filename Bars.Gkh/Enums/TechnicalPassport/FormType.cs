namespace Bars.Gkh.Enums.TechnicalPassport
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип формы
    /// </summary>
    public enum FormType
    {
        /// <summary>
        /// Панель
        /// </summary>
        [Display("Панель")]
        Panel = 10,

        /// <summary>
        /// Таблица
        /// </summary>
        [Display("Таблица")]
        Grid = 20,

        /// <summary>
        /// Таблица со свойствами
        /// </summary>
        [Display("Таблица со свойствами")]
        PropertyGrid = 30,

        /// <summary>
        /// Таблица со встроенным редактированием
        /// </summary>
        [Display("Таблица со встроенным редактированием")]
        InlineGrid = 40,

        /// <summary>
        /// Таблица со встроенным редактированием, имеющая фиксированные значения
        /// </summary>
        [Display("Таблица со встроенным редактированием, имеющая фиксированные значения")]
        FixedInlineGrid = 50
    }
}
