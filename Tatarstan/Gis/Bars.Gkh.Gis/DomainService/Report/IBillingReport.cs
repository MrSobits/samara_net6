namespace Bars.Gkh.Gis.DomainService.Report
{
    using System.Collections.Generic;
    using System.IO;
    using B4;
    using B4.Modules.Reports;
    using Castle.Windsor;

    public interface IBillingReport
    {
        IWindsorContainer Container { get; set; }

        IList<PrintFormExportFormat> GetExportFormats();

        void PrepareReport(ReportParams reportParams);

        void SetExportFormat(int formatId);

        void SetUserParams(BaseParams baseParams);

        void Open(byte[] reportTemplate);

        void Generate(Stream result);

        MemoryStream GetGeneratedReport();

        byte[] GetTemplate();

        string Name { get; set; }

        string Desciption { get; set; }

        string GroupName { get; set; }

        string ParamsController { get; set; }

        string RequiredPermission { get; set; }
    }
}
