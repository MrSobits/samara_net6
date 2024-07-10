namespace Bars.B4.Modules.Analytics.Extensions;

using System.Reflection;

using Bars.B4.Alt;
using Bars.B4.Controller.Provider;
using Bars.B4.DataAccess;

using Castle.MicroKernel.Registration;
using Castle.Windsor;

/// <summary>
/// Методы расширения для WindsorContainer
/// </summary>
public static class WindsorExtensions
{
    /// <summary>
    /// Зарегистрировать <see cref="InlineDataController{T}"/> контроллер
    /// </summary>
    /// <param name="container">IoC контейнер</param>
    /// <param name="controllerName">Имя контроллера</param>
    /// <typeparam name="T">Тип сушности</typeparam>
    public static void RegisterInlineDataController<T>(this IWindsorContainer container, string controllerName = null) where T : PersistentObject
    {
        var controllerType = typeof(InlineDataController<T>);
        var controllerProvider = container.Resolve<ControllerProvider>();
        var typeInfo = controllerType.GetTypeInfo();
        var controllerUid = controllerProvider.GetControllerUid(typeInfo);
        controllerProvider.RegisterController(typeInfo, controllerName);
        container.Register(Component
            .For<BaseController>()
            .ImplementedBy(controllerType)
            .Named(controllerUid)
            .LifestyleTransient());
    }
}