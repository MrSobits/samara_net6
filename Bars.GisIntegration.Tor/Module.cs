namespace Bars.GisIntegration.Tor
{
	using Bars.B4;
	using Bars.B4.IoC;
	using Bars.B4.Windsor;
	using Bars.GisIntegration.Tor.ConfigSections;
    using Bars.GisIntegration.Tor.Entities;
    using Bars.GisIntegration.Tor.Service;
	using Bars.GisIntegration.Tor.Service.Impl;
    using Bars.GisIntegration.Tor.Service.LogService;
    using Bars.Gkh.Utils;

	public class Module : AssemblyDefinedModule
	{
		public override void Install()
		{
			this.Container.RegisterTransient<IResourceManifest, ResourceManifest>();
			this.Container.RegisterGkhConfig<TorIntegrationConfig>();
			this.RegisterControllers();
			this.RegisterServices();
		}

		private void RegisterControllers()
		{
            this.Container.RegisterAltDataController<TorTask>();
		}

		private void RegisterServices()
		{
			this.Container.RegisterTransient<ITorIntegrationService, TorIntegrationService>();
            this.Container.RegisterSingleton<ITorLogService, TorLogService>();
		}
	}
}