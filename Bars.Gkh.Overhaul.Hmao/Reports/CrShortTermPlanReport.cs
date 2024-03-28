using Bars.B4.DataAccess;
using Bars.Gkh.Config;
using Bars.Gkh.Entities;
using Bars.Gkh.Enums;
using Bars.Gkh.Utils;

namespace Bars.Gkh.Overhaul.Hmao.Reports
{
    using System.Linq;
    using Bars.B4;
    using B4.Modules.Reports;

    using Bars.B4.Utils;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.Gkh.Overhaul.Hmao.Properties;
    using Castle.Windsor;

    class CrShortTermPlanReport : BasePrintForm
    {
        public CrShortTermPlanReport()
            : base(new ReportTemplateBinary(Resources.CrShortTermPlan))
        {
        }

        private long[] municipalityIds; 
        private int StartYear;  //Начало периода. Выбор года из периода ДПКР
        private int EndYear;    //Окончание периода. Выбор года из периода ДПКР

        public IWindsorContainer Container { get; set; }

        public override string Name
        {
            get { return "Краткосрочный план проведения капитальных ремонтов"; }
        }

        public override string Desciption
        {
            get { return "Краткосрочный план проведения капитальных ремонтов"; }
        }

        public override string GroupName
        {
            get { return "Региональная программа"; }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.CrShortTermPlan";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GkhOverhaul.CrShortTermPlanReport";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            var moIds = baseParams.Params.GetAs<long[]>("municipalityIds");
            this.StartYear = baseParams.Params.GetAs<int>("startYear");
            this.EndYear = baseParams.Params.GetAs<int>("endYear");

            var appParams = this.Container.Resolve<IGkhParams>().GetParams();
            var hasMoParam = appParams.ContainsKey("MoLevel") && !string.IsNullOrEmpty(appParams["MoLevel"].To<string>());
            var moLevel = hasMoParam
                ? appParams["MoLevel"].To<MoLevel>()
                : MoLevel.MunicipalUnion;

            municipalityIds = Container.Resolve<IRepository<Municipality>>().GetAll()
                .WhereIf(moIds.Any(), x => moIds.Contains(x.Id))
                .Select(x => new 
                {
                    x.Id,
                    x.Level
                })
                .AsEnumerable()
                .Where(x => !hasMoParam || x.Level.ToMoLevel(Container) == moLevel)
                .Select(x => x.Id)
                .ToArray();
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            // словарь цен по каждой работе
            var dictWorkPrice = this.Container.Resolve<IDomainService<WorkPrice>>().GetAll()
                .WhereIf(this.municipalityIds.Count() > 0, x => this.municipalityIds.Contains(x.Municipality.Id))
                .WhereIf(this.StartYear != 0, x => x.Year >= this.StartYear)
                .WhereIf(this.EndYear != 0, x => x.Year <= this.EndYear)
                .AsEnumerable()
                .GroupBy(x => x.Job.Id)
                .ToDictionary(x => x.Key, y => y.Select(z => z.NormativeCost).FirstOrDefault());

            // список объемов КЭ по дому
            var listRoSeVol = this.Container.Resolve<IDomainService<RealityObjectStructuralElement>>().GetAll()
                .WhereIf(this.municipalityIds.Count() > 0, x => this.municipalityIds.Contains(x.RealityObject.Municipality.Id))
                .Select(x => new 
                {
                    SeId = x.StructuralElement.Id, 
                    RoId = x.RealityObject.Id,
                    x.LastOverhaulYear,
                    x.Volume
                })
                .ToList();

            // словарь идентификатор оои - наименование и цены работ
            var dictWorks = this.Container.Resolve<IDomainService<StructuralElementWork>>().GetAll()
                    .Select(x => new
                    {
                        SeId = x.StructuralElement.Id,
                        WorkName = x.Job.Work.Name,
                        JobId = x.Job.Id
                    })
                    .AsEnumerable()
                    .Select(x => new
                    {
                        x.SeId,
                        x.WorkName,
                        WorkPrice = dictWorkPrice.ContainsKey(x.JobId) ? dictWorkPrice[x.JobId] : 0m
                    })
                    .GroupBy(x => x.SeId)
                    .ToDictionary(
                    x => x.Key, 
                    y => y.Select(z => new 
                    {
                        z.WorkName, 
                        z.WorkPrice
                    })
                    .FirstOrDefault());

            // соответствие этапа и вида работы
            var st1WorkDict = this.Container.Resolve<IDomainService<VersionRecordStage1>>().GetAll()
                .WhereIf(this.municipalityIds.Count() > 0, x => this.municipalityIds.Contains(x.Stage2Version.Stage3Version.ProgramVersion.Municipality.Id))
                .Select(x => new
                {
                    Stage2Id = x.Stage2Version.Id,
                    SeId = x.StructuralElement.StructuralElement.Id
                })
                .AsEnumerable()
                .Select(x => new
                {
                    x.Stage2Id,
                    x.SeId,
                    WorkName = dictWorks.ContainsKey(x.SeId) ? dictWorks[x.SeId].WorkName : string.Empty,
                    WorkPrice = dictWorks.ContainsKey(x.SeId) ? dictWorks[x.SeId].WorkPrice : 0
                })
                .GroupBy(x => x.Stage2Id)
                .ToDictionary(
                x => x.Key, 
                y => y.Select(z => new
                {
                    z.SeId, 
                    z.WorkName, 
                    z.WorkPrice
                })
                .FirstOrDefault());

            var data = this.Container.Resolve<IDomainService<PublishedProgramRecord>>().GetAll()
                .Where(x => x.Stage2 != null)
                .Join(
                    Container.Resolve<IDomainService<VersionRecord>>().GetAll(),
                    x => x.Stage2.Stage3Version.Id,
                    y => y.Id,
                    (a, b) => new { PublishedProgramRecord = a, VersionRecord = b })
                .Where(x => x.PublishedProgramRecord.PublishedProgram.ProgramVersion.IsMain)
                .Where(x => x.VersionRecord.ProgramVersion.IsMain)
                .WhereIf(this.municipalityIds.Count() > 0, x => this.municipalityIds.Contains(x.PublishedProgramRecord.PublishedProgram.ProgramVersion.Municipality.Id))
                .WhereIf(this.municipalityIds.Count() > 0, x => this.municipalityIds.Contains(x.VersionRecord.ProgramVersion.Municipality.Id))
                .WhereIf(this.StartYear != 0, x => x.PublishedProgramRecord.PublishedYear >= this.StartYear)
                .WhereIf(this.EndYear != 0, x => x.PublishedProgramRecord.PublishedYear <= this.EndYear)
                .Select(x => new
                {
                    MuName = x.VersionRecord.RealityObject.Municipality.Name,
                    roId = x.VersionRecord.RealityObject.Id,
                    x.VersionRecord.RealityObject.Address,
                    Locality = x.VersionRecord.RealityObject.FiasAddress.PlaceName,
                    Street = x.VersionRecord.RealityObject.FiasAddress.StreetName,
                    x.VersionRecord.RealityObject.FiasAddress.House,
                    x.VersionRecord.RealityObject.FiasAddress.Housing,
                    x.VersionRecord.RealityObject.BuildYear,
                    x.VersionRecord.RealityObject.AreaLiving,
                    CapitalGroupCode = x.VersionRecord.RealityObject.CapitalGroup.Code,
                    x.PublishedProgramRecord.CommonEstateobject,
                    Stage2Id = x.PublishedProgramRecord.Stage2.Id,
                    x.VersionRecord.Sum,
                    x.VersionRecord.IndexNumber
                })
                .AsEnumerable()
                .Select(x => new
                {
                    x.MuName,
                    x.Address,
                    x.Locality,
                    x.Street,
                    House = string.IsNullOrEmpty(x.Housing) ? x.House : x.House + x.Housing,
                    x.BuildYear,
                    x.AreaLiving,
                    x.CommonEstateobject,
                    x.CapitalGroupCode,
                    st1Info = st1WorkDict.ContainsKey(x.Stage2Id) ? st1WorkDict[x.Stage2Id] : null,
                    WorkName = st1WorkDict.ContainsKey(x.Stage2Id) ? st1WorkDict[x.Stage2Id].WorkName : string.Empty,
                    WorkPrice = st1WorkDict.ContainsKey(x.Stage2Id) ? st1WorkDict[x.Stage2Id].WorkPrice : 0,
                    SeId = st1WorkDict.ContainsKey(x.Stage2Id) ? st1WorkDict[x.Stage2Id].SeId : 0,
                    VolumeInfo = listRoSeVol.FirstOrDefault(y => y.RoId == x.roId && st1WorkDict.ContainsKey(x.Stage2Id) && y.SeId == st1WorkDict[x.Stage2Id].SeId),
                    x.Sum,
                })
                .OrderBy(x => x.MuName)
                .ThenBy(x => x.Address)
                .ToList();

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("Section");

            foreach (var item in data)
            {
                section.ДобавитьСтроку();

                section["muName"] = item.MuName;
                section["Settlement"] = item.Locality;
                section["Street"] = item.Street;
                section["House"] = item.House;
                section["BuildYear"] = item.BuildYear;
                section["AreaLiving"] = item.AreaLiving;
                section["ConstrElement"] = item.CommonEstateobject;
                section["WorkName"] = item.WorkName;
                section["CapitalGroup"] = item.CapitalGroupCode;
                var volume = item.VolumeInfo != null ? item.VolumeInfo.Volume : 0;
                section["Volume"] = volume;
                section["LastOverhaulYear"] = item.VolumeInfo != null ? item.VolumeInfo.LastOverhaulYear : 0;
                //section["TotalCost"] = (volume * item.WorkPrice).RoundDecimal(2);
                section["TotalCost"] = item.Sum;
                section["WorkPrice"] = item.WorkPrice;
            }
        }
    }
}
