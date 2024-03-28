namespace Bars.B4.Modules.ESIA.Auth.Handlers
{
    using System.Web;

    /// <summary>
    /// Обработчик логаута
    /// </summary>
    public class OAuthLogOutHandler : IHttpHandler
    {
        /// <inheritdoc />
        public void ProcessRequest(HttpContext context)
        {
            var request = HttpContext.Current.Request;
            var appUrl = HttpRuntime.AppDomainAppVirtualPath;

            if (appUrl != "/")
                appUrl = "/" + appUrl;

            var baseUrl = $"{request.Url.Scheme}://{request.Url.Authority}{appUrl}";

            context.Response.Redirect(baseUrl);
        }

        /// <inheritdoc />
        public bool IsReusable => true;
    }
}