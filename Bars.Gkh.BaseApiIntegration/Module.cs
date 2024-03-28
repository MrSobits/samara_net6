namespace Bars.Gkh.BaseApiIntegration
{
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.BaseApiIntegration.Services;
    using Bars.Gkh.BaseApiIntegration.Services.Impl;

    /// <inheritdoc />
    public class Module : AssemblyDefinedModule
    {
        /// <inheritdoc />
        public override void Install()
        {
            this.RegisterServices();
        }

        private void RegisterServices()
        {
            this.Container.RegisterTransient<IApiLoginService, ApiLoginService>();
        }
    }
}