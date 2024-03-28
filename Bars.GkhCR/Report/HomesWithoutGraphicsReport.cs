namespace Bars.GkhCr.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhCr.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Отчет по домам, у которых отсутствуют графики
    /// </summary>
    public class HomesWithoutGraphicsReport : BasePrintForm
    {
        // идентификатор программы КР
        private long programCrId;
        private long[] municipalityIds;
        private long[] finSourceIds;
        private DateTime reportDate = DateTime.MinValue;
        public HomesWithoutGraphicsReport()
            : base(new ReportTemplateBinary(Properties.Resources.HomesWithoutGraphics))
        {
        }

        public IWindsorContainer Container { get; set; }

        public override string RequiredPermission
        {
            get { return "Reports.CR.HomesWithoutGraphics"; }
        }

        public override string Desciption
        {
            get { return "Отчет по домам, у которых отсутствуют графики"; }
        }

        public override string GroupName
        {
            get { return "Отчеты ГЖИ"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.HomesWithoutGraphics"; }
        }

        public override string Name
        {
            get { return "Отчет по домам, у которых отсутствуют графики"; }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            this.programCrId = baseParams.Params["programCrId"].ToInt();

            var dateReportPar = baseParams.Params["reportDate"].ToDateTime();

            var dateReport = dateReportPar == DateTime.Today.Date ? DateTime.Today.AddDays(1) : dateReportPar;

            var municipalityIdsList = baseParams.Params.GetAs("municipalityIds", string.Empty);
            municipalityIds = !string.IsNullOrEmpty(municipalityIdsList)
                                  ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToArray()
                                  : new long[0];

            var finSourceIdsList = baseParams.Params.ContainsKey("finSourceIds")
                      ? baseParams.Params["finSourceIds"].ToString()
                      : string.Empty;

            this.finSourceIds = !string.IsNullOrEmpty(finSourceIdsList) ? finSourceIdsList.Split(',').Select(id => id.ToLong()).ToArray() : new long[0];

            reportDate = baseParams.Params["reportDate"].ToDateTime();
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var serviceTypeWork = this.Container.Resolve<IDomainService<TypeWorkCr>>();
            var serviceMunicipality = this.Container.Resolve<IDomainService<Municipality>>();
            var serviceObjectCr = this.Container.Resolve<IDomainService<ObjectCr>>();

            var program = this.Container.Resolve<IDomainService<ProgramCr>>().Load(this.programCrId);
            reportParams.SimpleReportParams["Date"] = reportDate.ToShortDateString();
            reportParams.SimpleReportParams["ProgramCr"] = program != null ? program.Name : string.Empty;

            var muList = serviceMunicipality.GetAll()
                 .WhereIf(municipalityIds.Length > 0,x => this.municipalityIds.Contains(x.Id))
                 .Select(x => new { x.Id, x.Name, Group = x.Group ?? string.Empty })
                 .OrderBy(x => x.Group)
                 .ThenBy(x => x.Name)
                 .ToList();

            var muDictionary = muList.ToDictionary(x => x.Id, x => new { x.Name, x.Group });
            var alphabeticalGroups = new List<List<long>>();
            var lastGroup = "extraordinaryString";

            foreach (var municipality in muList)
            {
                if (municipality.Group != lastGroup)
                {
                    lastGroup = municipality.Group;
                    alphabeticalGroups.Add(new List<long>());
                }

                if (alphabeticalGroups.Any())
                {
                    alphabeticalGroups.Last().Add(municipality.Id);
                }
            }

            var realtyObjectsByMuIdDict =
                serviceObjectCr.GetAll()
                .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .Where(x => x.ProgramCr.Id == this.programCrId)
                .GroupBy(x => x.RealityObject.Municipality.Id)
                .Select(x => new { x.Key, Count = x.Count() })
                .ToDictionary(x => x.Key, x => x.Count);
            
            var workDataDict = serviceTypeWork.GetAll()
                    .WhereIf(this.municipalityIds.Length > 0, x => this.municipalityIds.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                    .Where(x => x.ObjectCr.ProgramCr.Id == this.programCrId)
                    .Where(x => x.Work.TypeWork == TypeWork.Work)
                    .Where(x => x.DateStartWork == null || x.DateEndWork == null)
                    .Select(x => new
                    {
                        roId = x.ObjectCr.RealityObject.Id,
                        muId = x.ObjectCr.RealityObject.Municipality.Id,
                        workCode = x.Work.Code
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.muId)
                    .ToDictionary(
                        x => x.Key,
                        x =>
                        x.GroupBy(y => y.roId)
                        .ToDictionary(
                            y => y.Key,
                            y => y.ToList()));

            var groupSection = reportParams.ComplexReportParams.ДобавитьСекцию("sectiongroup");
            var groupNameSection = groupSection.ДобавитьСекцию("sectiongroupname");
            var municipalitySection = groupSection.ДобавитьСекцию("sectionmunicipality");

            var totals = new int[26];
            
            foreach (var group in alphabeticalGroups)
            {
                var anyRowInGroup = group.Any(workDataDict.ContainsKey);

                if (!anyRowInGroup)
                {
                    continue;
                }

                groupSection.ДобавитьСтроку();

                var firstMu = muDictionary[group.First()];

                if (firstMu.Group != string.Empty)
                {
                    groupNameSection.ДобавитьСтроку();
                    groupNameSection["groupname"] = firstMu.Group;
                    groupNameSection["groupROCount"] = realtyObjectsByMuIdDict.Where(x => group.Contains(x.Key) && workDataDict.ContainsKey(x.Key)).Sum(x => x.Value);
                }

                foreach (var muId in group)
                {
                    if (workDataDict.ContainsKey(muId))
                    {
                        municipalitySection.ДобавитьСтроку();
                        municipalitySection["Municipality"] = muDictionary[muId].Name;
                        
                        if (realtyObjectsByMuIdDict.ContainsKey(muId))
                        {
                            municipalitySection["RoCount"] = realtyObjectsByMuIdDict[muId];
                            totals[0] += realtyObjectsByMuIdDict[muId];
                        }                    
                        
                        var worksByMu = workDataDict[muId].SelectMany(x => x.Value);

                        for (var i = 1; i <= 23; i++)
                        {
                            var countWorksByCurrentCode = worksByMu.Count(x => x.workCode == i.ToStr());
                            
                            municipalitySection[string.Format("param{0}", i)] =
                                countWorksByCurrentCode > 0 ? countWorksByCurrentCode.ToStr() : string.Empty;

                            totals[i] += countWorksByCurrentCode;
                        }

                        municipalitySection["worksCount"] = worksByMu.Count();
                        totals[24] += worksByMu.Count();
                        municipalitySection["AllWorksCount"] = workDataDict[muId].Keys.Count;
                        totals[25] += workDataDict[muId].Keys.Count;
                    }
                }
            }

            for (var i = 0; i < 26; i++)
            {
                reportParams.SimpleReportParams[string.Format("totalparam{0}", i)] = totals[i] > 0 ? totals[i].ToStr() : string.Empty;
            }
        }
    }
}