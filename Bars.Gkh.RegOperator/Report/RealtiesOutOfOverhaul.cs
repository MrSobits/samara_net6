namespace Bars.Gkh.RegOperator.Report
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Domain;
    using Bars.Gkh.Overhaul.DomainService;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Properties;
    using Castle.Windsor;

    public class RealtiesOutOfOverhaul : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }
        private long[] _municipalityIds;

        public RealtiesOutOfOverhaul()
            : base(new ReportTemplateBinary(Resources.RealtiesOutOfOverhaul))
        {
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var realtyDomain = Container.Resolve<IDomainService<RealityObject>>();
            var municipalityDomain = Container.Resolve<IDomainService<Municipality>>();
            var roInProgramService = Container.Resolve<IRealityObjectsInPrograms>();
            var accountDomain = Container.Resolve<IDomainService<BasePersonalAccount>>();
            var dpkrParamService = Container.Resolve<IDpkrParamsService>();

            using (Container.Using(realtyDomain, municipalityDomain, roInProgramService, dpkrParamService))
            {
                var useRealObjCollection = dpkrParamService.Keys.Contains("UseRealObjCollection") &&
                                           dpkrParamService.GetParams()["UseRealObjCollection"].ToBool();

                var singleOwnerRoCountsByMo = accountDomain.GetAll()
                    .WhereIf(_municipalityIds.Any(),
                        x => _municipalityIds.Contains(x.Room.RealityObject.Municipality.Id))
                    .Where(x => x.Room.RealityObject.TypeHouse == TypeHouse.Individual)
                    .Select(x => new
                    {
                        MoId = x.Room.RealityObject.Municipality.Id,
                        RoId = x.Room.RealityObject.Id,
                        OwnerId = x.AccountOwner.Id
                    })
                    .ToList()
                    .GroupBy(x => x.MoId, x => new { x.OwnerId, x.RoId })
                    .ToDictionary(x => x.Key, x => x.GroupBy(y => y.RoId, y => y.OwnerId)
                        .ToDictionary(y => y.Key, y => y.Distinct().Count()).Where(y => y.Value == 1).Select(y => y.Key));

                var municipalities = municipalityDomain.GetAll()
                    .WhereIf(_municipalityIds.Any(), x => _municipalityIds.Contains(x.Id))
                    .OrderBy(x => x.Name);

                var mkdByMo = realtyDomain.GetAll()
                    .Where(x => x.TypeHouse == TypeHouse.ManyApartments)
                    .WhereIf(_municipalityIds.Any(), x => _municipalityIds.Contains(x.Municipality.Id))
                    .Select(x => new
                    {
                        Realty = x,
                        MoId = x.Municipality.Id
                    })
                    .GroupBy(x => x.MoId, x => x.Realty)
                    .ToDictionary(x => x.Key);

                var published = roInProgramService.GetInPublishedProgram()
                    .WhereIf(_municipalityIds.Any(), x => _municipalityIds.Contains(x.Municipality.Id))
                    .Select(x => new
                    {
                        RoId = x.Id,
                        MoId = x.Municipality.Id
                    })
                    .ToList()
                    .GroupBy(x => x.MoId)
                    .ToDictionary(x => x.Key, x => x.Distinct(y => y.RoId).Count());

                Dictionary<long, List<RealityObject>> maxCostExceedRealtiesByMo = null;

                if (useRealObjCollection)
                {
                    var maxCostExceedServices = Container.ResolveAll<IMaxCostExceededService>();
                    if (maxCostExceedServices.IsNotEmpty())
                    {
                        var service = maxCostExceedServices[0];
                        maxCostExceedRealtiesByMo = service.GetAll()
                            .WhereIf(_municipalityIds.Any(),
                                x => _municipalityIds.Contains(x.RealityObject.Municipality.Id))
                            .Select(x => new
                            {
                                Realty = x.RealityObject,
                                MoId = x.RealityObject.Municipality.Id
                            })
                            .AsEnumerable()
                            .GroupBy(x => x.MoId, x => x.Realty)
                            .ToDictionary(x => x.Key, x => x.Distinct(y => y.Id).ToList());
                    }
                }

                var i = 0;
                var sect = reportParams.ComplexReportParams.ДобавитьСекцию("section");

                var sumMkd = 0;
                var sumEmergen = 0;
                var sumSingleOwner = 0;
                var sumPublished = 0;
                var sumRoomLess3 = 0;
                var sumOld = 0;
                var sumOutOfLimit = 0;


                foreach (var mo in municipalities)
                {
                    sect.ДобавитьСтроку();
                    sect["Номер"] = ++i;
                    sect["МунОбр"] = mo.Name;
                    var mkdCount = mkdByMo.ContainsKey(mo.Id) ? mkdByMo[mo.Id].Count() : 0;
                    var publishedCount = 0;
                    publishedCount = published.ContainsKey(mo.Id) ? published[mo.Id] : 0;
                    sect["МКДОпубликованных"] = publishedCount;

                    var emergenHouses = mkdByMo.ContainsKey(mo.Id)
                        ? mkdByMo[mo.Id].Where(
                            x =>
                                x.ConditionHouse == ConditionHouse.Emergency ||
                                x.ConditionHouse == ConditionHouse.Razed).ToList()
                        : new List<RealityObject>();
                    var emergenHousesIds = emergenHouses.Select(x => x.Id).Distinct().ToList();
                    var emergenCount = emergenHouses.Count();

                    var singleOwnerRoIds = singleOwnerRoCountsByMo.ContainsKey(mo.Id)
                        ? singleOwnerRoCountsByMo[mo.Id].Where(x => !emergenHousesIds.Contains(x)).ToList()
                        : new List<long>();
                    var singleOwnerCount = singleOwnerRoIds.Count();

                    var roomsLess3Ids = mkdByMo.ContainsKey(mo.Id)
                        ? mkdByMo[mo.Id]
                            .Where(x => !emergenHousesIds.Contains(x.Id))
                            .Where(x => !singleOwnerRoIds.Contains(x.Id))
                            .Where(x => x.NumberApartments < 3)
                            .Select(x => x.Id).Distinct().ToList()
                        : new List<long>();

                    var roomsLess3Count = roomsLess3Ids.Count();

                    var oldRealties = mkdByMo.ContainsKey(mo.Id)
                        ? mkdByMo[mo.Id]
                            .Where(x => !emergenHousesIds.Contains(x.Id))
                            .Where(x => !singleOwnerRoIds.Contains(x.Id))
                            .Where(x => !roomsLess3Ids.Contains(x.Id))
                            .Where(x => x.PhysicalWear > 70)
                            .Select(x => x.Id).Distinct().ToList()
                        : new List<long>();

                    var oldRealtiesCount = oldRealties.Count();

                    var outOfLimitCount = 0;
                    if (maxCostExceedRealtiesByMo != null && maxCostExceedRealtiesByMo.ContainsKey(mo.Id))
                    {
                        outOfLimitCount = maxCostExceedRealtiesByMo[mo.Id]
                            .Where(x => !emergenHousesIds.Contains(x.Id))
                            .Where(x => !singleOwnerRoIds.Contains(x.Id))
                            .Where(x => !roomsLess3Ids.Contains(x.Id)).Count(x => !oldRealties.Contains(x.Id));
                    }

                    sect["МКДВсего"] = mkdCount;
                    sect["АварийныхСнесенных"] = emergenCount;
                    sect["ПомещенияОдногоСобственника"] = singleOwnerCount;
                    sect["КвартирМеньше3"] = roomsLess3Count;
                    sect["Изношенные"] = oldRealtiesCount;
                    sect["СтоимостьПревышаетПредел"] = outOfLimitCount;

                    sect["ВсегоВМо"] = emergenCount + publishedCount + roomsLess3Count + oldRealtiesCount +
                                       singleOwnerCount + outOfLimitCount;

                    sumMkd += mkdCount;
                    sumPublished += publishedCount;
                    sumEmergen += emergenCount;
                    sumSingleOwner += singleOwnerCount;
                    sumRoomLess3 += roomsLess3Count;
                    sumOld += oldRealtiesCount;
                    sumOutOfLimit += outOfLimitCount;
                }

                reportParams.SimpleReportParams["СумМКДВсего"] = sumMkd;
                reportParams.SimpleReportParams["СумМКДОпубликованных"] = sumPublished;
                reportParams.SimpleReportParams["СумВсегоВМо"] = sumOutOfLimit + sumEmergen + sumSingleOwner +
                                                                 sumRoomLess3 + sumOld + sumOutOfLimit;
                reportParams.SimpleReportParams["СумАварийныхСнесенных"] = sumEmergen;
                reportParams.SimpleReportParams["СумПомещенияОдногоСобственника"] = sumSingleOwner;
                reportParams.SimpleReportParams["СумКвартирМеньше3"] = sumRoomLess3;
                reportParams.SimpleReportParams["СумИзношенные"] = sumOld;
                reportParams.SimpleReportParams["СумСтоимостьПревышаетПредел"] = sumOutOfLimit;
            }
        }

        public override string Name
        {
            get { return "Дома, не включенные в ДПКР"; }
        }

        public override string Desciption
        {
            get { return "Дома, не включенные в ДПКР"; }
        }

        public override string GroupName
        {
            get { return "Долгосрочная программа"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.RealtiesOutOfOverhaul"; }
        }

        public override string RequiredPermission
        {
            get { return "Reports.GkhRegOp.RealtiesOutOfOverhaul"; }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            var municipalityIdsList = baseParams.Params.GetAs("municipalityIds", string.Empty);
            _municipalityIds = !string.IsNullOrEmpty(municipalityIdsList)
                ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToArray()
                : new long[0];
        }
    }
}