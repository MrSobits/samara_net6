namespace Bars.Gkh.Authentification
{
    using System;
    using System.Security.Principal;
    using System.Text;

    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.Modules.Security.Web.Service;

    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc.Filters;

    public class BasicAuthorizationAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Авторизовать пользователя в сессии
        /// </summary>
        public bool SessionAuth { get; set; }

        public IHttpContextAccessor HttpContextAccessor { get; set; }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                return;
            }

            var authData = this.ParseRequest(filterContext.HttpContext.Request);
            if (authData == null)
            {
                filterContext.Result = new NotAuthorizedActionResult();
            }

            var username = authData.Item1;
            var password = authData.Item2;
            var authResult = AuthService.AuthenticationUser(username, password);
            if (!authResult.Success)
            {
                filterContext.Result = new NotAuthorizedActionResult(authResult.Error);
                return;
            }

            var userIdentity = this.GetUserIdentity(authResult);

            if (this.SessionAuth)
            {
                this.AuthUser(userIdentity, filterContext.HttpContext);
            }
        }

        private Tuple<string, string> ParseRequest(HttpRequest request)
        {
            var auth = request.Headers["Authorization"].ToString();
            if (!string.IsNullOrEmpty(auth) && auth.StartsWith("Basic "))
            {
                var decodedToken = Encoding.ASCII.GetString(Convert.FromBase64String(auth.Substring(6)));
                var username = decodedToken.Substring(0, decodedToken.IndexOf(":", StringComparison.Ordinal));
                var password = decodedToken.Substring(decodedToken.IndexOf(":", StringComparison.Ordinal) + 1);

                return Tuple.Create(username, password);
            }

            return null;
        }

        private IUserIdentity GetUserIdentity(AuthenticationResult authResult)
        {
            return new UserIdentity(authResult.UserId, authResult.UserName, authResult.TrackId, authResult.UserData, "Basic");
        }

        protected void AuthUser(IUserIdentity userIdentity, HttpContext context)
        {
            context.User = new GenericPrincipal(userIdentity, null);
            var requestingUserInformation = ApplicationContext.Current.Container.Resolve<RequestingUserInformation>();
            requestingUserInformation.UserIdentity = userIdentity;
            requestingUserInformation.RequestIpAddress = HttpContextAccessor.HttpContext?.Connection.RemoteIpAddress.ToString();
        }
    }
}
