namespace Bars.Gkh.InspectorMobile.Api.Version1.ModelBinders
{
    using System.Web.Http.Controllers;
    using System.Web.Http.ModelBinding;

    using Newtonsoft.Json;

    /// <summary>
    /// Базовый класс-привязчик модели
    /// </summary>
    public class DefaultJsonModelBinder : IModelBinder
    {
        /// <inheritdoc />
        public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
        {
            var jsonString = bindingContext.ValueProvider.GetValue(bindingContext.ModelName).AttemptedValue;

            bindingContext.Model = JsonConvert.DeserializeObject(jsonString, bindingContext.ModelType);

            return true;
        }
    }
}