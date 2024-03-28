namespace Bars.Gkh.StateTariffCommittee
{
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.StateTariffCommittee.Api.Version1.Services;
    using Bars.Gkh.StateTariffCommittee.Api.Version1.Services.Impl;

    /// <inheritdoc />
    public class Module : AssemblyDefinedModule
    {
        /// <inheritdoc />
        public override void Install()
        {
            this.RegisterCommonComponents();
            this.RegisterApiServices();
        }

        private void RegisterCommonComponents()
        {
            this.Container.RegisterTransient<IStartup, Startup>();
        }

        private void RegisterApiServices()
        {
            this.Container.RegisterTransient<IRealityObjectService, RealityObjectService>();
        }
    }
}