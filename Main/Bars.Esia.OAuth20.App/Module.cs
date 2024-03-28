namespace Bars.Esia.OAuth20.App
{
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Esia.OAuth20.App.Providers;
    using Bars.Esia.OAuth20.App.Providers.Impl;
    using Bars.Esia.OAuth20.App.Services;
    using Bars.Esia.OAuth20.App.Services.Impl;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            this.RegisterProviders();
            this.RegisterServices();
        }

        private void RegisterProviders()
        {
            this.Container.RegisterSingleton<IAuthAppSecurityProvider, AuthAppSecurityProvider>();
            this.Container.RegisterSingleton<IAuthAppOptionProvider, AuthAppOptionProvider>();
        }

        private void RegisterServices()
        {
            this.Container.RegisterTransient<IAuthAppOperationService, AuthAppOperationService>();
        }
    }
}