namespace Bars.Gkh.Reforma.Utils
{
    using System.Linq;

    using Bars.B4.Modules.FIAS;
    using Bars.B4.Utils;
    using Bars.Gkh.Reforma.ReformaService;

    using Castle.Windsor;

    using FiasAddress = Bars.Gkh.Reforma.ReformaService.FiasAddress;

    public static class AddressUtils
    {
        /// <summary>
        /// Преобразование B4 ФИАС адреса в ФИАС адрес Реформы
        /// </summary>
        /// <param name="address">B4 ФИАС адрес</param>
        /// <param name="container">IoC контейнер</param>
        /// <returns>ФИАС адрес реформы</returns>
        public static FiasAddress ToReformaFias(this B4.Modules.FIAS.FiasAddress address, IWindsorContainer container)
        {
            if (address == null)
            {
                return new FiasAddress();
            }

            var service = container.Resolve<IFiasRepository>();
            try
            {
                var placeGuid = address.PlaceGuidId;
                var streetGuid = address.StreetGuidId;
                if (!string.IsNullOrEmpty(address.AddressGuid) && (string.IsNullOrEmpty(placeGuid) || string.IsNullOrEmpty(streetGuid)))
                {
                    var addressParts = address.AddressGuid.Split('#').Select(x => x.Split('_'));
                    if (string.IsNullOrEmpty(placeGuid))
                    {
                        var placePart = addressParts.Where(x => x[0].In("4", "5", "6")).OrderByDescending(x => x[0]).FirstOrDefault();
                        if (placePart != null)
                        {
                            placeGuid = placePart[1];
                        }
                    }

                    if (string.IsNullOrEmpty(streetGuid))
                    {
                        var streetPart = addressParts.FirstOrDefault(x => x[0] == "7");
                        if (streetPart != null)
                        {
                            streetGuid = streetPart[1];
                        }
                    }
                }

                var house = address.House ?? string.Empty;
                if (!string.IsNullOrEmpty(address.Letter) && !house.ToLower().EndsWith(address.Letter.ToLower()))
                {
                    house += address.Letter;
                }

                return new FiasAddress
                           {
                               city_id = service.GetDinamicAddress(placeGuid).Return(x => x.MirrorGuid).Or(placeGuid),
                               street_id = service.GetDinamicAddress(streetGuid).Return(x => x.MirrorGuid).Or(streetGuid),
                               houseguid = address.HouseGuid?.ToString(),
                               block = address.Housing,
                               building = address.Building,
                               house_number = house,
                               room_number = address.Flat
                           };
            }
            finally
            {
                container.Release(service);
            }
        }

        public static string UnmirrorGuid(IWindsorContainer container, string guid)
        {
            if (guid.IsEmpty())
            {
                return null;
            }

            var repo = container.Resolve<IFiasRepository>();
            try
            {
                var fias = repo.GetDinamicAddress(guid);
                if (fias == null)
                {
                    return null;
                }

                return fias.Return(x => x.MirrorGuid).Or(guid);
            }
            finally
            {
                container.Release(repo);
            }
        }

        public static string[] GetGuidAliases(IWindsorContainer container, params string[] guids)
        {
            string[] filtered;
            if (guids.IsEmpty() || (filtered = guids.Where(x => !x.IsEmpty()).ToArray()).IsEmpty())
            {
                return new string[0];
            }

            var repo = container.Resolve<IFiasRepository>();
            try
            {
                return repo.GetAll().Where(x => filtered.Contains(x.AOGuid) || filtered.Contains(x.MirrorGuid)).Select(x => x.AOGuid).ToArray();
            }
            finally
            {
                container.Release(repo);
            }
        }

