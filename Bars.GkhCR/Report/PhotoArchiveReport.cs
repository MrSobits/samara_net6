namespace Bars.GkhCr.Report
{
    using Bars.B4;
    
    using B4.Modules.Reports;

    public class PhotoArchiveReport : BasePrintForm
    {
        public PhotoArchiveReport()
            : base(new ReportTemplateBinary(Properties.Resources.PhotoArchive))
        {
        }

        public override string Name
        {
            get
            {
                return "Фото архив";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Фото архив";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Отчеты ГЖИ";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.PhotoArchiveReport";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.CR.PhotoArchiveReport";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
        }
    }
}