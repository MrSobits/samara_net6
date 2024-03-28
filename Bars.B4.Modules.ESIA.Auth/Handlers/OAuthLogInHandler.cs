namespace Bars.B4.Modules.ESIA.Auth.Handlers
{
    using System;
    using System.Web;

    using Bars.B4.Application;
    using Bars.B4.IoC;
    using Bars.B4.Logging;
    using Bars.B4.Modules.ESIA.Auth.Services;

    using Castle.Windsor;

    /// <summary>
    /// Обработчик логина
    /// </summary>
    public class OAuthLogInHandler : IHttpHandler
    {
        private IWindsorContainer Container = ApplicationContext.Current.Container;

        /// <inheritdoc />
        public void ProcessRequest(HttpContext context)
        {
            try
            {
                var authAppIntegrationService = this.Container.Resolve<IAuthAppIntegrationService>();

                using (this.Container.Using(authAppIntegrationService))
                {
                    var redirectUrl = authAppIntegrationService.GetRedirectUrl();
                    
                    // Редирект с корректным завершением текущего потока
                    context.Response.Redirect(redirectUrl, false);
                    context.ApplicationInstance.CompleteRequest();
                }
            }
            catch (Exception e)
            {
                var logManager = this.Container.Resolve<ILogManager>();
                logManager.Error("Ошибка логина через OAuth", e);
                context.Response.Write(e.Message);
            }
        }

        /// <inheritdoc />
        public bool IsReusable => true;
    }
}