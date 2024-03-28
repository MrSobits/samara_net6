namespace Bars.B4.Modules.Analytics.Reports.Generators
{
    using System.Collections.Generic;
    using System.IO;

    using Bars.B4.Application;
    using Bars.B4.Modules.Analytics.Reports.Domain;
    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Utils;

    /// <summary>
    /// Генератор отчётов
    /// </summary>
    public class StimulReportGenerator : IReportGenerator
    {
        public IRemoteReportService RemoteReportService { get; set; }

        /// <inheritdoc />
        public Stream Generate(
            IReport report,
            Stream template,
            BaseParams baseParams,
            ReportPrintFormat printFormat,
            IDictionary<string, object> customArgs)
        {
            customArgs.TryGetValue("ExportSettings", out var exportSettings);

            return this.RemoteReportService.Generate(report, template, baseParams, printFormat, this.PrepareExtraParams(baseParams, customArgs),
                (Dictionary<string, string>)exportSettings);
        }

        private IDictionary<string, object> PrepareExtraParams(BaseParams baseParams, IDictionary<string, object> customArgs)
        {
            var result = new Dictionary<string, object>();

            foreach (var param in baseParams.Params)
            {
                var value = param.Value.ToString();
                if (!string.IsNullOrWhiteSpace(value))
                {
                    result[param.Key] = value;
                }
            }

            if (!customArgs.Get("UseTemplateConnectionString").ToBool())
            {
                result["ConnectionString"] = ApplicationContext.Current.Configuration.ConnString;
            }

            return result;
        }
    }
}
