namespace Bars.B4.Modules.Analytics.Reports.Generators.Models
{
    using System.Collections.Generic;

    public class ReportData
    {
        public string ReportId { get; set; }

        public string Group { get; set; }

        public string Format { get; set; }

        public string ConnectionString { get; set; }

        public IDictionary<string, string> ExtraParams { get; set; }

        public IEnumerable<MetaData> MetaSources { get; set; }

        public IDictionary<string, string> ExportSettings { get; set; }
    }
}