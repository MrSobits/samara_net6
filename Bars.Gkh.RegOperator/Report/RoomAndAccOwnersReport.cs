namespace Bars.Gkh.RegOperator.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Config;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.RegOperator.Entities;

    using Castle.Windsor;

    public class RoomAndAccOwnersReport : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        private List<long> municipalityIds;

        private List<TypeHouse> houseTypes;
        private List<ConditionHouse> houseCondition;

        public IGkhParams GkhParams { get; set; }

        public RoomAndAccOwnersReport()
            : base(new ReportTemplateBinary(Properties.Resources.RoomAndAccOwnersReport))
        {
        }

        public override string Name
        {
            get
            {
                return "Сводный отчет о помещениях и абонентах";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Сводный отчет о помещениях и абонентах";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Региональный фонд";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.RoomAndAccOwnersReport";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.GkhRegOp.RoomAndAccOwnersReport";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            var strMunicpalIds = baseParams.Params.GetAs<string>("municipalityIds");

            this.municipalityIds = !string.IsNullOrEmpty(strMunicpalIds)
                                       ? strMunicpalIds.Split(',').Select(x => x.ToLong()).ToList()
                                       : new List<long>();

            var houseTypesList = baseParams.Params.GetAs("houseTypes", string.Empty);
            this.houseTypes = !string.IsNullOrEmpty(houseTypesList)
                                  ? houseTypesList.Split(',').Select(id => (TypeHouse)id.ToInt()).ToList()
                                  : new List<TypeHouse>();

            var houseConditionList = baseParams.Params.GetAs("houseCondition", string.Empty);
            this.houseCondition = !string.IsNullOrEmpty(houseConditionList)
                                      ? houseConditionList.Split(',').Select(id => (ConditionHouse)id.ToInt()).ToList()
                                      : new List<ConditionHouse>();
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            reportParams.SimpleReportParams["ReportDate"] = DateTime.Now.ToShortDateString();

            var realObjDomain = this.Container.ResolveDomain<RealityObject>();
            var roomDomain = this.Container.ResolveDomain<Room>();
            var persAccDomain = this.Container.ResolveDomain<BasePersonalAccount>();
            var muDomain = this.Container.ResolveDomain<Municipality>();

            try
            {
                var roQuery = realObjDomain.GetAll()
                    .WhereIf(this.municipalityIds.Any(), x => this.municipalityIds.Contains(x.Municipality.Id))
                    .WhereIf(this.houseTypes.Any(), x => this.houseTypes.Contains(x.TypeHouse))
                    .WhereIf(this.houseCondition.Any(), x => this.houseCondition.Contains(x.ConditionHouse));

                var municipalities = muDomain.GetAll()
                    .WhereIf(this.municipalityIds.Any(), x => this.municipalityIds.Contains(x.Id))
                    .Select(x => new
                    {
                        x.Id,
                        x.Name
                    })
                    .OrderBy(x => x.Name)
                    .ToList();

                var roCountByMu = roQuery
                    .GroupBy(x => x.Municipality.Id)
                    .ToDictionary(x => x.Key, y => y.Count());

                var roomsCount = roomDomain.GetAll()
                    .Where(x => roQuery.Any(y => y.Id == x.RealityObject.Id))
                    .Select(x => new
                    {
                        MuId = x.RealityObject.Municipality.Id,
                        x.Area,
                        x.OwnershipType
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.MuId)
                    .ToDictionary(
                        x => x.Key, 
                        y => new
                        {
                            RoomCnt = y.Count(),
                            MuRoomCnt = y.Count(x => x.OwnershipType == RoomOwnershipType.Municipal),
                            PrivateRoomCnt = y.Count(x => x.OwnershipType == RoomOwnershipType.Private),
                            GovermentRoomCnt = y.Count(x => x.OwnershipType == RoomOwnershipType.Goverenment),
                            CommerseRoomCnt = y.Count(x => x.OwnershipType == RoomOwnershipType.Commerse),
                            MixedRoomCnt = y.Count(x => x.OwnershipType == RoomOwnershipType.Mixed),
                            NoSetRoomCnt = y.Count(x => x.OwnershipType == RoomOwnershipType.NotSet),
                            RoomArea = y.SafeSum(x => x.Area),
                            MuRoomArea = y.Where(x => x.OwnershipType == RoomOwnershipType.Municipal).SafeSum(x => x.Area),
                            PrivateRoomArea = y.Where(x => x.OwnershipType == RoomOwnershipType.Private).SafeSum(x => x.Area),
                            GovermentRoomArea = y.Where(x => x.OwnershipType == RoomOwnershipType.Goverenment).SafeSum(x => x.Area),
                            CommerseRoomArea = y.Where(x => x.OwnershipType == RoomOwnershipType.Commerse).SafeSum(x => x.Area),
                            MixedRoomArea = y.Where(x => x.OwnershipType == RoomOwnershipType.Mixed).SafeSum(x => x.Area),
                            NoSetRoomArea = y.Where(x => x.OwnershipType == RoomOwnershipType.NotSet).SafeSum(x => x.Area),
                        });

                var accCount = persAccDomain.GetAll()
                    .Where(x => roQuery.Any(y => y.Id == x.Room.RealityObject.Id))
                    .Select(x => new
                    {
                        x.State.StartState,
                        OwnId = x.AccountOwner.Id,
                        MuId = x.Room.RealityObject.Municipality.Id
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.MuId)
                    .ToDictionary(
                        x => x.Key, 
                        y => new
                        {
                            OwnCount = y.Select(x => x.OwnId).Distinct().Count(),
                            AccCnt = y.Count(x => x.StartState)
                        });

                var section = reportParams.ComplexReportParams.ДобавитьСекцию("section");

                var number = 1;
                var emptyRoomInfo = new
                {
                    RoomCnt = 0,
                    MuRoomCnt = 0,
                    PrivateRoomCnt = 0,
                    GovermentRoomCnt = 0,
                    CommerseRoomCnt = 0,
                    MixedRoomCnt = 0,
                    NoSetRoomCnt = 0,
                    RoomArea = 0M,
                    MuRoomArea = 0M,
                    PrivateRoomArea = 0M,
                    GovermentRoomArea = 0M,
                    CommerseRoomArea = 0M,
                    MixedRoomArea = 0M,
                    NoSetRoomArea = 0M,
                };

                var emptyAccInfo = new
                {
                    OwnCount = 0,
                    AccCnt = 0 
                };

                foreach (var municipality in municipalities)
                {
                    section.ДобавитьСтроку();
                    section["Number"] = number++;
                    section["Municipality"] = municipality.Name;
                    section["RoCnt"] = roCountByMu.Get(municipality.Id);

                    var roomInfo = roomsCount.Get(municipality.Id) ?? emptyRoomInfo;

                    section["RoomCnt"] = roomInfo.RoomCnt;
                    section["MuRoomCnt"] = roomInfo.MuRoomCnt;
                    section["PrivateRoomCnt"] = roomInfo.PrivateRoomCnt;
                    section["GovermentRoomCnt"] = roomInfo.GovermentRoomCnt;
                    section["CommerseRoomCnt"] = roomInfo.CommerseRoomCnt;
                    section["MixedRoomCnt"] = roomInfo.MixedRoomCnt;
                    section["NoSetRoomCnt"] = roomInfo.NoSetRoomCnt;
                    section["RoomArea"] = roomInfo.RoomArea;
                    section["MuRoomArea"] = roomInfo.MuRoomArea;
                    section["PrivateRoomArea"] = roomInfo.PrivateRoomArea;
                    section["GovermentRoomArea"] = roomInfo.GovermentRoomArea;
                    section["CommerseRoomArea"] = roomInfo.CommerseRoomArea;
                    section["MixedRoomArea"] = roomInfo.MixedRoomArea;
                    section["NoSetRoomArea"] = roomInfo.NoSetRoomArea;

                    var ownInfo = accCount.Get(municipality.Id) ?? emptyAccInfo;

                    section["OwnersCnt"] = ownInfo.OwnCount;
                    section["OpenPersAccCnt"] = ownInfo.AccCnt;
                }

                reportParams.SimpleReportParams["TotalRoCnt"] = roCountByMu.Values.SafeSum(x => x);
                reportParams.SimpleReportParams["TotalRoomCnt"] = roomsCount.Values.SafeSum(x => x.RoomCnt);
                reportParams.SimpleReportParams["TotalMuRoomCnt"] = roomsCount.Values.SafeSum(x => x.MuRoomCnt);
                reportParams.SimpleReportParams["TotalPrivateRoomCnt"] = roomsCount.Values.SafeSum(x => x.PrivateRoomCnt);
                reportParams.SimpleReportParams["TotalGovermentRoomCnt"] = roomsCount.Values.SafeSum(x => x.GovermentRoomCnt);
                reportParams.SimpleReportParams["TotalCommerseRoomCnt"] = roomsCount.Values.SafeSum(x => x.CommerseRoomCnt);
                reportParams.SimpleReportParams["TotalMixedRoomCnt"] = roomsCount.Values.SafeSum(x => x.MuRoomCnt);
                reportParams.SimpleReportParams["TotalNoSetRoomCnt"] = roomsCount.Values.SafeSum(x => x.NoSetRoomCnt);
                reportParams.SimpleReportParams["TotalRoomArea"] = roomsCount.Values.SafeSum(x => x.RoomArea);
                reportParams.SimpleReportParams["TotalMuRoomArea"] = roomsCount.Values.SafeSum(x => x.MuRoomArea);
                reportParams.SimpleReportParams["TotalPrivateRoomArea"] = roomsCount.Values.SafeSum(x => x.PrivateRoomArea);
                reportParams.SimpleReportParams["TotalGovermentRoomArea"] = roomsCount.Values.SafeSum(x => x.GovermentRoomArea);
                reportParams.SimpleReportParams["TotalCommerseRoomArea"] = roomsCount.Values.SafeSum(x => x.CommerseRoomArea);
                reportParams.SimpleReportParams["TotalMixedRoomArea"] = roomsCount.Values.SafeSum(x => x.MixedRoomArea);
                reportParams.SimpleReportParams["TotalNoSetRoomArea"] = roomsCount.Values.SafeSum(x => x.NoSetRoomArea);
                reportParams.SimpleReportParams["TotalOwnersCnt"] = accCount.Values.SafeSum(x => x.OwnCount);
                reportParams.SimpleReportParams["TotalOpenPersAccCnt"] = accCount.Values.SafeSum(x => x.AccCnt);
            }
            catch
            {
                this.Container.Release(realObjDomain);
                this.Container.Release(roomDomain);
                this.Container.Release(persAccDomain);
                this.Container.Release(muDomain);
            }
        }
    }
}