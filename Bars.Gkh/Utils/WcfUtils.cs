/* TODO wcf
namespace Bars.Gkh.Utils
{
    using Bars.B4.Application;
    using Bars.B4.Config;
    using Castle.Facilities.WcfIntegration;
    using Castle.Facilities.WcfIntegration.Rest;
    using Castle.MicroKernel.Registration;
    using System;
    using System.ServiceModel;

    /// <summary>
    /// Класс утилит для работы с Wcf
    /// </summary>
    public static class WcfUtils
    {
        /// <summary>
        /// Регистрация сервиса Wcf, работающего по обоим протоколам http и https
        /// </summary>
        /// <typeparam name="T">Тип сервиса</typeparam>
        /// <param name="registration">Регистрация</param>
        /// <returns>Регистрация</returns>
        public static ComponentRegistration<T> AsWcfSecurityService<T>(this ComponentRegistration<T> registration)
            where T : class
        {
            return registration
                .AsWcfService(new RestServiceModel().Hosted())
                .AsWcfService(new DefaultServiceModel().AddDefaultEndpoints().Hosted());
        }

        /// <summary>
        /// Регистрация сервиса Wcf, работающего по обоим протоколам http и https
        /// </summary>
        /// <typeparam name="T">Тип сервиса</typeparam>
        /// <param name="registration">Регистрация</param>
        /// <returns>Регистрация</returns>
        public static ComponentRegistration<T> AsWcfRestService<T>(this ComponentRegistration<T> registration)
            where T : class
        {
            return registration
                .AsWcfService(new RestServiceModel().Hosted())
                .AsWcfService(new RestServiceModel().AddWebEndpoints().Hosted());
        }

        /// <summary>
        /// Добавить основные точки доступа для SOAP (для REST ни в коем случае нельзя добавлять)
        /// </summary>
        /// <param name="model">Модель</param>
        /// <returns>Модель</returns>
        public static DefaultServiceModel AddDefaultEndpoints(this DefaultServiceModel model)
        {
            var _container = ApplicationContext.Current.Container;
            var configProvider = _container.Resolve<IConfigProvider>();
            var config = configProvider.GetConfig().GetModuleConfig("Bars.B4.Gkh");
            bool HttpBinding = config.GetAs<bool>("EnableHttpBinding", true, true);
            bool HttpsBinding = config.GetAs<bool>("EnableHttpsBinding", false, true);

            if (HttpBinding)
            {
                model.AddEndpoints(
                    WcfEndpoint.BoundTo(
                        new BasicHttpBinding
                        {
                            MaxReceivedMessageSize = int.MaxValue,
                            OpenTimeout = TimeSpan.MaxValue,
                            CloseTimeout = TimeSpan.MaxValue,
                            ReceiveTimeout = TimeSpan.MaxValue,
                            Security =
                                new BasicHttpSecurity
                                {
                                    Mode = BasicHttpSecurityMode.None,
                                    Transport = new HttpTransportSecurity { ClientCredentialType = HttpClientCredentialType.None }
                                }
                        }));
            }

            if (HttpsBinding)
            {
                model.AddEndpoints(
                    WcfEndpoint.BoundTo(
                        new BasicHttpsBinding
                        {
                            MaxReceivedMessageSize = int.MaxValue,
                            OpenTimeout = TimeSpan.MaxValue,
                            CloseTimeout = TimeSpan.MaxValue,
                            ReceiveTimeout = TimeSpan.MaxValue,
                            Security =
                                new BasicHttpsSecurity
                                {
                                    Mode = BasicHttpsSecurityMode.Transport,
                                    Transport = new HttpTransportSecurity { ClientCredentialType = HttpClientCredentialType.None }
                                }
                        }));
            }

            return model;
        }

        /// <summary>
        /// Добавить основные точки доступа для REST (для SOAP ни в коем случае нельзя добавлять)
        /// </summary>
        /// <param name="model">Модель</param>
        /// <returns>Модель</returns>
        public static RestServiceModel AddWebEndpoints(this RestServiceModel model)
        {
            var _container = ApplicationContext.Current.Container;
            var configProvider = _container.Resolve<IConfigProvider>();
            var config = configProvider.GetConfig().GetModuleConfig("Bars.B4.Gkh");
            bool HttpBinding = config.GetAs<bool>("EnableHttpBinding", true, true);
            bool HttpsBinding = config.GetAs<bool>("EnableHttpsBinding", false, true);

            if (HttpBinding)
            {
                var endpoint = WcfEndpoint.BoundTo(
                        new WebHttpBinding
                        {
                            MaxReceivedMessageSize = int.MaxValue,
                            OpenTimeout = TimeSpan.MaxValue,
                            CloseTimeout = TimeSpan.MaxValue,
                            ReceiveTimeout = TimeSpan.MaxValue,
                            ContentTypeMapper = new JsonContentTypeMaper(),
                            Security =
                                new WebHttpSecurity
                                {
                                    Mode = WebHttpSecurityMode.None,
                                    Transport = new HttpTransportSecurity { ClientCredentialType = HttpClientCredentialType.None }
                                }
                        });

                model.AddEndpoints(endpoint);
            }

            if (HttpsBinding)
            {
                var endpoint = WcfEndpoint.BoundTo(
                        new WebHttpBinding
                        {
                            MaxReceivedMessageSize = int.MaxValue,
                            OpenTimeout = TimeSpan.MaxValue,
                            CloseTimeout = TimeSpan.MaxValue,
                            ReceiveTimeout = TimeSpan.MaxValue,
                            ContentTypeMapper = new JsonContentTypeMaper(),
                            Security =
                                new WebHttpSecurity
                                {
                                    Mode = WebHttpSecurityMode.Transport,
                                    Transport = new HttpTransportSecurity { ClientCredentialType = HttpClientCredentialType.None }
                                }
                        });

                model.AddEndpoints(endpoint);
            }

            return model;
        }
    }
}*/