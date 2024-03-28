namespace Bars.B4.Modules.ESIA.Auth
{
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.ESIA.Auth.Controllers;
    using Bars.B4.Modules.ESIA.Auth.Entities;
    using Bars.B4.Modules.ESIA.Auth.Interceptors;
    using Bars.B4.Modules.ESIA.Auth.Services;
    using Bars.B4.Modules.ESIA.Auth.Services.Impl;
    using Bars.B4.Windsor;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Класс модуля профиля B4 пользователя для RMS.
    /// </summary>
    [Bars.B4.Utils.Display("Модуль включающий авторизацию ESIA в B4 приложении")]
    [Bars.B4.Utils.Description("")]
    [Bars.B4.Utils.CustomValue("Version", 1)]
    [Bars.B4.Utils.CustomValue("Uid", "eab36a7d-aef5-46f0-8f31-711e9c869504")]
    public class Module : AssemblyDefinedModule
    {
        /*
         Конфиг модуля
         <Module Id ="Bars.B4.Modules.ESIA.Auth">
            <parameters>
                <clear />
		        <add key="SocketListeningPort" value="10001" />
                <!--Указывать для случаев, когда сервис авторизации и основное приложение на разных машинах-->
		        <add key="SocketListeningAddress" value="IP-Адрес сервера с развернутым сервисом" />
            </parameters>
	    </Module>
         */

        /*
         * Дополнительные параметры:
         * SocketReceiveBufferLength - размер буффера при считывании сообщения (по умолчанию 1024 байта)
         * ReceiveTimeout - тайм-аут считывания сообщения (по умолчанию 60000 миллисек)
         */

        public override void Install()
        {
            this.Container.RegisterTransient<IResourceManifest, ResourceManifest>("ESIA.Auth resources");

            this.RegisterControllers();

            this.RegisterServices();

            this.RegisterInterceptors();
        }

        private void RegisterControllers()
        {
            this.Container.RegisterController<OAuthLoginController>();

            this.Container.ReplaceController<LoginController>("Login");
        }

        private void RegisterServices()
        {
            this.Container.RegisterTransient<IOAuthLoginService, OAuthLoginService>();
            this.Container.RegisterTransient<IAuthAppIntegrationService, AuthAppIntegrationService>();
        }

        private void RegisterInterceptors()
        {
            this.Container.RegisterDomainInterceptor<Operator, OperatorInterceptor>();
        }
    }
}