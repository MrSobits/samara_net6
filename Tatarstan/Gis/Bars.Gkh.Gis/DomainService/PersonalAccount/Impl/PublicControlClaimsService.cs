namespace Bars.Gkh.Gis.DomainService.House.Claims.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
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
    using Entities.PersonalAccount.PublicControlClaims;
    using Gkh.Entities;
    using Newtonsoft.Json;

    /// <summary>
    /// Сервис получения претензий по лицевому счету от Народного Контроля
    /// </summary>
    public class PublicControlClaimsService : IPublicControlClaimsService
    {
        protected string ServiceUri;
        protected string AppToken;

        protected IWindsorContainer Container;
        protected IRepository<RealityObject> RealityObjectRepository;
        protected IRepository<Fias> FiasRepository;
        protected Dictionary<int, string> ServiceDictionary;

        public PublicControlClaimsService(IWindsorContainer container,
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
            var publicControlConfig = configProvider.Get<GisConfig>().PublicControlIntegrationConfig;
            ServiceUri = publicControlConfig.ServiceAddress.IsNotEmpty() ? publicControlConfig.ServiceAddress : "https://api.tatar.ru/open-gov";
            AppToken = publicControlConfig.ServiceAddress.IsNotEmpty() ? publicControlConfig.AppToken : "ed8f5b7e74398143b43a93dc753618ae";
        }

        /// <summary>
        /// Список заявок
        /// </summary>
        public IDataResult OrderList(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();
            //внутренний идентификатор лицевого счета
            var personalAccountId = baseParams.Params.GetAs<long>("apartmentId");
            var realityObjectId = baseParams.Params.GetAs<long>("realityObjectId");
            var month = baseParams.Params.GetAs<int>("month");
            var year = baseParams.Params.GetAs<int>("year");
            var dateStart = new DateTime(year, month, 1);
            var dateEnd = new DateTime(year, month + 1, 1).AddDays(-1);

            var authenticationInfo = Authenticate();
            if (authenticationInfo == null)
            {
                return new ListDataResult
                {
                    Message = "Не удалось получить токен",
                    Success = false
                };
            }

            //ищем лицевой счет
            var gisPersonalAccount = (Container.Resolve<IGisHouseService>("GisHouseService")
                .GetHousePersonalAccounts(realityObjectId, dateStart) ??
                                      Container.Resolve<IGisHouseService>("KpHouseService")
                                          .GetHousePersonalAccounts(realityObjectId, dateStart)).FirstOrDefault(
                                              x => x.Id == personalAccountId);
            if (gisPersonalAccount == null)
                return null;
            //получаем платежный код лицевого счета
            var accountNumber =
                gisPersonalAccount.PaymentCode.ToString("####");
                 if (String.IsNullOrEmpty(accountNumber)) return new ListDataResult
            {
                Message = "Не определен платежный код лицевого счета",
                Success = false
            };


            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters.Add("account_number", accountNumber);
            parameters.Add("from_date", dateStart.ToString("yyyy-MM-dd"));
            parameters.Add("to_date", dateEnd.ToString("yyyy-MM-dd"));
            parameters.Add("token", authenticationInfo.Data.Token);

            //Запрос на получение списка заявок
            var webRequest = GetPreparedWebRequest("orders.json", "GET", parameters);

            ListDataResult result;

            var webResponse = webRequest.GetResponse();

            using (var reader = new StreamReader(webResponse.GetResponseStream()))
            {
                var respString = reader.ReadToEnd();

                //Десериализованный объект с заявками, полученными от сервиса
                var responseObject = JsonConvert.DeserializeObject<List<PublicControlResponse>>(respString);

                var resList = responseObject
                    .Select(x => new
                    {
                        x.Id,
                        x.CategoryName,
                        x.Address,
                        x.Territory,
                        x.OrganizationName,
                        x.StateName,
                        CreatedDate = x.CreatedDate.ToDateTime(),
                        UpdateDate = x.UpdateDate.ToDateTime()
                    })
                    .AsQueryable()
                    .OrderIf(loadParam.Order.Length == 0, true, x => x.CreatedDate)
                    .OrderThenIf(loadParam.Order.Length == 0, true, x => x.CategoryName)
                    .Filter(loadParam, Container);

                result = new ListDataResult(resList.Order(loadParam).Paging(loadParam).ToList(), resList.Count());
            }

            return result;

        }

        /// <summary>
        /// Получить токен аутентификации
        /// </summary>
        public AuthenticationInfoPublicControl Authenticate()
        {
            var parameters = HttpUtility.ParseQueryString(string.Empty);
            parameters.Add("user_type", "municipal_housing");
            parameters.Add("app_token", AppToken);

            var webRequest = GetPreparedWebRequest("sessions.json", "GET", parameters);

            var webResponse = webRequest.GetResponse();

            using (var reader = new StreamReader(webResponse.GetResponseStream()))
            {
                var respString = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<AuthenticationInfoPublicControl>(respString);
            }
        }

        /// <summary>
        /// Получить подготовленный запрос
        /// </summary>
        private HttpWebRequest GetPreparedWebRequest(string uri, string method, NameValueCollection parameters = null)
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(string.Format("{0}/{1}?{2}", ServiceUri, uri, parameters));
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
            webRequest.Method = method;
            return webRequest;
        }
    }
}