namespace Bars.Gkh.Overhaul.Hmao.Enum
{
    using B4.Utils;

    /// <summary>
    /// Действие актуализации
    /// </summary>
    public enum VersionActualizeAction
    {
        /// <summary>
        /// Добавление
        /// </summary>
        [Display("Добавление")]
        Save = 1,

        /// <summary>
        /// Изменение
        /// </summary>
        [Display("Изменение")]
        Update = 2,

        /// <summary>
        /// Удаление
        /// </summary>
        [Display("Удаление")]
        Delete = 3
    }
}