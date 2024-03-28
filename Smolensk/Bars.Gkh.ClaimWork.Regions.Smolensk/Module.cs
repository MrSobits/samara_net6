namespace Bars.Gkh.ClaimWork.Regions.Smolensk
{
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.ClaimWork.Regions.Smolensk.Interceptors;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.RegOperator.Entities.Dict;

    using Castle.MicroKernel.Registration;
    using Sobits.RosReg.Entities;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            this.Container.RegisterResources<ResourceManifest>();
            this.Container.RegisterSingleton<IPermissionSource, PermissionMap>();
            this.Container.ReplaceComponent<IViewModel<ExtractEgrn>>(
                typeof(Sobits.RosReg.ViewModel.ExtractEgrnViewModel), 
                Component.For<IViewModel<ExtractEgrn>>().ImplementedBy<ViewModel.ExtractEgrnViewModel>().LifestyleTransient());
            this.Container.ReplaceComponent<IViewModel<PaymentDocInfo>>(
                typeof(Bars.Gkh.RegOperator.ViewModels.Dict.PaymentDocInfoViewModel), 
                Component.For<IViewModel<PaymentDocInfo>>().ImplementedBy<ViewModel.PaymentDocInfoViewModel>().LifestyleTransient());
            this.RegisterInterceptors();
        }

        private void RegisterInterceptors()
        {
            this.Container.RegisterDomainInterceptor<Petition, LawsuitInterceptorSml<Petition>>();
            this.Container.RegisterDomainInterceptor<CourtOrderClaim, LawsuitInterceptorSml<CourtOrderClaim>>();
            this.Container.RegisterDomainInterceptor<Lawsuit, LawsuitInterceptorSml<Lawsuit>>();
        }
    }
}