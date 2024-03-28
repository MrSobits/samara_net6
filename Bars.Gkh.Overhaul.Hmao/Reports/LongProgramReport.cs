using Bars.Gkh.Domain;

namespace Bars.Gkh.Overhaul.Hmao.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.Modules.Reports;
    using B4.Utils;
    using Bars.Gkh.Utils;
    using Castle.Windsor;
    using Entities;
    using Overhaul.Entities;

    public class LongProgramReport : BasePrintForm
    {
        public LongProgramReport() : base(new ReportTemplateBinary(Properties.Resources.LongProgramReport))
        {
        }

        public IWindsorContainer Container { get; set; }

        public IDomainService<VersionRecord> VersionRecordDomain { get; set; }

        public IDomainService<DpkrCorrectionStage2> DpkrCorrectionDomain { get; set; }

        public IDomainService<VersionRecordStage1> VersionStage1Domain { get; set; }

        public IDomainService<StructuralElementWork> StructElWorkDomain { get; set; }

        public override string Name
        {
            get { return "Региональная адресная программа кап.ремонта МКД"; }
        }

        public override string Desciption
        {
            get { return "Региональная адресная программа кап.ремонта МКД"; }
        }

        public override string GroupName
        {
            get { return "Региональная программа"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.LongProgramReport"; }
        }

        public override string RequiredPermission
        {
            get { return "Reports.GkhOverhaul.LongProgramReport"; }
        }

        private long[] municipalityIds = new long[0];

        private DateTime dateReport = DateTime.Now.Date;

        private int year = 0;

        public override void SetUserParams(BaseParams baseParams)
        {
            municipalityIds = baseParams.Params.GetAs<string>("muIds").ToLongArray();

            this.dateReport = baseParams.Params.GetAs<DateTime>("dateReport");

            if (this.dateReport == DateTime.MinValue)
            {
                dateReport = DateTime.Now;
            }

            year = baseParams.Params.GetAs("year", 0);
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            reportParams.SimpleReportParams["DateReport"] = dateReport.ToShortDateString();

            var dataCorrection =
                    DpkrCorrectionDomain.GetAll()
                        .Where(x => x.Stage2.Stage3Version.ProgramVersion.IsMain)
                        .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.Stage2.Stage3Version.ProgramVersion.Municipality.Id))
                        .WhereIf(year > 0,  x => x.PlanYear == year)
                        .Select(x => new { x.Stage2.Stage3Version.Id, x.PlanYear })
                        .AsEnumerable()
                        .GroupBy(x => x.Id)
                        .ToDictionary(x => x.Key, y => y.Select(x => x.PlanYear).FirstOrDefault());

            var data = VersionRecordDomain.GetAll()
                .Where(x => DpkrCorrectionDomain.GetAll().Any(y => y.Stage2.Stage3Version.Id == x.Id && (year == 0 || y.PlanYear == year)))
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .Where(x => x.ProgramVersion.IsMain)
                .Select(x => new
                {
                    x.Id,
                    Municipality = x.RealityObject.Municipality.Name,
                    RoId = x.RealityObject.Id,
                    x.RealityObject.Address,
                    x.RealityObject.AreaMkd,
                    x.RealityObject.DateCommissioning,
                    x.IndexNumber
                })
                .AsEnumerable()
                .Select(x => new
                 {
                     x.Id,
                     x.Municipality,
                     x.RoId,
                     x.Address,
                     x.AreaMkd,
                     x.DateCommissioning,
                     Year = dataCorrection.Get(x.Id),
                     x.IndexNumber
                 })
                .OrderBy(x => x.Year)
                .GroupBy(x => x.Municipality)
                .ToDictionary(x => x.Key, 
                              v => v.GroupBy(x => x.RoId).ToDictionary(x => x.Key, z => z.ToList()));

            var dataStage1 = VersionStage1Domain.GetAll()
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .Where(x => DpkrCorrectionDomain.GetAll().Any(y => y.Stage2.Id == x.Stage2Version.Id && (year == 0 || y.PlanYear == year)))
                .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.IsMain)
                .Select(x => new
                {
                    Stage3Id = x.Stage2Version.Stage3Version.Id,
                    x.Volume,
                    x.Sum,
                    x.SumService,
                    StructElemId = x.StructuralElement.StructuralElement.Id
                })
                .AsEnumerable()
                .GroupBy(x => x.Stage3Id)
                .ToDictionary(x => x.Key, y => new
                {
                    Volume = y.Sum(x => x.Volume),
                    Sum = y.Sum(x => x.Sum + x.SumService),
                    StructElems = y.Select(x => x.StructElemId).AsEnumerable()
                });

            var structElemWorks = StructElWorkDomain.GetAll()
                .Where(x => x.Job.Work.Name != null)
                .Select(x => new
                {
                    StructElemId = x.StructuralElement.Id,
                    WorkName = x.Job.Work.Name
                })
                .AsEnumerable()
                .GroupBy(x => x.StructElemId)
                .ToDictionary(x => x.Key, y => y.Select(x => x.WorkName));

            var sectionMu = reportParams.ComplexReportParams.ДобавитьСекцию("sectionMu");

            foreach (var muData in data.OrderBy(x => x.Key))
            {
                sectionMu.ДобавитьСтроку();
                sectionMu["Municipality"] = muData.Key;

                var sectionRealObj = sectionMu.ДобавитьСекцию("sectionRealObj");

                var sumMuVolume = 0M;
                var sumMuSum = 0M;
                foreach (var roData in muData.Value)
                {
                    sectionRealObj.ДобавитьСтроку();
                    sectionRealObj["Address"] = roData.Value.First().Address;

                    var sectionData = sectionRealObj.ДобавитьСекцию("sectionData");

                    var sumRoVolume = 0M;
                    var sumRoSum = 0M;
                    foreach (var item in roData.Value)
                    {
                        sectionData.ДобавитьСтроку();
                        sectionData["Address"] = item.Address;
                        sectionData["AreaMkd"] = item.AreaMkd.HasValue ? item.AreaMkd.Value : 0m;
                        sectionData["DateCommissioning"] = item.DateCommissioning.HasValue ? item.DateCommissioning.Value.ToShortDateString() : string.Empty;
                        sectionData["CorrectionYear"] = item.Year;
                        sectionData["IndexNumber"] = item.IndexNumber;

                        if (dataStage1.ContainsKey(item.Id))
                        {
                            sectionData["Volume"] = dataStage1[item.Id].Volume;
                            sectionData["Sum"] = dataStage1[item.Id].Sum;

                            sumRoVolume += dataStage1[item.Id].Volume;
                            sumRoSum += dataStage1[item.Id].Sum;

                            var listWorks = new List<string>();

                            foreach (var seId in dataStage1[item.Id].StructElems)
                            {
                                if (structElemWorks.ContainsKey(seId))
                                {
                                    listWorks.AddRange(structElemWorks[seId]);
                                }
                            }

                            sectionData["WorkName"] = listWorks.Distinct().AggregateWithSeparator(", ");
                        }
                    }

                    sectionRealObj["SumRoVolume"] = sumRoVolume;
                    sectionRealObj["SumRoSum"] = sumRoSum;
                    sumMuVolume += sumRoVolume;
                    sumMuSum += sumRoSum;
                }


                sectionMu["SumMuVolume"] = sumMuVolume;
                sectionMu["SumMuSum"] = sumMuSum;
            }

            reportParams.SimpleReportParams["TotalVolume"] = dataStage1.Values.Sum(x => x.Volume);
            reportParams.SimpleReportParams["TotalSum"] = dataStage1.Values.Sum(x => x.Sum);
        }
    }
}