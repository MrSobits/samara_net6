using Bars.Gkh.Config;
using Bars.Gkh.Enums;

namespace Bars.Gkh.Overhaul.Hmao.Reports
{
    using System.Linq;

    using Bars.B4;
    using B4.Modules.Reports;

    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Hmao.ConfigSections;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Castle.Windsor;
    using Gkh.Utils;

    internal class PublishProgramByStructEl : BasePrintForm
    {
        public PublishProgramByStructEl()
            : base(new ReportTemplateBinary(Properties.Resources.PublishProgramByStructEl))
        {
        }

        private long[] municipalityIds;
        public IWindsorContainer Container { get; set; }

        public IDomainService<VersionRecordStage1> VersStage1Domain { get; set; }

        public IDomainService<PublishedProgramRecord> PublishedProgramRecordDomain { get; set; }

        public IDomainService<StructuralElementWork> StructElWorkDomain { get; set; }

        public IDomainService<WorkPrice> WorkPriceDomain { get; set; }

        public IGkhParams GkhParams { get; set; }

        public override string Name
        {
            get { return "Расчет опубликованной программы КР"; }
        }

        public override string Desciption
        {
            get { return "Расчет опубликованной программы КР"; }
        }

        public override string GroupName
        {
            get { return "Долгосрочная программа"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.PublishProgramByStructEl"; }
        }

        public override string RequiredPermission
        {
            get { return "Reports.GkhOverhaul.PublishProgramByStructEl"; }
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

            var appParams = GkhParams.GetParams();

            var moLevel = appParams.ContainsKey("MoLevel") && !string.IsNullOrEmpty(appParams["MoLevel"].To<string>())
                ? appParams["MoLevel"].To<MoLevel>()
                : MoLevel.MunicipalUnion;

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

            var dataPublished =
                    PublishedProgramRecordDomain.GetAll()
                        .WhereIf(municipalityIds.Any(), x => municipalityIds.Contains(x.PublishedProgram.ProgramVersion.Municipality.Id))
                        .Where(x => x.PublishedProgram.ProgramVersion.IsMain)
                        .Where(x => x.Stage2 != null)
                        .Select(x => new { x.Stage2.Stage3Version.Id, x.PublishedYear })
                        .AsEnumerable()
                        .GroupBy(x => x.Id)
                        .ToDictionary(x => x.Key, y => y.Select(x => x.PublishedYear).FirstOrDefault());

            var query = VersStage1Domain.GetAll()
                .Where(x => x.Stage2Version.Stage3Version.ProgramVersion.IsMain)
                .WhereIf(municipalityIds.Any(), x => municipalityIds.Contains(x.Stage2Version.Stage3Version.ProgramVersion.Municipality.Id))
                .Where(x => PublishedProgramRecordDomain.GetAll().Any(y => y.Stage2.Stage3Version.Id == x.Stage2Version.Stage3Version.Id))
                .Select(x => new
                {
                    x.Stage2Version.Stage3Version.Id,
                    MunicipalityId = x.StructuralElement.RealityObject.Municipality != null ?
                        x.StructuralElement.RealityObject.Municipality.Id : 0,
                    Municipality = x.StructuralElement.RealityObject.Municipality != null ?
                        x.StructuralElement.RealityObject.Municipality.Name : string.Empty,
                    SettlementId = x.StructuralElement.RealityObject.MoSettlement != null ? 
                        x.StructuralElement.RealityObject.MoSettlement.Id : 0,
                    Settlement = x.StructuralElement.RealityObject.MoSettlement != null ? 
                        x.StructuralElement.RealityObject.MoSettlement.Name : string.Empty,
                    RealityObject = x.StructuralElement.RealityObject.Address,
                    x.StructuralElement.RealityObject.BuildYear,
                    StructElName = x.StructuralElement.StructuralElement.Name,
                    StructElId = x.StructuralElement.StructuralElement.Id,
                    x.StructuralElement.LastOverhaulYear,
                    x.StructuralElement.Wearout,
                    x.StructuralElement.StructuralElement.LifeTime,
                    x.Year,
                    x.StructuralElement.StructuralElement.CalculateBy,
                    x.StructuralElement.Volume,
                    x.StructuralElement.RealityObject.AreaMkd,
                    x.StructuralElement.RealityObject.AreaLivingNotLivingMkd,
                    x.StructuralElement.RealityObject.AreaLiving,
                    x.Sum,
                    x.Stage2Version.Stage3Version.IndexNumber,
                    PublishedYear = 0
                })
                .AsEnumerable();

            var data = query.Select(x => new
            {
                x.MunicipalityId,
                x.Municipality,
                x.SettlementId,
                x.Settlement,
                x.RealityObject,
                x.BuildYear,
                x.StructElName,
                x.StructElId,
                x.LastOverhaulYear,
                x.Wearout,
                x.LifeTime,
                x.Year,
                x.CalculateBy,
                x.Volume,
                x.AreaMkd,
                x.AreaLivingNotLivingMkd,
                x.AreaLiving,
                x.Sum,
                x.IndexNumber,
                PublishedYear = dataPublished.ContainsKey(x.Id) ? dataPublished[x.Id] : 0
            })
                .OrderBy(x => x.Municipality)
                .ThenBy(x => x.Settlement)
                .ThenBy(x => x.Year)
                .ThenBy(x => x.IndexNumber);

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
                sectionRealObj["PlanYear"] = item.PublishedYear;
                sectionRealObj["CalcBy"] = item.CalculateBy.GetEnumMeta().Display;
                sectionRealObj["Volume"] = item.CalculateBy == PriceCalculateBy.Volume ? item.Volume :
                    item.CalculateBy == PriceCalculateBy.TotalArea ? item.AreaMkd :
                    item.CalculateBy == PriceCalculateBy.LivingArea ? item.AreaLiving:
                    item.CalculateBy == PriceCalculateBy.AreaLivingNotLivingMkd ? item.AreaLivingNotLivingMkd : 0M;
                sectionRealObj["Sum"] = item.Sum;

                var jobId = dictStructElWork.ContainsKey(item.StructElId) ? dictStructElWork[item.StructElId] : 0;

                var muId = moLevel == MoLevel.Settlement ? item.SettlementId : item.MunicipalityId;

                var workPrice = dictPrices.ContainsKey(muId) && dictPrices[muId].ContainsKey(jobId) ?
                                                                          dictPrices[muId][jobId] : null;

                if (workPrice != null)
                {
                    sectionRealObj["WorkPrice"] = item.CalculateBy == PriceCalculateBy.Volume
                                                      ? workPrice.NormativeCost
                                                      : workPrice.SquareMeterCost;
                }
            }
        }
    }
}