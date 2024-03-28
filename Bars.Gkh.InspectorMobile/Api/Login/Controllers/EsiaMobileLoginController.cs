namespace Bars.Gkh.InspectorMobile.Api.Login.Controllers
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Http;
    using System.Web.Http.Description;

    using Bars.B4.IoC;
    using Bars.B4.Modules.ESIA.Auth.Dto;
    using Bars.B4.Modules.ESIA.Auth.Exceptions;
    using Bars.B4.Modules.ESIA.Auth.Services;
    using Bars.Gkh.BaseApiIntegration.Controllers;
    using Bars.Gkh.BaseApiIntegration.Models;
    using Bars.Gkh.InspectorMobile.Api.Login.Services;

    /// <summary>
    /// Контроллер для авторизации пользователя через ЕСИА
    /// </summary>
    [RoutePrefix("esiaMobileLogin")]
    [AllowAnonymous]
    public class EsiaMobileLoginController : BaseApiController
    {
        /// <summary>
        /// Получить URL для авторизации в ЕСИА
        /// </summary>
        /// <response code="502">
        /// Во время работы сервера в качестве шлюза для получения ответа, нужного для обработки запроса,
        /// получен недействительный (недопустимый) ответ
        /// </response>
        /// <response code="503">Сервер не готов обрабатывать запрос</response>
        [Route]
        [HttpGet]
        [ResponseType(typeof(BaseApiResponse<string>))]
        public IHttpActionResult Get()
        {
            Exception exception = null;
            try
            {
                var authAppIntegrationService = this.Container.Resolve<IAuthAppIntegrationService>();

                using (this.Container.Using(authAppIntegrationService))
                {
                    var redirectUrl = authAppIntegrationService.GetRedirectUrl(HttpContext.Current.Request.Url.AbsoluteUri + "/authCode");

                    this.LogManager.Debug("Пользователем МП запрошен URL для авторизации через ЕСИА");
                    return this.Ok(new BaseApiResponse { Data = redirectUrl });
                }
            }
            catch (BadGatewayException e)
            {
                exception = e;
                return this.StatusCode(HttpStatusCode.BadGateway);
            }
            catch (ServiceUnavailableException e)
            {
                exception = e;
                return this.StatusCode(HttpStatusCode.ServiceUnavailable);
            }
            catch (Exception e)
            {
                exception = e;
                return this.InternalServerError(e);
            }
            finally
            {
                if (exception != null)
                {
                    this.LogManager.Error("Ошибка авторизации пользователя МП через ЕСИА", exception);
                }
            }
        }

        /// <summary>
        /// Авторизовать пользователя, используя авторизационный код
        /// </summary>
        /// <param name="code">Авторизационный код</param>
        /// <returns>
        /// В случае успеха возвращает пустой ответ с кукками авторизации, либо ответ в формате <see cref="BaseApiResponse"/> со списком организаций пользователя в ЕСИА
        /// В случае ошибки возвращает ответ в формате <see cref="BaseApiResponse"/> с текстом ошибки
        /// </returns>
        [Route("authCode")]
        [HttpGet]
        [ResponseType(typeof(BaseApiResponse<OrganizationInfoDto[]>))]
        public async Task<IHttpActionResult> AuthCode(string code)
        {
            var oAuthLoginService = this.Container.Resolve<IOAuthLoginService>();
            using (this.Container.Using(oAuthLoginService))
            {
                var userInfoGetting = oAuthLoginService.GetUserInfo(code);
                if (userInfoGetting.Success)
                {
                    return this.Ok();
                }

                if (userInfoGetting.Data?.Organizations?.Count < 1)
                {
                    return this.ErrorWithLog(userInfoGetting.Message);
                }

                var mobileServiceLogin = this.Container.Resolve<IMobileLoginService>();
                using (this.Container.Using(mobileServiceLogin))
                {
                    var authorizeResult = await mobileServiceLogin.AuthorizeEsiaUser(userInfoGetting.Data);
                    if (!authorizeResult.Success)
                    {
                        return this.ErrorWithLog(authorizeResult.Message);
                    }

                    var matchingResult = oAuthLoginService.MatchOrganizationWithContragent(userInfoGetting.Data);
                    if (!matchingResult.Success)
                    {
                        return this.ErrorWithLog(matchingResult.Message);
                    }

                    var loginResult = oAuthLoginService.PerformLoginActions(userInfoGetting.Data);
                    if (!loginResult.Success)
                    {
                        return this.ErrorWithLog(loginResult.Message);
                    }

                    return this.Ok();
                }
            }
        }

        /// <summary>
        /// Сформировать ответ с ошибкой и залогировать сообщение
        /// </summary>
        /// <param name="message">Текст ошибки</param>
        /// <returns>Ответ с текстом ошибки в формате <see cref="BaseApiResponse"/></returns>
        private IHttpActionResult ErrorWithLog(string message)
        {
            this.LogManager.Debug($"Не удалось авторизовать пользователя МП через ЕСИА: {message}");
            return this.ErrorResult(message);
        }
    }
}