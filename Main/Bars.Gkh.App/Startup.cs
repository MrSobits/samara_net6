namespace Bars.Gkh.App
{
    using System;
    using System.Text;

    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.Extensions;

    using ElmahCore;
    using ElmahCore.Mvc;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Diagnostics;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public class Startup : B4MvcStartup
    {
        /// <inheritdoc />
        protected override void ConfigureMvc(IServiceCollection services, IModule[] modules, IAppContext ctx)
        {
            services
                .AddMvc()
                .AddViewOptions(opt => opt.HtmlHelperOptions.ClientValidationEnabled = false);

            base.ConfigureMvc(services, modules, ctx);
        }

        /// <inheritdoc />
        public override IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services
                .AddCoreWcf()
                .AddSession()
                .AddSignalR()
                .AddJsonProtocol(options =>
                {
                    options.PayloadSerializerOptions.PropertyNamingPolicy = null;
                });

            services.AddMemoryCache();
            services.AddElmah<XmlFileErrorLog>(o => o.LogPath = "~/.errors");

            return base.ConfigureServices(services);
        }

        /// <inheritdoc />
        public override void Configure(IApplicationBuilder app,
            IHostingEnvironment env,
            IApplicationLifetime applicationLifetime,
            IServiceProvider serviceProvider,
            IConfiguration configuration)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            app.UseSession();
            base.Configure(app, env, applicationLifetime, serviceProvider, configuration);
        }

        /// <inheritdoc />
        protected override void AddMiddlewareAtTheStart(IApplicationBuilder app, IConfiguration configuration)
        {
            app.UseExceptionHandler(action => action.Run(async context =>
            {
                var exception = context.Features
                    .Get<IExceptionHandlerPathFeature>()
                    ?.Error;

                var exceptionMessage = new ExceptionMessage
                {
                    Message = exception?.Message,
                    StackTrace = exception?.StackTrace
                };

                await context.Response.WriteAsJsonAsync(exceptionMessage);
            }));

            app.UseElmah();
        }

        private class ExceptionMessage
        {
            public virtual string StackTrace { get; set; }
            public virtual string Message { get; set; }
        }
    }
}