namespace Bars.GkhGji.Regions.Yanao
{
    using B4.Modules.Reports;
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Contracts;
    using Bars.GkhGji.NumberValidation;
    using Bars.GkhGji.Regions.Yanao.NumberValidation;
    using Bars.GkhGji.Regions.Yanao.Report;
    using Bars.GkhGji.StateChange;
    using Castle.MicroKernel.Registration;
    using Entities;
    using Interceptor;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
	        this.Container.RegisterTransient<INavigationProvider, NavigationProvider>();

	        this.Container.Register(
                Component.For<IResourceManifest>().Named("GkhGji.Regions.Yanao resources").ImplementedBy<ResourceManifest>()
                .LifeStyle.Transient);

	        this.Container.ReplaceComponent<IDisposalText>(
                typeof(GkhGji.TextValues.DisposalText),
                Component.For<IDisposalText>().ImplementedBy<TextValues.DisposalText>());

	        this.Container.ReplaceComponent<IBaseStatementAction>(
                typeof(InspectionAction.BaseStatementAction),
                Component.For<IBaseStatementAction>().ImplementedBy<BaseStatementAction>());

	        this.Container.ReplaceComponent<IGkhBaseReport>(
                typeof(GkhGji.Report.DisposalGjiReport),
                Component.For<IGkhBaseReport>().ImplementedBy<Report.DisposalGji.DisposalGjiReport>());

	        this.Container.ReplaceComponent<IDomainServiceInterceptor<PrescriptionViol>>(
                typeof(Interceptors.PrescriptionViolInterceptor),
                Component.For<IDomainServiceInterceptor<PrescriptionViol>>().ImplementedBy<PrescriptionViolInterceptor>());

            //ЯНАО
	        this.Container.Register(Component.For<IRuleChangeStatus>().ImplementedBy<ActCheckValidationNumberYanaoRule>().LifeStyle.Transient);
	        this.Container.Register(Component.For<IRuleChangeStatus>().ImplementedBy<ActRemovalValidationNumberYanaoRule>().LifeStyle.Transient);
	        this.Container.Register(Component.For<IRuleChangeStatus>().ImplementedBy<ActSurveyValidationNumberYanaoRule>().LifeStyle.Transient);
	        this.Container.Register(Component.For<IRuleChangeStatus>().ImplementedBy<DisposalValidationNumberYanaoRule>().LifeStyle.Transient);
	        this.Container.Register(Component.For<IRuleChangeStatus>().ImplementedBy<PrescriptionValidationNumberYanaoRule>().LifeStyle.Transient);
	        this.Container.Register(Component.For<IRuleChangeStatus>().ImplementedBy<PresentationValidationNumberYanaoRule>().LifeStyle.Transient);
	        this.Container.Register(Component.For<IRuleChangeStatus>().ImplementedBy<ProtocolValidationNumberYanaoRule>().LifeStyle.Transient);
	        this.Container.Register(Component.For<IRuleChangeStatus>().ImplementedBy<ResolProsValidationNumberYanaoRule>().LifeStyle.Transient);
	        this.Container.Register(Component.For<IRuleChangeStatus>().ImplementedBy<ResolutionValidationNumberYanaoRule>().LifeStyle.Transient);
	        this.Container.Register(Component.For<IRuleChangeStatus>().ImplementedBy<DocGjiValidationNumberYanaoRule>().LifeStyle.Transient);

	        this.Container.Register(Component.For<INumberValidationRule>().ImplementedBy<BaseYanaoValidationRule>().LifeStyle.Transient);

	        this.Container.Register(Component.For<IPermissionSource>().ImplementedBy<GkhGjiPermissionMap>());

	        this.Container.Register(Component.For<IPrintForm>().Named("GkhGji Reports.GJI.Form1StateHousingInspection").ImplementedBy<Form1StateHousingInspection>().LifeStyle.Transient);
            
        }
    }
}