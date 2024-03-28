namespace Bars.Gkh.Report
{
    using System.Collections.Generic;

    using Bars.B4.Modules.Analytics.Data;
    using Bars.B4.Modules.Analytics.Reports;
    using Bars.Gkh.Properties;

    /// <summary>
    /// Отчет Обращение граждан (с портала)
    /// </summary>
    public class CitizenSuggestionPortalReport : BaseCodedReport
    {
        /// <summary>
        /// Шаблон
        /// </summary>
        protected override byte[] Template
        {
            get
            {
                return Resources.CitizenSuggestionPortalReport;
            }
        }

        /// <summary>
        /// Наименование
        /// </summary>
        public override string Name
        {
            get { return "Обращения граждан (с портала)"; }
        }

        /// <summary>
        /// Описание
        /// </summary>
        public override string Description { get; }

        /// <summary>
        /// Получить источники данных
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<IDataSource> GetDataSources()
        {
            return new List<IDataSource>();
        }
    }
}