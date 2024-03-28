using Bars.B4;
using Bars.B4.IoC;
using Bars.B4.Windsor;
using Bars.Gkh.RegOperator.Regions.Tyumen.Entities;
using Bars.Gkh.RegOperator.Regions.Tyumen.Controllers;
using Bars.Gkh.RegOperator.Regions.Tyumen.ViewModel;
using Castle.MicroKernel.Registration;
using Bars.Gkh.RegOperator.Regions.Tyumen.Permissions;
using Bars.Gkh.Helpers;
using Bars.Gkh.RegOperator.Regions.Tyumen.Services;
using Bars.B4.Modules.States;
using Bars.Gkh.RegOperator.Regions.Tyumen.StateChanges;

namespace Bars.Gkh.RegOperator.Regions.Tyumen
{
    public partial class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            Container.RegisterController<RequestStateContoller>();
            this.RegisterController();
            this.RegisterViewModels();
            this.RegisterNavigations();
            this.Container.Register(Component.For<IPermissionSource>().ImplementedBy<GkhTyumenPermissionMap>());
            this.Container.RegisterTransient<IClientRouteMapRegistrar, ClientRouteMapRegistrar>();
            this.Container.RegisterTransient<IResourceManifest, ResourceManifest>();

            this.Container.RegisterTransient<IESIAEMailSender, ESIAEMailSender>();
            RegisterBundlers();

            this.Container.RegisterTransient<IRuleChangeStatus, RealityObjectSendStateRule>(); 
            this.Container.RegisterTransient<IRuleChangeStatus, RealityObjectSendPersonStateRule>(); 
        }

        private void RegisterController()
        {
            this.Container.RegisterAltDataController<RequestStatePerson>();
            this.Container.RegisterAltDataController<RequestState>();
        }

        private void RegisterViewModels()
        {
            this.Container.RegisterViewModel<RequestStatePerson, RequestStatePersonViewModel>();
            this.Container.RegisterViewModel<RequestState, RequestStateViewModel>();
        }

        private void RegisterNavigations()
        {
            this.Container.Register(Component.For<INavigationProvider>().Named("Gkh.RegOperator.Regions.Tyumen navigation").ImplementedBy<NavigationProvider>()
                   .LifeStyle.Transient);
        }
    }
}