namespace Bars.Gkh.Regions.Tyumen.Services.Override
{
    using System;
    using System.Linq;
    using System.Text;

    using Bars.B4.Modules.FIAS;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Services.DataContracts;
    using Bars.Gkh.Services.DataContracts.HouseSearch;

    using Castle.Windsor;

    public class ServiceOverride : Bars.Gkh.Services.Override.ServiceOverride
    {
        /// <inheritdoc />
        public override HousesResponse GetHouses(string streetId, string filter)
        {
            var street = this.GetObjects(FiasLevelEnum.Street).Any(x => x.AOGuid == streetId);
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
                .WhereIf(
                    city,
                    x => (x.FiasAddress.StreetGuidId == null || x.FiasAddress.StreetGuidId == "")
                        && x.FiasAddress.PlaceGuidId == streetId)
                .WhereIf(!realObjStateCode.IsEmpty(), x => x.State.Code == realObjStateCode)
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
                        var houseNum = new StringBuilder(x.HouseNum);

                        if (!string.IsNullOrWhiteSpace(x.Housing))
                        {
                            houseNum.AppendFormat(", корп. {0}", x.Housing);
                        }

                        if (!string.IsNullOrWhiteSpace(x.Building) && x.Letter.IsEmpty())
                        {
                            houseNum.AppendFormat(", {0}", x.Building);
                        }

                        if (!string.IsNullOrWhiteSpace(x.Building))
                        {
                            FiasStructureTypeEnum strType;

                            if (Enum.TryParse(x.Letter, out strType))
                            {
                                var structName = strType != FiasStructureTypeEnum.NotDefined
                                    ? strType.GetDisplayName().ToLower()
                                    : FiasStructureTypeEnum.Structure.GetDisplayName().ToLower();

                                houseNum.AppendFormat(", {0} {1}", structName, x.Building);
                            }
                        }

                        return new House
                        {
                            Id = x.Id.ToStr(),
                            HouseNum = houseNum.ToStr(),
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
        public override HousesResponse GetHousesByMu(string munId)
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
                    .Select(x => new
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
                            var houseNum = new StringBuilder(x.HouseNum);

                            if (!string.IsNullOrWhiteSpace(x.Housing))
                            {
                                houseNum.AppendFormat(", корп. {0}", x.Housing);
                            }

                            if (!string.IsNullOrWhiteSpace(x.Building) && x.Letter.IsEmpty())
                            {
                                houseNum.AppendFormat(", {0}", x.Building);
                            }

                            if (!string.IsNullOrWhiteSpace(x.Building))
                            {
                                FiasStructureTypeEnum strType;

                                if (Enum.TryParse(x.Letter, out strType))
                                {
                                    var structName = strType != FiasStructureTypeEnum.NotDefined
                                        ? strType.GetDisplayName().ToLower()
                                        : FiasStructureTypeEnum.Structure.GetDisplayName().ToLower();

                                    houseNum.AppendFormat(", {0} {1}", structName, x.Building);
                                }
                            }

                            return new House
                            {
                                Id = x.Id.ToStr(),
                                HouseNum = houseNum.ToStr(),
                                Street = x.Street,
                                City = x.City,
                                IdMo = x.IdMo,
                                Type = x.Type
                            };
                        }).ToArray();

            var result = houses.Length == 0 ? Result.DataNotFound : Result.NoErrors;
            return new HousesResponse { Houses = houses, Result = result };
        }

        /// <inheritdoc />
        public ServiceOverride(IWindsorContainer container)
            : base(container)
        {
        }
    }
}