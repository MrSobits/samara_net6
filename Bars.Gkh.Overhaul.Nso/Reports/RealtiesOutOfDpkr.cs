namespace Bars.Gkh.Overhaul.Nso.Reports
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
    using Bars.Gkh.Overhaul.Nso.ConfigSections;
    using Bars.Gkh.Overhaul.Nso.Entities;
    using Bars.Gkh.Overhaul.Nso.Enum;
    using Bars.Gkh.Overhaul.Nso.Properties;

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

            using(Container.Using(realtyDomain, municipalityDomain, roStructElDomain, realEstRealObjService, dpkrCorrectionStage2Domain))
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

                var config = Container.GetGkhConfig<OverhaulNsoConfig>();
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

                    var appParams = Container.Resolve<IGkhParams>().GetParams();

                    var moLevel = appParams.ContainsKey("MoLevel") && !string.IsNullOrEmpty(appParams["MoLevel"].To<string>())
                        ? appParams["MoLevel"].To<MoLevel>()
                        : MoLevel.MunicipalUnion;

                    var reason = OutOfDpkrReason.None;
                    foreach(var mkd in mkds.Value)
                    {
                        var isOutOfDpkr = false;

                        if(!isOutOfDpkr && mkd.NumberApartments < minimumCountApartments)
                        {
                            isOutOfDpkr = true;
                            reason |= OutOfDpkrReason.NumberOfApartments;
                        }

                        if(!isOutOfDpkr && usePhysicalWearout == TypeUsage.Used &&
                           mkd.PhysicalWear.HasValue &&
                           physicalWear < mkd.PhysicalWear.Value)
                        {
                            isOutOfDpkr = true;
                            reason |= OutOfDpkrReason.PhysicalWearout;
                        }

                        if (!isOutOfDpkr && useLimitCost == TypeUsage.Used && maxCostExceedRealtiesByMo != null && maxCostExceedRealtiesByMo.ContainsKey(municipality.Id) && maxCostExceedRealtiesByMo[municipality.Id].Any(item => item.Id == mkd.Id))
                        {
                            isOutOfDpkr = true;
                            reason |= OutOfDpkrReason.MaxCost;
                        }

                        if(!isOutOfDpkr &&
                           typeUseWearMainCeo == TypeUseWearMainCeo.NotUsed)
                        {
                            continue;
                        }

                        var structEls = roStructElDomain.GetAll().Where(item => item.RealityObject.Id == mkd.Id && item.StructuralElement.Group.CommonEstateObject.IsMain).ToArray();
                        var wearoutEls = (from structEl in structEls
                            where wearMainCeo <= structEl.Wearout
                            select new Tuple<string, decimal>(structEl.Name, structEl.Wearout)).ToList();

                        if (!isOutOfDpkr && typeUseWearMainCeo == TypeUseWearMainCeo.OnlyOne &&
                           wearoutEls.Any())
                        {
                            isOutOfDpkr = true;
                            reason |= OutOfDpkrReason.StructElementsWearout;
                        }

                        if (!isOutOfDpkr && typeUseWearMainCeo == TypeUseWearMainCeo.AllCeo &&
                           wearoutEls.Count == structEls.Length)
                        {
                            isOutOfDpkr = true;
                            reason |= OutOfDpkrReason.StructElementsWearout;
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
                        sect["КолвоКвартир"] = ((reason & OutOfDpkrReason.NumberOfApartments) != 0) && mkd.NumberApartments.HasValue ? mkd.NumberApartments.ToString() : string.Empty;
                        sect["МинКолвоКвартир"] = ((reason & OutOfDpkrReason.NumberOfApartments) != 0) ? minimumCountApartments.ToString() : string.Empty;
                        sect["КЭМаксИзнос"] = ((reason & OutOfDpkrReason.StructElementsWearout) != 0) ? string.Join("\n", wearoutEls.Select(item => item.Item1 + " " + Math.Round(item.Item2, 2) + "%")): string.Empty;
                        sect["МинИзнос"] = ((reason & OutOfDpkrReason.StructElementsWearout) != 0) ? (typeUseWearMainCeo == 0 ? "Не используется" : "Используется") : string.Empty;
                        sect["ФизИзносКвартир"] = ((reason & OutOfDpkrReason.PhysicalWearout) != 0) && mkd.PhysicalWear.HasValue ? Math.Round(mkd.PhysicalWear.Value, 2).ToString() : string.Empty;
                        sect["МинФизИзнос"] = ((reason & OutOfDpkrReason.PhysicalWearout) != 0) ? Math.Round(physicalWear, 2).ToString() : string.Empty;

                        if((reason & OutOfDpkrReason.StructElementsWearout) != 0)
                        {
                            if (realObjServices.ContainsKey(municipality.Id))
                            {
                                var type = realObjServices[municipality.Id].FirstOrDefault();
                                if (type != null &&
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

                            if (useLimitCost == TypeUsage.Used)
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

                                sect["СредняяЦенаРаботПоДомуНаМетр"] = area.Value == 0 ? 0 : Math.Round(sum / area.Value, 2);
                            }
                            else
                            {
                                sect["СредняяЦенаРаботПоДомуНаМетр"] = 0;
                            }
                        }
                        else
                        {
                            sect["СредняяЦенаРаботПоДомуНаМетр"] = string.Empty;
                            sect["МаксЦенаРаботНаМетр"] = string.Empty;
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