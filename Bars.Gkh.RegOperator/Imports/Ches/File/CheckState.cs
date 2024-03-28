namespace Bars.Gkh.RegOperator.Imports.Ches.File
{
    using Bars.B4.Utils;

    /// <summary>
    /// Статус проверки
    /// </summary>
    public enum CheckState
    {
        /// <summary>
        /// Пусто
        /// </summary>
        None = 0,

        /// <summary>
        /// Проверка пройдена
        /// </summary>
        [Display("Проверка пройдена")]
        Checked = 1,

        /// <summary>
        /// Проверка не пройдена
        /// </summary>
        [Display("Проверка не пройдена")]
        NotCheked = 2
    }
}