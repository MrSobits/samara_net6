namespace Bars.GkhCr.Report
{
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.GkhCr.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Отчет " Ход выполнения работ в Мониторинге СМР "
    /// </summary>
    public class ArchiveSmrReport : BasePrintForm
    {
        // идентификатор программы КР
        private long programCrId;
        private long[] municipalityIds;

        public ArchiveSmrReport()
            : base(new ReportTemplateBinary(Properties.Resources.ArchiveSmrReport))
        {
        }

        public IWindsorContainer Container { get; set; }

        public IDomainService<ArchiveSmr> ArchiveSmrDomain { get; set; }

        public override string RequiredPermission
        {
            get { return "Reports.CR.ArchiveSmrReport"; }
        }

        public override string Desciption
        {
            get { return "Ход выполнения работ в Мониторинге СМР"; }
        }

        public override string GroupName
        {
            get { return "Исполнение КР"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.ArchiveSmrReport"; }
        }

        public override string Name
        {
            get { return "Ход выполнения работ в Мониторинге СМР"; }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            programCrId = baseParams.Params.GetAs<long>("programCrId");

            var municipalityIdsList = baseParams.Params.GetAs<string>("municipalityIds");

            this.municipalityIds = !string.IsNullOrEmpty(municipalityIdsList) ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToArray() : new long[0];
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {

            var data = ArchiveSmrDomain.GetAll()
                   .Where(x => x.TypeWorkCr.ObjectCr.ProgramCr.Id == programCrId)
                   .WhereIf(municipalityIds.Any(), x => municipalityIds.Contains(x.TypeWorkCr.ObjectCr.RealityObject.Municipality.Id))
                   .Select(x => new
                                     {
                                         MuName = x.TypeWorkCr.ObjectCr.RealityObject.Municipality.Name,
                                         x.TypeWorkCr.ObjectCr.RealityObject.Address,
                                         WorkName = x.TypeWorkCr.Work.Name,
                                         FinSourceName = x.TypeWorkCr.FinanceSource.Name,
                                         x.DateChangeRec,
                                         UnitMeasureName = x.TypeWorkCr.Work.UnitMeasure.Name,
                                         x.VolumeOfCompletion,
                                         x.PercentOfCompletion,
                                         x.CostSum,
                                         x.ManufacturerName
                                     })
                  .OrderBy(x => x.MuName)
                  .ThenBy(x => x.Address)
                  .ThenBy(x => x.Address)
                  .ThenBy(x => x.FinSourceName)
                  .ThenBy(x => x.WorkName)
                  .ThenBy(x => x.DateChangeRec)
                  .ToArray();

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");

            foreach (var item in data)
            {
                section.ДобавитьСтроку();

                section["Municipality"] = item.MuName;
                section["Address"] = item.Address;
                section["WorkName"] = item.WorkName;
                section["FinSourceName"] = item.FinSourceName;
                section["ChangeDate"] = item.DateChangeRec.HasValue ? item.DateChangeRec.Value.ToShortDateString() : string.Empty;
                section["Volume"] = item.VolumeOfCompletion;
                section["Percent"] = item.PercentOfCompletion;
                section["CostSum"] = item.CostSum;
                section["ManufacturerName"] = item.ManufacturerName;
                section["UnitMeasure"] = item.UnitMeasureName;
            }

        }
    }
}