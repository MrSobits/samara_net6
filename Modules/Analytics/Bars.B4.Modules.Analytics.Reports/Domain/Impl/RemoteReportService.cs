namespace Bars.B4.Modules.Analytics.Reports.Domain;

using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;

using Bars.B4.Application;
using Bars.B4.IoC;
using Bars.B4.Modules.Analytics.Reports.Entities;
using Bars.B4.Modules.Analytics.Reports.Enums;
using Bars.B4.Modules.Analytics.Reports.Extensions;
using Bars.B4.Modules.Analytics.Reports.Generators.Models;
using Bars.B4.Utils;

using Castle.Windsor;

using Newtonsoft.Json;


/// <inheritdoc />
public class RemoteReportService : IRemoteReportService
{
    private const string defaultGroupName = "GkhReports";
    private const string notAccessServiceError = "Ошибка вызова сервиса отчётов";
    private const string notSuccessError = "Ошибка при генерации отчёта";

    private readonly Uri baseUri;
    private readonly IWindsorContainer container;

    public RemoteReportService(IWindsorContainer container)
    {
        this.container = container;
        var address = ApplicationContext.Current.Configuration.AppSettings.GetAs<string>("RemoteReport_EndpointAddress");
        if (string.IsNullOrWhiteSpace(address))
        {
            throw new ConfigurationErrorsException($"Не указан адрес удалённого сервера генерации отчётов в файле конфигурации");
        }

        this.baseUri = new Uri(address);
    }

    /// <inheritdoc />
    public Stream Generate(IReport report, Stream template, BaseParams baseParams, ReportPrintFormat printFormat, IDictionary<string, object> extraParams,
        IDictionary<string, string> exportSettings = default)
    {
        try
        {
            return this.GenerateReport(report, template, baseParams, printFormat, extraParams, exportSettings);
        }
        catch (AggregateException e)
        {
            throw new Exception(RemoteReportService.notAccessServiceError, e);
        }
        catch (HttpRequestException e)
        {
            throw new Exception(RemoteReportService.notAccessServiceError, e);
        }
    }

    /// <inheritdoc />
    public bool SaveOrUpdateTemplate(IReport report, byte[] template)
    {
        try
        {
            return this.SaveOrUpdateTemplate(report, new MemoryStream(template));
        }
        catch (AggregateException e)
        {
            throw new Exception(RemoteReportService.notAccessServiceError, e);
        }
        catch (HttpRequestException e)
        {
            throw new Exception(RemoteReportService.notAccessServiceError, e);
        }
    }

    /// <inheritdoc />
    public Stream GetTemplateWithMeta(IReport report, BaseParams baseParams, IDictionary<string, object> extraParams)
    {
        try
        {
            this.SaveOrUpdateTemplate(report, report.GetTemplate());
            
            var metaData = report.GetDataSources()
                .Select(x => new MetaData
                {
                    SourceName = x.Name,
                    MetaType = x.GetMetaData().Name,
                    Data = x.GetData(baseParams),
                });
            
            var reportData = new ReportData
            {
                ReportId = report.Key,
                Group = this.GetGroupReport(report),
                ConnectionString = extraParams.Get("ConnectionString")?.ToString(),
                ExtraParams = extraParams.ToDictionary(x => x.Key, x => x.Value.ToString()),
                MetaSources = metaData
            };

            var uri = new Uri(this.baseUri, "api/Report/getTemplateWithMeta");
            var json = JsonConvert.SerializeObject(reportData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            
            using var client = new HttpClient();
            client.Timeout = TimeSpan.FromMinutes(10);

            var response = client.PostAsync(uri, content).ConfigureAwait(false).GetAwaiter().GetResult();
            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(RemoteReportService.notSuccessError);
            }

            var bytes = response.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult();
            return new MemoryStream(bytes);
        }
        catch (AggregateException e)
        {
            throw new Exception(RemoteReportService.notAccessServiceError, e);
        }
        catch (HttpRequestException e)
        {
            throw new Exception(RemoteReportService.notAccessServiceError, e);
        }
    }

