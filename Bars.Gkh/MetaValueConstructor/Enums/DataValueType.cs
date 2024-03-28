namespace Bars.Gkh.MetaValueConstructor.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип значения
    /// </summary>
    public enum DataValueType
    {
        /// <summary>
        /// Строка
        /// </summary>
        [Display("Строка")]
        String,

        /// <summary>
        /// Дробный
        /// </summary>
        [Display("Число")]
        Number,

        /// <summary>
        /// Логический
        /// </summary>
        [Display("Логический")]
        Boolean,

        /// <summary>
        /// Справочник
        /// </summary>
        [Display("Справочник")]
        Dictionary
    }
}