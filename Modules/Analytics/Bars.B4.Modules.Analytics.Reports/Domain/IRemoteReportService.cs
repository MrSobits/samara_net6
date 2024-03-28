namespace Bars.B4.Modules.Analytics.Reports.Domain;

using System.Collections.Generic;
using System.IO;
using Bars.B4.Modules.Analytics.Reports.Enums;

/// <summary>
/// Удалённый сервис отчётов
/// </summary>
public interface IRemoteReportService
{
    /// <summary>
    /// Сформировать отчет
    /// </summary>
    Stream Generate(IReport report, Stream template, BaseParams baseParams, ReportPrintFormat printFormat, IDictionary<string, object> extraParams,
        IDictionary<string, string> exportSettings = default);

    /// <summary>
    /// Сохранить шаблон
    /// </summary>
    bool SaveOrUpdateTemplate(IReport report, byte[] template);

    /// <summary>
    /// Получить шаблон с метаданными
    /// </summary>
    Stream GetTemplateWithMeta(IReport report, BaseParams baseParams, IDictionary<string, object> extraParams);
}