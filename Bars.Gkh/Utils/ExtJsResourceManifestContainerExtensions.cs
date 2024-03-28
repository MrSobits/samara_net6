namespace Bars.Gkh.Utils
{
    using Bars.B4;
    using Bars.Gkh.ContentResource;
    using Bars.Gkh.Entities.Dicts;

    public static class ExtJsResourceManifestContainerExtensions
    {
        /// <summary>
        /// Зарегистрировать js-контроллер справочника
        /// </summary>
        /// <typeparam name="T">Экземпляр справочника <see cref="BaseGkhDict"/></typeparam>
        /// <param name="container">Контейнер ресурсов</param>
        /// <param name="title">Заголовок справочника</param>
        /// <param name="controllerPath">Namespace контроллера B4.controller.dict.T</param>
        /// <param name="permissionPrefix">Префикс прав доступа для CRUD операций</param>
        /// <param name="mainView">Относительное наименование view B4.view.mainView (dict.BaseGkhDictGrid)</param>
        /// <param name="mainViewSelector">Алиас view widget.mainViewSelector (basegkhdictgrid)</param>
        public static void RegisterBaseDictController<T>(
            this IResourceManifestContainer container, 
            string title, 
            string permissionPrefix = null, 
            string controllerPath = null, 
            string mainView = null, 
            string mainViewSelector = null)
            where T : BaseGkhDict
        {
            var controllerName = typeof(T).Name;
            controllerPath = controllerPath ?? $"B4.controller.dict.{controllerName}";
            container.Add($"~/libs/{controllerPath.Replace('.', '/')}.js",
                new BaseGkhDictControllerContent(controllerName, controllerPath, title, permissionPrefix, mainView, mainViewSelector));
        }
    }
}