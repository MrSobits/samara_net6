namespace Bars.Gkh.Overhaul.DomainService.Impl
{
    using System.IO;

    using Bars.B4;
    using B4.Modules.Reports;


    using Castle.Windsor;

    public class RealtyObjectDataService : IRealtyObjectDataService 
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult GetPrintFormResult(BaseParams baseParams)
        {
            var printForm = Container.Resolve<IPrintForm>("RealtyObjectDataReport");

            var rp = new ReportParams();

            printForm.SetUserParams(baseParams);
            printForm.PrepareReport(rp);
            var template = printForm.GetTemplate();

            IReportGenerator generator;
            if (printForm is IGeneratedPrintForm)
            {
                generator = printForm as IGeneratedPrintForm;
            }
            else
            {
                generator = Container.Resolve<IReportGenerator>("XlsIoGenerator");
            }

            var result = new MemoryStream();

            generator.Open(template);
            generator.Generate(result, rp);
            result.Seek(0, SeekOrigin.Begin);

            //var reportName = string.Format("report.xls", printFormObject.Name);
            return new BaseDataResult(result) { Message = "report.xlsx" };
        }
    }
}