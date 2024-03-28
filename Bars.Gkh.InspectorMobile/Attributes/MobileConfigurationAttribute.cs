namespace Bars.Gkh.InspectorMobile.Attributes
{
    using System.Net;
    using System.Net.Http;
    using System.Text;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;

    using Bars.B4.Application;
    using Bars.Gkh.Config;
    using Bars.Gkh.InspectorMobile.ConfigSections;

    /// <summary>
    /// Атрибут для контроллеров МП для проверки, что интеграция с МП включена
    /// </summary>
    public class MobileConfigurationAttribute : ActionFilterAttribute
    {
        private readonly IGkhConfigProvider gkhConfigProvider;

        /// <inheritdoc cref="MobileConfigurationAttribute" />
        public MobileConfigurationAttribute()
        {
            this.gkhConfigProvider = ApplicationContext.Current.Container.Resolve<IGkhConfigProvider>();
        }

        /// <inheritdoc />
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            base.OnActionExecuting(actionContext);

            var isMobileIntegrationEnabled = this.gkhConfigProvider.Get<InspectorMobileConfig>().IntegrationEnabled;
            if (!isMobileIntegrationEnabled)
            {
                actionContext.Response = new HttpResponseMessage(HttpStatusCode.Forbidden)
                {
                    Content = new StringContent("Интеграция с Системой отключена", Encoding.UTF8)
                };
            }
        }
    }
}