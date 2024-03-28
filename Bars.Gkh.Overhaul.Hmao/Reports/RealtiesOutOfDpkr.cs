namespace Bars.Gkh.Overhaul.Hmao.Reports
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using B4.Modules.Reports;

    using Bars.B4.Utils;
    using Bars.Gkh.Config;
    using Bars.Gkh.ConfigSections.Overhaul.Enums;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Domain;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Hmao.ConfigSections;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.Gkh.Overhaul.Hmao.Properties;

    using Castle.Windsor;
    using Gkh.Entities.RealEstateType;
    using Gkh.Utils;

    public sealed class RealtiesOutOfDpkr : BasePrintForm
    {
        public IWindsorContainer Container { get; set; }

        private long[] _moIds;

        private long[] _mrIds;

        public RealtiesOutOfDpkr() : base(new ReportTemplateBinary(Resources.RealitiesOutOfDpkr))
        {
        }

        /// <summary>
        /// Выполнить сборку отчета
        /// </summary>
        /// <param name="reportParams">Параметры отчета
        ///             </param>
        public override void PrepareReport(ReportParams reportParams)
        {
            var realtyDomain = Container.ResolveDomain<RealityObject>();
            var municipalityDomain = Container.ResolveDomain<Municipality>();
            var roStructElDomain = Container.ResolveDomain<RealityObjectStructuralElement>();
            var realEstRealObjService = Container.ResolveDomain<RealEstateTypeRealityObject>();
            var dpkrCorrectionStage2Domain = Container.ResolveDomain<DpkrCorrectionStage2>();

            Dictionary<long, List<RealityObject>> maxCostExceedRealtiesByMo = null;

            using(this.Container.Using(realtyDomain, municipalityDomain, roStructElDomain, realEstRealObjService, dpkrCorrectionStage2Domain))
            {
                var municipalities = municipalityDomain.GetAll()
                    .WhereIf(!EnumerableExtension.IsEmpty(_moIds),
                    x => _moIds.Contains(x.Id)
                         || _moIds.Contains(x.Id))
                .WhereIf(!EnumerableExtension.IsEmpty(_mrIds),
                    x => _mrIds.Contains(x.Id)
                         || _mrIds.Contains(x.Id))
                    .OrderBy(x => x.Name).ToArray();

                var mkdByMo = realtyDomain.GetAll()
                    .Where(x => x.TypeHouse == TypeHouse.ManyApartments)
                    .WhereIf(!EnumerableExtension.IsEmpty(_moIds),
                    x => _moIds.Contains(x.Municipality.Id)
                         || _moIds.Contains(x.MoSettlement.Id))
                .WhereIf(!EnumerableExtension.IsEmpty(_mrIds),
                    x => _mrIds.Contains(x.Municipality.Id)
                         || _mrIds.Contains(x.MoSettlement.Id))
                    .Select(x => new
                    {
                        Realty = x,
                        MoId = x.Municipality.Id
                    })
                    .GroupBy(x => x.MoId, x => x.Realty)
                    .ToDictionary(x => x.Key);

                /*var singleOwnerRoCountsByMo = accountDomain.GetAll()
                    .WhereIf(!EnumerableExtension.IsEmpty(_moIds),
                    x => _moIds.Contains(x.Room.RealityObject.Municipality.Id)
                         || _moIds.Contains(x.Room.RealityObject.MoSettlement.Id))
                .WhereIf(!EnumerableExtension.IsEmpty(_mrIds),
                    x => _mrIds.Contains(x.Room.RealityObject.Municipality.Id)
                         || _mrIds.Contains(x.Room.RealityObject.MoSettlement.Id))
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
                        .ToDictionary(y => y.Key, y => y.Distinct().Count()).Where(y => y.Value == 1).Select(y => y.Key));*/

                var realObjServices = realEstRealObjService.GetAll()
                    .WhereIf(!EnumerableExtension.IsEmpty(_moIds),
                    x => _moIds.Contains(x.RealityObject.Municipality.Id)
                         || _moIds.Contains(x.RealityObject.MoSettlement.Id))
                .WhereIf(!EnumerableExtension.IsEmpty(_mrIds),
                    x => _mrIds.Contains(x.RealityObject.Municipality.Id)
                         || _mrIds.Contains(x.RealityObject.MoSettlement.Id))
                    .Select(
                        x => new
                        {
                            moId = x.RealityObject.Municipality.Id,
                            roId = x.RealityObject.Id,
                            maxPrice = x.RealEstateType.MarginalRepairCost
                        })
                    .ToList()
                    .GroupBy(x => x.moId, x => new {x.roId, x.maxPrice})
                    .ToDictionary(x => x.Key, x => x.ToArray());

                var i = 0;
                var sect = reportParams.ComplexReportParams.ДобавитьСекцию("section");

                var config = Container.GetGkhConfig<OverhaulHmaoConfig>();
                var usePhysicalWearout = config.HouseAddInProgramConfig.UsePhysicalWearout;
                var physicalWear = config.HouseAddInProgramConfig.PhysicalWear;
                var useLimitCost = config.HouseAddInProgramConfig.UseLimitCost;

                if (useLimitCost == TypeUsage.Used)
                {
                    var maxCostExceedServices = Container.ResolveAll<IMaxCostExceededService>();
                    if (maxCostExceedServices.IsNotEmpty())
                    {
                        var service = maxCostExceedServices[0];
                        maxCostExceedRealtiesByMo = service.GetAll()
                            .WhereIf(!EnumerableExtension.IsEmpty(_moIds),
                    x => _moIds.Contains(x.RealityObject.Municipality.Id)
                         || _moIds.Contains(x.RealityObject.MoSettlement.Id))
                .WhereIf(!EnumerableExtension.IsEmpty(_mrIds),
                    x => _mrIds.Contains(x.RealityObject.Municipality.Id)
                         || _mrIds.Contains(x.RealityObject.MoSettlement.Id))
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

                var typeUseWearMainCeo = config.HouseAddInProgramConfig.TypeUseWearMainCeo;
                var wearMainCeo = config.HouseAddInProgramConfig.WearMainCeo;
                var minimumCountApartments = config.HouseAddInProgramConfig.MinimumCountApartments;

                var rateCalcTypeArea = config.RateCalcTypeArea;

                foreach(var mkds in mkdByMo)
                {
                    var municipality = municipalities.FirstOrDefault(item => item.Id == mkds.Key);
                    if(municipality == null)
                    {
                        continue;
                    }

                    var emergenHousesIds = mkds.Value.Where(
                        x =>
                            x.ConditionHouse == ConditionHouse.Emergency ||
                            x.ConditionHouse == ConditionHouse.Razed).Select(x => x.Id).Distinct().ToList();

                    /*var singleOwnerRoIds = singleOwnerRoCountsByMo.ContainsKey(municipality.Id)
                        ? singleOwnerRoCountsByMo[municipality.Id].Where(x => !emergenHousesIds.Contains(x)).ToList()
                        : new List<long>();*/


                    var appParams = Container.Resolve<IGkhParams>().GetParams();

                    var moLevel = appParams.ContainsKey("MoLevel") && !string.IsNullOrEmpty(appParams["MoLevel"].To<string>())
                        ? appParams["MoLevel"].To<MoLevel>()
                        : MoLevel.MunicipalUnion;

                    foreach(var mkd in mkds.Value)
                    {
                        var isOutOfDpkr = false;
                        if(emergenHousesIds.Contains(mkd.Id))
                        {
                            isOutOfDpkr = true;
                        }

                        /*if(!isOutOfDpkr && singleOwnerRoIds.Contains(mkd.Id))
                        {
                            isOutOfDpkr = true;
                        }*/

                        if(!isOutOfDpkr && mkd.NumberApartments < minimumCountApartments)
                        {
                            isOutOfDpkr = true;
                        }

                        if(!isOutOfDpkr && usePhysicalWearout == TypeUsage.Used &&
                           mkd.PhysicalWear.HasValue &&
                           physicalWear < mkd.PhysicalWear.Value)
                        {
                            isOutOfDpkr = true;
                        }

                        if (!isOutOfDpkr && useLimitCost == TypeUsage.Used && maxCostExceedRealtiesByMo != null && maxCostExceedRealtiesByMo.ContainsKey(municipality.Id) && maxCostExceedRealtiesByMo[municipality.Id].Any(item => item.Id == mkd.Id))
                        {
                            isOutOfDpkr = true;
                        }

                        if(!isOutOfDpkr &&
                           typeUseWearMainCeo == TypeUseWearMainCeo.NotUsed)
                        {
                            continue;
                        }

                        var structEls = roStructElDomain.GetAll().Where(item => item.RealityObject.Id == mkd.Id).ToArray();
                        var wearoutEls = (from structEl in structEls
                            where wearMainCeo <= structEl.Wearout
                            select new Tuple<string, decimal>(structEl.Name, structEl.Wearout)).ToList();

                        if (!isOutOfDpkr && typeUseWearMainCeo == TypeUseWearMainCeo.OnlyOne &&
                           wearoutEls.Any())
                        {
                            isOutOfDpkr = true;
                        }

                        if (!isOutOfDpkr && typeUseWearMainCeo == TypeUseWearMainCeo.AllCeo &&
                           wearoutEls.Count == structEls.Length)
                        {
                            isOutOfDpkr = true;
                        }

                        if(!isOutOfDpkr)
                        {
                            continue;
                        }

                        sect.ДобавитьСтроку();

                        sect["Номер"] = ++i;
                        if(moLevel == MoLevel.MunicipalUnion)
                        {
                            sect["МунОбр"] = municipality.Name;
                        }
                        else
                        {
                            sect["МунОбр"] = mkd.MoSettlement.Name;
                        }

                        sect["Адрес"] = mkd.Address;
                        sect["КолвоКвартир"] = mkd.NumberApartments;
                        sect["МинКолвоКвартир"] = minimumCountApartments;
                        sect["КЭМаксИзнос"] = string.Join("\n", wearoutEls.Select(item => item.Item1 + " " + item.Item2 + "%"));
                        sect["МинИзнос"] = typeUseWearMainCeo == 0 ? "Не используется" : "Используется";
                        sect["ФизИзносКвартир"] = mkd.PhysicalWear;
                        sect["МинФизИзнос"] = physicalWear;

                        if(realObjServices.ContainsKey(municipality.Id))
                        {
                            var type = realObjServices[municipality.Id].FirstOrDefault();
                            if(type != null &&
                               type.maxPrice.HasValue)
                            {
                                sect["МаксЦенаРаботНаМетр"] = type.maxPrice.Value;
                            }
                            else
                            {
                                sect["МаксЦенаРаботНаМетр"] = 0;
                            }
                        }
                        else
                        {
                            sect["МаксЦенаРаботНаМетр"] = 0;
                        }

                        if(useLimitCost == TypeUsage.Used)
                        {
                            decimal? area;
                            switch (rateCalcTypeArea)
                            {
                                case RateCalcTypeArea.AreaLiving:
                                    area = mkd.AreaLiving;
                                    break;
                                case RateCalcTypeArea.AreaLivingNotLiving:
                                    area = mkd.AreaLivingNotLivingMkd;
                                    break;
                                default:
                                    area = mkd.AreaMkd;
                                    break;
                            }

                            var year = DateTime.Now.Year;
                            var works = dpkrCorrectionStage2Domain.GetAll()
                                .Where(item => item.RealityObject.Id == mkd.Id && item.PlanYear == year);

                            var sum = works.Any() ? works.Sum(item => item.Stage2.Sum) : 0;

                            sect["СредняяЦенаРаботПоДомуНаМетр"] = area.Value == 0 ? 0 : Math.Round(sum / area.Value, 4);
                        }
                        else
                        {
                            sect["СредняяЦенаРаботПоДомуНаМетр"] = 0;
                        }
                    }
                }
            }
        }

        public override string Name
        {
            get { return "Дома, не включенные в ДПКР (%)"; }
        }

        public override string Desciption
        {
            get { return "Дома, не включенные в ДПКР (%)"; }
        }

        public override string GroupName
        {
            get { return "Долгосрочная программа"; }
        }

        public override string ParamsController
        {
            get { return "B4.controller.report.RealtiesOutOfDpkr"; }
        }

        public override string RequiredPermission
        {
            get { return "Reports.GkhRegOp.RealtiesOutOfDpkr"; }
        }

        /// <summary>
        /// The set user params.
        /// </summary>
        /// <param name="baseParams">The base params.
        ///             </param>
        public override void SetUserParams(BaseParams baseParams)
        {
            _moIds = baseParams.Params.GetAs<string>("moIds").ToLongArray();
            _mrIds = baseParams.Params.GetAs<string>("mrIds").ToLongArray();
        }
    }
}