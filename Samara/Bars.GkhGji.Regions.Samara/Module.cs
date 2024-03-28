using Bars.B4.Modules.States;

namespace Bars.GkhGji.Regions.Samara
{
    using B4.Modules.Reports;
    using Bars.B4;
    using Bars.B4.IoC;
    
    using Bars.B4.Windsor;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Contracts.Reminder;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.InspectionRules;
    using Bars.GkhGji.Regions.Samara.Controllers;
    using Bars.GkhGji.Regions.Samara.DomainService;
    using Bars.GkhGji.Regions.Samara.DomainService.Scripts.Impl;
    using Bars.GkhGji.Regions.Samara.Entities;
    using Bars.GkhGji.Regions.Samara.Interceptors;
    using Bars.GkhGji.Regions.Samara.Permissions;
    using Bars.GkhGji.Regions.Samara.Report;
    using Bars.GkhGji.Regions.Samara.Report.Form123Samara;
    using Bars.GkhGji.Regions.Samara.ViewModel;

    using Castle.MicroKernel.Registration;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            Container.Register(Component.For<IResourceManifest>().Named("GkhGji.Regions.Samara resources").ImplementedBy<ResourceManifest>().LifeStyle.Transient);
            Container.Register(Component.For<IPermissionSource>().ImplementedBy<GkhGjiRegionsSamaraPermissionMap>());

            Container.Register(
                Component.For<IReminderRule>().Named("GkhGji.Regions.Samara.Reminder.AppealCitsReminderRule")
                .ImplementedBy<AppealCitsReminderRule>()
                .LifeStyle.Transient);

            Container.Register(
                Component.For<IReminderRule>().Named("GkhGji.Regions.Samara.Reminder.InspectionReminderRule")
                .ImplementedBy<InspectionReminderRule>()
                .LifeStyle.Transient);


            ReplaceComponents();

            RegisterReports();

            RegisterControllers();

            RegisterViewModels();

            RegisterServices();

            RegisterInspectionRules();

            RegisterInterceptors();
        }

        private void RegisterInterceptors()
        {
            Container.RegisterDomainInterceptor<ProtocolArticleLaw, ProtocolArticleLawInterceptor>();
        }

        private void RegisterInspectionRules()
        {
            Container.ReplaceComponent<IDocumentGjiRule>(typeof(ActCheckToProtocolRule),
                       Component.For<IDocumentGjiRule>().ImplementedBy<SamaraActCheckToProtocolRule>().LifeStyle.Transient);

            Container.ReplaceComponent<IDocumentGjiRule>(typeof(ActCheckToPrescriptionRule),
                        Component.For<IDocumentGjiRule>().ImplementedBy<SamaraActCheckToPrescriptionRule>().LifeStyle.Transient);

            Container.ReplaceComponent<IDocumentGjiRule>(typeof(ActRemovalToProtocolRule),
                        Component.For<IDocumentGjiRule>().ImplementedBy<SamaraActRemovalToProtocolRule>().LifeStyle.Transient);

        }

        private void RegisterServices()
        {
            Container.RegisterTransient<IAppealCitsTesterService, AppealCitsTesterService>();
        }

        private void RegisterReports()
        {
            #region Отчеты
            Container.Register(Component.For<IPrintForm>().Named("GJI Report.ProtocolResponsibility_2").ImplementedBy<ProtocolResponsibility_2>().LifeStyle.Transient);
            Container.Register(Component.For<IPrintForm>().Named("GJI Report.PrescriptionViolationRemoval").ImplementedBy<PrescriptionViolationRemoval>().LifeStyle.Transient);
            Container.Register(Component.For<IPrintForm>().Named("GJI Report.Form123Samara").ImplementedBy<Form123SamaraReport>().LifeStyle.Transient);
            Container.Register(Component.For<IPrintForm>().Named("GJI Report.ControlAppealsExecution").ImplementedBy<ControlAppealsExecution>().LifeStyle.Transient);
            Container.Register(Component.For<IPrintForm>().Named("GJI Report.JournalAppealsSamara").ImplementedBy<JournalAppeals>().LifeStyle.Transient);
            #endregion
        }

        private void RegisterControllers()
        {
            Container.RegisterController<AppealCitsTesterController>();
        }

        private void RegisterViewModels()
        {
            Container.RegisterTransient<IViewModel<AppealCitsTester>, AppealCitsTesterViewModel>();
        }

        private void ReplaceComponents()
        {
            Container.ReplaceComponent<IGkhBaseReport>(
                typeof(GkhGji.Report.PrescriptionGjiReport),
                Component.For<IGkhBaseReport>().ImplementedBy<PrescriptionGjiReport>().LifeStyle.Transient);

            Container.ReplaceComponent<IGjiScriptService>(
                typeof(GjiScriptService),
                Component.For<IGjiScriptService>().ImplementedBy<SamaraGjiScriptService>().LifeStyle.Transient);

            Container.ReplaceComponent<IDomainServiceInterceptor<AppealCits>>(
                typeof(Bars.GkhGji.Interceptors.AppealCitsServiceInterceptor),
                Component.For<IDomainServiceInterceptor<AppealCits>>().ImplementedBy<Bars.GkhGji.Regions.Samara.Interceptors.AppealCitsServiceInterceptor>().LifeStyle.Transient);

            this.Container.ReplaceTransient<IAppealCitsService<ViewAppealCitizens>, GkhGji.DomainService.AppealCitsService, DomainService.AppealCitsService>();

            Container.ReplaceComponent<IRuleChangeStatus>(
                typeof(GkhGji.StateChange.InspectionValidationRule),
                Component.For<IRuleChangeStatus>().ImplementedBy<StateChange.InspectionValidationRule>().LifeStyle.Transient);

            Container.ReplaceComponent<IRuleChangeStatus>(
                typeof(GkhGji.StateChange.DisposalValidationNumberTatRule),
                Component.For<IRuleChangeStatus>().ImplementedBy<StateChange.DisposalValidationNumberRuleSamara>().LifeStyle.Transient);

            Container.ReplaceComponent<IDomainServiceInterceptor<PrescriptionCancel>>(
                typeof(GkhGji.Interceptors.PrescriptionCancelInterceptor),
                Component.For<IDomainServiceInterceptor<PrescriptionCancel>>().ImplementedBy<PrescriptionCancelInterceptor>().LifeStyle.Transient);
        }
    }
}