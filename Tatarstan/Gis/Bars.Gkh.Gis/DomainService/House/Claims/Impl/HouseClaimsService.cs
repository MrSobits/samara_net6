namespace Bars.Gkh.Gis.DomainService.House.Claims.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Web;
    using B4;
    using B4.DataAccess;
    using B4.Modules.FIAS;
    using B4.Utils;

    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Gis.ConfigSections;

    using Castle.Windsor;
    using Config;
    using Entities.House.Claims;
    using Gkh.Entities;
    using Newtonsoft.Json;

    /// <summary>
    /// Сервис получения претензий по дому
    /// </summary>
    public class HouseClaimsService : IHouseClaimsService
    {
        protected string ServiceUri;
        protected string RestToken;
        protected string RestSecret;
        protected string Login;
        protected string Password;

        protected IWindsorContainer Container;
        protected IRepository<RealityObject> RealityObjectRepository;
        protected IRepository<Fias> FiasRepository;
        protected Dictionary<int, string> ServiceDictionary;

        public HouseClaimsService(IWindsorContainer container, 
            IRepository<RealityObject> realityObjectRepository, 
            IRepository<Fias> fiasRepository,
            IGkhConfigProvider configProvider, 
            IRepository<ServiceDictionary> serviceRepository)
        {
            Container = container;
            RealityObjectRepository = realityObjectRepository;
            FiasRepository = fiasRepository;

            ServiceDictionary = serviceRepository
                .GetAll()
                .ToDictionary(x => x.Code, x => x.Name);

            //Получение настроек сервиса
            var openKazanConfig = configProvider.Get<GisConfig>().OpenKazanIntegrationConfig;
            ServiceUri = openKazanConfig.ServiceAddress.IsNotEmpty() ? openKazanConfig.ServiceAddress : "https://rest.open.kzn.ru";
            RestToken = openKazanConfig.ServiceAddress.IsNotEmpty() ? openKazanConfig.RestToken : "gisgkh";
            RestSecret = openKazanConfig.ServiceAddress.IsNotEmpty() ? openKazanConfig.RestSecret : "abdf9ec50e7f514137f3d38f2ca2abb3f121d0c8";
            Login = openKazanConfig.ServiceAddress.IsNotEmpty() ? openKazanConfig.Login : "techbars";
            Password = openKazanConfig.ServiceAddress.IsNotEmpty() ? openKazanConfig.Password : "mykazan15";
        }

        /// <summary>
        /// Список заявок
        /// </summary>
        public IDataResult OrderList(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();
            var realityObjectId = baseParams.Params.GetAs<long>("realityObjectId");
            var date = baseParams.Params.GetAs<DateTime>("date");
            var datebegin = GetTimestamp(new DateTime(date.Year, date.Month, 1));
            var dateend = GetTimestamp(new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month)));

            var result = new ListDataResult();

            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters.Add("datestart", datebegin);
            parameters.Add("dateend", dateend);

            var authenticationInfo = Authenticate();
            if (authenticationInfo == null)
            {
                return new ListDataResult
                {
                    Message = "Аутентификация прошла неудачно",
                    Success = false
                };
            }

            //Запрос на получение списка заявок
            var webRequest = GetPreparedWebRequest("orders/barsList", "GET", parameters);
            webRequest.Headers.Add("Authorization", string.Format("{0} {1}", authenticationInfo.Key, authenticationInfo.Token));

            try
            {
                var webResponse = webRequest.GetResponse();

                using (var reader = new StreamReader(webResponse.GetResponseStream()))
                {
                    var respString = reader.ReadToEnd();
                    //Десериализованный объект с заявками, полученными от сервиса
                    var responseObject = JsonConvert.DeserializeObject<BarsListResponse>(respString);

                    //Дом, по которому требуется получить данные
                    var realityObject = RealityObjectRepository
                        .Get(realityObjectId);
                    var faddr = realityObject.FiasAddress;

                    if (faddr == null)
                    {
                        return new ListDataResult
                        {
                            Message = "Не найден ФИАС адрес, соответствующий дому",
                            Success = false
                        };
                    }

                    //Объект с адресом по ФИАСУ
                    var house = FillAddressFromFias(faddr);

                    var completedStates = new long[] { 8, 1, 2, 3, 4, 5, 13, 18, 7 };
                    var inProgressStates = new long[] { 9, 11, 16, 19, 12, 17, 14, 15, 6 };

                    var resList = responseObject
                        .Orders
                        .Rows
                        .Select(x => x.Value)
                        .WhereIf(house.City != null, x => x.City == house.City)
                        .WhereIf(house.District != null, x => x.District == house.District)
                        .WhereIf(house.Street != null, x => x.Street == house.Street)
                        .WhereIf(faddr.House != null, x => x.HouseNumber == faddr.House)
                        .WhereIf(faddr.Housing != null, x => x.BuildingNumber == faddr.Housing)
                        .GroupBy(x => x.ProblemId)
                        .Select(x => new
                        {
                            Service = ServiceDictionary.ContainsKey(x.Key)
                                ? ServiceDictionary[x.Key]
                                : "Не определено",
                            Total = x.Count(),
                            Completed = x.Count(y => y.States.Any() && completedStates.Contains(y.States.OrderBy(z => z.Value).Last().Key)),
                            InProgress = x.Count(y => y.States.Any() && inProgressStates.Contains(y.States.OrderBy(z => z.Value).Last().Key)),
                            Overdue = x.Count(y => y.PerformTime != null && y.NormalTime != null && y.PerformTime > y.NormalTime),
                            New = x.Count(y => y.States.Any() && y.States.OrderBy(z => z.Value).Last().Key == 10)
                        }).AsQueryable()
                        .Filter(loadParam, Container);

                    result = new ListDataResult(resList.Order(loadParam).Paging(loadParam).ToList(), resList.Count());
                }
            }
            catch (WebException ex)
            {
                using (var reader = new StreamReader(ex.Response.GetResponseStream()))
                {
                    var respObj = reader.ReadToEnd();
                }
            }

            return result;
        }

        /// <summary>
        /// Получить токен аутентификации
        /// </summary>
        public AuthenticationInfoOk Authenticate()
        {
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters.Add("login", Login);
            parameters.Add("password", Password);

            var webRequest = GetPreparedWebRequest("auth/login", "POST", parameters);
            try
            {
                var webResponse = webRequest.GetResponse();

                using (var reader = new StreamReader(webResponse.GetResponseStream()))
                {
                    var respString = reader.ReadToEnd();
                    return JsonConvert.DeserializeObject<AuthenticationInfoOk>(respString);
                }
            }
            catch (WebException ex)
            {
                using (var reader = new StreamReader(ex.Response.GetResponseStream()))
                {
                    var respObj = reader.ReadToEnd();
                }
            }

            return null;
        }

        /// <summary>
        /// Получить подготовленный запрос
        /// </summary>
        private HttpWebRequest GetPreparedWebRequest(string uri, string method, NameValueCollection parameters = null)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/{1}?{2}", ServiceUri, uri, parameters));
            webRequest.Headers.Add("Rest-Client", RestToken);
            webRequest.Headers.Add("Rest-Secret", RestSecret);
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            webRequest.Method = method;
            return webRequest;
        }

        /// <summary>
        /// Получить timestamp представление даты
        /// </summary>
        private static String GetTimestamp(DateTime value)
        {
            long unixTimestamp = value.Ticks - new DateTime(1970, 1, 1).Ticks;
            unixTimestamp /= TimeSpan.TicksPerSecond;
            return unixTimestamp.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Заполнить адрес дома из ФИАС'а
        /// </summary>
        private OrderInfoOk FillAddressFromFias(FiasAddress faddr)
        {
            var guid = faddr.StreetGuidId ?? faddr.PlaceGuidId;

            if (guid == null)
            {
                return null;
            }

            FiasLevelEnum level;
            var house = new OrderInfoOk();

            do
            {
                var parent = FiasRepository
                    .GetAll()
                    .Where(x => x.ActStatus == FiasActualStatusEnum.Actual && x.KladrCurrStatus == 0)
                    .FirstOrDefault(x => x.AOGuid == guid);

                if (parent == null)
                {
                    break;
                }

                var name = parent.OffName;
                level = parent.AOLevel;
                switch (level)
                {
                    case FiasLevelEnum.Street:
                        house.Street = name;
                        break;
                    case FiasLevelEnum.Place:
                    case FiasLevelEnum.City:
                        house.City = name;
                        break;
                    case FiasLevelEnum.Raion:
                        house.District = name;
                        break;
                }
                guid = parent.ParentGuid;
            } while (level != FiasLevelEnum.Region);

            return house;
        }
    }
}