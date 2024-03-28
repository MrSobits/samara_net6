namespace Bars.Gkh.Regions.Perm
{
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Modules.Analytics.Reports;
    using Bars.B4.Modules.States;
    using Bars.B4.Windsor;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.DomainService.AddressMatching;
    using Bars.Gkh.Entities;
    using Bars.Gkh.ExecutionAction.Impl;
    using Bars.Gkh.Regions.Perm.Controllers;
    using Bars.Gkh.Regions.Perm.DomainService;
    using Bars.Gkh.Regions.Perm.Interceptors;
    using Bars.Gkh.Regions.Perm.Navigation;
    using Bars.Gkh.Regions.Perm.Permissions;
    using Bars.Gkh.Regions.Perm.Reports;
    using Bars.Gkh.Regions.Perm.StateChanges;
    using Bars.Gkh.Report.Licensing;
    using Bars.Gkh.Utils;
    using Bars.Gkh.ViewModel;

    using Castle.MicroKernel.Registration;

    public class Module : AssemblyDefinedModule
    {
        /// <summary>
        /// Метод инициализации модуля
        /// </summary>
        public override void Install()
        {
            this.Container.RegisterResources<ResourceManifest>();
            this.Container.RegisterTransient<IClientRouteMapRegistrar, ClientRouteMapRegistrar>();
            this.Container.RegisterPermissionMap<PermPermissionMap>();
            this.Container.RegisterTransient<IFieldRequirementSource, PermFieldRequirementMap>();
            this.Container.RegisterTransient<INavigationProvider, NavigationProvider>("Gkh.Perm navigation");
            this.Container.ReplaceComponent<INavigationProvider>(typeof(Gkh.Navigation.ManOrgLicenseMenuProvider),
                Component.For<INavigationProvider>().ImplementedBy<ManOrgLicenseMenuProvider>());

            this.Container.ReplaceComponent<IRuleChangeStatus>(typeof(Gkh.StateChanges.LicenseRequestStateRule),
                Component.For<IRuleChangeStatus>().ImplementedBy<LicenseRequestStateRule>());

            this.Container.ReplaceComponent<IRuleChangeStatus>(typeof(Gkh.StateChanges.RevocationLicenseStateRule),
                Component.For<IRuleChangeStatus>().ImplementedBy<RevocationLicenseStateRule>());

            this.Container.RegisterTransient<IRuleChangeStatus, TerminationLicenseStateRule>();

            this.Container.RegisterTransient<IImportAddressMatchService, PermImportAddressMatchService>();
            this.Container.RegisterTransient<IPrintForm, UnderstandingHomeReport>("UnderstandingHomeReport");

            this.Container.ReplaceTransient<IPrintForm, Overhaul.Hmao.Reports.PublishedDpkrByWorkReport, PublishedDpkrByWorkReport>();

            this.RegisterControllers();
            this.RegisterViewModel();
            this.RegisterInterceptors();
            this.RegisterCodedReports();
            this.RegisterServices();
            this.RegisterExecutionActions();
        }

        private void RegisterControllers()
        {
            this.Container.ReplaceController<ParametersController>("Parameters");
        }

        private void RegisterViewModel()
        {
            this.Container.ReplaceTransient<IViewModel<PersonQualificationCertificate>, 
                PersonQualificationCertificateViewModel, 
                ViewModel.PersonQualificationCertificateViewModel>();
        }


        private void RegisterCodedReports()
        {
            this.Container.RegisterTransient<ICodedReport, FormGovernmentServiceReport>();
        }

        private void RegisterInterceptors()
        {
            this.Container.RegisterDomainInterceptor<PersonQualificationCertificate, PersonQualificationCertificateInterceptor>();
            this.Container.RegisterDomainInterceptor<RealityObject, RealityObjectSyncInterceptor>();
            this.Container.RegisterDomainInterceptor<EmergencyObject, EmergencyObjectInterceptor>();
            this.Container.ReplaceTransient<IDomainServiceInterceptor<ManOrgLicense>, Gkh.Interceptors.ManOrgLicenseInterceptor, ManOrgLicenseInterceptor>();
            this.Container.ReplaceTransient<IDomainServiceInterceptor<ManOrgLicenseRequest>, Gkh.Interceptors.ManOrgLicenseRequestInterceptor, ManOrgLicenseRequestInterceptor>();
        }

        private void RegisterServices()
        {
            this.Container.RegisterSessionScoped<IEmergencyObjectSyncService, EmergencyObjectSyncService>();
        }

        private void RegisterExecutionActions()
        {
            this.Container.RegisterExecutionAction<FillActivityStageAction>();
        }
    }
}