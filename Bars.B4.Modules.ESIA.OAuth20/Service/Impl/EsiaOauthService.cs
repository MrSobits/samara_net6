namespace Bars.B4.Modules.ESIA.OAuth20.Service.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;

    using Bars.B4.Application;
    using Bars.B4.Logging;
    using Bars.B4.Modules.ESIA.OAuth20.Entities;

    using Castle.Windsor;

    using EsiaNET;

    using Newtonsoft.Json.Linq;
    using static System.Net.WebRequestMethods;

    /// <summary>
    /// Сервис авторизации в ЕСИА через OAuth
    /// </summary>
    public class EsiaOauthService : IEsiaOauthService
    {
        public IWindsorContainer Container { get; set; }
        public ILogManager LogManager { get; set; }

        /// <summary>
        /// Обработать ответ от ЕСИА
        /// </summary>
        public IDataResult<EsiaUserInfo> HandleEsiaCallback(HttpContext context, string state, string code)
        {
            int g = 0;
            var esiaClient = this.Container.Resolve<EsiaClient>();
            g++;
            var esiaClientSobt = this.Container.Resolve<EsiaClientSobits>();
            g++;

            try
            {
                if (state == null)
                {
                    return this.Error("Не передан код состояния");
                }
                g++;
                if (code == null)
                {
                    return this.Error("Не передан код авторизации");
                }
                SendStyles styles = SendStyles.None;
                //Получаем токен авторизации
                g++; //1
                var tokenResponse = Task.Run(() => esiaClientSobt.GetOAuthTokenAsync(code)).Result;
                g++; //2
                esiaClient.Token = EsiaClient.CreateToken(tokenResponse);
                g++; //3
                //Получаем данные по пользователю
                var personInfo = Task.Run(() => esiaClient.GetPersonInfoAsync(styles)).Result;
                g++; //4
                //Собираем объект с данными пользователя
                var userInfo = new EsiaUserInfo
                {
                    Id = esiaClient.Token.SbjId,
                    LastName = personInfo.LastName,
                    FirstName = personInfo.FirstName,
                    MiddleName = personInfo.MiddleName,
                };
                g++; //5
                if (!personInfo.Trusted)
                {
                    return this.Error("Учетная запись не подтверждена", userInfo);
                }
                g++; //6
                //Получаем данные по организациях
                var organizationsInfo = this.GetPersonOrganizations(esiaClient);
                g++; //7
                userInfo.OrganizationsList = organizationsInfo;
                g++; //8
                if (organizationsInfo == null || !organizationsInfo.Any())
                {
                    return this.Error("В ЕСИА не привязана организация", userInfo);
                }
                g++; //9
                if (organizationsInfo?.Count() > 1)
                {
                    return this.Error(
                            "У пользователя зарегистрировано несколько организаций. Пожалуйста, выберите организацию, от имени которой вы будете работать в системе",
                            userInfo);
                }
                g++; //10
                //Если организация одна, то устанавлиаем её огрн
                userInfo.SelectedOrganizationKey = organizationsInfo.Select(x => x.FullName + "###" + x.Ogrn).First();

                //Выполнить действия логина
                var loginResult = this.PerformLoginActions(userInfo, context);
                if (!loginResult.Success)
                {
                    return this.Error(loginResult.Message);
                }
            }
            catch (Exception e)
            {
                if (e.InnerException != null && e.InnerException.Message == "Token start time has not come")
                {
                    return this.Error("Ошибка логина через OAuth. Возможно, причина в отставании времени на сервере приложения от времени ЕСИА");
                }
                EmailSender.Instance.TrySendIfLogEnabled(e.Message, e.InnerException + "\r\n" + e.StackTrace);
                this.LogManager.Error("Ошибка логина через OAuth", e);
                return this.Error("Ошибка отправки запроса к ЕСИА: " + g + e.Message + e.InnerException + e.StackTrace);
            }
            finally
            {
                this.Container.Release(esiaClient);
            }

            return new GenericDataResult<EsiaUserInfo>(message: "Пользователь успешно авторизован");
        }


        /// <summary>
        /// Выполнить действия логина
        /// </summary>
        public IDataResult PerformLoginActions(EsiaUserInfo userInfo, HttpContext context)
        {
            //Получаем действия, выполняемые при логине
            var loginActions = this.Container.ResolveAll<IEsiaOauthLoginAction>();

            try
            {
                foreach (var loginAction in loginActions)
                {
                    //Выполняем логин
                    var res = loginAction.PerformLogin(userInfo, context);

                    if (!res.Success)
                    {
                        return res;
                    }
                }
            }
            catch (Exception e)
            {
                EmailSender.Instance.TrySendIfLogEnabled(e.Message, e.InnerException + "\r\n" + e.StackTrace);
                this.LogManager.Error("Ошибка выполнения действия логина", e);
                return BaseDataResult.Error("Ошибка выполнения действия логина");
            }
            finally
            {
                foreach (var loginAction in loginActions)
                {
                    this.Container.Release(loginAction);
                }
            }

            return new BaseDataResult();
        }

        private IDataResult<EsiaUserInfo> Error(string message, EsiaUserInfo userInfo = null)
        {
            return new GenericDataResult<EsiaUserInfo>
            {
                Success = false,
                Message = message,
                Data = userInfo
            };
        }

        /// <summary>
        /// Запросить список организаций юзера
        /// </summary>
        private List<OrganizationInfo> GetPersonOrganizations(EsiaClient client, SendStyles styles = SendStyles.None)
        {
            if (string.IsNullOrEmpty(client.Token.SbjId))
                throw new ArgumentNullException("oid");
            var format = "{0}{1}";
            var str1 = EsiaHelpers.NormalizeUri(client.Options.RestUri, client.Options.PrnsSfx);
            var str2 = client.Token.SbjId;
            var url = string.Format(format, str1, str2) + "/roles";

            //запрашиваем список организаций
            var res = Task.Run(() => client.GetAsync(url, styles)).Result;

            //парсим результат
            var jObject = JObject.Parse(res);
            var organizationList = new OrganizationInfoList(jObject);

            //дополняем
            return organizationList.Organizations.ToList();//.Select(x => GetOrganizationInfo(client, x, styles)).ToList();
        }

        /// <summary>
        /// Запросить инфу по организации
        /// </summary>
        private OrganizationInfo GetOrganizationInfo(EsiaClient client, OrganizationInfo info, SendStyles styles = SendStyles.Normal)
        {
            var format = "{0}{1}";
            var str1 = EsiaHelpers.NormalizeUri(client.Options.RestUri, client.Options.OrgsSfx);
            var str2 = info.Id;
            var url = string.Format(format, str1, str2);

            var res = Task.Run(() => client.GetAsync(url, styles)).Result;

            var jObject = JObject.Parse(res);
            var organization = new OrganizationInfo(jObject);
            return organization;
        }
    }
}
