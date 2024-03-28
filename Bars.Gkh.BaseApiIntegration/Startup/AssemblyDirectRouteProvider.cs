namespace Bars.Gkh.BaseApiIntegration.Startup
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using System.Web.Http.Controllers;
    using System.Web.Http.Routing;

    /// <summary>
    /// Поставщик для формирования роутов контроллеров ТОЛЬКО в указанной сборке
    /// </summary>
    public class AssemblyDirectRouteProvider : DefaultDirectRouteProvider
    {
        /// <summary>
        /// Сборка модуля
        /// </summary>
        private readonly Assembly moduleAssembly;

        public AssemblyDirectRouteProvider(Assembly moduleAssembly)
        {
            this.moduleAssembly = moduleAssembly;
        }

        /// <inheritdoc />
        public override IReadOnlyList<RouteEntry> GetDirectRoutes(
            HttpControllerDescriptor controllerDescriptor,
            IReadOnlyList<HttpActionDescriptor> actionDescriptors,
            IInlineConstraintResolver constraintResolver) =>
            this.moduleAssembly.Equals(controllerDescriptor.ControllerType.Assembly)
                ? base.GetDirectRoutes(controllerDescriptor, actionDescriptors, constraintResolver)
                : Array.Empty<RouteEntry>();
    }
}