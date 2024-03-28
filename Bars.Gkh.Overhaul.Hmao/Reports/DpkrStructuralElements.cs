namespace Bars.Gkh.Overhaul.Hmao.Reports
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.CommonEstateObject;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    using Castle.Windsor;

    public class DpkrStructuralElements : BasePrintForm
    {
        /// <inheritdoc />
        public override string Name => "Опубликованная программа (по конструктивным элементам)";

        /// <inheritdoc />
        public override string Desciption => "Опубликованная программа (по конструктивным элементам)";

        /// <inheritdoc />
        public override string GroupName => "Долгосрочная программа";

        /// <inheritdoc />
        public override string ParamsController => "B4.controller.report.DpkrStructuralElements";

        /// <inheritdoc />
        public override string RequiredPermission => "Reports.GkhOverhaul.DpkrStructuralElements";

        private readonly IWindsorContainer container;
        private List<long> municipalityIds;

        /// <inheritdoc />
        public DpkrStructuralElements(IWindsorContainer container)
            : base(new ReportTemplateBinary(Properties.Resources.DpkrStructuralElements))
        {
            this.container = container;
        }

        /// <inheritdoc />
        public override void SetUserParams(BaseParams baseParams)
        {
            var strMunicpalIds = baseParams.Params.GetAs<string>("municipalityIds");

            this.municipalityIds = !string.IsNullOrEmpty(strMunicpalIds)
                ? strMunicpalIds.Split(',').Select(x => x.ToLong()).ToList()
                : new List<long>();
        }

        /// <inheritdoc />
        public override void PrepareReport(ReportParams reportParams)
        {
            var versionRecordService = this.container.ResolveDomain<VersionRecord>();
            var realityObjectService = this.container.ResolveDomain<RealityObject>();
            var programVersionService = this.container.ResolveDomain<ProgramVersion>();
            var versionRecordStage2Service = this.container.ResolveDomain<VersionRecordStage2>();
            var publishedProgramRecordService = this.container.ResolveDomain<PublishedProgramRecord>();
            var versionRecordStage1Service = this.container.ResolveDomain<VersionRecordStage1>();
            var commonEstateObjectService = this.container.ResolveDomain<CommonEstateObject>();
            var realityObjectStructuralElementService = this.container.ResolveDomain<RealityObjectStructuralElement>();
            var structuralElementService = this.container.ResolveDomain<StructuralElement>();

            using (this.container.Using(versionRecordService, realityObjectService, programVersionService,
                versionRecordStage2Service, publishedProgramRecordService, versionRecordStage1Service
                ,commonEstateObjectService, realityObjectStructuralElementService, structuralElementService))
            {
                var data = versionRecordService.GetAll()
                .Join(realityObjectService.GetAll(), f => f.RealityObject, s => s, (versionRecord, realityObject) => new { versionRecord, realityObject })
                .Join(programVersionService.GetAll(),
                    f => f.versionRecord.ProgramVersion,
                    s => s,
                    (f, programVersion) => new { f.versionRecord, f.realityObject, programVersion })
                .Join(versionRecordStage2Service.GetAll(),
                    f => f.versionRecord,
                    s => s.Stage3Version,
                    (f, versionRecordStage2) => new { f.versionRecord, f.realityObject, f.programVersion, versionRecordStage2 })
                .Join(publishedProgramRecordService.GetAll(),
                    f => f.versionRecordStage2,
                    s => s.Stage2,
                    (f, publishedProgramRecord) => new
                        { f.versionRecord, f.realityObject, f.programVersion, f.versionRecordStage2, publishedProgramRecord })
                .Join(versionRecordStage1Service.GetAll(),
                    f => f.versionRecordStage2,
                    s => s.Stage2Version,
                    (f, versionRecordStage1) => new
                        { f.versionRecord, f.realityObject, f.programVersion, f.versionRecordStage2, f.publishedProgramRecord, versionRecordStage1 })
                .Join(commonEstateObjectService.GetAll(),
                    f => f.versionRecordStage2.CommonEstateObject,
                    s => s,
                    (f, commonEstateObject) => new
                    {
                        f.versionRecord, f.realityObject, f.programVersion, f.versionRecordStage2, f.publishedProgramRecord, f.versionRecordStage1,
                        commonEstateObject
                    })
                .Join(realityObjectStructuralElementService.GetAll(),
                    f => f.versionRecordStage1.StructuralElement,
                    s => s,
                    (f, realityObjectStructuralElement) => new
                    {
                        f.versionRecord, f.realityObject, f.programVersion, f.versionRecordStage2, f.publishedProgramRecord, f.versionRecordStage1,
                        f.commonEstateObject, realityObjectStructuralElement
                    })
                .Join(structuralElementService.GetAll(),
                    f => f.realityObjectStructuralElement.StructuralElement,
                    s => s,
                    (f, structuralElement) => new
                    {
                        f.versionRecord, f.realityObject, f.programVersion, f.versionRecordStage2, f.publishedProgramRecord, f.versionRecordStage1,
                        f.commonEstateObject, f.realityObjectStructuralElement, structuralElement
                    })
                .Where(w => w.programVersion.IsMain)
                .WhereIf(this.municipalityIds.Count > 0, w => this.municipalityIds.Contains(w.programVersion.Municipality.Id))
                .Select(s => new
                {
                    Municipality = s.realityObject.Municipality.Name,
                    Settlement = s.realityObject.MoSettlement.Name,
                    s.realityObject.Address,
                    CommonEstateObjectName = s.commonEstateObject.Name,
                    StructuralElementName = s.structuralElement.Name,
                    s.realityObjectStructuralElement.Volume,
                    s.versionRecordStage1.Sum,
                    s.realityObjectStructuralElement.LastOverhaulYear,
                    s.structuralElement.LifeTime,
                    s.versionRecord.Year,
                    s.publishedProgramRecord.PublishedYear
                })
                .ToList()
                .Select(s => new
                {
                    s.Municipality,
                    s.Settlement,
                    HouseAddress = s.Address,
                    ConstructElem = s.CommonEstateObjectName + '(' + s.StructuralElementName + ')',
                    s.Volume,
                    s.Sum,
                    s.LastOverhaulYear,
                    s.LifeTime,
                    PlanYear = s.Year,
                    s.PublishedYear
                })
                .OrderBy(t => t.HouseAddress)
                .ThenBy(t => t.PublishedYear)
                .ThenBy(t => t.ConstructElem);

                var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");

                foreach (var line in data)
                {
                    section.ДобавитьСтроку();

                    section["Municipality"] = line.Municipality;
                    section["Settlement"] = line.Settlement;
                    section["HouseAddress"] = line.HouseAddress;
                    section["ConstructElem"] = line.ConstructElem;
                    section["Volume"] = line.Volume;
                    section["Sum"] = line.Sum;
                    section["LastOverhaulYear"] = line.LastOverhaulYear;
                    section["LifeTime"] = line.LifeTime;
                    section["PlanYear"] = line.PlanYear;
                    section["PublishedYear"] = line.PublishedYear;
                }
            }
        }
    }
}