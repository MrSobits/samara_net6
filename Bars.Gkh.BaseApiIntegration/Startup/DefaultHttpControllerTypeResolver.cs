namespace Bars.Gkh.BaseApiIntegration.Startup
{
    using System;
    using System.Web.Http.Controllers;
    using System.Web.Http.Dispatcher;

    /// <summary>
    /// Переопределение <see cref="System.Web.Http.Dispatcher.DefaultHttpControllerTypeResolver"/> для возможности использования защищенных методов
    /// </summary>
    public class DefaultHttpControllerTypeResolver : System.Web.Http.Dispatcher.DefaultHttpControllerTypeResolver
    {
        /// <inheritdoc />
        protected DefaultHttpControllerTypeResolver(Predicate<Type> predicate)
            : base(predicate)
        {
        }

        /// <summary>
        /// Соответствие типу контроллера (наследование от IHttpController)
        /// </summary>
        protected static bool IsControllerType(Type t) => t != (Type)null && t.IsClass && t.IsVisible && !t.IsAbstract &&
            typeof(IHttpController).IsAssignableFrom(t) && DefaultHttpControllerTypeResolver.HasValidControllerName(t);

        /// <summary>
        /// Коррекность наименования контроллера
        /// </summary>
        private static bool HasValidControllerName(Type controllerType)
        {
            var controllerSuffix = DefaultHttpControllerSelector.ControllerSuffix;
            return controllerType.Name.Length > controllerSuffix.Length &&
                controllerType.Name.EndsWith(controllerSuffix, StringComparison.OrdinalIgnoreCase);
        }
    }
}