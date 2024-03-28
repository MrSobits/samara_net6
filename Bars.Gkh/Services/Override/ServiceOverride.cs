namespace Bars.Gkh.Services.Override
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Config;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Utils;
    using Bars.Gkh.ConfigSections.Administration;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Services.DataContracts;
    using Bars.Gkh.Services.DataContracts.HouseSearch;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    public class ServiceOverride : IServiceOverride
    {
        public ServiceOverride(IWindsorContainer container)
        {
            this.container = container;
        }
        protected IWindsorContainer container { get; set; }

        public IDomainService<RealityObject> RealObjDomainService { get; set; }

        public IDomainService<Fias> FiasDomainService { get; set; }

        public IDomainService<Municipality> MunicipalityDomainService { get; set; }

        public IConfigProvider ConfigProvider { get; set; }

        /// <inheritdoc />
        public virtual HousesResponse GetHouses(string streetId, string filter)
        {
            //смотрим в настройки приложения какие типы счетов отдавать
            var config = this.container.GetGkhConfig<AdministrationConfig>();
            var portalHouseTypes = config.PortalExportConfig.PortalExportParams.TypeHouseConfig;
            
            List<CrFundFormationType?> aprovedTypes = new List<CrFundFormationType?>();

            if (portalHouseTypes.Unknown) {aprovedTypes.Add(CrFundFormationType.Unknown); }
            if (portalHouseTypes.RegopCalcAccount) {aprovedTypes.Add(CrFundFormationType.RegOpAccount); }
            if (portalHouseTypes.SpecialCalcAccount) {aprovedTypes.Add(CrFundFormationType.SpecialAccount); }
            if (portalHouseTypes.RegopSpecialCalcAccount) {aprovedTypes.Add(CrFundFormationType.SpecialRegOpAccount); }

            var street = this.GetObjects(FiasLevelEnum.Street).Any(x => x.AOGuid == streetId);
            if (!street)
            {
                street = this.GetObjects(FiasLevelEnum.PlanningStruct).Any(x => x.AOGuid == streetId);
            }

            var city = this.GetObjects(new[] { FiasLevelEnum.City, FiasLevelEnum.Ctar, FiasLevelEnum.Raion, FiasLevelEnum.Place })
                .Any(x => x.AOGuid == streetId);

            if (!street)
            {
                if (!city)
                {
                    return new HousesResponse
                    {
                        Houses = null,
                        Result = new Result
                        {
                            Code = "05",
                            Name = $"В таблице ФИАС нет обьекта с AOGud {streetId}"
                        }
                    };
                }
            }

            var realObjStateCode = this.ConfigProvider.GetConfig().AppSettings.GetAs<string>("Service_RealObjStateCode");

            var houses = this.RealObjDomainService.GetAll()
                .WhereIf(street, x => x.FiasAddress.StreetGuidId == streetId)
                //.Where(x => x.AccountFormationVariant == CrFundFormationType.RegOpAccount)
                .Where(x => x.ExportedToPortal)
              //  .Where(x => aprovedTypes.Contains(x.AccountFormationVariant))
                .WhereIf(
                    city,
                    x => (x.FiasAddress.StreetGuidId == null || x.FiasAddress.StreetGuidId == "")
                        && x.FiasAddress.PlaceGuidId == streetId)

                //.WhereIf(!realObjStateCode.IsEmpty(), x => x.State.Code == realObjStateCode)
                .WhereIf(filter == "mkd", x => x.TypeHouse == TypeHouse.ManyApartments)
                .Select(
                    x => new
                    {
                        x.Id,
                        HouseNum = x.FiasAddress.House,
                        Street = x.FiasAddress.StreetName,
                        City = x.FiasAddress.PlaceName,
                        IdMo = x.Municipality.Id,
                        x.TypeHouse,
                        x.FiasAddress.Housing,
                        x.FiasAddress.Building,
                        x.FiasAddress.Letter
                    })
                .AsEnumerable()
                .Select(
                    x =>
                    {
                        var houseNum = x.HouseNum;

                        if (!string.IsNullOrWhiteSpace(x.Housing))
                        {
                            houseNum = $"{houseNum}, корп. {x.Housing}";
                        }

                        if (!string.IsNullOrWhiteSpace(x.Building))
                        {
                            houseNum = $"{houseNum}, секц. {x.Building}";
                        }

                        if (!string.IsNullOrWhiteSpace(x.Letter))
                        {
                            houseNum = $"{houseNum}, лит. {x.Letter}";
                        }

                        return new House
                        {
                            Id = x.Id.ToStr(),
                            HouseNum = houseNum,
                            Street = x.Street,
                            City = x.City,
                            IdMo = x.IdMo,
                            Type = x.TypeHouse.GetEnumMeta().Display
                        };
                    })
                .ToArray();

            var result = houses.Length == 0 ? Result.DataNotFound : Result.NoErrors;
            return new HousesResponse { Houses = houses, Result = result };
        }

        /// <inheritdoc />
        public virtual HousesResponse GetHousesByMu(string munId)
        {
            var mo = this.GetObjects(FiasLevelEnum.Raion).Any(x => x.AOGuid == munId);
            if (!mo)
            {
                var city = this.MunicipalityDomainService.GetAll().Where(x => x.FiasId == munId);
                if (!city.Any())
                {
                    return new HousesResponse { Houses = null, Result = Result.NotStreetCode };
                }
            }

            var houses =
                this.RealObjDomainService.GetAll()
                    .Where(x => x.Municipality.FiasId == munId)
                    .Select(
                        x => new
                        {
                            Id = x.Id.ToStr(),
                            HouseNum = x.FiasAddress.House,
                            Street = x.FiasAddress.StreetName,
                            City = x.FiasAddress.PlaceName,
                            IdMo = x.Municipality.Id,
                            Type = x.TypeHouse.GetEnumMeta().Display,
                            x.FiasAddress.Housing,
                            x.FiasAddress.Letter,
                            x.FiasAddress.Building
                        })
                    .AsEnumerable()
                    .Select(
                        x =>
                        {
                            var houseNum = x.HouseNum;

                            if (!string.IsNullOrWhiteSpace(x.Housing))
                            {
                                houseNum = $"{houseNum}, корп. {x.Housing}";
                            }

                            if (!string.IsNullOrWhiteSpace(x.Building))
                            {
                                houseNum = $"{houseNum}, секц. {x.Building}";
                            }

                            if (!string.IsNullOrWhiteSpace(x.Letter))
                            {
                                houseNum = $"{houseNum}, лит. {x.Letter}";
                            }

                            return new House
                            {
                                Id = x.Id.ToStr(),
                                HouseNum = houseNum,
                                Street = x.Street,
                                City = x.City,
                                IdMo = x.IdMo,
                                Type = x.Type
                            };
                        })
                    .ToArray();

            var result = houses.Length == 0 ? Result.DataNotFound : Result.NoErrors;
            return new HousesResponse { Houses = houses, Result = result };
        }

        /// <inheritdoc />
        public IQueryable<Fias> GetObjects(IEnumerable<FiasLevelEnum> aolevel, string parentGuid = "")
        {
            var mirrorGuid = string.Empty;
            if (!string.IsNullOrEmpty(parentGuid))
            {
                mirrorGuid = this.FiasDomainService.GetAll()
                    .Where(x => x.AOGuid == parentGuid)
                    .Select(x => x.MirrorGuid)
                    .FirstOrDefault();

                parentGuid = !string.IsNullOrEmpty(mirrorGuid) ? mirrorGuid : parentGuid;
            }

            return
                this.FiasDomainService.GetAll()
                    .Where(x => aolevel.Contains(x.AOLevel) && x.ActStatus == FiasActualStatusEnum.Actual)
                    .WhereIf(!string.IsNullOrEmpty(parentGuid), x => x.ParentGuid == parentGuid);
        }

        /// <inheritdoc />
        public IQueryable<Fias> GetObjects(FiasLevelEnum aolevel, string parentGuid = "")
        {
            return this.GetObjects(new[] { aolevel }, parentGuid);
        }
    }
}