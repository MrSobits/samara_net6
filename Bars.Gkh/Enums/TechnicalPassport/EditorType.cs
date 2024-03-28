namespace Bars.Gkh.Enums.TechnicalPassport
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип редактора
    /// </summary>
    public enum EditorType
    {
        /// <summary>
        /// Текст
        /// </summary>
        [Display("Текст")]
        Text = 10,

        /// <summary>
        /// Дата
        /// </summary>
        [Display("Дата")]
        Date = 20,

        /// <summary>
        /// Целое
        /// </summary>
        [Display("Целое")]
        Int = 30,

        /// <summary>
        /// Число
        /// </summary>
        [Display("Число")]
        Double = 40,

        /// <summary>
        /// Финансовый
        /// </summary>
        [Display("Финансовый")]
        Decimal = 50,

        /// <summary>
        /// Логический
        /// </summary>
        [Display("Логический")]
        Bool = 60,

        /// <summary>
        /// Справочник
        /// </summary>
        [Display("Справочник")]
        Dictionary = 70,

        /// <summary>
        /// Ссылка на таблицу
        /// </summary>
        [Display("Ссылка на таблицу")]
        TableReference = 80,

        /// <summary>
        /// Ссылка на форму
        /// </summary>
        [Display("Ссылка на форму")]
        FormReference = 90
    }
}
