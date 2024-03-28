namespace Bars.GkhGji.DomainService.Impl
{
    using System;
    using System.IO;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.Reports;
    using Bars.Gkh.Report;
    using Bars.Gkh.StimulReport;
    using Bars.GkhGji.Domain.SpecialAccountReport.Serialize;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Report;
    using Castle.Windsor;
    using FileInfo = B4.Modules.FileStorage.FileInfo;

    public class SpecialAccountReportSignature : ISignature<SpecialAccountReport>
    {
        private IFileManager _fileManager;
        private IDomainService<FileInfo> _fileDomain;
        public SpecialAccountReportSignature(IFileManager fileManager, IDomainService<FileInfo> fileDomain)
        {
            _fileManager = fileManager;
            _fileDomain = fileDomain;
        }

        public IWindsorContainer Container { get; set; }

        public IDomainService<SpecialAccountReport> DomainService { get; set; }

        public MemoryStream GetXmlStream(long id)
        {
            var reports = this.Container.ResolveAll<IGkhBaseReport>();
            var report = reports.FirstOrDefault(x => x.Id == "SpecialAccountReportReport");
            if (report == null)
            {
                throw new Exception("Не найдена реализация отчета для выбранного документа");
            }
            var userParam = new UserParamsValues();
            userParam.AddValue("DocumentId", id);
            MemoryStream stream;
            report.SetUserParams(userParam);
            var reportProvider = Container.Resolve<IGkhReportProvider>();
            if (report is IReportGenerator && report.GetType().IsSubclassOf(typeof(StimulReport)))
            {
                //Вот такой вот костыльный этот метод Все над опеределывать
            
                stream = (report as StimulReport).GetGeneratedReport();
            }
            else
            {
                var reportParams = new ReportParams();
                report.PrepareReport(reportParams);

                // получаем Генератор отчета
                var generatorName = report.GetReportGenerator();

                stream = new MemoryStream();
                var generator = Container.Resolve<IReportGenerator>(generatorName);
                reportProvider.GenerateReport(report, stream, generator, reportParams);
            }
            var newfile = _fileManager.SaveFile(stream, "report.pdf");

            var specaccreport = DomainService.Get(id);
            specaccreport.File = newfile;
            DomainService.Update(specaccreport);
            var pdfId = specaccreport.File.Id;
            byte[] data;
            if (pdfId == 0)
            {
                data = new byte[0];
            }
            else
            {
                using (var file = _fileManager.GetFile(_fileDomain.Get(pdfId)))
                {
                    using (var tmpStream = new MemoryStream())
                    {
                        file.CopyTo(tmpStream);
                        data = tmpStream.ToArray();
                    }
                }
            }
            return new MemoryStream(data);
        }
    }
}