namespace Bars.Gkh.Gku.Regions.Kamchatka
{
    using B4;
    using B4.IoC;
    using B4.ResourceBundling;
    using B4.Windsor;
    using Controllers;
    using Castle.MicroKernel.Registration;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            Container.ReplaceComponent(
                typeof(INavigationProvider),
                typeof(Gku.NavigationProvider),
                Component.For<INavigationProvider>().ImplementedBy<NavigationProvider>().LifestyleTransient());

            Component.For<IClientRouteMapRegistrar>().ImplementedBy<ClientRouteMapRegistrar>().LifestyleTransient().RegisterIn(this.Container);
            Component.For<IResourceManifest>().ImplementedBy<ResourceManifest>().LifestyleTransient().RegisterIn(this.Container);
            
            Container.RegisterController<GkuExternalLinksController>();
            
            var bundler = Container.Resolve<IResourceBundler>();

            bundler.RegisterCssBundle("b4-all", new[]
                {
                    "~/content/css/gkhGkuKamchatka.css"
                });
        }
    }
}
