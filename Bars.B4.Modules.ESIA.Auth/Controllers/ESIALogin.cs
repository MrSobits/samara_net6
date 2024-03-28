namespace Bars.B4.Modules.ESIA.Auth.Controllers
{
    using Bars.B4.Config;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Security.Web.Controller;
    using Bars.B4.Utils;
    using DataAccess;
    using Gkh.Entities.Administration.Operator;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using System.Web.Security;

    /// <summary>
    /// Контроллер ЕСИА авторизации
    /// </summary>
    public class ESIALoginController : LoginController
    {
        /// <summary>
        /// Авторизовать пользователя
        /// </summary>
        /// <param name="login">Логин</param>
        /// <param name="password">Пароль</param>
        /// <param name="forceAuth">Принудительная авторизация</param>
        /// <returns>Результат запроса</returns>
        public override ActionResult Index(string login, string password, string forceAuth)
        {
            var result = base.Index(login, password, forceAuth);

            var view = result as ViewResult;
            if (view != null && view.ViewName == string.Empty)
            {
                view.ViewName = "ESIA";
            }

            this.SetOauthSpecified();

            return result;
        }

        /// <summary>
        /// Вернуть представление авторизации
        /// </summary>
        /// <returns>Результат запроса</returns>
        public override ActionResult Index()
        {

            var result = base.Index();

            this.SetOauthSpecified();

            return this.View("ESIA");
        }

        /// <summary>
        /// Установить флаг, указан ли в конфиге OAuth
        /// </summary>
        private void SetOauthSpecified()
        {
            var configProvider = this.Container.Resolve<IConfigProvider>();

            using (this.Container.Using(configProvider))
            {
                if (configProvider.GetConfig().ModulesConfig.ContainsKey("Bars.B4.Modules.ESIA.OAuth20"))
                {
                    this.ViewData["oauthSpecified"] = true;
                }

                this.ViewData["HideLoginButton"] = configProvider.GetConfig().AppSettings.GetAs("HideLoginButton", false);
            }
        }

        /// <summary>
        /// Выйти из системы
        /// </summary>
        /// <returns>Результат запроса</returns>
        public override ActionResult LogOut()
        {
            var esiaOperatorDomain = this.Container.ResolveDomain<EsiaOperator>();
            var userIdentity = this.Container.Resolve<IUserIdentity>();

            try
            {
                var isEsiaAuth = this.IsEsiaAuth();
                var userId = userIdentity.UserId;

                if (this.Container.Kernel.HasComponent(typeof(IAuthenticationService)))
                {
                    var authorizationService = this.Container.Resolve<IAuthenticationService>();
                    authorizationService.Logout();
                }

                FormsAuthentication.SignOut();

                if (isEsiaAuth && esiaOperatorDomain.GetAll().FirstOrDefault(x => x.Operator.User.Id == userId) != null)
                {
                    return this.Redirect("~/esia/logout.ashx");
                }

                return this.Redirect("~/login");
            }
            finally
            {
                this.Container.Release(esiaOperatorDomain);
                this.Container.Release(userIdentity);
            }
        }

        private bool IsEsiaAuth()
        {
            var authCookie = this.Request.Cookies[FormsAuthentication.FormsCookieName];

            if (authCookie.IsNotNull() && authCookie.Value.IsNotEmpty())
            {
                var ticket = FormsAuthentication.Decrypt(authCookie.Value);
                if (ticket.IsNotNull())
                {
                    var userData = DynamicDictionary.FromJson(ticket.UserData);
                    return userData.GetAs("EsiaAuth", false);
                }
            }

            return false;
        }
    }
}