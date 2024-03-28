namespace Bars.Gkh.Overhaul.Hmao.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.Modules.Reports;
    using B4.Utils;
    using Castle.Windsor;
    using DomainService.Version;
    using Entities.Version;

    public class ActualizeVersionLogReport : BasePrintForm
    {
        #region Dependency injection members
        public IWindsorContainer Container { get; set; }
        public IDomainService<VersionActualizeLog> VersionActualizeLogDomain { get; set; }

        #endregion

        public ActualizeVersionLogReport()
            : base(new ReportTemplateBinary(Properties.Resources.ActualizeVersionLogReport))
        {
        }

        public override string Name => "Лог актуализации версии ДПКР";

        public override string Desciption => "Лог актуализации версии ДПКР";

        public override string GroupName => "Региональная программа";

        public override string ParamsController => null;

        public override string RequiredPermission => null;

        private IEnumerable<ActualizeVersionLogRecord> LogRecords { get; set; }

        public override void SetUserParams(BaseParams baseParams)
        {
            this.LogRecords = baseParams.Params.GetAs<IEnumerable<ActualizeVersionLogRecord>>("Data");

            if (this.LogRecords == null)
            {
                throw new Exception("Нет строк для логирования");
            }
        }

        public override string ReportGenerator { get; set; }

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

                sect["TypeAction"] = record.TypeAction.GetEnumMeta().Display;
                sect["Action"] = record.Action;
                sect["Description"] = record.Description;
                sect["Address"] = record.Address;
                sect["Ceo"] = record.Ceo;
                sect["PlanYear"] = record.PlanYear;
                sect["Volume"] = record.Volume.RoundDecimal(2);
                sect["Sum"] = record.Sum.RoundDecimal(2);
                sect["Number"] = record.Number;
                sect["ChangeCeo"] = record.ChangeCeo;
                sect["ChangeNumber"] = record.ChangeNumber;
                sect["ChangePlanYear"] = record.ChangePlanYear;
                sect["ChangeVolume"] = record.ChangeVolume.RoundDecimal(2);
                sect["ChangeSum"] = record.ChangeSum.RoundDecimal(2);
            }
        }
    }
}