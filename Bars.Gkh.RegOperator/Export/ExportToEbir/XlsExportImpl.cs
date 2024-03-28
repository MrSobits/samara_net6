namespace Bars.Gkh.RegOperator.Export.ExportToEbir
{
    using B4.Modules.Reports;

    public class XlsExportImpl : BaseReport
    {
        public XlsExportImpl()
            : base(new ReportTemplateBinary(Properties.Resources.EbirExport))
        {
        }

        public override string Name { get { return string.Empty; } }

        public override void PrepareReport(ReportParams reportParams)
        {
        }
    }
}