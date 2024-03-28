namespace Bars.B4.Modules.FIAS
{
    using Bars.B4.IoC;
    using Bars.B4.Windsor;

    public class Module : AssemblyDefinedModule
    {        
        public override void Install()
        {
            Container.RegisterResourceManifest<ResourceManifest>();
            Container.RegisterNavigationProvider<NavigationProvider>();
            Container.RegisterPermission<Permissions>();
            Container.RegisterTransient<IFiasRepository, FiasRepository>();
            Container.RegisterDomainInterceptor<Fias, FiasServiceInterceptor>();

            Container.RegisterController<FiasController>();
        }

        protected override void SetPredecessors()
        {
            SetPredecessor<B4.Modules.NH.Module>();
            SetPredecessor<ExtJs.Module>();
        }
    }
}