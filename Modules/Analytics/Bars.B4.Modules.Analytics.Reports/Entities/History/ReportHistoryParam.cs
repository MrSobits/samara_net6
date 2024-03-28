namespace Bars.B4.Modules.Analytics.Reports.Entities.History
{
    /// <summary>
    /// Сохраняемые параметры отчета
    /// </summary>
    public struct ReportHistoryParam
    {
        /// <summary>
        /// Значение параметра
        /// </summary>
        public object Value;

        /// <summary>
        /// Отображаемое значение параметра
        /// </summary>
        public string DisplayValue;

        /// <summary>
        /// Отображаемое имя параметра
        /// </summary>
        public string DisplayName;
    }
}