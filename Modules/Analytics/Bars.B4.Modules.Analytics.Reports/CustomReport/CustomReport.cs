namespace Bars.B4.Modules.Analytics.Reports;

using System.Collections.Generic;
using System.IO;

using Bars.B4.Modules.Analytics.Data;
using Bars.B4.Modules.Analytics.Reports.Enums;

/// <summary>
/// Кастомный отчет
/// </summary>
public class CustomReport : IReport
{
    public CustomReport(IEnumerable<IDataSource> dataSources, IEnumerable<IParam> reportParams, string key, string name, Stream template,
        Dictionary<string, string> exportSettings = default)
    {
        this.dataSources = dataSources;
        this.reportParams = reportParams;
        this.Key = key;
        this.Name = name;
        this.template = template;
        this.exportSettings = exportSettings;
    }

    /// <inheritdoc />
    public string Key { get; }

    /// <inheritdoc />
    public string Name { get; }

    private readonly IEnumerable<IDataSource> dataSources;
    private readonly IEnumerable<IParam> reportParams;
    private readonly Stream template;
    private readonly Dictionary<string, string> exportSettings;

    /// <inheritdoc />
    public Stream GetTemplate() => this.template;

    /// <inheritdoc />
    public IEnumerable<IDataSource> GetDataSources() => this.dataSources;

    /// <inheritdoc />
    public IEnumerable<IParam> GetParams() => this.reportParams;

    /// <inheritdoc />
    public Dictionary<string, string> GetExportSettings(ReportPrintFormat format) => this.exportSettings;
}