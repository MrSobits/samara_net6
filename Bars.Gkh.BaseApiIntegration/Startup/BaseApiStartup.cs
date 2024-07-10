namespace Bars.Gkh.BaseApiIntegration.Startup
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net.Http.Formatting;
    using System.Net.Http.Headers;
    using System.Web.Http;
    using System.Web.Http.Controllers;
    using System.Web.Http.Dispatcher;

    using Bars.B4.Application;
    using Bars.B4.Utils;
    using Bars.Gkh.BaseApiIntegration.SwaggerFilters;

    using Newtonsoft.Json.Serialization;

    using Owin;

    using Swashbuckle.Application;

    /// <summary>
    /// Базовый класс конфигурации WebApi приложения
    /// </summary>
    // TODO заменить IStartup на IAspNetCoreApplicationConfigurator
    public abstract class BaseApiStartup : IStartup
    {
        /// <summary>
        /// Префикс роутов API-сервисов модуля
        /// </summary>
        protected virtual string ApiPrefix { get; set; }

        /// <inheritdoc />
        public void Configure(IAppBuilder app)
        {
            if (this.ApiPrefix.IsEmpty())
            {
                throw new Exception($"Для экземпляра конфигурации WebApi не указан {nameof(this.ApiPrefix)}!");
            }

            // Конфигурация WebApi шаблон модуля
            app.Map($"/{this.ApiPrefix}", this.ApiMapConfigure);
        }

        /// <summary>
        /// Настроить WebApi модуля
        /// </summary>
        /// <param name="map"></param>
        protected virtual void ApiMapConfigure(IAppBuilder map)
        {
            // Создаем конфигурацию с обработкой запросов модуля
            var config = this.CreateConfiguration();

            // Configure JSON Formatter for REST API
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/octet-stream"));
            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            // Configure CORS to allow JavaScript clients from any
            // domain to access our REST API
            map.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            // Конфиг Swagger'а
            this.ConfigureSwagger(config);

            // Дополнительная конфигурация модуля
            this.AdditionalConfigure(config);

            // Создаем обработчик по конфигу
            map.UseWebApi(config);
        }

        /// <summary>
        /// Создать конфигурацию для обработки запросов
        /// </summary>
        protected abstract HttpConfiguration CreateConfiguration();

        /// <summary>
        /// Configure Swagger via Swashbuckle
        /// </summary>
        private void ConfigureSwagger(HttpConfiguration config)
        {
            config
                .EnableSwagger(
                    c =>
                    {
                        var rootUrl = ApplicationContext.Current.Configuration.AppSettings.GetAs<string>("SwaggerRootUrl");
                        if (rootUrl != null)
                        {
                            c.RootUrl(_ => $"{rootUrl}/{this.ApiPrefix}");
                        }

                        c.SingleApiVersion("v1", "REST API");

                        // Подгружаем файлы с документацией API-шки
                        Directory.GetFiles(
                                $@"{AppDomain.CurrentDomain.BaseDirectory}\\bin\\Documentation",
                                "*ApiDocumentation.xml")
                            .ToList()
                            .ForEach(c.IncludeXmlComments);

                        c.SchemaFilter<SwaggerIgnoreFilter>();
                        c.SchemaFilter<PossibleValuesDescription>();
                        c.DocumentFilter<SwaggerAddEnumDescriptions>();
                    }
                )
                .EnableSwaggerUi();
        }

        /// <summary>
        /// Дополнительная конфигурация
        /// </summary>
        protected virtual void AdditionalConfigure(HttpConfiguration config)
        {
        }
    }

    /// <summary>
    /// Конфигурация WebApi по сборке
    /// </summary>
    public abstract class AssemblyApiStartup : BaseApiStartup
    {
        /// <summary>
        /// Создать конфигурацию с обработкой запросов по сборке
        /// </summary>
        /// <remarks>
        /// Для обработки будут использоваться ТОЛЬКО те контроллеры, которые находятся в сборке
        /// </remarks>
        protected override HttpConfiguration CreateConfiguration()
        {
            var assembly = this.GetType().Assembly;
            var config = new HttpConfiguration();

            // Подменяем Resolver определения контроллеров по сборке
            config.Services.Replace(typeof(IHttpControllerTypeResolver), new AssemblyHttpControllerTypeResolver(assembly));
            // Собираем роуты для контроллеров по сборке
            config.MapHttpAttributeRoutes(new AssemblyDirectRouteProvider(assembly));

            return config;
        }
    }

    /// <summary>
    /// Конфигурация WebApi по иерархии от типа контроллера
    /// </summary>
    public abstract class InheritControllerApiStartup<TBaseApiController> : BaseApiStartup
        where TBaseApiController : IHttpController
    {
        /// <summary>
        /// Создать конфигурацию с обработкой запросов по типу контроллера
        /// </summary>
        /// <typeparam name="TBaseApiController">Тип базового класса контроллера модуля</typeparam>
        /// <remarks>
        /// Для обработки будут использоваться ТОЛЬКО те контроллеры, которые наследованы от TBaseController
        /// </remarks>
        protected override HttpConfiguration CreateConfiguration()
        {
            var config = new HttpConfiguration();

            // Подменяем Resolver определения контроллеров по иерархии
            config.Services.Replace(typeof(IHttpControllerTypeResolver), new TypedHttpControllerTypeResolver<TBaseApiController>());
            // Собираем роуты для контроллеров по иерархии
            config.MapHttpAttributeRoutes(new TypedDirectRouteProvider<TBaseApiController>());

            return config;
        }
    }
}