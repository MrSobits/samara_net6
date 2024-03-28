namespace Bars.Gkh.BaseApiIntegration.Startup
{
    using System;
    using System.Collections.Generic;
    using System.Web.Http.Controllers;
    using System.Web.Http.Routing;

    /// <summary>
    /// Поставщик для формирования роутов ТОЛЬКО тех контроллеров, которые наследованы от TBaseController
    /// </summary>
    /// <typeparam name="TBaseApiController">Тип базового контроллера</typeparam>
    public class TypedDirectRouteProvider<TBaseApiController> :
        DefaultDirectRouteProvider
        where TBaseApiController : IHttpController
    {
        /// <inheritdoc />
        public override IReadOnlyList<RouteEntry> GetDirectRoutes(
            HttpControllerDescriptor controllerDescriptor,
            IReadOnlyList<HttpActionDescriptor> actionDescriptors,
            IInlineConstraintResolver constraintResolver) =>
            typeof(TBaseApiController).IsAssignableFrom(controllerDescriptor.ControllerType)
                ? base.GetDirectRoutes(controllerDescriptor, actionDescriptors, constraintResolver)
                : Array.Empty<RouteEntry>();
    }
}