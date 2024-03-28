namespace Bars.Esia.OAuth20.App.Providers.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Net;

    using Bars.B4.Config;
    using Bars.B4.Utils;
    using Bars.Esia.OAuth20.App.Entities;
    using Bars.Esia.OAuth20.App.Enums;

    using Castle.Core;
    using Castle.Core.Internal;
    using Castle.Windsor;

    using Fasterflect;

    /// <summary>
    /// Поставщик параметров приложения
    /// </summary>
    public class AuthAppOptionProvider : IAuthAppOptionProvider, IInitializable
    {
        /// <summary>
        /// IoC-контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Настройки приложения
        /// </summary>
        private AppConfig appConfig;

        /// <summary>
        /// Параметры приложения
        /// </summary>
        private AuthAppOptions authAppOptions;

        /// <summary>
        /// Параметры ЕСИА
        /// </summary>
        private EsiaOptions esiaOptions;

        /// <summary>
        /// Перечень не найденных обязательных
        /// параметров из настроек приложения
        /// </summary>
        private List<string> notExistRequiredOptions;

        /// <inheritdoc />
        public void Initialize()
        {
            var configProvider = this.Container.Resolve<IConfigProvider>();

            this.appConfig = configProvider.GetConfig();
            this.notExistRequiredOptions = new List<string>();

            this.authAppOptions = new AuthAppOptions
            {
                SocketListeningPort = this.GetApplicationParam("SocketListeningPort", "порт для прослушивания")?.ToInt(),
                SocketReceiveBufferLength = this.GetApplicationParam("SocketReceiveBufferLength", defaultValue: 1024).ToInt(),
                SocketConnectionsLength = this.GetApplicationParam("SocketConnectionsLength", defaultValue: 1).ToInt()
            };

            var newAddress = this.GetApplicationParam("SocketListeningAddress", isRequired: false);

            if (!newAddress.IsNullOrEmpty())
            {
                this.authAppOptions.SocketListeningAddress = IPAddress.Parse(newAddress);
            }

            this.esiaOptions = new EsiaOptions
            {
                AccessType = AccessType.Online, // тип доступа, online - только при наличии владельца
                RequestTimeout = TimeSpan.FromSeconds(double.Parse(this.GetApplicationParam("RequestTimeout", defaultValue: 60.0))),
                MaxResponseContentBufferSize = this.GetApplicationParam("MaxResponseContentBufferSize", defaultValue: 10485760).ToInt(),
                CertificateThumbPrint = this.GetApplicationParam("CertificateThumbPrint", "отпечаток сертификата"),
                ClientId = this.GetApplicationParam("ClientId", "идентификатор ИС (мнемоника)"),
                Scope = this.GetApplicationParam("Scope", "область доступа"),
                CallbackUri = this.GetApplicationParam("CallbackUri", "адрес, на который должна вернуть ЕСИА после авторизации"),
                RedirectUri = this.GetApplicationParam("RedirectUri", "адрес для получения кода доступа из ЕСИА"),
                TokenUri = this.GetApplicationParam("TokenUri", "адрес для получения маркера доступа из ЕСИА"),
                RestUri = this.GetApplicationParam("RestUri", "адрес для получения данных из ЕСИА"),
                RequestType = this.GetApplicationParam("RequestType", defaultValue: "code"),
                PrnsRef = this.GetApplicationParam("PrnsRef", defaultValue: "prns"),
                CttsRef = this.GetApplicationParam("CttsRef", defaultValue: "ctts"),
                AddrsRef = this.GetApplicationParam("AddrsRef", defaultValue: "addrs"),
                DocsRef = this.GetApplicationParam("DocsRef", defaultValue: "docs"),
                OrgsRef = this.GetApplicationParam("OrgsRef", defaultValue: "orgs")
            };
        }

        /// <inheritdoc />
        public AuthAppOptions GetAuthAppOptions() => this.authAppOptions;

        /// <inheritdoc />
        public EsiaOptions GetEsiaOptions() => this.esiaOptions.DeepClone();

        /// <inheritdoc />
        public IEnumerable<string> GetNotExistRequiredOptions() => this.notExistRequiredOptions;

        /// <summary>
        /// Получить параметр приложения
        /// </summary>
        private string GetApplicationParam(string name, string description = "", object defaultValue = null, bool isRequired = true)
        {
            var value = this.appConfig.AppSettings.Get(name) ?? defaultValue;

            if (value.IsNull() && isRequired)
            {
                this.notExistRequiredOptions.Add($"{description} ({name})");
            }

            return value?.ToString();
        }
    }
}