    private Stream GenerateReport(
        IReport report,
        Stream actualTemplate,
        BaseParams baseParams,
        ReportPrintFormat printFormat,
        IDictionary<string, object> extraParams,
        IDictionary<string, string> exportSettings)
    {
        if (!this.SaveOrUpdateTemplate(report, actualTemplate))
        {
            throw new Exception("Не удалось сохранить шаблон отчёта");
        }

        var metaData = report.GetDataSources()
            .Select(x => new MetaData
            {
                SourceName = x.Name,
                MetaType = x.GetMetaData().Name,
                Data = x.GetData(baseParams)
            });

        var reportData = new ReportData
        {
            ReportId = report.Key,
            Group = this.GetGroupReport(report),
            Format = this.GetExportFormat(printFormat),
            ConnectionString = extraParams.Get("ConnectionString")?.ToString(),
            ExtraParams = extraParams.ToDictionary(x => x.Key, x => x.Value.ToString()),
            MetaSources = metaData,
            ExportSettings = exportSettings,
        };

        var uri = new Uri(this.baseUri, "api/Report/generateReport");
        var json = JsonConvert.SerializeObject(reportData);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        using var client = new HttpClient();
        client.Timeout = TimeSpan.FromSeconds(ApplicationContext.Current.Configuration.AppSettings?.GetAs<int>("RemoteReport_RequestTimeoutSec") ?? 300);

        var response = client.PostAsync(uri, content).ConfigureAwait(false).GetAwaiter().GetResult();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(RemoteReportService.notSuccessError);
        }

        var bytes = response.Content.ReadAsByteArrayAsync().GetAwaiter().GetResult();
        return new MemoryStream(bytes);
    }
    
    private string GetGroupReport(IReport report)
    {
        var result = RemoteReportService.defaultGroupName;

        if (report is StoredReport)
        {
            result = nameof(StoredReport);
        }
        else if (report is BaseCodedReport)
        {
            result = nameof(BaseCodedReport);
        }

        return result;
    }
    
    private string GetExportFormat(ReportPrintFormat printFormat)
    {
        var exportFormat = printFormat.ExportFormat();
        if (exportFormat == StiExportFormat.None)
        {
            throw new ArgumentOutOfRangeException(nameof(printFormat), @"Не поддерживаемый формат файла");
        }

        return exportFormat.ToString();
    }
    
    private bool SaveOrUpdateTemplate(IReport report, Stream templateStream)
    {
        if (this.NeedSaveOrUpdateTemplate(report, templateStream))
        {
            return this.SaveReportTemplate(report, templateStream);
        }

        return true;
    }
    
    private bool SaveReportTemplate(IReport report, Stream template)
    {
        long userId;

        var userIdentity = this.container.Resolve<IUserIdentity>();
        using (this.container.Using(userIdentity))
        {
            userId = userIdentity.UserId;
        }

        var httpQuery = $"ReportName={report.Name}&ReportId={report.Key}&Group={this.GetGroupReport(report)}&UserId={userId}";
        var uri = new Uri(this.baseUri, $"api/Report/saveReportTemplate?{httpQuery}");

        using (var client = new HttpClient())
        {
            template.Seek(0, SeekOrigin.Begin);
            var content = new MultipartFormDataContent {{new StreamContent(template), "file", report.Name}};

            var response = client.PostAsync(uri, content).Result;

            return response.IsSuccessStatusCode;
        }
    }
    
    private bool NeedSaveOrUpdateTemplate(IReport report, Stream templateStream)
    {
        using var client = new HttpClient();

        var httpQuery = $"reportId={report.Key}&groupName={this.GetGroupReport(report)}";
        var uri = new Uri(this.baseUri, $"api/Report/downloadReportTemplate?{httpQuery}");

        var response = client.GetAsync(uri).Result;
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception("Ошибка при вызове удалённого сервиса");
        }

        var bytes = response.Content.ReadAsByteArrayAsync().Result;
        var actualTemplate = templateStream.ReadAllBytes();

        return bytes == null || !bytes.SequenceEqual(actualTemplate);
    }
}