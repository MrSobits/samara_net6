using Bars.B4;
using Bars.B4.Modules.Reports;
using Bars.B4.Utils;
using Bars.Gkh.Overhaul.Hmao.DomainService.Version;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bars.Gkh.Overhaul.Hmao.Reports
{
    /// <inheritdoc />
    public class ImportPublishYearLogReport : BasePrintForm
    {
        public ImportPublishYearLogReport()
            : base(new ReportTemplateBinary(Properties.Resources.ImportPublishYearLogReport))
        {
        }

        public override string Desciption => "Лог импорта сведений о сроках проведения капитального ремонта";

        public override string GroupName => "Региональная программа";

        public override string ParamsController => null;

        public override string RequiredPermission => null;

        public override string Name => "Лог импорта сведений о сроках проведения КР";

        private IEnumerable<ImportPublishYearLogRecord> LogRecords { get; set; }

        public override void SetUserParams(BaseParams baseParams)
        {
            this.LogRecords = baseParams.Params.GetAs<IEnumerable<ImportPublishYearLogRecord>>("Data");

            if (this.LogRecords == null)
            {
                throw new Exception("Нет строк для логирования");
            }
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            if (!this.LogRecords.Any())
            {
                throw new Exception("Нет строк для логирования");
            }

            var sect = reportParams.ComplexReportParams.ДобавитьСекцию("секция_строк");
            foreach (var record in this.LogRecords)
            {
                sect.ДобавитьСтроку();

                sect["Id"] = record.Id;
                sect["Address"] = record.Address;
                sect["Ceo"] = record.Ceo;
                sect["Number"] = record.Number;
                sect["Sum"] = record.Sum.RoundDecimal(2);
                sect["PublishYear"] = record.PublishYear;
                sect["ChangePublishYear"] = record.ChangePublishYear;
                sect["Note"] = record.Note;
            }
        }
    }
}
