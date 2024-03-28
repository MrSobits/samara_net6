namespace Bars.Gkh.Services.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Services.DataContracts;
    using Bars.Gkh.Services.DataContracts.HouseSearch;
    using Bars.Gkh.Authentification;

    public partial class Service
    {
        /// <summary>
        /// Получить районы
        /// </summary>
        /// <returns>Ответ со списком районов</returns>
        public RaionListResponse GetRaionList()
        {
            var cities = this.ServiceOverride.GetObjects(new[] { FiasLevelEnum.City, FiasLevelEnum.Place })
                .Select(x => x.AOGuid).ToList();

            var municipalityRepo = this.Container.Resolve<IRepository<Municipality>>();
            var userManager = this.Container.Resolve<IGkhUserManager>();

            using (this.Container.Using(municipalityRepo, userManager))
            {
                var municipalityIds = userManager.GetMunicipalityIds();

                var parents = municipalityRepo.GetAll()
                    .Where(x => x.ParentMo == null)
                    .Select(x => x.Id);

                var unions = municipalityRepo.GetAll()
                    .WhereIf(municipalityIds.Count > 0, x => municipalityIds.Contains(x.Id))
                    .Where(x => parents.Any(p => p == x.Id))
                    .Select(x => new MunicipalUnion
                    {
                        Id = x.FiasId,
                        Name = x.Name,
                        TypeMU = x.Level.GetEnumMeta().Display,
                        PresenceStreet = cities.Contains(x.FiasId).ToString(),
                        Parent = x.ParentMo.Name ?? ""
                    })
                    .OrderBy(x => x.Name)
                    .ToArray();

                var result = unions.Length == 0 ? Result.DataNotFound : Result.NoErrors;
                return new RaionListResponse { MunicipalUnions = unions, Result = result };
            }
        }

        /// <summary>
        /// Получить города
        /// </summary>
        /// <param name="munId">ИД муниципального образования</param>
        /// <returns>Ответ со списком городов</returns>
        public CitiesResponse GetCities(string munId)
        {
            var mo = this.ServiceOverride.GetObjects(FiasLevelEnum.Raion).Any(x => x.AOGuid == munId);
            var resp = new CitiesResponse();
            var municipalityRepo = this.Container.Resolve<IRepository<Municipality>>();

            using (this.Container.Using(municipalityRepo))
            {
                if (!mo)
                {
                    var city = municipalityRepo.GetAll().Where(x => x.FiasId == munId);
                    if (city.Any())
                    {
                        resp = new CitiesResponse
                        {
                            Cities =
                                city.Select(x => new City { Id = x.FiasId, Name = x.Name })
                                    .ToArray(),
                            Result = Result.NoErrors
                        };
                    }
                    else
                    {
                        resp = new CitiesResponse { Cities = null, Result = Result.NotMoCode };
                    }
                }

                // Добавить населенные пункты которые относятся к данному МО
                var cities = this.ServiceOverride.GetObjects(new[] { FiasLevelEnum.City, FiasLevelEnum.Place }, munId)
                    .Select(x => new City { Id = x.AOGuid, Name = x.OffName + ' ' + x.ShortName })
                    .ToList()
                    .OrderBy(x => x.Name)
                    .ToList();

                // Добавить населенные пункты которые относятся к другому населенному пункту находящемуся в данном МО
                foreach (var city in cities.ToArray())
                {
                    cities.AddRange(
                        this.ServiceOverride.GetObjects(FiasLevelEnum.Place, city.Id)
                            .Select(y => new City { Id = y.AOGuid, Name = city.Name + ", " + y.OffName + ' ' + y.ShortName })
                            .ToList()
                            .OrderBy(y => y.Name));
                }

                var citiesres = (resp.Cities?.Concat(cities) ?? cities).ToArray();

                resp.Result = citiesres.Length == 0 ? Result.DataNotFound : Result.NoErrors;
                resp.Cities = citiesres;
                return resp;
            }
        }

        /// <summary>
        /// Получить улицы в населенном пункте
        /// </summary>
        /// <param name="settlementId">ИД населенного пункта</param>
        /// <returns>Ответ со списком улиц</returns>
        public StreetsResponse GetStreets(string settlementId)
        {
            // Проверяем есть ли населенный пункт с данным AOGuid
            var settlement = this.ServiceOverride.GetObjects(new[] { FiasLevelEnum.City, FiasLevelEnum.Ctar, FiasLevelEnum.Raion, FiasLevelEnum.Place })
                .FirstOrDefault(x => x.AOGuid == settlementId);

            // Если нет населенного пункта с данным AOGuid то возвращаем NotCityCode
            if (settlement == null)
            {
                return new StreetsResponse { Streets = null, Result = Result.NotCityCode };
            }

            // Переменная списка улиц
            var streets = new List<Street>();

            // Проверяем есть ли в населенном пункте дома без улиц 
            var housesWithoutStreet = this.GetHouses(settlementId);

            // Если в населенном пункте есть дома без улиц, то добавляем населенный пункт в качестве улицы.
            if (housesWithoutStreet.Houses != null && housesWithoutStreet.Houses.Length > 0)
            {
                streets.Add(new Street { Id = settlement.AOGuid, Name = string.Format("{0} {1}", settlement.OffName, settlement.ShortName) });
            }

            // Добавляем улицы населенного пункта
            // Добавляем улицы населенного пункта
            var streetsInCity = this.ServiceOverride.GetObjects(FiasLevelEnum.Street, settlementId)
                .Select(x => new Street { Id = x.AOGuid, Name = string.Format("{0} {1}", x.OffName, x.ShortName) })
                .ToList();
            var planStructsInCity = this.ServiceOverride.GetObjects(FiasLevelEnum.PlanningStruct, settlementId)
                .Select(x => new Street { Id = x.AOGuid, Name = string.Format("{0} {1}", x.OffName, x.ShortName) })
                .ToList();
            streets.AddRange(streetsInCity);
            streets.AddRange(planStructsInCity);

            var result = !streets.Any() ? Result.DataNotFound : Result.NoErrors;
            return new StreetsResponse { Streets = streets.ToArray(), Result = result };
        }

        /// <summary>
        /// Получить дома в МО
        /// </summary>
        /// <param name="munId">ИД МО</param>
        /// <returns>Ответ со списком домов</returns>
        public HousesResponse GetHousesByMu(string munId)
        {
            return this.ServiceOverride.GetHousesByMu(munId);
        }

        /// <summary>
        /// Получить дома
        /// </summary>
        /// <param name="streetId">ИД улицы</param>
        /// <param name="filter"></param>
        /// <returns>Ответ со списком домов</returns>
        public HousesResponse GetHouses(string streetId, string filter = null)
        {
            return this.ServiceOverride.GetHouses(streetId, filter);
        }

        /// <summary>
        /// Получить помещения
        /// </summary>
        /// <param name="houseId">ИД дома</param>
        /// <returns>Ответ со списком помещений</returns>
        public FlatsResponse GetFlats(string houseId)
        {
            var realObjDomain = this.Container.ResolveDomain<RealityObject>();
            var roomDomain = this.Container.ResolveDomain<Room>();

            try
            {
                var realObj = realObjDomain.GetAll().FirstOrDefault(x => x.Id == houseId.ToLong()).Return(x => x.Id);

                if (realObj == 0)
                {
                    return new FlatsResponse
                    {
                        Flats = null,
                        Result = new Result
                        {
                            Code = "06",
                            Name = string.Format("Не найден дом с houseId {0}", houseId)
                        }
                    };
                }

                var rooms = roomDomain.GetAll()
                    .Where(x => x.RealityObject.Id == houseId.ToLong())
                    .AsEnumerable()
                    .Select(x =>
                    {
                        return new Flat
                        {
                            Id = x.Id.ToStr(),
                            FlatNum = x.RoomNum
                        };
                    })
                    .ToArray();

                var result = rooms.Length == 0 ? Result.DataNotFound : Result.NoErrors;
                return new FlatsResponse { Flats = rooms, Result = result };
            }
            finally
            {
                this.Container.Release(realObjDomain);
                this.Container.Release(roomDomain);
            }
        }
    }
}