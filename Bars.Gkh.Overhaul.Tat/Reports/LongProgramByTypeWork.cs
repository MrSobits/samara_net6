namespace Bars.Gkh.Overhaul.Tat.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Bars.Gkh.Utils;
    using Castle.Windsor;
    using Overhaul.Entities;

    public class LongProgramByTypeWork : BasePrintForm
    {
        #region Dependency injection members

        public IWindsorContainer Container { get; set; }

        public IDomainService<StructuralElementWork> StrElWorksDomain { get; set; }

        public IDomainService<VersionRecordStage1> Stage1Domain { get; set; }

        public IDomainService<Municipality> MunicipalityDomain { get; set; }
        public IDomainService<RealityObject> RealityObjectDomain { get; set; }

        #endregion

        private List<long> municipalityIds;

        private DateTime dateTimeReport;

        private int startYear;

        private int endYear;

        private List<long> typeWorkIds;

        public LongProgramByTypeWork()
            : base(new ReportTemplateBinary(Properties.Resources.LongProgramByTypeWork))
        {

        }

        public override string Name
        {
            get
            {
                return "Долгосрочная  программа по видам работ";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Долгосрочная  программа по видам работ";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Долгосрочная программа";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.LongProgramByTypeWork";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GkhOverhaul.LongProgramByTypeWork";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            var strMunicpalIds = baseParams.Params.GetAs<string>("municipalityIds");

            municipalityIds = !string.IsNullOrEmpty(strMunicpalIds)
                ? strMunicpalIds.Split(',').Select(x => x.ToLong()).ToList()
                : new List<long>();

            var strTypeWorkIds = baseParams.Params.GetAs<string>("typeWorks");

            typeWorkIds = !string.IsNullOrEmpty(strTypeWorkIds)
                ? strTypeWorkIds.Split(',').Select(x => x.ToLong()).ToList()
                : new List<long>();

            var date = baseParams.Params.GetAs<DateTime?>("dateTimeReport");

            dateTimeReport = date.HasValue ? date.Value : DateTime.Now.Date;

            startYear = baseParams.Params.GetAs<int>("startYear");
            endYear = baseParams.Params.GetAs<int>("endYear");
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var municipalityDict = MunicipalityDomain.GetAll().ToDictionary(x => x.Id, y => y.Name);

            var structElsQuery = StrElWorksDomain.GetAll()
                                                 .WhereIf(typeWorkIds.Any(), x => typeWorkIds.Contains(x.Job.Work.Id));

            var jobs = structElsQuery
                    .Select(x => new 
                    {
                        WorkName = x.Job.Work.Name,
                        strElId = x.StructuralElement.Id
                    })
                    .ToList();


            var data =
                Stage1Domain.GetAll()
                    .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.IsMain)
                    .WhereIf(municipalityIds.Any(), x => municipalityIds.Contains(x.RealityObject.Municipality.Id))
                    .WhereIf(typeWorkIds.Any(), x => structElsQuery.Any(y => y.StructuralElement.Id == x.StrElement.Id))
                    .Select(
                        x =>
                            new 
                            {
                                x.Stage2Version.Stage3Version.CorrectYear,
                                strElId = x.StrElement.Id,
                                RoId = x.RealityObject.Id,
                                MuId = x.RealityObject.Municipality.Id,
                                MuName = x.RealityObject.Municipality.Name,
                                x.RealityObject.Address,
                                x.RealityObject.AreaMkd,
                                x.RealityObject.NumberLiving,
                                x.Sum,
                                x.SumService,
                                x.Volume
                            })
                    .AsEnumerable()
                    .Where(x => x.CorrectYear >= startYear && x.CorrectYear <= endYear)
                    .GroupBy(x => x.CorrectYear)
                    .OrderBy(x => x.Key)
                    .ToDictionary(x => x.Key, 
                                 y => y.OrderBy(x => x.MuName)
                                       .GroupBy(x => x.MuId)
                                       .ToDictionary(x => x.Key,
                                                   z => z.GroupBy(x => x.RoId)
                                                       .ToDictionary(x => x.Key)));

            reportParams.SimpleReportParams["ReportDate"] = dateTimeReport.ToShortDateString();
            var sectionPeriod = reportParams.ComplexReportParams.ДобавитьСекцию("sectionPeriod");


            var regionAreaMkd = 0m;
            var regionCitizenCnt = 0;
            foreach (var dataByYear in data)
            {
                sectionPeriod.ДобавитьСтроку();
                sectionPeriod["Year"] = dataByYear.Key;
                sectionPeriod["RealObjRegionCount"] = dataByYear.Value.Sum(x => x.Value.Count());
                sectionPeriod["RegionSum"] = dataByYear.Value.Sum(x => x.Value.Sum(z => z.Value.Sum(y => y.Sum + y.SumService)));
                sectionPeriod["RegionVolume"] = dataByYear.Value.Sum(x => x.Value.Sum(z => z.Value.Sum(y => y.Volume)));

                var sectionMu = sectionPeriod.ДобавитьСекцию("sectionMu");
                foreach (var dataByMunicipality in dataByYear.Value)
                {
                    sectionMu.ДобавитьСтроку();
                    sectionMu["Municipality"] = municipalityDict.ContainsKey(dataByMunicipality.Key)
                                                    ? municipalityDict[dataByMunicipality.Key]
                                                    : string.Empty;
                    sectionMu["RealObjMuCount"] = dataByMunicipality.Value.Count;
                    sectionMu["MunicipalitySum"] = dataByMunicipality.Value.Sum(x => x.Value.Sum(y => y.Sum + y.SumService));
                    sectionMu["MunicipalityVolume"] = dataByMunicipality.Value.Sum(x => x.Value.Sum(y => y.Volume));

                    var sectionRealObj = sectionMu.ДобавитьСекцию("sectionRealObj");
                    var muWorksCount = 0;
                    var muAreaMkd = 0m;
                    var muCitizenCnt = 0;
                    foreach (var dataByRealObj in dataByMunicipality.Value)
                    {
                        sectionRealObj.ДобавитьСтроку();
                        var realObj = dataByRealObj.Value.First();
                        var works = jobs.Where(x => dataByRealObj.Value.Any(y => y.strElId == x.strElId)).Select(x => x.WorkName).Distinct().ToArray();
                        sectionRealObj["Address"] = realObj.Address;
                        sectionRealObj["Sum"] = dataByRealObj.Value.Sum(x => x.Sum + x.SumService);
                        sectionRealObj["Volume"] = dataByRealObj.Value.Sum(x => x.Volume);
                        sectionRealObj["TypeWorks"] = works.Any() ? works.AggregateWithSeparator(", ") : string.Empty;
                        sectionRealObj["WorkCount"] = works.Count();
                        sectionRealObj["AreaMkd"] = realObj.AreaMkd;
                        sectionRealObj["CitizensCnt"] = realObj.NumberLiving;
                        muWorksCount += works.Count();
                        muAreaMkd += realObj.AreaMkd.ToDecimal();
                        muCitizenCnt += realObj.NumberLiving.ToInt();
                    }

                    sectionMu["MunicipalityWorkCount"] = muWorksCount;
                    sectionMu["MunicipalityAreaMkd"] = muAreaMkd;
                    sectionMu["MunicipalityCitizensCnt"] = muCitizenCnt;
                    regionAreaMkd += muAreaMkd;
                    regionCitizenCnt += muCitizenCnt;
                }

                sectionPeriod["RegionAreaMkd"] = regionAreaMkd;
                sectionPeriod["RegionCitizensCnt"] = regionCitizenCnt;
            }
        }
    }
}