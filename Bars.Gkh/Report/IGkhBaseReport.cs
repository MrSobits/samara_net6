namespace Bars.Gkh.Report
{
    using System.Collections.Generic;
    using System.IO;

    using Bars.B4;
    using B4.Modules.Reports;

    public interface IGkhBaseReport : IBaseReport
    {
        string Id { get; }

        string CodeForm { get; }

        new string Name { get; }

        string Description { get; }

        string Permission { get; }

        string Extention { get; }

        string ReportGeneratorName { get; }

        BaseParams BaseParams { get; set; }

        new void PrepareReport(ReportParams reportParams);

        new Stream GetTemplate();

        string GetFileExtension();

        string GetReportGenerator();

        void SetUserParams(UserParamsValues userParamsValues);

        List<TemplateInfo> GetTemplateInfo();

        bool PrintingAllowed { get; }
    }
}