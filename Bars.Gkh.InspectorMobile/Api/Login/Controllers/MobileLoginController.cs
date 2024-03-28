namespace Bars.Gkh.InspectorMobile.Api.Login.Controllers
{
    using System.Web.Http;
    using System.Web.Http.Description;

    using Bars.Gkh.BaseApiIntegration.Controllers;
    using Bars.Gkh.BaseApiIntegration.Models;
    using Bars.Gkh.InspectorMobile.Api.Login.Services;

    /// <summary>
    /// Контроллер для авторизации пользователя в мобильном приложении
    /// </summary>
    [RoutePrefix("mobileLogin")]
    public class MobileLoginController : BaseApiLoginController<IMobileLoginService>
    {
        /// <summary>
        /// Авторизовать пользователя
        /// </summary>
        /// <param name="requestModel">Логин и пароль пользователя</param>
        /// <returns>
        /// - В случае успеха вернется пустой ответ с кукками авторизации
        /// - В случае неудачи вернется ответ в формате <see cref="BaseApiResponse"/> с текстом ошибки
        /// </returns>
        [HttpPost]
        [Route]
        [ResponseType(typeof(BaseApiResponse))]
        public IHttpActionResult Post([FromBody] LoginRequestModel requestModel) =>
            this.Login(requestModel);

        /// <summary>
        /// Выйти из системы
        /// </summary>
        [HttpPost]
        [Route("logOut")]
        public IHttpActionResult LogOut() =>
            this.Logout();
    }
}