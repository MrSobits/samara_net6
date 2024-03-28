namespace Bars.Gkh.RegOperator.Export.ExportToEbir
{
    using System.IO;

    using B4.Modules.Reports;
    using Bars.Gkh.Report;

    public class XlsExport : BaseExport, IEbirExport
    {
        public string Format { get { return "xls"; } }

        protected override B4.Modules.FileStorage.FileInfo SaveFile()
        {
            var report = new XlsExportImpl();

            var reportParams = new ReportParams();
            report.PrepareReport(reportParams);

            var generator = Container.Resolve<IReportGenerator>("XlsIoGenerator");

            this.FillData(reportParams);

            var reportProvider = Container.Resolve<IGkhReportProvider>();
            var stream = new MemoryStream();
            reportProvider.GenerateReport(report, stream, generator, reportParams);

            var fileInfo = FileManager.SaveFile(stream, "ebirExport.xlsx");

            return fileInfo;
        }
        
        private void FillData(ReportParams reportParams)
        {
            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");

            foreach (var record in records)
            {
                section.ДобавитьСтроку();

                section["AccountOperator"] = record.AccountOperator;
                section["AccountNum"] = record.AccountNum;
                section["Surname"] = record.Surname;
                section["Name"] = record.Name;
                section["Patronymic"] = record.Patronymic;
                section["Ulname"] = record.Ulname;
                section["INN"] = record.INN;
                section["KPP"] = record.KPP;
                section["Dolya"] = record.Dolya;
                section["Address"] = record.Address;
                section["ServiceCode"] = record.ServiceCode;
                section["ProviderCode"] = record.ProviderCode;
                section["TYear"] = record.TYear;
                section["TMonth"] = record.TMonth;
                section["SaldoIn"] = record.SaldoIn;
                section["SaldoFineIn"] = record.SaldoFineIn;
                section["ChargeType"] = record.ChargeType;
                section["UnitCode"] = record.UnitCode;
                section["ChargeVolume"] = record.ChargeVolume;
                section["Tarif"] = record.Tarif;
                section["Area"] = record.Area;
                section["ChargeSum"] = record.ChargeSum;
                section["ReChargeSum"] = record.ReChargeSum;
                section["CostsSum"] = record.CostsSum;
                section["FineSum"] = record.FineSum;
                section["PaySum"] = record.PaySum;
                section["PayBreak"] = record.PayBreak;
                section["LastPayDate"] = record.LastPayDate;
                section["PayFineSum"] = record.PayFineSum;
                section["SaldoOut"] = record.SaldoOut;
                section["SaldoFineOut"] = record.SaldoFineOut;
                section["NameKO"] = record.NameKO;
                section["KredOrg"] = record.KredOrg;
                section["BIKKO"] = record.BIKKO;
            }
        }
    }
}