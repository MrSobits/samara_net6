namespace Bars.Gkh.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Text.RegularExpressions;

    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Services.DataContracts;
    using Bars.Gkh.Services.DataContracts.HousesInfo;
    using Bars.B4;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities.RealityObj;
    using Bars.Gkh.Modules.RegOperator.Entities.RegOperator;
    using Bars.Gkh.Utils;
    using Bars.B4.DataAccess;
    using NHibernate.Transform;

    public partial class Service
    {

        public ISessionProvider SessionProvider { get; set; }
        /// <summary>
        /// Домен-сервис для <see cref="RealityObjectBuildingFeature"/>
        /// </summary>
        public IDomainService<RealityObjectBuildingFeature> RealityObjectBuildingFeatureDomain { get; set; }

        /// <summary>
        /// Домен-сервис для <see cref="RegOperator"/>
        /// </summary>
        public IDomainService<RegOperator> RegOperatorDomainService { get; set; }

        /// <summary>
        /// Сервис для работы с паспортом жилого дома
        /// </summary>
        public ITechPassportService TechPassportService { get; set; }

        private readonly Tuple<string, string> techPassportGasType = new Tuple<string, string>("Form_3_4", "1:3");
        private readonly Tuple<string, string> techPassportGasLastYear = new Tuple<string, string>("Form_3_4", "5:1");
        private readonly Tuple<string, string> techPassportFirefightingType = new Tuple<string, string>("Form_3_8", "1:3");

        private readonly Regex gasLastYearRegex = new Regex(@"^\d{4}", RegexOptions.Compiled);

        private readonly Dictionary<string, string> gasTypesDict = new Dictionary<string, string>
        {
            {"1", "Центральное" },
            {"2", "Нецентральное" },
            {"3", "Отсутствует" }
        };

        private readonly Dictionary<string, string> firefightingTypesDict = new Dictionary<string, string>
        {
            {"0", "Не заполнено" },
            {"1", "Отсутствует" },
            {"2", "Автоматическая" },
            {"3", "Пожарные гидранты" }
        };

        private readonly Dictionary<string, string> ventilationTypeDict = new Dictionary<string, string>
        {
            {"1", "Приточная вентиляция" },
            {"2", "Вытяжная вентиляция" },
            {"3", "Приточно-вытяжная вентиляция" },
            {"4", "Отсутствует"}
        };

        /// <summary>
        /// Получение сведений о жилых домах 
        /// </summary>
        /// <param name="houseIds">Id жилых домов</param>
        /// <returns></returns>
        public GetHousesInfoResponse GetHousesInfo(string houseIds)
        {
            var ids = houseIds.ToLongArray();

            var housesInfo = this.GetHouseInfo(ids);

            var result = housesInfo.Length == 0 ? Result.DataNotFound : Result.NoErrors;

            return new GetHousesInfoResponse { HousesInfo = housesInfo, Result = result };
        }

        /// <summary>
        /// Получение сведений о жилых домах 
        /// </summary>
        /// <param name="houseIds">Id жилых домов</param>
        /// <returns></returns>
        public GetHouseInfoByFiasResponse GetHouseInfoByFias(string fiasguid)
        {
            var ro_id = RealObjDomainService.GetAll().Where(x => x.HouseGuid == fiasguid).FirstOrDefault();
            if (ro_id == null)
            {
                ro_id = RealObjDomainService.GetAll().Where(x => x.FiasAddress.HouseGuid.ToString() == fiasguid).FirstOrDefault();
            }
            if (ro_id != null)
            {
                var housesInfo = this.GetHouseInfo(new long[] { ro_id.Id });
                var result = housesInfo.Length == 0 ? Result.DataNotFound : Result.NoErrors;
                return new GetHouseInfoByFiasResponse { HouseInfo = housesInfo[0], Result = result };
            }
            else
            {
                var result = Result.DataNotFound;
                return new GetHouseInfoByFiasResponse { Result = result };
            }
            
        }

        private HouseInfo[] GetHouseInfo(long[] ids)
        {
            var ownersCount = this.OwnersService?.GetOwnersCount(ids);
            var perfomedWorkActIntegration = this.PerfomedWorkActIntegrationService.GetPerfomedWorkActProxies(ids);

            var regOpName = this.RegOperatorDomainService.GetAll().Select(x => x.Contragent.Name).FirstOrDefault();

            IRealObjOverhaulDataService realObjOverhaulDataService = null;

            var roIdBuildingFeatureList = this.RealityObjectBuildingFeatureDomain
                .GetAll()
                .Where(y => ids.Contains(y.RealityObject.Id))
                .Where(y => y.BuildingFeature.Code == "1")
                .GroupBy(y => y.RealityObject.Id)
                .Select(y => y.Key)
                .ToList();

            if (this.Container.Kernel.HasComponent(typeof(IRealObjOverhaulDataService)))
            {
                realObjOverhaulDataService = this.Container.Resolve<IRealObjOverhaulDataService>();
            }

            IRealityObjectRealEstateTypeService realObjRealEstTypeService = null;
            if (this.Container.Kernel.HasComponent(typeof(IRealityObjectRealEstateTypeService)))
            {
                realObjRealEstTypeService = this.Container.Resolve<IRealityObjectRealEstateTypeService>();
            }

            var ci = CultureInfo.InvariantCulture.Clone() as CultureInfo;
            NumberFormatInfo numberformat = null;
            if (ci != null)
            {
                ci.NumberFormat.NumberDecimalSeparator = ".";
                numberformat = ci.NumberFormat;
            }



            try
            {
                string roids = "";
                foreach (var str in ids)
                {
                    roids += str + ",";
                }
                roids = roids.Substring(0, roids.Length - 1);

                //собираем словарь сальдо
                var saldoDict = new Dictionary<long, decimal>();

                var session = this.Container.Resolve<ISessionProvider>().OpenStatelessSession();

                var sql = $@"select ro_id, debt_total - credit_total as saldo from regop_ro_payment_account where ro_id in ({roids})";

                var SaldoResponceList = session.CreateSQLQuery(sql)
                    .SetResultTransformer(Transformers.AliasToBean<SaldoResponce>())
                    .List<SaldoResponce>();

                foreach (var str in SaldoResponceList)
                {
                    if (!saldoDict.ContainsKey(str.ro_id))
                    {
                        saldoDict.Add(str.ro_id, Decimal.Round(str.saldo, 2));
                    }
                }

                Container.Release(session);

                var houses = this.RealObjDomainService
                .GetAll()
                .Where(x => ids.Contains(x.Id))
                .AsEnumerable()
                .Select(x =>
                    {
                        var isAutoRealEstateType = false;
                        var roTypes = string.Empty;

                        if (realObjRealEstTypeService != null)
                        {
                            var result = realObjRealEstTypeService.GetAutoRealEstateType(x);
                            isAutoRealEstateType = result.Success;
                            roTypes = result.Data.ToStr();
                        }

                        return new HouseInfo
                        {
                            Id = x.Id,
                            MUnion = x.Municipality.Name,
                            Settlement = x.MoSettlement != null ? x.MoSettlement.Name : string.Empty,
                            BuildYear = x.BuildYear ?? 0,
                            ExplYear = x.DateCommissioning?.Year ?? 0,
                            FlatNumber = x.NumberApartments.ToInt(),
                            Floor = x.MaximumFloors.ToInt(),
                            LiveArea = x.AreaLiving?.RoundDecimal(2).ToString(numberformat) ?? string.Empty,
                            LiveUnliveArea = x.AreaLivingNotLivingMkd?.RoundDecimal(2).ToString(numberformat) ?? string.Empty,
                            OfWearPers = x.PhysicalWear?.RoundDecimal(2).ToString(numberformat) ?? string.Empty,
                            PeopleLivingArea = x.AreaLivingOwned?.RoundDecimal(2).ToString(numberformat) ?? string.Empty,
                            TotalArea = x.AreaMkd?.RoundDecimal(2).ToString(numberformat) ?? string.Empty,
                            RoofMaterial = x.RoofingMaterial?.Name,
                            RoofType = x.TypeRoof.GetEnumMeta().Display,
                            WallMaterial = x.WallMaterial?.Name ?? string.Empty,
                            Residents = x.NumberLiving.ToInt(),
                            Address = x.Address,
                            AddressFias = x.FiasAddress.AddressName,
                            MinFloor = x.Floors?.ToString() ?? string.Empty,
                            PrivatRoom = x.HasPrivatizedFlats.HasValue && x.HasPrivatizedFlats.Value ? "Да" : "Нет",
                            Data1PrivatRoom = x.PrivatizationDateFirstApartment?.ToShortDateString() ?? string.Empty,
                            NeedKapRem = x.NecessaryConductCr.GetEnumMeta().Display,
                            LastKapRemDate = x.DateLastOverhaul?.ToShortDateString() ?? string.Empty,
                            ProjectTip = x.TypeProject?.Name ?? string.Empty,
                            CadastrNumber = x.CadastreNumber,
                            CadastralHouseNumber = x.CadastralHouseNumber,
                            StaircaseNum = x.NumberEntrances?.ToString() ?? string.Empty,
                            LiftNum = x.NumberLifts?.ToString() ?? string.Empty,
                            HeatingSystem = x.HeatingSystem.GetEnumMeta().Display,
                            FloorHeight = x.FloorHeight?.RoundDecimal(2).ToString(numberformat) ?? string.Empty,
                            AreaCleaning = x.AreaCleaning?.RoundDecimal(2).ToString(numberformat) ?? string.Empty,
                            PublicArea = x.AreaCommonUsage?.RoundDecimal(2).ToString(numberformat) ?? string.Empty,
                            AreaNonResidental = x.AreaNotLivingPremises.ToDecimal(),
                            EntranceCount = x.NumberEntrances.ToInt(),
                            FormingOverhaulFund = x.AccountFormationVariant.ToInt(),
                            HouseType = x.TypeHouse.GetDisplayName(),
                            BuiltYear = x.BuildYear ?? 0,
                            PeopleNotLivingArea = x.AreaNotLivingPremises ?? 0,
                            GeneralArea = (x.AreaNotLivingFunctional ?? 0).RoundDecimal(2),
                            FlatsCount = (x.NumberApartments ?? 0) + (x.NumberNonResidentialPremises ?? 0),
                            NotLivingQuartersCount = x.NumberNonResidentialPremises ?? 0,
                            FloorCountMax = x.MaximumFloors ?? 0,
                            FloorCountMin = x.Floors ?? 0,
                            HouseState = x.ConditionHouse.GetDisplayName(),
                            OverhaulProgramShip = !x.IsNotInvolvedCr,
                            DateOfInclusionInProgram = (realObjOverhaulDataService?.GetPublishDateByRo(x)).ToDateString(),
                            DateOfExclusion = x.UnpublishDate.ToDateString(),
                            TechnicalSurveyDate = x.LatestTechnicalMonitoring.ToDateString(),
                            MunicipalArea = x.AreaMunicipalOwned?.RoundDecimal(2).ToStr(),
                            PublicalArea = x.AreaGovernmentOwned?.RoundDecimal(2).ToStr(),
                            SpecialMark = roIdBuildingFeatureList.Contains(x.Id) ? "YES" : "NO",
                            HouseClassification = isAutoRealEstateType ? roTypes : x.RealEstateType?.Name,
                            DemolitionDate = x.DateDemolition.ToDateString(),
                            Monument = x.IsCulturalHeritage,
                            CRSaldo = saldoDict.ContainsKey(x.Id) ? saldoDict[x.Id] : 0
                        };
                    })
.
        ToArray();

                var insurancesByRo = this.BelayPolicyMkdDomainService
                    .GetAll()
                    .Where(x => ids.Contains(x.RealityObject.Id))
                    .Where(
                        x =>
                            x.BelayPolicy.DocumentStartDate == null
                            || x.BelayPolicy.DocumentStartDate <= DateTime.Now)
                    .Where(
                        x =>
                            x.BelayPolicy.DocumentEndDate == null
                            || x.BelayPolicy.DocumentEndDate >= DateTime.Now)
                    .ToList()
                    .Select(x => new
                    {
                        BelayPolicy = x.BelayPolicy.BelayOrganization.Contragent.Name,
                        RoId = x.RealityObject.Id
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.RoId)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.BelayPolicy).First());


                var manorgByRo = this.MoContractRoDomainService.GetAll()
                    .Where(x => ids.Contains(x.RealityObject.Id))
                    .Where(x => x.ManOrgContract != null)
                    .Where(x => x.ManOrgContract.StartDate == null || x.ManOrgContract.StartDate <= DateTime.Now)
                    .Where(x => x.ManOrgContract.EndDate == null || x.ManOrgContract.EndDate >= DateTime.Now)
                    .ToList()
                    .Select(x => new
                    {
                        Name = Service.GetMoName(x, this.MoContractDirectManagService),
                        Id = x.ManOrgContract.ReturnSafe(z => z.ManagingOrganization).ReturnSafe(z => z.Id),
                        RoId = x.RealityObject.Id
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.RoId)
                    .ToDictionary(x => x.Key, y => y.First());


                var photosIdsByRo = this.RoImageDomainService
                    .GetAll()
                    .Where(x => ids.Contains(x.RealityObject.Id))
                    .Where(x => x.ImagesGroup == ImagesGroup.BeforeOverhaul || x.ImagesGroup == ImagesGroup.AfterOverhaul)
                    .Where(x => x.File != null)
                    .Select(x => new { x.ImagesGroup, x.File.Id, RoId = x.RealityObject.Id })
                    .AsEnumerable()
                    .GroupBy(x => x.RoId)
                    .ToDictionary(x => x.Key, y => y.GroupBy(x => x.ImagesGroup)
                        .ToDictionary(x => x.Key, x => x.Select(z => z.Id).ToArray()));

                var roomsByRo = this.RoomDomainService.GetAll()
                    .Where(x => ids.Contains(x.RealityObject.Id))
                    .Select(x => new
                    {
                        x.Id,
                        x.Type,
                        x.Area,
                        RoId = x.RealityObject.Id
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.RoId)
                    .ToDictionary(x => x.Key);

                var liftsDict = this.TehPassportValueService?.GetLiftsByHouseIds(ids);
                var meteringDeviceDict = this.TehPassportValueService?.GetMeteringDevicesByHouseIds(ids);
                var garbageInfoDict = this.TehPassportValueService?.GetGarbageInfoByHouseIds(ids);
                var ventilationInfoDict = this.TehPassportValueService?.GetNumVentilationDuctByHouseIds(ids);
                var facadeDict = this.TehPassportValueService?.GetFacadesByHouseIds(ids);
                var engineeringSystemDict = this.TehPassportValueService?.GetEngineeringSystemByHouseIds(ids);

                foreach (var house in houses)
                {
                    house.Insurance = insurancesByRo.Get(house.Id);

                    house.ManOrg = string.Empty;
                    house.IdManOrg = string.Empty;
                    house.NotLivingArea = string.Empty;

                    house.HouseLifts = new LiftArray
                    {
                        HouseLifts = liftsDict?.Get(house.Id)?.ToArray()
                    };

                    house.MeteringDevices = new MeteringDeviceArray
                    {
                        MeteringDevices = meteringDeviceDict?.Get(house.Id)?.ToArray()
                    };

                    var garbageInfo = garbageInfoDict?.Get(house.Id);
                    house.ConstructionGarbage = garbageInfo?.ConstructionGarbage;
                    house.NumberOfRefuseChutes = garbageInfo?.NumberOfRefuseChutes ?? 0;

                    house.Facades = facadeDict?.Get(house.Id);

                    var ventilationInfo = ventilationInfoDict?.Get(house.Id);
                    house.TypeVentilation = ventilationInfo?.TypeVentilation;
                    house.NumVentilationDuct = ventilationInfo?.NumVentilationDuct;

                    var engineeringSystem = engineeringSystemDict?.Get(house.Id);
                    if (engineeringSystem.IsNotNull())
                    {
                        house.HotWaterType = engineeringSystem.HotWaterType;
                        house.ColdWaterType = engineeringSystem.ColdWaterType;
                        house.ElectricalType = engineeringSystem.ElectricalType;
                        house.HeatingType = engineeringSystem.HeatingType;
                        house.SewerageType = engineeringSystem.SewerageType;
                    }

                    if (manorgByRo.ContainsKey(house.Id))
                    {
                        house.ManOrg = manorgByRo[house.Id].Name;
                        house.IdManOrg = manorgByRo[house.Id].Id.ToStr();
                    }

                    house.AccountOwner = house.FormingOverhaulFund == 0
                        ? regOpName
                        : house.FormingOverhaulFund == 2
                            ? house.ManOrg
                            : house.FormingOverhaulFund == 1
                                ? regOpName
                                : string.Empty;

                    if (photosIdsByRo.ContainsKey(house.Id))
                    {
                        house.BeforeKRPhoto = photosIdsByRo[house.Id].Get(ImagesGroup.BeforeOverhaul);
                        house.AfterKRPhoto = photosIdsByRo[house.Id].Get(ImagesGroup.AfterOverhaul);
                    }

                    if (roomsByRo.ContainsKey(house.Id))
                    {
                        house.NotLivingArea = roomsByRo[house.Id].Where(x => x.Type == RoomType.NonLiving)
                            .SafeSum(x => x.Area)
                            .RoundDecimal(2)
                            .ToString(numberformat);

                        house.NotLivingRoomCount = roomsByRo[house.Id].Count(x => x.Type == RoomType.NonLiving);

                        house.LivingRoomCount = roomsByRo[house.Id].Count(x => x.Type == RoomType.Living);
                    }

                    if (perfomedWorkActIntegration.ContainsKey(house.Id))
                    {
                        house.TypesWorkData = perfomedWorkActIntegration[house.Id].Select(
                                x => new TypesWork
                                {
                                    DatePerformance = x.DatePerformance?.ToShortDateString() ?? string.Empty,
                                    TypesWorkOverhaul = x.TypesWorkOverhaul
                                })
                            .ToArray();
                    }

                    if (ownersCount.IsNotNull() && ownersCount.ContainsKey(house.Id))
                    {
                        house.NumberOwners = ownersCount[house.Id];
                    }

                    var gasType = this.TechPassportService.GetValue(house.Id, this.techPassportGasType.Item1, this.techPassportGasType.Item2)?.Value;
                    var gasLastYear = this.TechPassportService.GetValue(house.Id, this.techPassportGasLastYear.Item1, this.techPassportGasLastYear.Item2)?.Value ?? string.Empty;
                    var gasLastYearRegexResult = this.gasLastYearRegex.Match(gasLastYear);
                    var firefightingType = this.TechPassportService.GetValue(house.Id, this.techPassportFirefightingType.Item1, this.techPassportFirefightingType.Item2)?.Value;

                    house.GasInfo = new GasInfo
                    {
                        GasType = gasType != null ? this.gasTypesDict.Get(gasType) : string.Empty,
                        GasLastYear = gasLastYearRegexResult.Success ? gasLastYearRegexResult.Value : string.Empty
                    };

                    house.FirefightingInfo = new FirefightingInfo
                    {
                        FirefightingType = firefightingType != null ? this.firefightingTypesDict.Get(firefightingType) : string.Empty
                    };

                    house.VentilationInfo = new VentilationInfo
                    {
                        TypeVentilation = ventilationInfo?.TypeVentilation != null ? this.ventilationTypeDict.Get(ventilationInfo.TypeVentilation) : string.Empty,
                        NumVentilationDuct = house.NumVentilationDuct
                    };
                }

                return houses;
            }
            finally
            {
                this.Container.Release(realObjOverhaulDataService);

            }
        }

        private static string GetMoName(ManOrgContractRealityObject house, IDomainService<RealityObjectDirectManagContract> moContractDirectManagServ)
        {
            if (house.ManOrgContract.TypeContractManOrgRealObj == TypeContractManOrg.DirectManag)
            {
                return moContractDirectManagServ.GetAll().Any(y => y.Id == house.ManOrgContract.Id && y.IsServiceContract)
                    ? ManOrgBaseContract.DirectManagementWithContractText
                    : ManOrgBaseContract.DirectManagementText;
            }

            return house.ManOrgContract
                .ReturnSafe(z => z.ManagingOrganization)
                .ReturnSafe(z => z.Contragent)
                .ReturnSafe(z => z.Name);
        }

        public class SaldoResponce
        {
            public long ro_id { get; set; }

            public decimal saldo { get; set; }
        }
    }
}