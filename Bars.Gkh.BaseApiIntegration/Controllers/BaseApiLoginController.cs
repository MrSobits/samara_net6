namespace Bars.Gkh.BaseApiIntegration.Controllers
{
    using System;
    using System.Globalization;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Reflection;
    using System.Web.Http;
    using System.Web.Security;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Security.Web.Service;
    using Bars.Gkh.BaseApiIntegration.Models;
    using Bars.Gkh.BaseApiIntegration.Services;

    using Castle.Core.Internal;

    /// <inheritdoc />
    public class BaseApiLoginController : BaseApiLoginController<IApiLoginService>
    {
    }

    /// <summary>
    /// Контроллер для авторизации пользователя
    /// </summary>
    /// <remarks>
    /// Для аутентификации пользователя используется стандартный функционал платформы B4.
    /// Кастом заключается в дополнительных проверках при авторизации и способом добавлении кукки в ответе 
    /// </remarks>
    [AllowAnonymous]
    public class BaseApiLoginController<TIApiLoginService> : BaseApiController
        where TIApiLoginService : IApiLoginService
    {
        /// <summary>
        /// Авторизовать пользователя
        /// </summary>
        /// <param name="requestModel">Логин и пароль пользователя</param>
        /// <returns>
        /// - В случае успеха вернется пустой ответ с кукками авторизации
        /// - В случае неудачи вернется ответ в формате <see cref="BaseApiResponse"/> с текстом ошибки
        /// </returns>
        protected IHttpActionResult Login(LoginRequestModel requestModel)
        {
            if (requestModel == null || requestModel.Login.IsNullOrEmpty() || requestModel.Password.IsNullOrEmpty())
            {
                return this.BadRequest("Один или несколько параметров не переданы: логин, пароль пользователя");
            }

            var logMsg = $"{Assembly.GetCallingAssembly().GetName().Name}|Попытка входа пользователя {requestModel.Login}: ";

            // аутентификация
            var authResult = AuthService.AuthenticationUser(requestModel.Login, requestModel.Password);
            if (!authResult.Success)
            {
                this.LogManager.Debug(logMsg + "не пройдена аутентификация");
                return this.ErrorResult("Логин не найден или логин не соответствует паролю.");
            }

            var service = this.Container.Resolve<TIApiLoginService>();
            using (this.Container.Using(service))
            {
                // авторизация
                var authorizeResult = service.AuthorizeUser(authResult.UserId);
                if (!authorizeResult.Success)
                {
                    this.LogManager.Debug(logMsg + authorizeResult.Message);
                    return this.ErrorResult(authorizeResult.Message);
                }
            }

            // формируем и добавляем кукки в ответе
            var resp = new HttpResponseMessage();

            var now = DateTime.Now;
            var expiration = now.AddMinutes(Bars.B4.Modules.Security.Web.Settings.AuthenticationTimeout);
            var userData = JsonNetConvert.SerializeObject(this.Container, authResult.UserData);
            var ticket = new FormsAuthenticationTicket(2, authResult.UserId.ToString(CultureInfo.InvariantCulture), now, expiration, true, userData);

            var cookie = new CookieHeaderValue(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket))
            {
                Expires = ticket.Expiration
            };

            resp.Headers.AddCookies(new[] { cookie });

            this.LogManager.Debug(logMsg + "успех");
            return this.ResponseMessage(resp);
        }

        /// <summary>
        /// Выйти из системы
        /// </summary>
        protected IHttpActionResult Logout()
        {
            AuthService.LogOut(this.Container.Resolve<IUserIdentity>());
            return this.Ok();
        }
    }
}