namespace Bars.Gkh.Overhaul.Nso.DomainService.Impl
{
    using System.IO;
    using B4;
    using B4.Modules.Reports;
    using Gkh.Report;

    public class CommonEstateObjectService : Overhaul.DomainService.Impl.CommonEstateObjectService
    {
        public override Stream PrintReport(BaseParams baseParams)
        {
            var stream = new MemoryStream();

            var report = Container.Resolve<IGkhBaseReport>("StructElementListNso");

            var reportProvider = Container.Resolve<IGkhReportProvider>();

            //собираем сборку отчета и формируем reportParams
            var reportParams = new ReportParams();
            report.PrepareReport(reportParams);

            //получаем Генератор отчета
            var generatorName = report.GetReportGenerator();

            var generator = Container.Resolve<IReportGenerator>(generatorName);

            reportProvider.GenerateReport(report, stream, generator, reportParams);

            return stream;
        }
    }
}