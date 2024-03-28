namespace Bars.Gkh.BaseApiIntegration.Attributes
{
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Filters;

    using Bars.B4.Application;
    using Bars.B4.Logging;
    using Bars.Gkh.BaseApiIntegration.Models;

    using Newtonsoft.Json;

    /// <summary>
    /// Атрибут валидации модели
    /// </summary>
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        /// <inheritdoc />
        public override void OnActionExecuting(HttpActionContext httpActionContext)
        {
            // удаляем из ModelState все свойства, помеченные атрибутом JsonIgnoreAttribute
            var args = httpActionContext.ActionArguments;
            foreach (var arg in args)
            {
                var type = arg.Value.GetType();
                var properties = type.GetProperties()
                    .Where(x => x.GetCustomAttributes(false).Any(y => y is JsonIgnoreAttribute));
                foreach (var property in properties)
                {
                    httpActionContext.ModelState.Remove($"{arg.Key}.{property.Name}");
                }
            }

            if (!httpActionContext.ModelState.IsValid)
            {
                var container = ApplicationContext.Current.Container;
                var logManager = container.Resolve<ILogManager>();

                logManager.Error($"Ошибка API-метода|Не заполнены обязательные поля:\n" +
                    $"{string.Join(",\n", httpActionContext.ModelState.Keys)}");

                var errorMessages = httpActionContext.ModelState.Values.SelectMany(x => x.Errors.Select(y => y.ErrorMessage));
                var errorResponse = new BaseApiResponse
                {
                    Error = new CustomError
                    {
                        Message = "При проверке входных параметров возникли ошибки: " + string.Join(" ", errorMessages)
                    }
                };
                httpActionContext.Response = httpActionContext.Request.CreateResponse(HttpStatusCode.BadRequest, errorResponse);
            }
        }
    }
}