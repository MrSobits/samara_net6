namespace Bars.Gkh.Regions.Yanao
{
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage.DomainService;
    using Bars.B4.Windsor;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Interceptors;
    using Bars.Gkh.Regions.Yanao.Controllers.RealityObj;
    using Bars.Gkh.Regions.Yanao.DomainService;
    using Bars.Gkh.Regions.Yanao.DomainService.Impl;
    using Bars.Gkh.Regions.Yanao.Entities;
    using Bars.Gkh.Regions.Yanao.ViewModel;
    using Bars.Gkh.ViewModel;

    using Castle.MicroKernel.Registration;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            this.Container.RegisterTransient<IResourceManifest, ResourceManifest>("Gkh.Regions.Yanao resources");
            this.Container.Register(Component.For<IPermissionSource>().ImplementedBy<GkhPermissionMapYanao>());

            this.Container.ReplaceComponent<IViewModel<RealityObject>>(
                typeof(RealityObjectViewModel),
                Component.For<IViewModel<RealityObject>>().ImplementedBy<YanaoRealityObjectViewModel>());

            this.Container.ReplaceController<RealityObjectController>("realityobject");
            this.Container.RegisterDomainService<RealityObjectExtension, FileStorageDomainService<RealityObjectExtension>>();
            this.Container.RegisterTransient<IRealityObjectExtensionService, RealityObjectExtensionService>();

            this.Container.ReplaceComponent<IDomainServiceInterceptor<RealityObject>>(
                typeof(RealityObjectInterceptor),
                Component.For<IDomainServiceInterceptor<RealityObject>>().ImplementedBy<Interceptors.RealityObject.RealityObjectInterceptor>());
        }
    }
}