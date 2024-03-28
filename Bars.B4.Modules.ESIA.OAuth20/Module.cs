namespace Bars.B4.Modules.ESIA.OAuth20
{
    using Bars.B4.IoC;
    using Bars.B4.Modules.ESIA.OAuth20.Entities;
    using Bars.B4.Modules.ESIA.OAuth20.Service;
    using Bars.B4.Modules.ESIA.OAuth20.Service.Impl;

    using Castle.MicroKernel.Registration;

    using EsiaNET;

    /// <summary>
    /// Класс модуля профиля B4 пользователя для RMS.
    /// </summary>
    [Bars.B4.Utils.Display("Модуль включающий авторизацию OAuth20 ESIA в B4 приложении")]
    [Bars.B4.Utils.Description("")]
    [Bars.B4.Utils.CustomValue("Version", 1)]
    [Bars.B4.Utils.CustomValue("Uid", "eab36a7d-aef5-46f0-8f31-711e9c869516")]
    public class Module : AssemblyDefinedModule
    {
        //Пример конфига модуля
        //    <Module Id = "Bars.B4.Modules.ESIA.OAuth20" >
        //  < parameters >
        //    < clear />
        //    < add key="CertificateThumbPrint" value="B7561E8722FB59180B56432D72C81AE890C7447C" />
        //    <add key = "ClientId" value="212807741" />
        //    <add key = "CallbackUri" value="https://gkh-test.bars-open.ru/gkhtest/" />
        //    <add key = "RedirectUri" value="https://esia-portal1.test.gosuslugi.ru/aas/oauth2/ac" />
        //    <add key = "TokenUri" value="https://esia-portal1.test.gosuslugi.ru/aas/oauth2/te" />
        //    <add key = "RestUri" value="https://esia-portal1.test.gosuslugi.ru/rs" />
        //  </parameters>
        //</Module>

        /// <summary>
        /// Метод конфигурации модуля.
        /// </summary>
        public override void Install()
        {
            this.Container.RegisterTransient<IResourceManifest, ResourceManifest>("ESIA.OAuth20 generatedresources");

            this.Container.RegisterSingleton<EsiaOauthOptions, EsiaOauthOptions>();

            Component.For<EsiaClient>()
                .LifestyleTransient()
                .UsingFactoryMethod(() => new EsiaClient(this.Container.Resolve<EsiaOauthOptions>().GetOptions()))
                .RegisterIn(this.Container);
            Component.For<EsiaClientSobits>()
              .LifestyleTransient()
              .UsingFactoryMethod(() => new EsiaClientSobits(this.Container.Resolve<EsiaOauthOptions>().GetOptions()))
              .RegisterIn(this.Container);

            this.Container.RegisterTransient<IEsiaOauthService, EsiaOauthService>();
        }
    }
}