namespace Bars.Gkh.InspectorMobile
{
    using System.Web.Http;

    using Bars.Gkh.BaseApiIntegration.Startup;
    using Bars.Gkh.InspectorMobile.Attributes;

    /// <summary>
    /// Конфигурация WebApi приложения
    /// </summary>
    public class Startup : AssemblyApiStartup
    {
        /// <inheritdoc />
        protected override string ApiPrefix => "inspectorMobile";

        /// <inheritdoc />
        protected override void AdditionalConfigure(HttpConfiguration config)
        {
            // добавляем атрибут, как глобальный фильтр для API мобилки
            config.Filters.Add(new MobileConfigurationAttribute());
        }
    }
}