namespace Bars.Gkh.Regions.Tatarstan.DomainService.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Regions.Tatarstan.Entities.NormConsumption;

    using Castle.Windsor;

    public class NormConsumptionService : INormConsumptionService
    {
        public IWindsorContainer Container { get; set; }

        private const string GvsCode = "ГВС";

        private const string HvsCode = "ХВС";

        private const string FiringCode = "Теплоснабжение";

        public IQueryable<NormConsumptionHotWaterProxy> GetNormConsumptionHotWaterQuery(BaseParams baseParams, out int totalCount)
        {
            var loadParams = baseParams.GetLoadParam();

            var normConsId = loadParams.Filter.GetAs<long>("normConsId");

            var hotWaterDomain = this.Container.Resolve<IDomainService<NormConsumptionHotWater>>();
            var normConsDomain = this.Container.ResolveDomain<NormConsumption>();
            var tehPassportValueDomain = this.Container.Resolve<IDomainService<TehPassportValue>>();
            var roDomain = this.Container.ResolveDomain<RealityObject>();
            var roStructElService = this.Container.Resolve<IRealObjectStructElementService>();

            try
            {
                var roQuery = this.GetRoQuery(loadParams, normConsId, out totalCount);

                var normsConsumptionDict = hotWaterDomain.GetAll()
                    .Where(x => x.NormConsumption.Id == normConsId)
                    .Where(x => roQuery.Any(y => y.Id == x.RealityObject.Id))
                    .ToDictionary(x => x.RealityObject.Id, y => y);

                var tpValues = tehPassportValueDomain.GetAll()
                    .Where(x => roQuery.Any(y => y.Id == x.TehPassport.RealityObject.Id))
                    .AsEnumerable()
                    .GroupBy(x => x.TehPassport.RealityObject.Id)
                    .ToDictionary(
                        x => x.Key,
                        y =>
                        {
                            var hvs = y.FirstOrDefault(x => x.FormCode == "Form_3_2_CW" && x.CellCode == "1:3")?.Value == "1";

                            var wastewater = y.FirstOrDefault(x => x.FormCode == "Form_3_3_Water" && x.CellCode == "1:3")?.Value == "1";

                            var gvs = y.FirstOrDefault(x => x.FormCode == "Form_3_3_Water" && x.CellCode == "1:3")?.Value == "1" ||
                                y.FirstOrDefault(x => x.FormCode == "Form_3_3_Water" && x.CellCode == "1:3")?.Value == "2";

                            var floor = y.Any(x => x.TehPassport.RealityObject.Floors >= 13);

                            return new
                            {
                                MetersInstalled = this.GetMeteringInstalledValue(y.FirstOrDefault(x => x.FormCode == "Form_6_6_2" && x.CellCode == "5:2")),
                                TypeSystemHotWater = this.GetTypeHotWater(y.FirstOrDefault(x => x.FormCode == "Form_3_2" && x.CellCode == "1:3")?.Value),
                                Gvs12Floor = (hvs && wastewater && gvs && floor) ? YesNo.Yes : YesNo.No
                            };
                        });

                var gvsDict = roStructElService.GetRealityObjectWearoutDictionary(roQuery, NormConsumptionService.GvsCode);

                var result = roQuery
                    .AsEnumerable()
                    .Select(x => new
                    {
                        RealityObject = x,
                        NormsConsumption = normsConsumptionDict.Get(x.Id),
                        TpValue = tpValues.Get(x.Id),
                        Wearout = gvsDict.Get(x.Id)
                    })
                    .Select(
                    x => new NormConsumptionHotWaterProxy
                    {
                        ObjectId = x.NormsConsumption?.Id ?? 0,
                        Id = x.RealityObject.Id,
                        RealityObject = x.RealityObject,
                        Municipality = x.RealityObject.Municipality.Name,
                        Address = x.RealityObject.Address,
                        FloorNumber = x.RealityObject.MaximumFloors,
                        MetersInstalled = x.TpValue?.MetersInstalled ?? 0,
                        BuildYear = x.RealityObject.BuildYear,
                        AreaHouse = x.RealityObject.AreaMkd,
                        AreaLivingRooms = x.RealityObject.AreaLiving,
                        AreaNotLivingRooms = x.RealityObject.AreaNotLivingPremises,
                        AreaOtherRooms = x.RealityObject.AreaCommonUsage,
                        IsIpuNotLivingPermises = x.NormsConsumption?.IsIpuNotLivingPermises ?? 0,
                        AreaIpuNotLivingPermises = x.NormsConsumption?.AreaIpuNotLivingPermises,
                        VolumeHotWaterNotLivingIsIpu = x.NormsConsumption?.VolumeHotWaterNotLivingIsIpu,
                        VolumeWaterOpuOnPeriod = x.NormsConsumption?.VolumeWaterOpuOnPeriod,
                        HeatingPeriod = x.NormsConsumption?.HeatingPeriod,
                        TypeSystemHotWater = x.TpValue?.TypeSystemHotWater,
                        ResidentsNumber = x.RealityObject.NumberLiving,
                        DepreciationIntrahouseUtilities = x.Wearout, 
                        OverhaulDate = x.RealityObject.DateLastOverhaul,
                        IsBath1200 = x.NormsConsumption?.IsBath1200 ?? 0,
                        IsBath1500With1550 = x.NormsConsumption?.IsBath1500With1550 ?? 0,
                        IsBath1650With1700 = x.NormsConsumption?.IsBath1650With1700 ?? 0,
                        IsBathNotShower = x.NormsConsumption?.IsBathNotShower ?? 0,
                        IsShower = x.NormsConsumption?.IsShower ?? 0,
                        SharedShowerInHostel = x.NormsConsumption?.SharedShowerInHostel ?? 0,
                        IsHostelShowerAllLivPermises = x.NormsConsumption?.IsHostelShowerAllLivPermises ?? 0,
                        ShowerInHostelInSection = x.NormsConsumption?.ShowerInHostelInSection ?? 0,
                        Gvs12Floor = x.TpValue?.Gvs12Floor ?? 0
                    })
                    .AsQueryable();

                return result;
            }
            finally
            {
                this.Container.Release(hotWaterDomain);
                this.Container.Release(normConsDomain);
                this.Container.Release(roDomain);
                this.Container.Release(tehPassportValueDomain);
                this.Container.Release(roStructElService);
            }
        }

        

        public IQueryable<NormConsumptionColdWaterProxy> GetNormConsumptionColdWaterQuery(BaseParams baseParams, out int totalCount)
        {
            var loadParams = baseParams.GetLoadParam();

            var normConsId = loadParams.Filter.GetAs<long>("normConsId");

            var coldWaterDomain = this.Container.Resolve<IDomainService<NormConsumptionColdWater>>();
            var tehPassportValueDomain = this.Container.Resolve<IDomainService<TehPassportValue>>();
            var roStructElService = this.Container.Resolve<IRealObjectStructElementService>();

            try
            {
                var roQuery = this.GetRoQuery(loadParams, normConsId, out totalCount);

                var normsConsumptionDict = coldWaterDomain.GetAll()
                    .Where(x => x.NormConsumption.Id == normConsId)
                    .Where(x => roQuery.Any(y => y.Id == x.RealityObject.Id))
                    .ToDictionary(x => x.RealityObject.Id, y => y);

                var tpValues = tehPassportValueDomain.GetAll()
                    .Where(x => roQuery.Any(y => y.Id == x.TehPassport.RealityObject.Id))
                    .AsEnumerable()
                    .GroupBy(x => x.TehPassport.RealityObject.Id)
                    .ToDictionary(
                        x => x.Key,
                        y =>
                        {
                            var hvs = y.FirstOrDefault(x => x.FormCode == "Form_3_2_CW" && x.CellCode == "1:3")?.Value == "1";

                            var wastewater = y.FirstOrDefault(x => x.FormCode == "Form_3_3_Water" && x.CellCode == "1:3")?.Value == "1";

                            var gvs = y.FirstOrDefault(x => x.FormCode == "Form_3_3_Water" && x.CellCode == "1:3")?.Value == "1" ||
                                y.FirstOrDefault(x => x.FormCode == "Form_3_3_Water" && x.CellCode == "1:3")?.Value == "2";

                            var floor = y.Any(x => x.TehPassport.RealityObject.Floors >= 13);

                            return new
                            {
                                MetersInstalled = this.GetMeteringInstalledValue(y.FirstOrDefault(x => x.FormCode == "Form_6_6_2" && x.CellCode == "6:2")),
                                TypeSystemHotWater = this.GetTypeHotWater(y.FirstOrDefault(x => x.FormCode == "Form_3_2" && x.CellCode == "1:3")?.Value),
                                Gvs12Floor = (hvs && wastewater && gvs && floor) ? YesNo.Yes : YesNo.No
                            };
                        });

                var gvsDict = roStructElService.GetRealityObjectWearoutDictionary(roQuery, NormConsumptionService.HvsCode);

                var result = roQuery
                    .AsEnumerable()
                    .Select(x => new
                    {
                        RealityObject = x,
                        NormsConsumption = normsConsumptionDict.Get(x.Id),
                        TpValue = tpValues.Get(x.Id),
                        Wearout = gvsDict.Get(x.Id)
                    })
                    .Select(
                    x => new NormConsumptionColdWaterProxy
                    {
                        ObjectId = x.NormsConsumption?.Id ?? 0,
                        Id = x.RealityObject.Id,
                        RealityObject = x.RealityObject,
                        Municipality = x.RealityObject.Municipality.Name,
                        Address = x.RealityObject.Address,
                        FloorNumber = x.RealityObject.MaximumFloors,
                        MetersInstalled = x.TpValue?.MetersInstalled ?? 0,
                        BuildYear = x.RealityObject.BuildYear,
                        AreaHouse = x.RealityObject.AreaMkd,
                        AreaLivingRooms = x.RealityObject.AreaLiving,
                        AreaNotLivingRooms = x.RealityObject.AreaNotLivingPremises,
                        AreaOtherRooms = x.RealityObject.AreaCommonUsage,
                        IsIpuNotLivingPermises = x.NormsConsumption?.IsIpuNotLivingPermises ?? 0,
                        AreaIpuNotLivingPermises = x.NormsConsumption?.AreaIpuNotLivingPermises,
                        VolumeColdWaterNotLivingIsIpu = x.NormsConsumption?.VolumeColdWaterNotLivingIsIpu,
                        VolumeWaterOpuOnPeriod = x.NormsConsumption?.VolumeWaterOpuOnPeriod,
                        HeatingPeriod = x.NormsConsumption?.HeatingPeriod,
                        TypeSystemHotWater = x.TpValue?.TypeSystemHotWater,
                        ResidentsNumber = x.RealityObject.NumberLiving,
                        DepreciationIntrahouseUtilities = x.Wearout,
                        OverhaulDate = x.RealityObject.DateLastOverhaul,
                        IsBath1200 = x.NormsConsumption?.IsBath1200 ?? 0,
                        IsBath1500With1550 = x.NormsConsumption?.IsBath1500With1550 ?? 0,
                        IsBath1650With1700 = x.NormsConsumption?.IsBath1650With1700 ?? 0,
                        IsBathNotShower = x.NormsConsumption?.IsBathNotShower ?? 0,
                        IsShower = x.NormsConsumption?.IsShower ?? 0,
                        HvsIsBath1200 = x.NormsConsumption?.HvsIsBath1200 ?? 0,
                        HvsIsBath1500With1550 = x.NormsConsumption?.HvsIsBath1500With1550 ?? 0,
                        HvsIsBathNotShower = x.NormsConsumption?.HvsIsBathNotShower ?? 0,
                        HvsIsShower = x.NormsConsumption?.HvsIsShower ?? 0,
                        IsNotBoiler = x.NormsConsumption?.IsNotBoiler ?? 0,
                        HvsIsNotBoiler = x.NormsConsumption?.HvsIsNotBoiler ?? 0,
                        IsHvsBathIsNotCentralSewage = x.NormsConsumption?.IsHvsBathIsNotCentralSewage ?? 0,
                        IsHvsIsNotCentralSewage = x.NormsConsumption?.IsHvsIsNotCentralSewage ?? 0,
                        IsStandpipes = x.NormsConsumption?.IsStandpipes ?? 0,
                        IsHostelNoShower = x.NormsConsumption?.IsHostelNoShower ?? 0,
                        IsHostelSharedShower = x.NormsConsumption?.IsHostelSharedShower ?? 0,
                        IsHostelShowerAllLivPermises = x.NormsConsumption?.IsHostelShowerAllLivPermises ?? 0,
                        ShowerInHostelInSection = x.NormsConsumption?.ShowerInHostelInSection ?? 0,
                        Gvs12Floor = x.TpValue?.Gvs12Floor ?? 0
                    })
                    .AsQueryable();

                return result;
            }
            finally
            {
                this.Container.Release(coldWaterDomain);
                this.Container.Release(tehPassportValueDomain);
                this.Container.Release(roStructElService);
            }
        }

        public IQueryable<NormConsumptionHeatingProxy> GetNormConsumptionHeatingQuery(BaseParams baseParams, out int totalCount)
        {
            var loadParams = baseParams.GetLoadParam();

            var normConsId = loadParams.Filter.GetAs<long>("normConsId");

            var heatingConsDomain = this.Container.ResolveDomain<NormConsumptionHeating>();
            var tehPassportValueDomain = this.Container.Resolve<IDomainService<TehPassportValue>>();

            try
            {
                var roQuery = this.GetRoQuery(loadParams, normConsId, out totalCount);

                var normsConsumptionDict = heatingConsDomain.GetAll()
                    .Where(x => x.NormConsumption.Id == normConsId)
                    .Where(x => roQuery.Any(y => y.Id == x.RealityObject.Id))
                    .ToDictionary(x => x.RealityObject.Id, y => y);

                var tpValues = tehPassportValueDomain.GetAll()
                    .Where(x => roQuery.Any(y => y.Id == x.TehPassport.RealityObject.Id))
                    .AsEnumerable()
                    .GroupBy(x => x.TehPassport.RealityObject.Id)
                    .ToDictionary(
                        x => x.Key,
                        y => new
                        {
                            GenerealBuildingHeatMeters =
                                this.GetMeteringInstalledValue(y.FirstOrDefault(x => x.FormCode == "Form_6_6_2" && x.CellCode == "2:2")),
                            TypeHotWaterSystemStr = this.GetTypeHotWaterStr(y.FirstOrDefault(x => x.FormCode == "Form_3_2" && x.CellCode == "1:3")?.Value)
                        });

                var result = roQuery
                    .AsEnumerable()
                    .Select(x => new
                    {
                        RealityObject = x,
                        NormsConsumption = normsConsumptionDict.Get(x.Id),
                        TpValue = tpValues.Get(x.Id)
                    })
                    .Select(
                    x => new NormConsumptionHeatingProxy
                    {
                        ObjectId = x.NormsConsumption?.Id ?? 0,
                        Id = x.RealityObject.Id,
                        RealityObject = x.RealityObject,
                        Municipality = x.RealityObject.Municipality.Name,
                        Address = x.RealityObject.Address,
                        FloorNumber = x.RealityObject.MaximumFloors,
                        BuildYear = x.RealityObject.BuildYear,
                        TechnicalCapabilityOpu = x.NormsConsumption?.TechnicalCapabilityOpu ?? 0,
                        GenerealBuildingHeatMeters = x.TpValue?.GenerealBuildingHeatMeters ?? 0,
                        AreaHouse = x.RealityObject.AreaMkd,
                        AreaLivingRooms = x.RealityObject.AreaLiving,
                        AreaNotLivingRooms = x.RealityObject.AreaNotLivingPremises,
                        AreaOtherRooms = x.RealityObject.AreaCommonUsage,
                        IsIpuNotLivingPermises = x.NormsConsumption?.IsIpuNotLivingPermises ?? 0,
                        HeatEnergyConsumptionInPeriod = x.NormsConsumption?.HeatEnergyConsumptionInPeriod,
                        HotWaterConsumptionInPeriod = x.NormsConsumption?.HotWaterConsumptionInPeriod,
                        TypeHotWaterSystemStr = x.TpValue?.TypeHotWaterSystemStr,
                        TypeHotWaterSystem = x.NormsConsumption?.TypeHotWaterSystem ?? 0,
                        IsHeatedTowelRail = x.NormsConsumption?.IsHeatedTowelRail ?? 0,
                        Risers = x.NormsConsumption?.Risers ?? 0,
                        HeatEnergyConsumptionNotLivInPeriod = x.NormsConsumption?.HeatEnergyConsumptionNotLivInPeriod,
                        HotWaterConsumptionNotLivInPeriod = x.NormsConsumption?.HotWaterConsumptionNotLivInPeriod,
                        HeatingPeriod = x.NormsConsumption?.HeatingPeriod,
                        AvgTempColdWater = x.NormsConsumption?.AvgTempColdWater,
                        WearIntrahouseUtilites =x.NormsConsumption?.WearIntrahouseUtilites,
                        OverhaulDate = x.RealityObject.DateLastOverhaul
                    })
                    .AsQueryable();

                return result;
            }
            finally
            {
                this.Container.Release(heatingConsDomain);
                this.Container.Release(tehPassportValueDomain);
            }
        }

        public IQueryable<NormConsumptionFiringProxy> GetNormConsumptionFiringQuery(BaseParams baseParams, out int totalCount)
        {
            var loadParams = baseParams.GetLoadParam();

            var normConsId = loadParams.Filter.GetAs<long>("normConsId");

            var heatingConsDomain = this.Container.ResolveDomain<NormConsumptionFiring>();
            var tehPassportValueDomain = this.Container.Resolve<IDomainService<TehPassportValue>>();
            var roStructElService = this.Container.Resolve<IRealObjectStructElementService>();

            try
            {

                var roQuery = this.GetRoQuery(loadParams, normConsId, out totalCount);

                var normsConsumptionDict = heatingConsDomain.GetAll()
                    .Where(x => x.NormConsumption.Id == normConsId)
                    .Where(x => roQuery.Any(y => y.Id == x.RealityObject.Id))
                    .ToDictionary(x => x.RealityObject.Id, y => y);

                var tpValues = tehPassportValueDomain.GetAll()
                    .Where(x => roQuery.Any(y => y.Id == x.TehPassport.RealityObject.Id))
                    .AsEnumerable()
                    .GroupBy(x => x.TehPassport.RealityObject.Id)
                    .ToDictionary(
                        x => x.Key,
                        y => new
                        {
                            GenerealBuildingFiringMeters =
                                this.GetMeteringInstalledValue(y.FirstOrDefault(x => x.FormCode == "Form_6_6_2" && x.CellCode == "2:1")),
                        });

                var firingDict = roStructElService.GetRealityObjectWearoutDictionary(roQuery, NormConsumptionService.FiringCode);

                var result = roQuery
                    .AsEnumerable()
                    .Select(x => new
                    {
                        RealityObject = x,
                        NormsConsumption = normsConsumptionDict.Get(x.Id),
                        TpValue = tpValues.Get(x.Id),
                        Wearout = firingDict.Get(x.Id)
                    })
                    .Select(
                    x => new NormConsumptionFiringProxy
                    {
                        ObjectId = x.NormsConsumption?.Id ?? 0,
                        Id = x.RealityObject.Id,
                        RealityObject = x.RealityObject,
                        Municipality = x.RealityObject.Municipality.Name,
                        Address = x.RealityObject.Address,
                        FloorNumber = x.RealityObject.MaximumFloors,
                        BuildYear = x.RealityObject.BuildYear,
                        GenerealBuildingFiringMeters = x.TpValue?.GenerealBuildingFiringMeters ?? 0,
                        TechnicalCapabilityOpu = x.NormsConsumption?.TechnicalCapabilityOpu ?? 0,
                        AreaHouse = x.RealityObject.AreaMkd,
                        AreaLivingRooms = x.RealityObject.AreaLiving,
                        AreaNotLivingRooms = x.RealityObject.AreaNotLivingPremises,
                        AreaOtherRooms = x.RealityObject.AreaCommonUsage,
                        IsIpuNotLivingPermises = x.NormsConsumption?.IsIpuNotLivingPermises ?? 0,
                        AreaNotLivingIpu = x.NormsConsumption?.AreaNotLivingIpu,
                        AmountHeatEnergyNotLivingIpu = x.NormsConsumption?.AmountHeatEnergyNotLivingIpu,
                        AmountHeatEnergyNotLivInPeriod = x.NormsConsumption?.AmountHeatEnergyNotLivInPeriod,
                        HeatingPeriod = x.NormsConsumption?.HeatingPeriod,
                        WallMaterial = x.RealityObject.WallMaterial.Name,
                        RoofMaterial = x.RealityObject.RoofingMaterial.Name,
                        HourlyHeatLoadForPassport = x.NormsConsumption?.HourlyHeatLoadForPassport,
                        HourlyHeatLoadForDocumentation = x.NormsConsumption?.HourlyHeatLoadForDocumentation,
                        WearIntrahouseUtilites = x.Wearout,
                        OverhaulDate = x.RealityObject.DateLastOverhaul
                    })
                    .AsQueryable();

                return result;
            }
            finally
            {
                this.Container.Release(heatingConsDomain);
                this.Container.Release(tehPassportValueDomain);
            }
        }

        private IQueryable<RealityObject> GetRoQuery(LoadParam loadParams, long normConsId, out int totalCount)
        {
            var normConsDomain = this.Container.ResolveDomain<NormConsumption>();
            var roDomain = this.Container.ResolveDomain<RealityObject>();

            using (this.Container.Using(normConsDomain, roDomain))
            {
                var munId = normConsDomain.GetAll()
                .Where(x => x.Id == normConsId)
                .Select(x => x.Municipality.Id)
                .FirstOrDefault();

                // Если не передавать параметры пагинга, то он просто не отработает, нам это наруку.
                var roQuery = roDomain.GetAll()
                    .Where(x => x.Municipality.Id == munId)
                    .Filter(loadParams, this.Container);

                    totalCount = roQuery.Count();

                roQuery = roQuery
                    .Paging(loadParams);

                return roQuery;
            }
        }

        private string GetTypeHotWater(string code)
        {
            switch (code)
            {
                case "1":
                    return "Центральное";

                case "2":
                    return "Автономная котельная (крышная, встроенно-пристроенная)";

                case "3":
                    return "Квартирное отопление (квартирный котел)";

                case "4":
                    return "Индивидуальный водонагреватель";

                case "5":
                    return "Печное";

                case "6":
                    return "Отсутствует";
            }

            return "";
        }

        private string GetTypeHotWaterStr(string code)
        {
            switch (code)
            {
                case "1":
                    return "Открытая";

                case "2":
                    return "Закрытая";

                case "3":
                    return "Закрытая";

                case "4":
                    return "Закрытая";

                case "5":
                    return "Закрытая";

                case "6":
                    return "Отсутствует";
            }

            return "";
        }

        private YesNo GetMeteringInstalledValue(TehPassportValue value)
        {
            if (value != null)
            {
                if (value.Value == "3")
                {
                    return YesNo.Yes;
                }

                if (value.Value == "1" || value.Value == "2")
                {
                    return YesNo.No;
                }
            }

            return 0;
        }
    }
}