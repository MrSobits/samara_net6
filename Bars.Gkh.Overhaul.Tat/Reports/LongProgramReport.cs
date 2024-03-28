namespace Bars.Gkh.Overhaul.Tat.Reports
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
            get { return "Ovrhl.LongProgramReport"; }
        }

        private long[] municipalityIds = new long[0];

        private DateTime dateReport = DateTime.Now.Date;

        private int year = 0;

        public override void SetUserParams(BaseParams baseParams)
        {
            var muIds = baseParams.Params["muIds"].ToStr();

            if (string.IsNullOrEmpty(muIds))
            {
                this.municipalityIds = new long[0];
            }
            else
            {
                this.municipalityIds = muIds.Split(',').Select(x => x.ToLong()).ToArray();
            }

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

            var domainService = Container.Resolve<IDomainService<VersionRecord>>();

            var data = domainService.GetAll()
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .WhereIf(year > 0, x => x.CorrectYear == year)
                .Where(x => x.ProgramVersion.IsMain)
                .Select(x => new
                {
                    x.Id,
                    Municipality = x.RealityObject.Municipality.Name,
                    x.RealityObject.Address,
                    x.RealityObject.AreaMkd,
                    x.CorrectYear,
                    x.IndexNumber
                })
                .AsEnumerable()
                .GroupBy(x => x.Municipality)
                .ToDictionary(x => x.Key, v => v.Select(y => new { y.Id, y.Address, y.AreaMkd, y.CorrectYear, y.IndexNumber }).ToList());

            var dataStage1 = Container.Resolve<IDomainService<VersionRecordStage1>>().GetAll()
                .WhereIf(municipalityIds.Length > 0, x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.IsMain)
                .Select(x => new
                {
                    Stage3Id = x.Stage2Version.Stage3Version.Id,
                    x.Volume,
                    x.Sum,
                    x.SumService,
                    StructElemId = x.StrElement.Id
                })
                .AsEnumerable()
                .GroupBy(x => x.Stage3Id)
                .ToDictionary(x => x.Key, y => new
                {
                    Volume = y.Sum(x => x.Volume),
                    Sum = y.Sum(x => x.Sum + x.SumService),
                    StructElems = y.Select(x => x.StructElemId).AsEnumerable()
                });

            var structElemWorks = Container.Resolve<IDomainService<StructuralElementWork>>().GetAll()
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
            var sectionData = sectionMu.ДобавитьСекцию("sectionData");

            foreach (var record in data.OrderBy(x => x.Key))
            {
                sectionMu.ДобавитьСтроку();

                foreach (var item in record.Value.OrderBy(x => x.IndexNumber))
                {
                    sectionData.ДобавитьСтроку();
                    sectionData["Municipality"] = record.Key;
                    sectionData["Address"] = item.Address;
                    sectionData["AreaMkd"] = item.AreaMkd.HasValue ? item.AreaMkd.Value : 0m;
                    sectionData["CorrectionYear"] = item.CorrectYear;
                    sectionData["IndexNumber"] = item.IndexNumber;

                    if (dataStage1.ContainsKey(item.Id))
                    {
                        sectionData["Volume"] = dataStage1[item.Id].Volume;
                        sectionData["Sum"] = dataStage1[item.Id].Sum;

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

                sectionMu["Municipality"] = record.Key;

                var tempIds = record.Value.Select(x => x.Id).ToList();
                var tempList = dataStage1.Where(x => tempIds.Contains(x.Key)).Select(x => x.Value).ToList();

                sectionMu["SumMuVolume"] = tempList.Sum(x => x.Volume);
                sectionMu["SumMuSum"] = tempList.Sum(x => x.Sum);
            }

            reportParams.SimpleReportParams["TotalVolume"] = dataStage1.Values.Sum(x => x.Volume);
            reportParams.SimpleReportParams["TotalSum"] = dataStage1.Values.Sum(x => x.Sum);
        }
    }
}