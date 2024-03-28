namespace Bars.Gkh.Regions.Tomsk
{
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Windsor;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Interceptors;
    using Bars.Gkh.Regions.Tomsk.Controllers;
    using Bars.Gkh.Regions.Tomsk.Entities;
    using Bars.Gkh.Regions.Tomsk.ViewModel;

    using Castle.MicroKernel.Registration;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            Container.RegisterTransient<IResourceManifest, ResourceManifest>("Gkh.Regions.Tomsk resources");
            
            Container.ReplaceComponent<IDomainServiceInterceptor<Contragent>>(
                typeof(ContragentServiceInterceptor),
                Component.For<IDomainServiceInterceptor<Contragent>>().ImplementedBy<Interceptors.ContragentServiceInterceptor>().LifeStyle.Transient);

            Container.ReplaceController<TomskOperatorController>("operator");

            Container.Register(
                Component.For<IDomainServiceInterceptor<TomskOperator>>()
                    .ImplementedBy<OperatorServiceInterceptor<TomskOperator>>()
                         .LifeStyle.Transient);

            Container.Register(
                Component.For<IViewModel<TomskOperator>>()
                    .ImplementedBy<TomskOperatorViewModel>()
                         .LifeStyle.Transient);

            Container.ReplaceComponent<IDomainService<Operator>>(
                typeof(Gkh.DomainService.OperatorDomainService),
                Component.For<IDomainService<Operator>>()
                         .ImplementedBy<GkhGji.Regions.Tomsk.DomainService.OperatorDomainService>()
                         .LifeStyle.Transient);
        }
    }
}
