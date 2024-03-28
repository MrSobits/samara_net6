namespace Bars.Gkh.AlphaBI
{
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Windsor;
    using Bars.Gkh.AlphaBI.Controllers;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            Container.RegisterResourceManifest<ResourceManifest>();
            Container.RegisterController<AlphaBiController>();


            Container.RegisterTransient<INavigationProvider, AlphaNavigationProvider>();
            Container.RegisterTransient<IPermissionSource, AlphaPermissionMap>();
            Container.RegisterTransient<IClientRouteMapRegistrar, AlphaClientRouteMapRegistrar>();
        }
    }
}
