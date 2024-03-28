namespace Bars.B4.Modules.ESIA.Auth.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    using Bars.B4.Modules.ESIA.Auth.Dto;
    using Bars.B4.Modules.ESIA.Auth.Services;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;

    /// <summary>
    /// Контроллер ЕСИА OAuth авторизации
    /// </summary>
    public class OAuthLoginController : BaseController
    {
        public IOAuthLoginService OAuthOauthService { get; set; }

        public ActionResult Index(UserInfoDto userInfo, IEnumerable<string> organizationIds, IEnumerable<string> contragentIds)
        {
            if (userInfo.Id == null)
            {
                this.PrepareUserInfo();
                return this.View();
            }

            if (organizationIds != null)
            {
                userInfo.SelectedOrganizationId = organizationIds.Single();
            }

            if (contragentIds != null)
            {
                userInfo.SelectedContragentId = contragentIds.Single().ToLong();
            }
            else
            {
                // Сопоставление выбранной организации с контрагентом
                var contragentMatching = this.OAuthOauthService.MatchOrganizationWithContragent(userInfo);

                if (!contragentMatching.Success)
                {
                    this.PrepareViewData(userInfo, contragentMatching.Message);

                    return this.View();
                }
            }

            return this.PerformLoginActions(userInfo);
        }

        /// <summary>
        /// Привязать аккаунт ЕСИА
        /// </summary>
        public ActionResult LinkEsiaAccount(BaseParams baseParams)
        {
            return this.OAuthOauthService.LinkEsiaAccount(baseParams, this.Request).ToJsonResult();
        }

        /// <summary>
        /// Подготовить данные о пользователе
        /// </summary>
        private void PrepareUserInfo()
        {
            var parameters = this.Request.Params;
            var code = parameters["code"];

            // Получение информации о пользователе
            var userInfoGetting = this.OAuthOauthService.GetUserInfo(code);

            if (!userInfoGetting.Success)
            {
                this.PrepareViewData(userInfoGetting.Data, userInfoGetting.Message);
            }
            else
            {
                this.RedirectToMain();
            }
        }

        /// <summary>
        /// Подготовить данные представления
        /// </summary>
        private void PrepareViewData(UserInfoDto userInfo, string errorMessage)
        {
            this.ViewData["userInfo"] = userInfo;
            
            this.ViewData["error"] = errorMessage;

            if (userInfo == null)
            {
                return;
            }

            if (userInfo.SelectedContragentId == null &&
                userInfo.MatchedContragents != null &&
                userInfo.MatchedContragents.Count > 1)
            {
                this.ViewData["contragents"] = userInfo.MatchedContragents
                    .Select(x => new SelectListItem
                    {
                        Text = x.ShortName,
                        Value = x.Id.ToString()
                    });
            }

            if (userInfo.SelectedOrganizationId == null &&
                userInfo.Organizations != null &&
                userInfo.Organizations.Count > 1)
            {
                this.ViewData["organizations"] = userInfo
                    .Organizations
                    .Select(x => new SelectListItem
                    {
                        Text = x.FullName,
                        Value = x.Id
                    });
            }
        }

        /// <summary>
        /// Выполнить действия логина
        /// </summary>
        private ActionResult PerformLoginActions(UserInfoDto userInfo)
        {
            var loginResult = this.OAuthOauthService.PerformLoginActions(userInfo);

            if (!loginResult.Success)
            {
                this.ViewData["error"] = loginResult.Message;
                return this.View();
            }

            return this.RedirectToMain();
        }

        /// <summary>
        /// Редирект на главную страницу
        /// </summary>
        private ActionResult RedirectToMain()
        {
            var appUrl = HttpRuntime.AppDomainAppVirtualPath;

            if (appUrl != "/")
            {
                appUrl = "/" + appUrl;
            }

            var baseUrl = $"{this.Request.Url?.Scheme}://{this.Request.Url?.Authority}{appUrl}";

            this.Response.Redirect(baseUrl);

            return new ContentResult();
        }
    }
}