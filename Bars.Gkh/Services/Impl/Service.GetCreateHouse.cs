namespace Bars.Gkh.Services.Impl
{
    using System;
    using System.Linq;
    using System.Text;
    using B4.DataAccess;
    using B4.Modules.FIAS;
    using B4.Modules.States;
    using B4.Utils;
    using Bars.B4;
    using Bars.Gkh.Entities;
    using DataContracts.GetOperationTime;
    using Enums;
    using Import;
    using Utils.AddressPattern;

    /// <summary>
    /// Сервис ЖКХ
    /// </summary>
    public partial class Service
    {
        /// <summary>
        /// Создание дома
        /// </summary>
        /// <param name="requestData">входные параметры</param>
        /// <returns></returns>
        public GetCreateHouseResult GetCreateHouse(GetCreateHouseResponse requestData)
        {
            var validateResult = Validate(requestData);

            if (validateResult != null)
            {
                return validateResult;
            }

            var fiasDomain = this.Container.ResolveDomain<Fias>();
            var fiasAddressDomain = this.Container.ResolveDomain<FiasAddress>();
            var roDomain = this.Container.ResolveRepository<RealityObject>();
            var fiasRepository = this.Container.Resolve<IFiasRepository>();
            var stateProvider = this.Container.Resolve<IStateProvider>();
            var addressPattern = this.Container.Resolve<IAddressPattern>();

            try
            {
                var fiasAdresses = fiasDomain.GetAll()
                    .Join(
                        fiasAddressDomain.GetAll(),
                        x => x.AOGuid,
                        y => y.StreetGuidId,
                        (a, b) => new {fias = a, fiasAddress = b}
                    )
                    .Where(x => x.fias.ActStatus == FiasActualStatusEnum.Actual)
                    .Where(x => x.fias.AOGuid == requestData.StreetAoGuid)
                    .Select(x => new FiasAddressRecord
                    {
                        KladrCode = x.fias.KladrCode,
                        AoGuid = x.fias.AOGuid,
                        Id = x.fiasAddress.Id,
                        House = x.fiasAddress.House,
                        Housing = x.fiasAddress.Housing,
                        Building = x.fiasAddress.Building
                    })
                    .AsEnumerable()
                    .ToList();

                var aoGuidByKladrCodeMap = fiasDomain.GetAll()
                    .Where(x => x.ActStatus == FiasActualStatusEnum.Actual)
                    .Where(x => x.AOGuid == requestData.StreetAoGuid)
                    .Select(x => x.AOGuid)
                    .FirstOrDefault();

                FiasAddress fiasAddress = null;
                IDinamicAddress dinamicAddress = null;
                if (fiasAdresses.Count > 0)
                {
                    var result = fiasAdresses.Where(x => x.House == requestData.HouseNum).ToList();

                    if (string.IsNullOrEmpty(requestData.Korpus))
                    {
                        result = result.Where(x => string.IsNullOrEmpty(x.Housing)).ToList();
                    }
                    else
                    {
                        result = result.Where(x => x.Housing == requestData.Korpus).ToList();
                    }

                    if (string.IsNullOrEmpty(requestData.Building))
                    {
                        result = result.Where(x => string.IsNullOrEmpty(x.Building)).ToList();
                    }
                    else
                    {
                        result = result.Where(x => x.Building == requestData.Building).ToList();
                    }

                    if (result.Count == 0)
                    {
                        var aoGuid = fiasAdresses.First().AoGuid;

                        dinamicAddress = fiasRepository.GetDinamicAddress(aoGuid);

                        fiasAddress = CreateAddressByStreetAoGuid(aoGuid, requestData.HouseNum, requestData.Korpus,
                            requestData.Building, dinamicAddress);
                    }
                    else
                    {
                        var fiasAddressIds = result.Select(x => x.Id).ToArray();

                        var realtyObjectIdsByAddress =
                            roDomain.GetAll()
                                .Where(x => fiasAddressIds.Contains(x.FiasAddress.Id))
                                .Select(x => x.Id)
                                .ToList();

                        if (realtyObjectIdsByAddress.Count == 0)
                        {
                            // Тут создаем новый ФИАС адрес, т.к. найденный адрес - не адрес дома

                            var aoGuid = fiasAdresses.First().AoGuid;

                            dinamicAddress = fiasRepository.GetDinamicAddress(aoGuid);

                            fiasAddress = CreateAddressByStreetAoGuid(aoGuid, requestData.HouseNum, requestData.Korpus,
                                requestData.Building, dinamicAddress);
                        }
                        else
                        {
                            // Дом уже в системе
                            return new GetCreateHouseResult()
                            {
                                Name = "House is already exist",
                                Code = "02",
                                Description = "Дом с указанным адресом уже существует"
                            };
                        }
                    }
                }
                else
                {
                    if (aoGuidByKladrCodeMap.IsNotEmpty())
                    {
                        dinamicAddress = fiasRepository.GetDinamicAddress(aoGuidByKladrCodeMap);

                        fiasAddress = CreateAddressByStreetAoGuid(aoGuidByKladrCodeMap, requestData.HouseNum,
                            requestData.Korpus, requestData.Building, dinamicAddress);
                    }
                    else
                    {
                        return new GetCreateHouseResult()
                        {
                            Name = "House is already exist",
                            Code = "02",
                            Description = "Дом с указанным адресом уже существует"
                        };
                    }
                }

                if (fiasAddress == null)
                {
                    return new GetCreateHouseResult()
                    {
                        Name = "Not found in the code of the street FIAS",
                        Code = "05",
                        Description = "Не найден код улицы в ФИАС"
                    };
                }

                if (fiasAddress.Id > 0)
                {
                    fiasAddress = fiasAddressDomain.Load(fiasAddress.Id);
                }
                else
                {
                    fiasAddressDomain.Save(fiasAddress);
                }

                var municipality = GetMunicipality(fiasAddress) ?? GetMunicipality(requestData.MunicipalCode);

                if (municipality == null)
                {
                    return new GetCreateHouseResult()
                    {
                        Name = "Not found municipal district",
                        Code = "06",
                        Description = "Не удалось определить муниципальное образование"
                    };
                }

                var realityObj = new RealityObject
                {
                    FiasAddress = fiasAddress,
                    Municipality = municipality,
                    Address = GetAddressForMunicipality(municipality, fiasAddress, dinamicAddress),
                    ObjectCreateDate = DateTime.Now,
                    ObjectEditDate = DateTime.Now
                };

                stateProvider.SetDefaultState(realityObj);

                realityObj.DateCommissioning = requestData.StartupDay.ToDateTime();
                realityObj.BuildYear = requestData.BuiltYear.ToInt();
                realityObj.AreaMkd = requestData.TotalArea.ToDecimal();
                realityObj.MaximumFloors = requestData.Maxfloors.ToInt();
                realityObj.Floors = requestData.Minfloors.ToInt();
                realityObj.TypeHouse = GetHouseType(requestData.HouseType);
                realityObj.ConditionHouse = ConditionHouse.Serviceable;
                realityObj.TypeRoof = TypeRoof.Plane;
                realityObj.HeatingSystem = HeatingSystem.Centralized;

                realityObj.Address = realityObj.Municipality != null
                        ? addressPattern.FormatShortAddress(realityObj.Municipality, realityObj.FiasAddress)
                        : realityObj.FiasAddress.AddressName;

                var settlementMo = Utils.Utils.GetSettlementByRealityObject(this.Container, realityObj);

                realityObj.MoSettlement = settlementMo;

                roDomain.Save(realityObj);

                return new GetCreateHouseResult()
                {
                    Name = "New house successfully created",
                    Code = "00",
                    Description = "Новый дом успешно создан"
                };
            }
            finally
            {
                Container.Release(fiasDomain);
                Container.Release(fiasAddressDomain);
                Container.Release(roDomain);
                Container.Release(fiasRepository);
                Container.Release(stateProvider);
            }
        }

        private TypeHouse GetHouseType(string houseType)
        {
            switch (houseType)
            {
                case "Общежития": return TypeHouse.SocialBehavior; 
                case "Индивидуальные дома": return TypeHouse.Individual;
                case "Блокированные дома": return TypeHouse.BlockedBuilding;
                case "Социальная ипотека (НО ГЖФ РТ)":
                case "Прочие гос. программы":
                case "Коммерческое":
                case "Долевое":
                case "Неопределен":
                case "Частично-социальная ипотека (ГЖФ)":
                case "Муниципальное жилье": return TypeHouse.ManyApartments;
            }

            return TypeHouse.NotSet;
        }

        private GetCreateHouseResult Validate(GetCreateHouseResponse requestData)
        {
            if (requestData.HouseType.IsEmpty() ||
                requestData.StreetAoGuid.IsEmpty() ||
                requestData.HouseNum.IsEmpty() ||
                requestData.StartupDay.IsEmpty() ||
                requestData.BuiltYear.IsEmpty() ||
                requestData.TotalArea.IsEmpty() ||
                requestData.Maxfloors.IsEmpty() ||
                requestData.Minfloors.IsEmpty())
            {
                return new GetCreateHouseResult()
                {
                    Name = "Mandatory fields is null",
                    Code = "03",
                    Description = "Не все обязательные поля заполнены"
                };
            }

            if (requestData.StartupDay.ToDateTime()  == DateTime.MinValue)
            {
                return new GetCreateHouseResult()
                {
                    Name = "Field 'Startupday' is not valid",
                    Code = "03",
                    Description = "Поле 'Дата сдачи в эксплуатацию' заполнено не верно"
                };
            }
            
            if (requestData.BuiltYear.ToInt() <= 0)
            {
                return new GetCreateHouseResult()
                {
                    Name = "Field 'Builtyear' is not valid",
                    Code = "03",
                    Description = "Поле 'Год постройки' заполнено не верно"
                };
            }

            if (requestData.TotalArea.ToDecimal() <= 0)
            {
                return new GetCreateHouseResult()
                {
                    Name = "Field 'Totalarea' is not valid",
                    Code = "03",
                    Description = "Поле 'Общая площадь дома' заполнено не верно"
                };
            }

            if (requestData.Maxfloors.ToInt() <= 0)
            {
                return new GetCreateHouseResult()
                {
                    Name = "Field 'Maxfloors' is not valid",
                    Code = "03",
                    Description = "Поле 'Максимальная этажность' заполнено не верно"
                };
            }

            if (requestData.Minfloors.ToInt() <= 0)
            {
                return new GetCreateHouseResult()
                {
                    Name = "Field 'Minfloors' is not valid",
                    Code = "03",
                    Description = "Поле 'Минимальная этажность' заполнено не верно"
                };
            }

            return null;

        }

        private Municipality GetMunicipality(FiasAddress address)
        {
            var muDomain = this.Container.ResolveRepository<Municipality>();

            try
            {
                if (address == null || string.IsNullOrEmpty(address.AddressGuid))
                {
                    return null;
                }

                var guidMass = address.AddressGuid.Split('#');

                Municipality result = null;

                foreach (var s in guidMass)
                {
                    var t = s.Split('_');

                    Guid g;

                    Guid.TryParse(t[1], out g);

                    if (g != Guid.Empty)
                    {
                        var guid = g.ToString();

                        var mu = muDomain.GetAll().FirstOrDefault(x => x.FiasId == guid);

                        if (mu != null)
                        {
                            result = mu;
                        }
                    }
                }

                return result;
            }
            finally
            {
                this.Container.Release(muDomain);
            }
        }

        private Municipality GetMunicipality(string municipalCode)
        {
            var muDomain = this.Container.ResolveRepository<Municipality>();

            try
            {
                return muDomain.GetAll().FirstOrDefault(x => x.Code == municipalCode);
            }
            finally
            {
                this.Container.Release(muDomain);
            }
        }

        private string GetAddressForMunicipality(Municipality mo, FiasAddress address, IDinamicAddress dinamicAddress)
        {
            if (address == null || mo == null)
            {
                return string.Empty;
            }

            var result = address.AddressName ?? string.Empty;

            if (string.IsNullOrEmpty(result) && string.IsNullOrEmpty(mo.FiasId))
            {
                return string.Empty;
            }

            if (dinamicAddress == null)
            {
                return string.Empty;
            }

            if (result.StartsWith(dinamicAddress.AddressName))
            {
                result = result.Replace(dinamicAddress.AddressName, string.Empty).Trim();
            }

            if (result.StartsWith(","))
            {
                result = result.Substring(1).Trim();
            }

            return result;
        }

        /// <summary>
        /// Создание FiasAddress на основе aoGuid улицы, номера и корпуса дома
        /// Важно! Корректно работает только для уровня улиц
        /// </summary>
        private FiasAddress CreateAddressByStreetAoGuid(string aoGuid, string house, string housing, string building, IDinamicAddress dynamicAddress)
        {
            var addressName = new StringBuilder(dynamicAddress.AddressName);

            if (!string.IsNullOrEmpty(house))
            {
                addressName.Append(", д. ");
                addressName.Append(house);

                if (!string.IsNullOrEmpty(housing))
                {
                    addressName.Append(", корп. ");
                    addressName.Append(housing);

                    if (!string.IsNullOrEmpty(building))
                    {
                        addressName.Append(", секц. ");
                        addressName.Append(building);
                    }
                }
            }

            var fiasAddress = new FiasAddress
            {
                AddressGuid = dynamicAddress.AddressGuid,
                AddressName = addressName.ToString(),
                PostCode = dynamicAddress.PostCode,
                StreetGuidId = dynamicAddress.GuidId,
                StreetName = dynamicAddress.Name,
                StreetCode = dynamicAddress.Code,
                House = house,
                Housing = housing,
                Building = building,
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,

                // Поля ниже коррекны только если входной параметр aoGuid улицы
                PlaceAddressName = dynamicAddress.AddressName.Replace(dynamicAddress.Name, string.Empty).Trim(' ').Trim(','),
                PlaceGuidId = dynamicAddress.ParentGuidId,
                PlaceName = dynamicAddress.ParentName
            };

            return fiasAddress;
        }

    }
}