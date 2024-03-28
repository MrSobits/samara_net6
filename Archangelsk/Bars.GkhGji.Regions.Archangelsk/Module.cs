namespace Bars.GkhGji.Regions.Archangelsk
{
    using B4;

    using Bars.B4.IoC;
    using Bars.B4.Modules.States;
    using Bars.B4.Windsor;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Contracts;
    using Bars.GkhGji.Contracts.Reminder;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Archangelsk.Controllers;
    using Bars.GkhGji.Regions.Archangelsk.DomainService;
    using Bars.GkhGji.Regions.Archangelsk.Entities;
    using Bars.GkhGji.Regions.Archangelsk.Interceptors;
    using Bars.GkhGji.Regions.Archangelsk.Report;
    using Bars.GkhGji.Regions.Archangelsk.ViewModel;

    using Castle.MicroKernel.Registration;

    public partial class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            Container.ReplaceComponent<IBaseStatementAction>(
                typeof(InspectionAction.BaseStatementAction),
                Component.For<IBaseStatementAction>().ImplementedBy<BaseStatementAction>());

            Container.RegisterTransient<IStatefulEntitiesManifest, StatefulEntityManifest>("GkhGji.Regions.Archangelsk statefulentity");

            Container.Register(
                Component.For<IResourceManifest>()
                         .Named("GkhGji.Regions.Archangelsk resources")
                         .ImplementedBy<ResourceManifest>()
                         .LifeStyle.Transient);

            Container.Register(
                Component.For<IReminderRule>().Named("GkhGji.Regions.Archangelsk.Reminder.AppealCitsReminderRule")
                .ImplementedBy<AppealCitsReminderRule>()
                .LifeStyle.Transient);

            Container.Register(
                Component.For<IReminderRule>().Named("GkhGji.Regions.Archangelsk.Reminder.InspectionReminderRule")
                .ImplementedBy<InspectionReminderRule>()
                .LifeStyle.Transient);

            RegisterReports();

            RegisterControllers();

            RegisterInterceptors();

            RegisterVirewModels();

            RegisterServices();

            ReplaceComponents();
        }

        private void RegisterReports()
        {
            // Печатные формы
            Container.Register(Component.For<IGkhBaseReport>().ImplementedBy<DisposalGjiMotivatedRequestReport>().LifeStyle.Transient);
        }

        private void RegisterControllers()
        {
            Container.RegisterController<AppealCitsExecutantController>();   
        }

        private void RegisterInterceptors()
        {
            Container.Register(Component.For<IDomainServiceInterceptor<AppealCitsExecutant>>().ImplementedBy<AppealCitsExecutantInterceptor>().LifeStyle.Transient);
        }

        private void RegisterVirewModels()
        {
            Container.RegisterViewModel<AppealCitsExecutant, AppealCitsExecutantViewModel>();
        }

        private void RegisterServices()
        {
            Container.RegisterTransient<IAppealCitsExecutantService, AppealCitsExecutantService>();
        }

        private void ReplaceComponents()
        {
            Container.ReplaceComponent<IAppealCitsService>(
                typeof(Bars.GkhGji.DomainService.AppealCitsService),
                Component.For<IAppealCitsService>().ImplementedBy<DomainService.AppealCitsService>().LifeStyle.Transient);

            Container.ReplaceComponent<IDomainServiceInterceptor<AppealCits>>(
                typeof(GkhGji.Interceptors.AppealCitsServiceInterceptor),
                Component.For<IDomainServiceInterceptor<AppealCits>>().ImplementedBy<Interceptors.AppealCitsServiceInterceptor>().LifeStyle.Transient);

            Container.ReplaceComponent<IDomainServiceInterceptor<PrescriptionCancel>>(
                typeof(GkhGji.Interceptors.PrescriptionCancelInterceptor),
                Component.For<IDomainServiceInterceptor<PrescriptionCancel>>().ImplementedBy<PrescriptionCancelInterceptor>().LifeStyle.Transient);
        }
    }
}