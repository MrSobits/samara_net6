namespace Bars.Gkh.RegOperator.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Статус разбора
    /// </summary>
    public enum ChesAnalysisState
    {
        /// <summary>
        /// Разобран
        /// </summary>
        [Display("Разобран")]
        Analyzed = 10,

        /// <summary>
        /// Загрузка исправлений
        /// </summary>
        [Display("Загрузка исправлений")]
        LoadingCorrection = 20
    }
}