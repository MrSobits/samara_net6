namespace Bars.Gkh.BaseApiIntegration.Startup
{
    using System.Web.Http.Controllers;

    /// <summary>
    /// Resolver для получения ТОЛЬКО тех контроллеров, которые наследованы от TBaseController
    /// </summary>
    /// <typeparam name="TBaseApiController">Тип базового контроллера</typeparam>
    public class TypedHttpControllerTypeResolver<TBaseApiController> :
        DefaultHttpControllerTypeResolver
        where TBaseApiController : IHttpController
    {
        /// <inheritdoc />
        public TypedHttpControllerTypeResolver()
            : base(type => DefaultHttpControllerTypeResolver.IsControllerType(type) && typeof(TBaseApiController).IsAssignableFrom(type))
        {
        }
    }
}