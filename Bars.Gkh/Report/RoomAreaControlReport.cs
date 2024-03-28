namespace Bars.Gkh.Report
{
    using System.Linq;

    using Bars.B4;

    using B4.Modules.Reports;

    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Properties;

    using Castle.Windsor;

    public class RoomAreaControlReport : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        public RoomAreaControlReport()
            : base(new ReportTemplateBinary(Resources.RoomAreaControl))
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

        public override string Name => "Контроль площадей помещений";

        public override string Desciption => "Контроль площадей помещений";

        public override string GroupName => "Региональный фонд";

        public override string ParamsController => "B4.controller.report.RoomAreaControl";

        public override string RequiredPermission => "Reports.RF.RoomAreaControl";

        public override void SetUserParams(BaseParams baseParams)
        {
            this.municipalityIds = baseParams.Params.GetAs<string>("municipalityIds").ToLongArray();
            this.conditionHouseIds = baseParams.Params.GetAs<string>("conditonHouseIds").ToLongArray();
            this.typeOwnershipIds = baseParams.Params.GetAs<string>("typeOwnershipIds").ToLongArray();
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
                    var areaMkd = house.AreaLivingNotLivingMkd ?? 0;
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
            using (this.Container.Using(roService))
            {
                var query = roService.GetAll()
                    .WhereIf(this.municipalityIds.IsNotEmpty(), x => this.municipalityIds.Contains(x.Municipality.Id))
                    .WhereIf(this.conditionHouseIds.IsNotEmpty(), x => this.conditionHouseIds.Contains((int) x.ConditionHouse))
                    .WhereIf(this.typeOwnershipIds.IsNotEmpty(), x => this.typeOwnershipIds.Contains(x.TypeOwnership.Id));
                return query;
            }
        }
    }
}