        public static FullAddress ToFullAddress(this B4.Modules.FIAS.FiasAddress address, IWindsorContainer container)
        {
            var result = new FullAddress();
            var repo = container.Resolve<IFiasRepository>();
            try
            {
                result.house_number = address.House;
                result.block = address.Housing;
                result.building = address.Building;

                var placeGuid = address.PlaceGuidId;
                var streetGuid = address.StreetGuidId;
                if (!string.IsNullOrEmpty(address.AddressGuid) && (string.IsNullOrEmpty(placeGuid) || string.IsNullOrEmpty(streetGuid)))
                {
                    var addressParts = address.AddressGuid.Split('#').Select(x => x.Split('_'));
                    if (string.IsNullOrEmpty(placeGuid))
                    {
                        var placePart = addressParts.Where(x => x[0].In("4", "5", "6")).OrderByDescending(x => x[0]).FirstOrDefault();
                        if (placePart != null)
                        {
                            placeGuid = placePart[1];
                        }
                    }

                    if (string.IsNullOrEmpty(streetGuid))
                    {
                        var streetPart = addressParts.FirstOrDefault(x => x[0] == "7");
                        if (streetPart != null)
                        {
                            streetGuid = streetPart[1];
                        }
                    }
                }

                Fias fias = null;
                if (!string.IsNullOrEmpty(streetGuid))
                {
                    fias = repo.GetAll().FirstOrDefault(x => x.AOGuid == streetGuid && x.ActStatus == FiasActualStatusEnum.Actual);
                }
                else if (!string.IsNullOrEmpty(placeGuid))
                {
                    fias = repo.GetAll().FirstOrDefault(x => x.AOGuid == placeGuid && x.ActStatus == FiasActualStatusEnum.Actual);
                }

                while (fias != null)
                {
                    if (!string.IsNullOrEmpty(fias.MirrorGuid))
                    {
                        fias = repo.GetAll().FirstOrDefault(x => x.AOGuid == fias.MirrorGuid && x.ActStatus == FiasActualStatusEnum.Actual);
                        if (fias == null)
                        {
                            break;
                        }
                    }

                    switch (fias.AOLevel)
                    {
                        case FiasLevelEnum.Region:
                            result.region_guid = fias.AOGuid;
                            result.region_code = fias.KladrCode;
                            result.region_formal_name = fias.FormalName.Or(fias.OffName);
                            result.region_short_name = fias.ShortName;
                            break;
                        case FiasLevelEnum.AutonomusRegion:
                            break;
                        case FiasLevelEnum.Raion:
                            result.area_guid = fias.AOGuid;
                            result.area_code = fias.KladrCode;
                            result.area_formal_name = fias.FormalName.Or(fias.OffName);
                            result.area_short_name = fias.ShortName;
                            break;
                        case FiasLevelEnum.City:
                            result.city1_guid = fias.AOGuid;
                            result.city1_code = fias.KladrCode;
                            result.city1_formal_name = fias.FormalName.Or(fias.OffName);
                            result.city1_short_name = fias.ShortName;
                            break;
                        case FiasLevelEnum.Ctar:
                            result.city2_guid = fias.AOGuid;
                            result.city2_code = fias.KladrCode;
                            result.city2_formal_name = fias.FormalName.Or(fias.OffName);
                            result.city2_short_name = fias.ShortName;
                            break;
                        case FiasLevelEnum.Place:
                            result.city3_guid = fias.AOGuid;
                            result.city3_code = fias.KladrCode;
                            result.city3_formal_name = fias.FormalName.Or(fias.OffName);
                            result.city3_short_name = fias.ShortName;
                            break;
                        case FiasLevelEnum.Street:
                            result.street_guid = fias.AOGuid;
                            result.street_code = fias.KladrCode;
                            result.street_formal_name = fias.FormalName.Or(fias.OffName);
                            result.street_short_name = fias.ShortName;
                            break;
                        case FiasLevelEnum.Extr:
                            result.additional_territory_guid = fias.AOGuid;
                            result.additional_territory_code = fias.KladrCode;
                            result.additional_territory_formal_name = fias.FormalName.Or(fias.OffName);
                            result.additional_territory_short_name = fias.ShortName;
                            break;
                        case FiasLevelEnum.Sext:
                            break;
                    }

                    fias = !string.IsNullOrEmpty(fias.ParentGuid) ? repo.GetAll().FirstOrDefault(x => x.AOGuid == fias.ParentGuid && x.ActStatus == FiasActualStatusEnum.Actual) : null;
                }

                return result;
            }
            finally
            {
                container.Release(repo);
            }
        }
    }
}