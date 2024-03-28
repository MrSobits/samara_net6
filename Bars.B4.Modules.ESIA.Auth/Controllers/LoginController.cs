namespace Bars.B4.Modules.ESIA.Auth.Controllers
{
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.Security;

    using Bars.B4.Config;
    using Bars.B4.IoC;
    using Bars.B4.Modules.ESIA.Auth.Entities;
    using Bars.B4.Utils;

    using DataAccess;

    /// <summary>
    /// Подмененная реализация login-контроллера
    /// </summary>
    public class LoginController : Bars.B4.Modules.Security.Web.Controller.LoginController
    {
        /// <summary>
        /// Авторизовать пользователя
        /// </summary>
        public override ActionResult Index(string login, string password, string forceAuth)
        {
            var result = base.Index(login, password, forceAuth);

            var view = result as ViewResult;
            if (view != null && view.ViewName == string.Empty)
            {
                view.ViewName = "esia";
            }

            this.SetOAuthSpecified();

            return result;
        }

        /// <summary>
        /// Вернуть представление авторизации
        /// </summary>
        public override ActionResult Index()
        {
            base.Index();

            this.SetOAuthSpecified();

            return this.View("esia");
        }

        /// <summary>
        /// Установить настройки
        /// </summary>
        private void SetOAuthSpecified()
        {
            var configProvider = this.Container.Resolve<IConfigProvider>();

            using (this.Container.Using(configProvider))
            {
                this.ViewData["HideLoginButton"] = configProvider.GetConfig().AppSettings.GetAs("HideLoginButton", false);
            }
        }

        /// <summary>
        /// Выйти из системы
        /// </summary>
        public override ActionResult LogOut()
        {
            var esiaOperatorDomain = this.Container.ResolveDomain<EsiaOperator>();

            var userIdentity = this.Container.Resolve<IUserIdentity>();

            using (this.Container.Using(esiaOperatorDomain, userIdentity))
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
                    return this.Redirect("~/esia/oauthlogout.ashx");
                }

                return this.Redirect("~/login");
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