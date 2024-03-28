namespace Bars.Gkh.Regions.Tula
{
    using B4;
    using B4.IoC;
    using B4.Windsor;
    using Castle.MicroKernel.Registration;
    using Controllers;
    using Entities;
    using Interceptors;
    using Overrides;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            Container.ReplaceComponent<IDomainServiceInterceptor<Municipality>>(
                typeof (MunicipalityServiceInterceptor),
                Component.For<IDomainServiceInterceptor<Municipality>>()
                    .ImplementedBy<MunicipalityInterceptor>()
                    .LifestyleTransient());
            Container.RegisterController<RealityObjectFixerController>();

            
        }
    }
}