namespace Bars.Gkh.Regions.Nnovgorod
{
    using B4.Windsor;
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Interceptors;
    using Bars.Gkh.Regions.Nnovgorod.DomainService;
    using Bars.Gkh.Regions.Nnovgorod.DomainService.Impl;
    using Bars.Gkh.TextValues;

    using Castle.MicroKernel.Registration;

    using Controllers;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            Container.RegisterController<RealityObjectFixerController>();
            Container.ReplaceController<ContragentController>("contragent");

            Container.RegisterTransient<IResourceManifest, ResourceManifest>();
            Container.Register(Component.For<IContragentInfoService>().ImplementedBy<ContragentInfoService>().LifeStyle.Transient);

            Container.UsingForResolved<IMenuItemText>((container, menuItemText) => menuItemText.Override("Зональные жилищные инспекции", "Отделы гос. жилищной инспекции"));

            Container.ReplaceComponent<IDomainServiceInterceptor<Municipality>>(
                typeof(MunicipalityServiceInterceptor),
                Component.For<IDomainServiceInterceptor<Municipality>>().ImplementedBy<Interceptors.MunicipalityServiceInterceptor>().LifestyleTransient());
        }
    }
}