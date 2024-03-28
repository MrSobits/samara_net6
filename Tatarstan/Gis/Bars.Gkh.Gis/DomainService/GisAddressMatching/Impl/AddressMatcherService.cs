namespace Bars.Gkh.Gis.DomainService.GisAddressMatching.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Utils;
    using Bars.Gkh.Gis.DomainService.GisAddressMatching;
    using Bars.Gkh.Gis.DomainService.GisAddressMatching.Impl.FuzzyStringsMatcher;
    using Bars.Gkh.Gis.Entities.Register.HouseRegister;
    using Castle.Windsor;

    /// <summary>
    /// 
    /// </summary>
    public class AddressMatcherService : IAddressMatcherService
    {
        protected IWindsorContainer Container;
        protected IRepository<FiasAddress> FiasAddressRepository;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="container"></param>
        /// <param name="fiasAddressRepository"></param>
        public AddressMatcherService(IWindsorContainer container, IRepository<FiasAddress> fiasAddressRepository)
        {
            Container = container;
            FiasAddressRepository = fiasAddressRepository;            
        }        

        /// <summary>
        /// Сопоставить адрес
        /// </summary>
        /// <param name="house">Сопоставляемый адрес</param>
        /// <returns>Адрес из ФИАС</returns>
        public FiasAddress MatchAddress(HouseRegister house)
        {
            //поиск по региону            
            var regionGuid = MatchAddressObject(house.Region, FiasLevelEnum.Region);
            if (regionGuid.IsEmpty())
            {                
                return null;
            }

            //поиск по району, если такой есть
            var areaGiud = String.Empty;
            var areaCode = house.Area == "-"
                ? house.Region
                : house.Area;
            if (!house.Area.IsEmpty())
            {                
                areaGiud = MatchAddressObject(areaCode, FiasLevelEnum.Raion, regionGuid);
                if (areaGiud.IsEmpty())
                {
                    areaGiud = MatchAddressObject(areaCode, FiasLevelEnum.City, regionGuid);
                    if (areaGiud.IsEmpty())
                    {
                        areaGiud = regionGuid;
                    }
                }
            }
            
            //поиск по населенному пункту            
            var cityCode = house.City == "-"
                ? areaCode
                : house.City;
            var cityGuid = MatchAddressObject(cityCode, FiasLevelEnum.City,
                areaGiud.IsEmpty() ? regionGuid : areaGiud);
            if (cityGuid.IsEmpty())
            {
                cityGuid = areaGiud;
            }

            //поиск по населенному пункту            
            var placeGuid = MatchAddressObject(cityCode, FiasLevelEnum.Place,
                cityGuid.IsEmpty() ? areaGiud : cityGuid);
            if (placeGuid.IsEmpty())
            {
                placeGuid = cityGuid;
            }



            //поиск по улице            
            var streetGuid = MatchAddressObject(house.Street, FiasLevelEnum.Street, placeGuid);
            if (streetGuid.IsEmpty())
            {                
                return null;
            }

            //поиск дома и корпуса
            var fiasAddress =
                FiasAddressRepository
                    .GetAll()
                    .Where(x =>
                        x.StreetGuidId == streetGuid
                        &&
                        x.House.ToUpper() == house.HouseNum.ToUpper())
                    .WhereIf(house.BuildNum != null && house.BuildNum != "" && house.BuildNum != "-",
                        x =>
                            (x.Housing != null && x.Housing.ToUpper() == house.BuildNum.ToUpper())
                    )
                    .WhereIf(house.BuildNum == null || house.BuildNum == "" || house.BuildNum == "-",
                        x => x.Housing == null || x.Housing == ""
                    )
                    .FirstOrDefault();

            return fiasAddress;
        }

        /// <summary>
        /// Споставить адреса с адресами из ФИАС
        /// </summary>
        /// <param name="adressesList"></param>
        /// <returns></returns>
        public List<HouseRegister> MatchAddresses(List<HouseRegister> adressesList)
        {
            return adressesList.Where(address => MatchAddress(address) == null).ToList();
        }

        /// <summary>
        /// Найти GUID адресного объекта
        /// </summary>
        /// <param name="addressObject">объект адреса</param>
        /// <param name="level">уровень</param>
        /// <param name="parentGuid"></param>
        /// <returns>GUID адресного объекта</returns>
        private string MatchAddressObject(string addressObject, FiasLevelEnum level, string parentGuid = null)
        {
            if (addressObject.IsEmpty())
            {
                return string.Empty;
            }

            //удаляем ключевые слова для конкретного уровня
            switch (level)
            {
                case FiasLevelEnum.Region:
                {
                    addressObject = Regex.Replace(addressObject, "(?i)\\s*\\bресп\\b(\\.|\\s)?", "");

                    //заменяем // на ()                    
                    addressObject = Regex.Replace(addressObject, "(?i)/(?<Name>якутия)/", "(${Name})");

                    break;
                }
                case FiasLevelEnum.Raion:
                {
                    // замена в строке всех вхождений г, у, прив и т.д на пустую строку
                    addressObject = Regex.Replace(addressObject, "(?i)\\s*\\b(г|у|улус|прив)\\b(\\.|\\s)?", "");
                    break;
                }
                case FiasLevelEnum.City:
                {
                    // замена в строке всех вхождений г и тер на пустую строку
                    addressObject = Regex.Replace(addressObject, "(?i)\\s*\\b(г|тер)\\b(\\.|\\s)?", "");
                    break;
                }
                case FiasLevelEnum.Place:
                {
                    // замена в строке всех вхождений снт, с, п и т.д на пустую строку
                    addressObject = Regex.Replace(addressObject,
                        "(?i)\\s*\\b(стн|с|п|мкр|нп|тер|ст|дп|г|пгт)\\b(\\.|\\s)?",
                        "");
                    break;
                }
                case FiasLevelEnum.Street:
                {
                    // замена в строке всех вхождений снт, м, ул и т.д на пустую строку
                    addressObject = Regex.Replace(addressObject,
                        "(?i)\\s*\\b(у|снт|м|ул|жт|с|пл|аллея|нп|проезд|пер|гск|тракт|мкр|км|п|пл-ка|тер|13|ш|пр-кт|ферма|кв-л|заезд|уч-к|туп)\\b(\\.|\\s)?",
                        "");
                    break;
                }
            }

            var fiasRepository = Container.Resolve<IRepository<Fias>>();
            using (Container.Using(fiasRepository))
            {

                //todo проваливается по времени тут!!!
                //все объекты текущего уровня            
                var availableAdressObjects =
                    fiasRepository
                        .GetAll()
                        .Where(x =>
                            x.AOLevel == level
                            //&&x.ActStatus == FiasActualStatusEnum.Actual
                            )
                        .WhereIf(!String.IsNullOrEmpty(parentGuid), x => x.ParentGuid == parentGuid);
                                                      
                //todo если использовать LINQ (преобразовать код через resharper) - работает непривильно                
                //поиск по объектам текущего уровня
                var similarAddressObjectsDict = new List<AddressObjectProxy>();
                foreach (var o in availableAdressObjects)
                {
                    similarAddressObjectsDict.Add(
                        new AddressObjectProxy
                        {
                            Ao = o,
                            Dice = DiceCoefficientExtensions.DiceCoefficient(addressObject.Trim().ToUpper(),o.FormalName.ToUpper())
                        });
                }

                if (similarAddressObjectsDict.Count == 0)
                {
                    return string.Empty;
                }

                //все подходящие                
                var suitableAoList = similarAddressObjectsDict.Where(x => Math.Abs(x.Dice - 1) < 0.000001).ToList();
                if (suitableAoList.Count == 0)
                {
                    return string.Empty;
                }

                //попытка найти актуальный
                var actualAo = suitableAoList.FirstOrDefault(x => x.Ao.ActStatus == FiasActualStatusEnum.Actual);
                if (actualAo != null)
                {
                    return actualAo.Ao.AOGuid;
                }

                //если не актуальный - Вероятно Казань
                var kazan = suitableAoList.FirstOrDefault(x => x.Ao.AOGuid == "93b3df57-4c89-44df-ac42-96f05e9cd3b9");
                if (kazan != null)
                {
                    return kazan.Ao.AOGuid;
                }                
            }

            return string.Empty;
        }
    }

    internal class AddressObjectProxy
    {
        public Fias Ao { get; set; }

        public double Dice { get; set; }
    }
}
