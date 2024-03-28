namespace Bars.Gkh.Overhaul.Hmao.Reports
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using B4.Modules.Reports;

    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Domain;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Enum;
    using Bars.Gkh.Overhaul.Hmao.ConfigSections;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.Gkh.Overhaul.Hmao.ProgrammPriorityParams;

    using Castle.Windsor;
    using Enums;
    using Gkh.Utils;

    internal class LongProgInfoByStructEl : BasePrintForm
    {
        public LongProgInfoByStructEl()
            : base(new ReportTemplateBinary(Properties.Resources.LongProgInfoByStructEl))
        {
        }

        private long[] municipalityIds;
        public IWindsorContainer Container { get; set; }

        public IDomainService<RealityObjectStructuralElementInProgramm> Stage1Domain { get; set; }

        public IDomainService<StructuralElementWork> StructElWorkDomain { get; set; }

        public IDomainService<WorkPrice> WorkPriceDomain { get; set; }

        public override string Name
        {
            get { return "Расчет долгосрочной программы КР"; }
        }

        public override string Desciption
        {
            get { return "Расчет долгосрочной программы КР"; }
        }

        public override string GroupName
        {
            get { return "Долгосрочная программа"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.LongProgInfoByStructEl"; }
        }

        public override string RequiredPermission
        {
            get { return "Reports.GkhOverhaul.LongProgInfoByStructEl"; }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            var municipalityIdsList = baseParams.Params.GetAs("municipalityIds", string.Empty);
            municipalityIds = !string.IsNullOrEmpty(municipalityIdsList)
                ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToArray()
                : new long[0];
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            var config = Container.GetGkhConfig<OverhaulHmaoConfig>();
            var startYear = config.ProgrammPeriodStart;

            var dictStructElWork = StructElWorkDomain.GetAll()
                    .Select(x => new
                    {
                        SeId = x.StructuralElement.Id,
                        JobId = x.Job.Id,
                        JobName = x.Job.Name
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.SeId)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.JobId).First());

            var dictPrices = WorkPriceDomain.GetAll()
                .WhereIf(municipalityIds.Any(), x => municipalityIds.Contains(x.Municipality.Id))
                .Where(x => x.Year == startYear)
                .GroupBy(x => x.Municipality.Id)
                .ToDictionary(x => x.Key, z => z.GroupBy(x => x.Job.Id).ToDictionary(x => x.Key, y => y.Select(x => new
                                                                                                                        {
                                                                                                                            x.SquareMeterCost,
                                                                                                                            x.NormativeCost
                                                                                                                        }).First()));

            var data = Stage1Domain.GetAll()
                .WhereIf(municipalityIds.Any(), x => municipalityIds.Contains(x.StructuralElement.RealityObject.Municipality.Id) ||
                    municipalityIds.Contains(x.StructuralElement.RealityObject.MoSettlement.Id))
                .Select(x => new
                {
                    MunicipalityId = x.StructuralElement.RealityObject.Municipality.Id,
                    Municipality = x.StructuralElement.RealityObject.Municipality.Name,
                    Settlement = x.StructuralElement.RealityObject.MoSettlement.Name,
                    RealityObject = x.StructuralElement.RealityObject.Address,
                    x.StructuralElement.RealityObject.BuildYear,
                    StructElName = x.StructuralElement.StructuralElement.Name,
                    StructElId = x.StructuralElement.StructuralElement.Id,
                    x.StructuralElement.LastOverhaulYear,
                    x.StructuralElement.Wearout,
                    x.StructuralElement.StructuralElement.LifeTime,
                    x.Stage2.Stage3.Point,
                    x.Year,
                    x.StructuralElement.StructuralElement.CalculateBy,
                    x.StructuralElement.Volume,
                    x.StructuralElement.RealityObject.AreaMkd,
                    x.StructuralElement.RealityObject.AreaLivingNotLivingMkd,
                    x.StructuralElement.RealityObject.AreaLiving,
                    x.Sum,
                    x.Stage2.Stage3.StoredCriteria,
                    x.Stage2.Stage3.IndexNumber
                })
                .OrderBy(x => x.Municipality)
                .ThenBy(x => x.Settlement)
                .ThenBy(x => x.Year)
                .ThenBy(x => x.IndexNumber)
                .AsEnumerable();

            var usedCriteries = data.GroupBy(x => x.MunicipalityId).Select(x => x.First()).SelectMany(x => x.StoredCriteria.Select(y => y.Criterion)).Distinct().ToList();

            CreateVertSections(reportParams, usedCriteries);

            var sectionRealObj = reportParams.ComplexReportParams.ДобавитьСекцию("sectionRealObj");

            foreach (var item in data)
            {
                sectionRealObj.ДобавитьСтроку();
                sectionRealObj["Number"] = item.IndexNumber;
                sectionRealObj["Municipality"] = item.Municipality;
                sectionRealObj["Settlement"] = item.Settlement;
                sectionRealObj["Address"] = item.RealityObject;
                sectionRealObj["BuildYear"] = item.BuildYear;
                sectionRealObj["StructEl"] = item.StructElName;
                sectionRealObj["LastOverhaulYear"] = item.LastOverhaulYear;
                sectionRealObj["Wear"] = item.Wearout;
                sectionRealObj["LifeTime"] = item.LifeTime;
                sectionRealObj["Point"] = item.Point;
                sectionRealObj["PlanYear"] = item.Year;
                sectionRealObj["CalcBy"] = item.CalculateBy.GetEnumMeta().Display;
                sectionRealObj["Volume"] = item.CalculateBy == PriceCalculateBy.Volume ? item.Volume :
                    item.CalculateBy == PriceCalculateBy.TotalArea ? item.AreaMkd :
                    item.CalculateBy == PriceCalculateBy.LivingArea ? item.AreaLiving:
                    item.CalculateBy == PriceCalculateBy.AreaLivingNotLivingMkd ? item.AreaLivingNotLivingMkd : 0M;
                sectionRealObj["Sum"] = item.Sum;

                var jobId = dictStructElWork.ContainsKey(item.StructElId) ? dictStructElWork[item.StructElId] : 0;
                var workPrice = dictPrices.ContainsKey(item.MunicipalityId) && dictPrices[item.MunicipalityId].ContainsKey(jobId) ?
                                                                          dictPrices[item.MunicipalityId][jobId] : null;

                if (workPrice != null)
                {
                    sectionRealObj["WorkPrice"] = item.CalculateBy == PriceCalculateBy.Volume
                                                      ? workPrice.NormativeCost
                                                      : workPrice.SquareMeterCost;
                }

                foreach (var criteia in item.StoredCriteria)
                {
                    sectionRealObj[criteia.Criterion] = criteia.Value;
                }
            }
        }

        private void CreateVertSections(ReportParams reportParams, List<string> usedCriterias)
        {
            var priorityParamsByCode = Container.ResolveAll<IProgrammPriorityParam>()
                .Select(x => new { x.Code, x.Name})
                .ToDictionary(x => x.Code, y => y.Name);

            var vertSection = reportParams.ComplexReportParams.ДобавитьСекцию("vertSection");

            foreach (var criteria in usedCriterias)
            {
                vertSection.ДобавитьСтроку();
                vertSection["CriteriaName"] = priorityParamsByCode.ContainsKey(criteria)
                                                  ? priorityParamsByCode[criteria]
                                                  : string.Empty;

                vertSection["CriteriaValue"] = string.Format("${0}$", criteria);
            }
        }
    }
}