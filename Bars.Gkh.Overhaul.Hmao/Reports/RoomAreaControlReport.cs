namespace Bars.Gkh.Overhaul.Hmao.Reports
{
    using Bars.B4;
    using B4.Modules.Reports;

    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Castle.Windsor;
    using System.Linq;

    using Bars.B4.IoC;
    using Bars.Gkh.Overhaul.Hmao.ConfigSections;

    using Gkh.Utils;

    public class RoomAreaControlReport : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }
        public RoomAreaControlReport()
            : base(new ReportTemplateBinary(Properties.Resources.RoomAreaControl))
        {
        }

        /// <summary>
        /// Идентификаторы муниципальных образований
        /// </summary>
        private long[] municipalityIds;

        /// <summary>
        /// Идентификаторы состояний дома
        /// </summary>
        private long[] conditionHouseIds;

        /// <summary>
        /// Идентификаторы записей справочника Форма собственности
        /// </summary>
        private long[] typeOwnershipIds;

        /// <summary>
        /// Собирать по
        /// </summary>
        private DataSource collectBy;

        public override string Name => "Контроль площадей помещений";

        public override string Desciption => "Контроль площадей помещений";

        public override string GroupName => "Региональный фонд";

        public override string ParamsController => "B4.controller.report.RoomAreaControl";

        public override string RequiredPermission => "Reports.RF.RoomAreaControl";

        public override void SetUserParams(BaseParams baseParams)
        {
            this.municipalityIds = baseParams.Params.GetAs<long[]>("municipalityIds");
            this.conditionHouseIds = baseParams.Params.GetAs<long[]>("conditonHouseIds");
            this.typeOwnershipIds = baseParams.Params.GetAs<long[]>("typeOwnershipIds");
            this.collectBy = baseParams.Params.GetAs<DataSource>("collectBy");
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var roomService = this.Container.Resolve<IDomainService<Room>>();

            using (this.Container.Using(roomService))
            {
                var rooms = roomService.GetAll()
                    .GroupBy(x => x.RealityObject.Id)
                    .ToDictionary(x => x.Key, x => x.Sum(y => y.Area));

                var housesQuery = this.GetHousesData();

                var houses = housesQuery
                    .Select(x => new
                    {
                        x.Id,
                        MunicipalityName = x.Municipality != null ? x.Municipality.Name : string.Empty,
                        x.Address,
                        x.AreaLivingNotLivingMkd
                    }).ToList();

                var i = 1;
                var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");

                foreach (var house in houses)
                {
                    var areaMkd = house.AreaLivingNotLivingMkd.HasValue ? house.AreaLivingNotLivingMkd.Value : 0;
                    var roomsArea = rooms.ContainsKey(house.Id) ? rooms[house.Id] : 0;
                    section.ДобавитьСтроку();
                    section["Num"] = i;
                    section["Municipality"] = house.MunicipalityName;
                    section["Address"] = house.Address;
                    section["Area"] = areaMkd;
                    section["RoomArea"] = roomsArea;
                    section["Error"] = roomsArea != areaMkd ? "Нет" : "Да";
                    i++;
                }
            }
        }

        private IQueryable<RealityObject> GetHousesData()
        {
            var roService = this.Container.Resolve<IDomainService<RealityObject>>();
            var domainService = this.Container.Resolve<IDomainService<DpkrCorrectionStage2>>();
            var versionRecordDomain = this.Container.Resolve<IDomainService<VersionRecord>>();

            using (this.Container.Using(roService, domainService, versionRecordDomain))
            {
                var realityObjects = roService.GetAll();

                IQueryable<long> correctionRealityObjectsIds = null;

                if (this.collectBy == DataSource.Dpkr)
                {

                    var config = this.Container.GetGkhConfig<OverhaulHmaoConfig>();
                    var groupByRoPeriod = config.GroupByRoPeriod;
                    var periodEndYear = config.ProgrammPeriodEnd;

                    if (groupByRoPeriod == 0)
                    {
                        correctionRealityObjectsIds = domainService.GetAll()
                            .WhereIf(this.municipalityIds.IsNotEmpty(),
                                x =>
                                    x.Stage2.Stage3Version.ProgramVersion.IsMain &&
                                    this.municipalityIds.Contains(x.Stage2.Stage3Version.ProgramVersion.Municipality.Id))
                            .Where(x => x.Stage2.Stage3Version.IndexNumber > 0 || x.Stage2.Stage3Version.Year < periodEndYear)
                            .Select(x => x.RealityObject.Id).Distinct();
                    }
                    else
                    {
                        correctionRealityObjectsIds = versionRecordDomain.GetAll()
                            .WhereIf(this.municipalityIds.IsNotEmpty(),
                                x => x.ProgramVersion.IsMain && this.municipalityIds.Contains(x.ProgramVersion.Municipality.Id))
                            .Where(x => domainService.GetAll().Any(y => y.Stage2.Stage3Version.Id == x.Id))
                            .Select(x => x.RealityObject.Id).Distinct();
                    }
                }

                var query = realityObjects
                    .WhereIf(this.municipalityIds.IsNotEmpty(), x => this.municipalityIds.Contains(x.Municipality.Id))
                    .WhereIf(this.conditionHouseIds.IsNotEmpty(), x => this.conditionHouseIds.Contains((int) x.ConditionHouse))
                    .WhereIf(this.typeOwnershipIds.IsNotEmpty(), x => this.typeOwnershipIds.Contains(x.TypeOwnership.Id))
                    .WhereIf(correctionRealityObjectsIds != null && correctionRealityObjectsIds.Any(), x => correctionRealityObjectsIds.Contains(x.Id));

                return query;
            }
        }

        /// <summary>
        /// Собирать по
        /// </summary>
        /// <!-- При изменении значений проверить, что значения здесь и на панели отчета идентичны -->
        protected enum DataSource
        {
            /// <summary>
            /// Реестр жилых домов
            /// </summary>
            Reestr = 0,

            /// <summary>
            /// Долгосрочная программа
            /// </summary>
            Dpkr = 1
        }
    }
}