namespace Bars.GkhCr;

using System.Collections.Generic;
using System.Linq;

using B4.Application;
using B4.Config;
using B4.DataAccess;
using B4.Events;
using B4.Modules.Quartz;
using B4.Modules.Reports;
using B4.Utils;

using Bars.B4;
using Bars.B4.IoC;
using Bars.B4.Modules.Analytics.Data;
using Bars.B4.Modules.Analytics.Reports.Params;
using Bars.B4.Modules.DataExport.Domain;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.FileStorage.DomainService;
using Bars.B4.Modules.NHibernateChangeLog;
using Bars.B4.Modules.Pivot;
using Bars.B4.Modules.States;
using Bars.B4.Windsor;
using Bars.Gkh;
using Bars.Gkh.ConfigSections.ClaimWork;
using Bars.Gkh.ConfigSections.ClaimWork.BuilderContract;
using Bars.Gkh.ConfigSections.Cr;
using Bars.Gkh.DomainService;
using Bars.Gkh.Entities.Dicts.Multipurpose;
using Bars.Gkh.Modules.ClaimWork.Contracts;
using Bars.Gkh.Modules.ClaimWork.Controllers;
using Bars.Gkh.Modules.ClaimWork.DomainService;
using Bars.Gkh.Modules.ClaimWork.Export;
using Bars.Gkh.Modules.ClaimWork.Extension;
using Bars.Gkh.PassportProvider;
using Bars.Gkh.SystemDataTransfer.Meta;
using Bars.GkhCr.Controllers;
using Bars.GkhCr.Controllers.BankStatement;
using Bars.GkhCr.Controllers.ObjectCr;
using Bars.GkhCr.DataProviders;
using Bars.GkhCr.DomainService;
using Bars.GkhCr.DomainService.Impl;
using Bars.GkhCr.DomainService.ObjectCr;
using Bars.GkhCr.DomainService.SpecialObjectCr;
using Bars.GkhCr.Entities;
using Bars.GkhCr.ExecutionAction;
using Bars.GkhCr.Export;
using Bars.GkhCr.FormatDataExport.Domain.Impl;
using Bars.GkhCr.Import;
using Bars.GkhCr.Interceptors;
using Bars.GkhCr.Interceptors.Dicts;
using Bars.GkhCr.LogMap.Provider;
using Bars.GkhCr.Modules.ClaimWork.DomainService.Impl;
using Bars.GkhCr.Modules.ClaimWork.Entities;
using Bars.GkhCr.Modules.ClaimWork.Navigation;
using Bars.GkhCr.Modules.ClaimWork.Permission;
using Bars.GkhCr.Modules.ClaimWork.ViewModel;
using Bars.GkhCr.Navigation;
using Bars.GkhCr.Permissions;
using Bars.GkhCr.Report;
using Bars.GkhCr.Report.AreaCrMkd;
using Bars.GkhCr.Report.ReportByWorkKinds;
using Bars.GkhCr.StateChange;
using Bars.GkhCr.SystemDataTransfer;
using Bars.GkhCr.ViewModel;
using Bars.GkhCr.ViewModel.SpecialObjectCr;
using Bars.GkhCR.ExecutionAction;
using Bars.GkhCR.Navigation;
using Bars.GkhCr.ViewModel.ContrDate;

using Castle.MicroKernel.Lifestyle;
using Castle.MicroKernel.Registration;

using DomainService.ObjectCr.Impl;

using Gkh.Entities;
using Gkh.Modules.ClaimWork.DomainService.Lawsuit;
using Gkh.Report;
using Gkh.Utils;

using Hmao.DomainService;

using Microsoft.Extensions.Logging;

using Modules.ClaimWork.Controllers;
using Modules.ClaimWork.DomainService;
using Modules.ClaimWork.DomainService.Lawsuit;
using Modules.ClaimWork.Export;

using Quartz;

using Services;

using ViewModel.ObjectCr;

using Enum = B4.Modules.Analytics.Reports.Params.Enum;
using RealtyObjTypeWorkProvider = Bars.GkhCr.Providers.RealtyObjTypeWorkProvider;

public partial class Module : AssemblyDefinedModule
{
    public override void Install()
    {
        this.Container.RegisterGkhConfig<GkhCrConfig>();
        this.Container.RegisterGkhConfig<BuilderContractClaimWorkConfig>();

        this.Container.RegisterTransient<IResourceManifest, ResourceManifest>("GkhCR resources");
        this.Container.RegisterTransient<IStatefulEntitiesManifest, StatefulEntityManifest>("GkhCR statefulentity");
        this.Container.RegisterTransient<IClientRouteMapRegistrar, ClientRouteMapRegistrar>();
        this.Container.Register(Component.For<IPermissionSource>().ImplementedBy<GkhCrPermissionMap>());

        this.Container.RegisterTransient<IViewCollection, GkhCrViewCollection>("GkhCrViewCollection");

        this.Container.RegisterTransient<IRuleChangeStatus, GkhCrAllowRenegRule>();
        this.Container.RegisterTransient<IRuleChangeStatus, ProtocolCrValidationRule>();

        this.RegisterControllers();

        this.Container.Register(
            Component.For<IPrintForm>().Named("CR Report.BuilderRegister").ImplementedBy<BuilderRegisterReport>().LifeStyle.Transient);
        this.Container.Register(
            Component.For<IPrintForm>().Named("CR Report.ObjectCrInfoService").ImplementedBy<ObjectCrInfoService>().LifeStyle.Transient);
        this.Container.Register(Component.For<IPrintForm>().Named("CR Report.CountPhoto").ImplementedBy<CountPhotoReport>().LifeStyle.Transient);
        this.Container.Register(
            Component.For<IPrintForm>().Named("CR Report.InfoActProtocolObjectCr").ImplementedBy<InfoActProtocolObjectCr>().LifeStyle.Transient);
        this.Container.Register(
            Component.For<IPrintForm>().Named("CR Report.WeeklyAboutCrProgress").ImplementedBy<WeeklyAboutCrProgress>().LifeStyle.Transient);
        this.Container.Register(
            Component.For<IPrintForm>().Named("CR Report.CountSumActAccepted").ImplementedBy<CountSumActAccepted>().LifeStyle.Transient);
        this.Container.Register(Component.For<IPrintForm>().Named("CR Report.YearlyForFund").ImplementedBy<YearlyForFund>().LifeStyle.Transient);
        this.Container.Register(
            Component.For<IPrintForm>().Named("CR Report.PlanedProgramIndicators").ImplementedBy<PlanedProgramIndicatorsReport>().LifeStyle.Transient);
        this.Container.Register(
            Component.For<IPrintForm>().Named("CR Report.MkdByTypeRepairRegister").ImplementedBy<MkdByTypeRepairRegister>().LifeStyle.Transient);
        this.Container.Register(Component.For<IPrintForm>().Named("CR Report.FormForMoscow3").ImplementedBy<FormForMoscow3>().LifeStyle.Transient);
        this.Container.Register(
            Component.For<IPrintForm>().Named("GJI Report.BuildContractsReestrReport").ImplementedBy<BuildContractsReestrReport>().LifeStyle.Transient);
        this.Container.Register(Component.For<IPrintForm>().Named("CR Report.RegistryBySource").ImplementedBy<RegistryBySource>().LifeStyle.Transient);
        this.Container.Register(
            Component.For<IPrintForm>().Named("CR Report.WorksProgressReport").ImplementedBy<WorksProgressReport>().LifeStyle.Transient);
        this.Container.Register(Component.For<IPrintForm>().Named("CR Report.WorksGraph").ImplementedBy<WorksGraphReport>().LifeStyle.Transient);
        this.Container.Register(Component.For<IPrintForm>().Named("CR Report.BuildersInfo").ImplementedBy<BuildersInfoReport>().LifeStyle.Transient);
        this.Container.Register(
            Component.For<IPrintForm>().Named("CR Report.RegisterPayOrderReport").ImplementedBy<RegisterPayOrderReport>().LifeStyle.Transient);
        this.Container.Register(
            Component.For<IPrintForm>()
                .Named("CR Report.DetectRepeatingProgramReport")
                .ImplementedBy<DetectRepeatingProgramReport>()
                .LifeStyle.Transient);
        this.Container.Register(
            Component.For<IPrintForm>().Named("CR Report.CompareProgramsReport").ImplementedBy<CompareProgramsReport>().LifeStyle.Transient);
        this.Container.Register(
            Component.For<IPrintForm>().Named("CR Report.FinancingAnnex4F1Report").ImplementedBy<FinancingAnnex4F1Report>().LifeStyle.Transient);
        this.Container.Register(
            Component.For<IPrintForm>().Named("CR Report.FinancingAnnex4F4").ImplementedBy<FinancingAnnex4F4>().LifeStyle.Transient);
        this.Container.Register(
            Component.For<IPrintForm>().Named("CR Report.WorkKindsByMonitoringSmr").ImplementedBy<WorkKindsByMonitoringSmrReport>().LifeStyle.Transient);
        this.Container.Register(
            Component.For<IPrintForm>().Named("CR Report.RepairProgressByKindOfWork").ImplementedBy<RepairProgressByKindOfWork>().LifeStyle.Transient);
        this.Container.Register(
            Component.For<IPrintForm>()
                .Named("CR Report.WorkKindsCompareWithBaseProgram")
                .ImplementedBy<WorkKindsCompareWithBaseProgram>()
                .LifeStyle.Transient);
        this.Container.Register(
            Component.For<IPrintForm>().Named("CR Report.FulfilledWorkAmountReport").ImplementedBy<FulfilledWorkAmountReport>().LifeStyle.Transient);
        this.Container.Register(
            Component.For<IPrintForm>()
                .Named("CR Report.CompletionOfTheGraphicOnGivenDate")
                .ImplementedBy<CompletionOfTheGraphicOnGivenDate>()
                .LifeStyle.Transient);
        this.Container.Register(
            Component.For<IPrintForm>().Named("CR Report.HousesCompletedWorkFact").ImplementedBy<HousesCompletedWorkFact>().LifeStyle.Transient);
        this.Container.Register(Component.For<IPrintForm>().Named("CR Report.StaffingWorkers").ImplementedBy<StaffingWorkers>().LifeStyle.Transient);
        this.Container.Register(
            Component.For<IPrintForm>().Named("CR Report.PlannedAllocationOfWorks").ImplementedBy<PlannedAllocationOfWorksReport>().LifeStyle.Transient);
        this.Container.Register(
            Component.For<IPrintForm>().Named("CR Report.NormativeStaffingWorkers").ImplementedBy<NormativeStaffingWorkers>().LifeStyle.Transient);
        this.Container.Register(
            Component.For<IPrintForm>().Named("CR Report.NeedMaterialsExtendedReport").ImplementedBy<NeedMaterialsExtendedReport>().LifeStyle.Transient);
        this.Container.Register(
            Component.For<IPrintForm>()
                .Named("CR Report.StaffingWorkersByRealtyObjects")
                .ImplementedBy<StaffingWorkersByRealtyObjects>()
                .LifeStyle.Transient);
        this.Container.Register(
            Component.For<IPrintForm>().Named("CR Report.ActAuditDataExpense").ImplementedBy<ActAuditDataExpense>().LifeStyle.Transient);
        this.Container.Register(
            Component.For<IPrintForm>().Named("CR Report.CountWorkCrReport").ImplementedBy<CountWorkCrReport>().LifeStyle.Transient);
        this.Container.Register(
            Component.For<IPrintForm>().Named("CR Report.PhotoArchiveReport").ImplementedBy<PhotoArchiveReport>().LifeStyle.Transient);
        this.Container.Register(
            Component.For<IPrintForm>().Named("CR Report.InformationOnObjectsCr").ImplementedBy<InformationOnObjectsCr>().LifeStyle.Transient);
        this.Container.Register(
            Component.For<IPrintForm>().Named("CR Report.HomesWithoutGraphicsReport").ImplementedBy<HomesWithoutGraphicsReport>().LifeStyle.Transient);
        this.Container.Register(
            Component.For<IPrintForm>()
                .Named("CR Report.ProtocolsActsGjiReestrReport")
                .ImplementedBy<ProtocolsActsGjiReestrReport>()
                .LifeStyle.Transient);
        this.Container.Register(
            Component.For<IPrintForm>()
                .Named("CR Report.ActuallyStartedWorksByHousesReport")
                .ImplementedBy<ActuallyStartedWorksByHousesReport>()
                .LifeStyle.Transient);
        this.Container.Register(
            Component.For<IPrintForm>()
                .Named("CR Report.DataStartedFinishedWorkReport")
                .ImplementedBy<DataStartedFinishedWorkReport>()
                .LifeStyle.Transient);
        this.Container.Register(
            Component.For<IPrintForm>().Named("CR Report.LaggingPerformanceOfWork").ImplementedBy<LaggingPerformanceOfWork>().LifeStyle.Transient);
        this.Container.Register(
            Component.For<IPrintForm>().Named("CR Report.CountRealObjByBacklogWork").ImplementedBy<CountRealObjByBacklogWork>().LifeStyle.Transient);
        this.Container.Register(
            Component.For<IPrintForm>().Named("CR Report.DataDevicesStatementReport").ImplementedBy<DataDevicesStatementReport>().LifeStyle.Transient);
        this.Container.Register(
            Component.For<IPrintForm>().Named("CR Report.StatusInformation").ImplementedBy<StatusInformation>().LifeStyle.Transient);
        this.Container.Register(
            Component.For<IPrintForm>().Named("CR Report.InformationByBuildersReport").ImplementedBy<InformationByBuildersReport>().LifeStyle.Transient);
        this.Container.Register(
            Component.For<IPrintForm>().Named("CR Report.ListByManyApartmentsHouses").ImplementedBy<ListByManyApartmentsHouses>().LifeStyle.Transient);
        this.Container.Register(Component.For<IPrintForm>().Named("CR Report.ByProgramCrNew").ImplementedBy<ByProgramCrNew>().LifeStyle.Transient);
        this.Container.Register(
            Component.For<IPrintForm>().Named("CR Report.UploadingAtDataForFund").ImplementedBy<UploadingAtDataForFund>().LifeStyle.Transient);
        this.Container.Register(
            Component.For<IPrintForm>().Named("CR Report.ProgrammCrRealization").ImplementedBy<ProgrammCrRealization>().LifeStyle.Transient);
        this.Container.Register(
            Component.For<IPrintForm>().Named("CR Report.NeedMaterialsReport").ImplementedBy<NeedMaterialsReport>().LifeStyle.Transient);
        this.Container.Register(
            Component.For<IPrintForm>().Named("CR Report.FillingPassportControl").ImplementedBy<FillingPassportControl>().LifeStyle.Transient);
        this.Container.Register(
            Component.For<IPrintForm>().Named("GJI Report.RealObjByMonthlyCr").ImplementedBy<RealObjByMonthlyCr>().LifeStyle.Transient);
        this.Container.Register(
            Component.For<IPrintForm>().Named("GJI Report.ContractObjectCrRegister").ImplementedBy<ContractObjectCrRegister>().LifeStyle.Transient);
        this.Container.Register(
            Component.For<IPrintForm>()
                .Named("GJI Report.ProgramCrInformationForFundReport")
                .ImplementedBy<ProgramCrInformationForFundReport>()
                .LifeStyle.Transient);
        this.Container.Register(
            Component.For<IPrintForm>().Named("CR Report.EconomyByTypeWork").ImplementedBy<EconomyByTypeWork>().LifeStyle.Transient);
        this.Container.Register(
            Component.For<IPrintForm>()
                .Named("CR Report.ProgramCrProgressOperativeReport")
                .ImplementedBy<ProgramCrProgressOperativeReport>()
                .LifeStyle.Transient);
        this.Container.Register(
            Component.For<IPrintForm>()
                .Named("CR Report.ExtendedInformationAboutTransferFunds")
                .ImplementedBy<ExtendedInformationAboutTransferFunds>()
                .LifeStyle.Transient);
        this.Container.Register(
            Component.For<IPrintForm>().Named("CR Report.AnalysOfRealizByProgram").ImplementedBy<AnalysOfRealizByProgram>().LifeStyle.Transient);
        this.Container.Register(
            Component.For<IPrintForm>().Named("CR Report.QuartAndAnnualRepByFundExt").ImplementedBy<QuartAndAnnualRepByFundExt>().LifeStyle.Transient);
        this.Container.Register(
            Component.For<IPrintForm>()
                .Named("CR Report.DetectRepeatingProgramDistribServicesReport")
                .ImplementedBy<DetectRepeatingProgramDistribServicesReport>()
                .LifeStyle.Transient);
        this.Container.Register(
            Component.For<IPrintForm>().Named("CR Report.InformationAboutContractors").ImplementedBy<InformationAboutContractors>().LifeStyle.Transient);
        this.Container.Register(
            Component.For<IPrintForm>().Named("CR Report.CrAggregatedReport").ImplementedBy<CrAggregatedReport>().LifeStyle.Transient);
        this.Container.Register(
            Component.For<IPrintForm>().Named("CR Report.EconomyByTypeWorkToFund").ImplementedBy<EconomyByTypeWorkToFund>().LifeStyle.Transient);
        this.Container.Register(Component.For<IPrintForm>().Named("CR Report.ObjectCrInfo").ImplementedBy<ObjectCrInfo>().LifeStyle.Transient);
        this.Container.Register(
            Component.For<IPrintForm>().Named("CR Report.RegisterMkdByTypeRepair").ImplementedBy<RegisterMkdByTypeRepair>().LifeStyle.Transient);
        this.Container.Register(
            Component.For<IPrintForm>()
                .Named("CR Report.InformOnHousesIncludedProgramCrReport")
                .ImplementedBy<InformOnHousesIncludedProgramCrReport>()
                .LifeStyle.Transient);
        this.Container.Register(Component.For<IPrintForm>().Named("CR Report.ArchiveSmrReport").ImplementedBy<ArchiveSmrReport>().LifeStyle.Transient);
        this.Container.RegisterTransient<IPrintForm, RegisterMkdToBeRepaired>("CR Report.RegisterMkdToBeRepaired");

        this.Container.Register(
            Component.For<IPrintForm, IPivotModel>().Named("CR Report.JournalKr34").ImplementedBy<JournalKr34Report>().LifeStyle.Transient);
        this.Container.Register(Component.For<IPrintForm, IPivotModel>().Named("CR Report.JournalCr6").ImplementedBy<JournalCr6>().LifeStyle.Transient);
        this.Container.Register(Component.For<IPrintForm, IPivotModel>().Named("CR Report.JournalKr1").ImplementedBy<JournalKr1>().LifeStyle.Transient);
        this.Container.Register(
            Component.For<IPrintForm, IPivotModel>()
                .Named("CR Report.WorkScheldudeInfoReport")
                .ImplementedBy<WorkScheldudeInfoReport>()
                .LifeStyle.Transient);
        this.Container.Register(Component.For<IPrintForm, IPivotModel>().Named("CR Report.AreaCrMkd").ImplementedBy<AreaCrMkd>().LifeStyle.Transient);
        this.Container.Register(
            Component.For<IPrintForm, IPivotModel>().Named("CR Report.DefectListReport").ImplementedBy<DefectListReport>().LifeStyle.Transient);

        this.Container.RegisterTransient<IPrintForm, TatListByManyApartmentsHouses>("CR Report.ListByManyApartmentsHouses.Tat");
        this.Container.RegisterTransient<IPrintForm, TatFormForMoscow3>("CR Report.FormForMoscow3.Tat");
        this.Container.RegisterTransient<IPrintForm, TatYearlyForFund>("CR Report.YearlyForFund.Tat");

        this.Container.RegisterSingleton<ITransferEntityProvider, TransferEntityProvider>();

        Component.For<IPrintForm>()
            .Named("CR Report.CrWorkTypeSum")
            .ImplementedBy<CrWorkTypeSumReport>()
            .LifestyleTransient()
            .RegisterIn(this.Container);

        this.Container.RegisterTransient<IGkhBaseReport, BuildContractCrReport>();

        #region ViewModel
        this.Container.RegisterViewModel<TypeWorkCrHistory, TypeWorkCrHistoryViewModel>();

        this.Container.RegisterViewModel<BuildContractClaimWork, BuildContractClwViewModel>();
        this.Container.RegisterViewModel<BuilderViolator, BuilderViolatorViewModel>();
        this.Container.RegisterViewModel<BuilderViolatorViol, BuilderViolatorViolViewModel>();
        this.Container.RegisterViewModel<BuildContractClwViol, BuildContractClwViolViewModel>();

        this.Container.RegisterViewModel<DesignAssignment, DesignAssignmentViewModel>();

        Component.For<IViewModel<BankStatement>>().ImplementedBy<BankStatementViewModel>().LifestyleTransient().RegisterIn(this.Container);

        Component.For<IViewModel<BasePaymentOrder>>().ImplementedBy<BasePaymentOrderViewModel>().LifestyleTransient().RegisterIn(this.Container);

        Component.For<IViewModel<PaymentOrderIn>>().ImplementedBy<PaymentOrderInViewModel>().LifestyleTransient().RegisterIn(this.Container);

        Component.For<IViewModel<PaymentOrderOut>>().ImplementedBy<PaymentOrderOutViewModel>().LifestyleTransient().RegisterIn(this.Container);

        Component.For<IViewModel<FinanceSource>>().ImplementedBy<FinanceSourceViewModel>().LifestyleTransient().RegisterIn(this.Container);

        Component.For<IViewModel<FinanceSourceWork>>().ImplementedBy<FinanceSourceWorkViewModel>().LifestyleTransient().RegisterIn(this.Container);

        Component.For<IViewModel<ProgramCr>>().ImplementedBy<ProgramCrViewModel>().LifestyleTransient().RegisterIn(this.Container);

        Component.For<IViewModel<QualificationMember>>().ImplementedBy<QualificationMemberViewModel>().LifestyleTransient().RegisterIn(this.Container);

        Component.For<IViewModel<EstimateCalculation>>().ImplementedBy<EstimateCalculationViewModel>().LifestyleTransient().RegisterIn(this.Container);

        Component.For<IViewModel<Estimate>>().ImplementedBy<EstimateViewModel>().LifestyleTransient().RegisterIn(this.Container);

        Component.For<IViewModel<PerformedWorkAct>>().ImplementedBy<PerformedWorkActViewModel>().LifestyleTransient().RegisterIn(this.Container);

        Component.For<IViewModel<PerformedWorkActRecord>>()
            .ImplementedBy<PerformedWorkActRecordViewModel>()
            .LifestyleTransient()
            .RegisterIn(this.Container);

        Component.For<IViewModel<Qualification>>().ImplementedBy<QualificationViewModel>().LifestyleTransient().RegisterIn(this.Container);

        Component.For<IViewModel<VoiceMember>>().ImplementedBy<VoiceMemberViewModel>().LifestyleTransient().RegisterIn(this.Container);

        Component.For<IViewModel<ObjectCr>>().ImplementedBy<ObjectCrViewModel>().LifestyleTransient().RegisterIn(this.Container);

        Component.For<IViewModel<BuildContract>>().ImplementedBy<BuildContractViewModel>().LifestyleTransient().RegisterIn(this.Container);

        Component.For<IViewModel<ContractCr>>().ImplementedBy<ContractCrViewModel>().LifestyleTransient().RegisterIn(this.Container);

        Component.For<IViewModel<DefectList>>().ImplementedBy<DefectListViewModel>().LifestyleTransient().RegisterIn(this.Container);

        Component.For<IViewModel<DocumentWorkCr>>().ImplementedBy<DocumentWorkCrViewModel>().LifestyleTransient().RegisterIn(this.Container);

        Component.For<IViewModel<FinanceSourceResource>>()
            .ImplementedBy<FinanceSourceResourceViewModel>()
            .LifestyleTransient()
            .RegisterIn(this.Container);

        Component.For<IViewModel<PersonalAccount>>().ImplementedBy<PersonalAccountViewModel>().LifestyleTransient().RegisterIn(this.Container);

        Component.For<IViewModel<ProtocolCr>>().ImplementedBy<ProtocolCrViewModel>().LifestyleTransient().RegisterIn(this.Container);

        Component.For<IViewModel<ResourceStatement>>().ImplementedBy<ResourceStatementViewModel>().LifestyleTransient().RegisterIn(this.Container);

        Component.For<IViewModel<StageWorkCr>>().ImplementedBy<StageWorkCrViewModel>().LifestyleTransient().RegisterIn(this.Container);

        Component.For<IViewModel<TypeWorkCr>>().ImplementedBy<TypeWorkCrViewModel>().LifestyleTransient().RegisterIn(this.Container);

        Component.For<IViewModel<ProgramCrFinSource>>().ImplementedBy<ProgramCrFinSourceViewModel>().LifestyleTransient().RegisterIn(this.Container);

        Component.For<IViewModel<ControlDate>>().ImplementedBy<ControlDateViewModel>().LifestyleTransient().RegisterIn(this.Container);

        Component.For<IViewModel<ControlDateStageWork>>()
            .ImplementedBy<ControlDateStageWorkViewModel>()
            .LifestyleTransient()
            .RegisterIn(this.Container);

        this.Container.RegisterViewModel<PerformedWorkActPayment, PerformedWorkActPaymentViewModel>();

        this.Container.RegisterViewModel<ProgramCrChangeJournal, ProgramChangeJournalViewModel>();
        this.Container.RegisterViewModel<BuildContractTypeWork, BuildContractTypeWorkViewModel>();
        this.Container.RegisterViewModel<ContractCrTypeWork, ContractCrTypeWorkViewModel>();

        this.Container.RegisterViewModel<Competition, CompetitionViewModel>();
        this.Container.RegisterViewModel<CompetitionLot, CompetitionLotViewModel>();
        this.Container.RegisterViewModel<CompetitionLotTypeWork, CompetitionLotTypeWorkViewModel>();
        this.Container.RegisterViewModel<CompetitionLotBid, CompetitionLotBidViewModel>();
        this.Container.RegisterViewModel<CompetitionDocument, CompetitionDocumentViewModel>();
        this.Container.RegisterViewModel<CompetitionProtocol, CompetitionProtocolViewModel>();


        this.Container.RegisterViewModel<SpecialBuildContractTypeWork, SpecialBuildContractTypeWorkViewModel>();
        this.Container.RegisterViewModel<SpecialBuildContract, SpecialBuildContractViewModel>();
        this.Container.RegisterViewModel<SpecialContractCrTypeWork, SpecialContractCrTypeWorkViewModel>();
        this.Container.RegisterViewModel<SpecialContractCr, SpecialContractCrViewModel>();
        this.Container.RegisterViewModel<SpecialDefectList, SpecialDefectListViewModel>();
        this.Container.RegisterViewModel<SpecialDesignAssignment, SpecialDesignAssignmentViewModel>();
        this.Container.RegisterViewModel<SpecialDocumentWorkCr, SpecialDocumentWorkCrViewModel>();
        this.Container.RegisterViewModel<SpecialFinanceSourceResource, SpecialFinanceSourceResourceViewModel>();
        this.Container.RegisterViewModel<SpecialObjectCr, SpecialObjectCrViewModel>();
        this.Container.RegisterViewModel<SpecialPersonalAccount, SpecialPersonalAccountViewModel>();
        this.Container.RegisterViewModel<SpecialProtocolCrTypeWork, SpecialProtocolCrTypeWorkViewModel>();
        this.Container.RegisterViewModel<SpecialProtocolCr, SpecialProtocolCrViewModel>();
        this.Container.RegisterViewModel<SpecialResourceStatement, SpecialResourceStatementViewModel>();
        this.Container.RegisterViewModel<SpecialTypeWorkCrHistory, SpecialTypeWorkCrHistoryViewModel>();
        this.Container.RegisterViewModel<SpecialTypeWorkCr, SpecialTypeWorkCrViewModel>();
        this.Container.RegisterViewModel<SpecialEstimateCalculation, SpecialEstimateCalculationViewModel>();
        this.Container.RegisterViewModel<SpecialEstimate, SpecialEstimateViewModel>();
        this.Container.RegisterViewModel<SpecialPerformedWorkActRecord, SpecialPerformedWorkActRecordViewModel>();
        this.Container.RegisterViewModel<SpecialPerformedWorkAct, SpecialPerformedWorkActViewModel>();
        this.Container.RegisterViewModel<SpecialQualification, SpecialQualificationViewModel>();
        this.Container.RegisterViewModel<SpecialVoiceMember, SpecialVoiceMemberViewModel>();
        #endregion

        #region DomainService
        this.Container.RegisterDomainService<DesignAssignment, DesignAssignmentDomainService>();
        this.Container.RegisterDomainService<ObjectCr, ObjectCrDomainService>();
        this.Container.RegisterDomainService<TypeWorkCr, TypeWorkCrDomainService>();
        this.Container.RegisterDomainService<TypeWorkCrRemoval, FileStorageDomainService<TypeWorkCrRemoval>>();
        this.Container.RegisterDomainService<DocumentWorkCr, FileStorageDomainService<DocumentWorkCr>>();
        this.Container.RegisterDomainService<ContractCr, FileStorageDomainService<ContractCr>>();
        this.Container.RegisterDomainService<DefectList, DefectListDomainService>();
        this.Container.RegisterDomainService<BuildContract, BuildContractDomainService>();
        this.Container.RegisterDomainService<ProtocolCr, ProtocolCrDomainService>();
        this.Container.RegisterDomainService<EstimateCalculation, FileStorageDomainService<EstimateCalculation>>();
        this.Container.RegisterDomainService<PerformedWorkAct, FileStorageDomainService<PerformedWorkAct>>();
        this.Container.RegisterDomainService<ProgramCr, ProgramCrDomainService>();
        this.Container.RegisterDomainService<Competition, FileStorageDomainService<Competition>>();
        this.Container.RegisterDomainService<CompetitionLot, FileStorageDomainService<CompetitionLot>>();
        this.Container.RegisterDomainService<CompetitionDocument, FileStorageDomainService<CompetitionDocument>>();
        this.Container.RegisterDomainService<CompetitionProtocol, FileStorageDomainService<CompetitionProtocol>>();
        this.Container.RegisterDomainService<DesignAssignment, FileStorageDomainService<DesignAssignment>>();
        this.Container.RegisterDomainService<ProgramCr, FileStorageDomainService<ProgramCr>>();

        this.Container.RegisterDomainService<SpecialObjectCr, SpecialObjectCrDomainService>();
        this.Container.RegisterDomainService<SpecialDesignAssignment, SpecialDesignAssignmentDomainService>();
        this.Container.RegisterDomainService<SpecialDesignAssignment, FileStorageDomainService<SpecialDesignAssignment>>();
        this.Container.RegisterDomainService<SpecialTypeWorkCr, SpecialTypeWorkCrDomainService>();
        this.Container.RegisterDomainService<SpecialTypeWorkCrRemoval, FileStorageDomainService<SpecialTypeWorkCrRemoval>>();
        this.Container.RegisterDomainService<SpecialDocumentWorkCr, FileStorageDomainService<SpecialDocumentWorkCr>>();
        this.Container.RegisterDomainService<SpecialContractCr, FileStorageDomainService<SpecialContractCr>>();
        this.Container.RegisterDomainService<SpecialDefectList, SpecialDefectListDomainService>();
        this.Container.RegisterDomainService<SpecialBuildContract, SpecialBuildContractDomainService>();
        this.Container.RegisterDomainService<SpecialProtocolCr, SpecialProtocolCrDomainService>();
        this.Container.RegisterDomainService<SpecialEstimateCalculation, FileStorageDomainService<SpecialEstimateCalculation>>();
        this.Container.RegisterDomainService<SpecialPerformedWorkAct, FileStorageDomainService<SpecialPerformedWorkAct>>();
        #endregion

        #region Service
        this.Container.RegisterTransient<IBaseClaimWorkService, IBaseClaimWorkService<BuildContractClaimWork>, BuildContractClaimWorkService>();
        this.Container.RegisterTransient<IClaimWorkNavigation, BuildContractCalmWorkNavProvider>();
        this.Container.RegisterTransient<IClaimWorkPermission, BuildContractClaimWorkPermission>();
        this.Container.RegisterTransient<IBaseClaimWorkExport<BuildContractClaimWork>, BaseClaimWorkExport<BuildContractClaimWork>>();
        this.Container.RegisterTransient<IBuilderViolatorService, BuilderViolatorService>();
        this.Container.Register(Component.For<ICompetitionService>().ImplementedBy<CompetitionService>().LifeStyle.Transient);
        this.Container.Register(Component.For<ICompetitionLotTypeWorkService>().ImplementedBy<CompetitionLotTypeWorkService>().LifeStyle.Transient);
        this.Container.Register(Component.For<IObjectCrCompetitionService>().ImplementedBy<ObjectCrCompetitionService>().LifeStyle.Transient);
        this.Container.Register(Component.For<IEstimateCalculationService>().ImplementedBy<EstimateCalculationService>().LifeStyle.Transient);
        this.Container.Register(Component.For<IPerformedWorkActService>().ImplementedBy<PerformedWorkActService>().LifeStyle.Transient);
        this.Container.Register(Component.For<IFinanceSourceWorkService>().ImplementedBy<FinanceSourceWorkService>().LifeStyle.Transient);
        this.Container.Register(Component.For<IProgramCrFinSourceService>().ImplementedBy<ProgramCrFinSourceService>().LifeStyle.Transient);
        this.Container.Register(Component.For<IProgramCrService>().ImplementedBy<ProgramCrService>().LifeStyle.Transient);
        this.Container.Register(Component.For<IMonitoringSmrService>().ImplementedBy<MonitoringSmrService>().LifeStyle.Transient);
        this.Container.Register(Component.For<IProtocolService>().ImplementedBy<ProtocolService>().LifeStyle.Transient);
        this.Container.Register(Component.For<ITypeWorkCrService>().ImplementedBy<TypeWorkCrService>().LifeStyle.Transient);
        this.Container.Register(Component.For<IQualificationService>().ImplementedBy<QualificationService>().LifeStyle.Transient);
        this.Container.Register(Component.For<IVoiceMemberService>().ImplementedBy<VoiceMemberService>().LifeStyle.Transient);
        this.Container.Register(Component.For<IQualificationMemberService>().ImplementedBy<QualificationMemberService>().LifeStyle.Transient);
        this.Container.Register(Component.For<IBasePaymentOrderService>().ImplementedBy<BasePaymentOrderService>().LifeStyle.Transient);
        this.Container.Register(Component.For<IObjectCrService>().ImplementedBy<ObjectCrService>().LifeStyle.Transient);
        this.Container.Register(Component.For<IBankStatementService>().ImplementedBy<BankStatementService>().LifeStyle.Transient);
        this.Container.Register(Component.For<IControlDateService>().ImplementedBy<ControlDateService>().LifeStyle.Transient);
        this.Container.Register(Component.For<IBuildContractService>().ImplementedBy<BuildContractService>().LifeStyle.Transient);
        this.Container.Register(Component.For<ITypeWorkCrHistoryService>().ImplementedBy<TypeWorkCrHistoryService>().LifeStyle.Transient);
        this.Container.RegisterTransient<IFinanceSourceResourceService, FinanceSourceResourceService>();
        this.Container.RegisterTransient<IPerfomedWorkActIntegrationService, PerfomedWorkActIntegrationService>();
        this.Container.RegisterTransient<IContractCrService, ContractCrService>();
        this.Container.RegisterTransient<IProgramCRImportRealityObject, ProgramCRImportRealityObjectService>();


        this.Container.RegisterTransient<ISpecialObjectCrService, SpecialObjectCrService>();
        this.Container.RegisterTransient<ISpecialTypeWorkCrService, SpecialTypeWorkCrService>();
        this.Container.RegisterTransient<ISpecialTypeWorkCrHistoryService, SpecialTypeWorkCrHistoryService>();

        this.Container.RegisterTransient<ISpecialBuildContractService, SpecialBuildContractService>();
        this.Container.RegisterTransient<ISpecialFinanceSourceResourceService, SpecialFinanceSourceResourceService>();
        this.Container.RegisterTransient<ISpecialContractCrService, SpecialContractCrService>();
        this.Container.RegisterTransient<ISpecialProtocolCrTypeWorkService, SpecialProtocolCrTypeWorkService>();

        this.Container.RegisterTransient<ISpecialEstimateCalculationService, SpecialEstimateCalculationService>();
        this.Container.RegisterTransient<ISpecialPerformedWorkActService, SpecialPerformedWorkActService>();
        this.Container.RegisterTransient<ISpecialMonitoringSmrService, SpecialMonitoringSmrService>();
        this.Container.RegisterTransient<ISpecialProtocolService, SpecialProtocolService>();
        this.Container.RegisterTransient<ISpecialQualificationService, SpecialQualificationService>();
        this.Container.RegisterTransient<ISpecialVoiceMemberService, SpecialVoiceMemberService>();
        #endregion

        #region Экспорты реестров
        this.Container.Register(
            Component.For<IDataExportService>().Named("ObjectCrDataExport").ImplementedBy<ObjectCrDataExport>().LifeStyle.Transient);
        this.Container.Register(
            Component.For<IDataExportService>().Named("PerformedWorkActDataExport").ImplementedBy<PerformedWorkActDataExport>().LifeStyle.Transient);
        this.Container.Register(
            Component.For<IDataExportService>()
                .Named("EstimateCalculationDataExport")
                .ImplementedBy<EstimateCalculationDataExport>()
                .LifeStyle.Transient);
        this.Container.Register(
            Component.For<IDataExportService>().Named("QualificationDataExport").ImplementedBy<QualificationDataExport>().LifeStyle.Transient);
        this.Container.Register(
            Component.For<IDataExportService>().Named("CompetitionDataExport").ImplementedBy<CompetitionDataExport>().LifeStyle.Transient);
        Component.For<IDataExportService>()
            .Named("CRPaymentOrderDataExport")
            .ImplementedBy<PaymentOrderDataExport>()
            .LifestyleTransient()
            .RegisterIn(this.Container);
        this.Container.Register(
            Component.For<IDataExportService>().Named("BuilderViolatorDataExport").ImplementedBy<BuilderViolatorDataExport>().LifeStyle.Transient);
        #endregion

        #region Интерсепторы
        this.Container.RegisterDomainInterceptor<BuildContractClaimWork, BuildContractClwInterceptor>();
        this.Container.RegisterDomainInterceptor<BuildContract, BuildContractInterceptor>();
        this.Container.RegisterDomainInterceptor<ObjectCr, ObjectCrInterceptor>();
        this.Container.RegisterDomainInterceptor<BuilderViolator, BuilderViolatorInterceptor>();
        this.Container.RegisterDomainInterceptor<Competition, CompetitionInterceptor>();
        this.Container.RegisterDomainInterceptor<CompetitionLotBid, CompetitionLotBidInterceptor>();
        this.Container.RegisterDomainInterceptor<CompetitionLot, CompetitionLotInterceptor>();
        this.Container.RegisterDomainInterceptor<ObjectCr, ObjectCrServiceInterceptor>();
        this.Container.RegisterDomainInterceptor<PerformedWorkAct, PerformedWorkActServiceInterceptor>();
        this.Container.RegisterDomainInterceptor<DefectList, DefectListServiceInterceptor>();
        this.Container.RegisterDomainInterceptor<EstimateCalculation, EstimateCalculationServiceInterceptor>();
        this.Container.RegisterDomainInterceptor<BuildContract, BuildContractServiceInterceptor>();
        this.Container.RegisterDomainInterceptor<ContractCr, ContractCrServiceInterceptor>();
        this.Container.RegisterDomainInterceptor<ProtocolCr, ProtocolServiceInterceptor>();
        this.Container.RegisterDomainInterceptor<TypeWorkCr, TypeWorkCrServiceInterceptor>();
        this.Container.RegisterDomainInterceptor<MonitoringSmr, MonitoringSmrServiceInterceptor>();
        this.Container.RegisterDomainInterceptor<VoiceMember, VoiceMemberServiceInterceptor>();
        this.Container.RegisterDomainInterceptor<BankStatement, BankStatementServiceInterceptor>();
        this.Container.RegisterDomainInterceptor<Qualification, QualificationServiceInterceptor>();
        this.Container.RegisterDomainInterceptor<QualificationMember, QualificationMemberServiceInterceptor>();
        this.Container.RegisterDomainInterceptor<StageWorkCr, StageWorkCrServiceInterceptor>();
        this.Container.RegisterDomainInterceptor<ControlDate, ControlDateInterceptor>();
        this.Container.RegisterDomainInterceptor<PerformedWorkActPayment, PerformedWorkActPaymentInterceptor>();
        this.Container.RegisterDomainInterceptor<ProgramCr, ProgramCrInterceptor>();
        this.Container.RegisterDomainInterceptor<BuildContractTypeWork, BuildContractTypeWorkInterceptor>();
        this.Container.RegisterDomainInterceptor<FinanceSourceResource, FinanceSourceResourceInterceptor>();
        this.Container.RegisterDomainInterceptor<TypeWorkCrRemoval, TypeWorkCrRemovalInterceptor>();
        this.Container.RegisterDomainInterceptor<EstimateCalculation, EstimateCalculationInterceptor>();
        this.Container.RegisterDomainInterceptor<MultipurposeGlossaryItem, MultipurposeGlossaryItemInterceptor>();

        #region SpecialObjectCr
        this.Container.RegisterDomainInterceptor<SpecialObjectCr, SpecialObjectCrServiceInterceptor>();
        this.Container.RegisterDomainInterceptor<SpecialTypeWorkCrRemoval, SpecialTypeWorkCrRemovalInterceptor>();
        this.Container.RegisterDomainInterceptor<SpecialTypeWorkCr, SpecialTypeWorkCrServiceInterceptor>();
        this.Container.RegisterDomainInterceptor<SpecialMonitoringSmr, SpecialMonitoringSmrServiceInterceptor>();
        this.Container.RegisterDomainInterceptor<SpecialQualification, SpecialQualificationServiceInterceptor>();
        this.Container.RegisterDomainInterceptor<SpecialPerformedWorkAct, SpecialPerformedWorkActServiceInterceptor>();
        this.Container.RegisterDomainInterceptor<SpecialPerformedWorkActPayment, SpecialPerformedWorkActPaymentInterceptor>();
        this.Container.RegisterDomainInterceptor<SpecialFinanceSourceResource, SpecialFinanceSourceResourceInterceptor>();
        this.Container.RegisterDomainInterceptor<SpecialEstimateCalculation, SpecialEstimateCalculationServiceInterceptor>();
        this.Container.RegisterDomainInterceptor<SpecialDesignAssignment, SpecialDesignAssignmentInterceptor>();
        this.Container.RegisterDomainInterceptor<SpecialDefectList, SpecialDefectListServiceInterceptor>();
        this.Container.RegisterDomainInterceptor<SpecialContractCr, SpecialContractCrServiceInterceptor>();
        this.Container.RegisterDomainInterceptor<SpecialBuildContractTypeWork, SpecialBuildContractTypeWorkInterceptor>();
        this.Container.RegisterDomainInterceptor<SpecialBuildContract, SpecialBuildContractServiceInterceptor>();
        #endregion

        this.Container.RegisterDomainInterceptor<FinanceSource, FinanceSourceInterceptor>();
        this.Container.RegisterDomainInterceptor<DesignAssignment, DesignAssignmentInterceptor>();
        this.Container.RegisterDomainInterceptor<TerminationReason, TerminationReasonInterceptor>();
        #endregion

        #region Импорт
        this.Container.RegisterImport<ContractsImport>();
        this.Container.RegisterImport<FinanceSourceImport>();
        this.Container.RegisterImport<PersonalAccountImport>();
        this.Container.RegisterImport<EstimateImport>();
        this.Container.RegisterImport<PerformedWorkActImport>();
        this.Container.RegisterImport<ResourceStatementImport>();
        this.Container.RegisterImport<EstimateImportFromArps>();
        this.Container.RegisterImport<PaymentOrderImport>();
        this.Container.RegisterImport<ProgramCrImport>();
        #endregion

        #region Левак
        // Для реестра '1.1. Сведения о кап ремонте МКД' техпаспорта
        this.Container.Register(Component.For<IRealtyObjTypeWorkProvider>().ImplementedBy<RealtyObjTypeWorkProvider>().LifeStyle.Transient);

        this.Container.RegisterTransient<IClaimWorkService, CrClaimWorkService>();
        #endregion

        #region Веб-сервисы
        // TODO wcf
        //Component.For<IService>().ImplementedBy<Service>().AsWcfSecurityService().RegisterIn(this.Container);
        #endregion

        // Регистрация класса для получения информации о зависимостях
        this.Container.Register(
            Component.For<IModuleDependencies>()
                .Named("Bars.GkhCr dependencies")
                .LifeStyle.Singleton.UsingFactoryMethod(() => new ModuleDependencies(this.Container).Init()));

        //this.Container.Resolve<IEventAggregator>().GetEvent<NhStartEvent>().Subscribe<TypeContractGlossaryInitializer>();
        //this.Container.Resolve<IEventAggregator>().GetEvent<AppStartEvent>().Subscribe<TypeDocumentCrGlossaryInitializer>();

        this.RegisterBundlers();

        this.RegisterAuditLogMap();

        this.RegisterExecutionActions();
        this.RegisterCatalogs();
        this.RegisterExports();
        this.RegisterDataProviders();
        this.RegisterDDD();
        this.RegisterNavigations();
        this.RegisterServices();
        this.RegisterViewModels();
        this.RegisterQuartzTasks();
        this.RegisterFormatDataExport();
    }

    private void RegisterControllers()
    {
        this.Container.RegisterController<MenuObjectCrController>();
        this.Container.RegisterController<PhotoArchivePrintController>();

        this.Container.RegisterAltDataController<WorksCrInspection>();

        this.Container.RegisterAltDataController<Official>();


        this.Container.RegisterController<BasePaymentOrderController>();
        this.Container.RegisterAltDataController<BankStatement>();
        this.Container.RegisterAltDataController<PaymentOrderIn>();
        this.Container.RegisterAltDataController<PaymentOrderOut>();


        #region Dict
        this.Container.RegisterAltDataController<FinanceSource>();
        this.Container.RegisterController<ProgramCrController>();
        this.Container.RegisterController<QualificationMemberController>();
        this.Container.RegisterController<FinanceSourceWorkController>();
        this.Container.RegisterController<ProgramCrFinSourceController>();
        this.Container.RegisterController<ControlDateController>();
        this.Container.RegisterAltDataController<ControlDateStageWork>();
        this.Container.RegisterAltDataController<ControlDateMunicipalityLimitDate>();
        this.Container.RegisterAltDataController<StageWorkCr>();
        this.Container.RegisterAltDataController<ProgramCrChangeJournal>();
        this.Container.RegisterAltDataController<TerminationReason>();
        #endregion Dict

        #region ObjectCr
        this.Container.RegisterController<ObjectCrController>();
        this.Container.RegisterController<ProtocolCrController>();
        this.Container.RegisterController<MonitoringSmrController>();
        this.Container.RegisterController<TypeWorkCrController>();
        this.Container.RegisterController<TypeWorkCrRemovalController>();
        this.Container.RegisterController<TypeWorkCrHistoryController>();
        this.Container.RegisterController<VoiceMemberController>();
        this.Container.RegisterController<QualificationController>();

        this.Container.RegisterController<FinanceSourceResourceController>();
        this.Container.RegisterAltDataController<PersonalAccount>();

        this.Container.RegisterController<PerformedWorkActController>();
        this.Container.RegisterAltDataController<PerformedWorkActRecord>();

        this.Container.RegisterController<EstimateController>();
        this.Container.RegisterController<EstimateCalculationController>();
        this.Container.RegisterController<ResourceStatementController>();
        this.Container.RegisterController<PerformedWorkActPaymentController>();

        this.Container.RegisterController<BuildContractController>();
        this.Container.RegisterController<FileStorageDataController<DefectList>>();
        this.Container.RegisterController<FileStorageDataController<DocumentWorkCr>>();
        this.Container.RegisterController<FileStorageDataController<DesignAssignment>>();
        this.Container.RegisterController<ContractCrController>();

        this.Container.RegisterController<BuildContractTypeWorkController>();
        this.Container.RegisterController<DefectController>();

        this.Container.RegisterController<ProtocolCrTypeWorkController>();
        this.Container.RegisterAltDataController<DesignAssignmentTypeWorkCr>();

        this.Container.RegisterAltDataController<AdditionalParameters>();
        this.Container.RegisterController<ContractCrTypeWorkController>();
        #endregion ObjectCr

        #region SpecialObjectCr
        this.Container.RegisterController<SpecialObjectCrController>();
        this.Container.RegisterController<SpecialProtocolCrController>();
        this.Container.RegisterController<SpecialMonitoringSmrController>();
        this.Container.RegisterController<SpecialTypeWorkCrController>();
        this.Container.RegisterController<FileStorageDataController<SpecialTypeWorkCrRemoval>>();
        this.Container.RegisterController<SpecialTypeWorkCrHistoryController>();
        this.Container.RegisterController<SpecialVoiceMemberController>();
        this.Container.RegisterController<SpecialQualificationController>();

        this.Container.RegisterController<SpecialFinanceSourceResourceController>();
        this.Container.RegisterAltDataController<SpecialPersonalAccount>();
        this.Container.RegisterController<SpecialPerformedWorkActController>();
        this.Container.RegisterAltDataController<SpecialPerformedWorkActRecord>();
        this.Container.RegisterAltDataController<SpecialEstimate>();
        this.Container.RegisterController<SpecialEstimateCalculationController>();
        this.Container.RegisterAltDataController<SpecialResourceStatement>();
        this.Container.RegisterController<SpecialBuildContractController>();
        this.Container.RegisterController<FileStorageDataController<SpecialDefectList>>();
        this.Container.RegisterController<FileStorageDataController<SpecialDocumentWorkCr>>();
        this.Container.RegisterController<FileStorageDataController<SpecialDesignAssignment>>();
        this.Container.RegisterController<SpecialContractCrController>();
        this.Container.RegisterAltDataController<SpecialBuildContractTypeWork>();
        this.Container.RegisterController<SpecialProtocolCrTypeWorkController>();
        this.Container.RegisterAltDataController<SpecialDesignAssignmentTypeWorkCr>();
        this.Container.RegisterAltDataController<SpecialAdditionalParameters>();
        this.Container.RegisterAltDataController<SpecialContractCrTypeWork>();
        #endregion SpecialObjectCr


        #region Competition
        this.Container.RegisterController<CompetitionController>();
        this.Container.RegisterFileStorageDataController<CompetitionLot>();
        this.Container.RegisterAltDataController<CompetitionLotBid>();
        this.Container.RegisterController<CompetitionLotTypeWorkController>();
        this.Container.RegisterAltDataController<CompetitionLotTypeWork>();
        this.Container.RegisterFileStorageDataController<CompetitionDocument>();
        this.Container.RegisterFileStorageDataController<CompetitionProtocol>();
        #endregion Competition

        #region претензионная работа
        this.Container.RegisterController<BaseClaimWorkController<BuildContractClaimWork>>();
        this.Container.RegisterController<BuilderViolatorController>();
        this.Container.RegisterAltDataController<BuilderViolatorViol>();
        this.Container.RegisterAltDataController<BuildContractClwViol>();
        this.Container.RegisterController<JurJournalBuildContractController>();
        this.Container.RegisterAltDataController<ViewBuildContract>();

        ApplicationContext.Current.Events.GetEvent<AppStartEvent>()
            .Subscribe(
                _ =>
                {
                    if (this.Container.GetGkhConfig<ClaimWorkConfig>().Enabled)
                        this.CreateStatesForPir();
                });
        #endregion
    }

    private void RegisterExecutionActions()
    {
        this.Container.RegisterExecutionAction<GkhCrConfigMigrationAction>();
        this.Container.RegisterExecutionAction<UseAddWorkFromLongSetAction>();
    }

    private void CreateStatesForPir()
    {
        var states = new List<State>();
        states.Add(new State { Name = ClaimWorkStates.FirstLevelDebt });
        states.Add(new State { Name = ClaimWorkStates.StartedEnforcement });

        this.Container.CreateStates<BuildContractClaimWork>(this.Container.Resolve<ILogger>(), states, x => !x.Name.StartsWith("Utility"));
    }

    private void RegisterAuditLogMap()
    {
        this.Container.RegisterTransient<IAuditLogMapProvider, AuditLogMapProvider>();
    }

    /// <summary>
    /// Регистрация справочников
    /// </summary>
    private void RegisterCatalogs()
    {
        CatalogRegistry.Add(new Catalog
        {
            Display = "Справочник муниципальных районов",
            Id = "Municipality",
            SelectFieldClass = "B4.catalogs.MunicipalitySelectField"
        });
        CatalogRegistry.Add(new Catalog
        {
            Display = "Справочник муниципальных районов (множественный)",
            Id = "MunicipalityMulti",
            SelectFieldClass = "B4.catalogs.MunicipalityMultiSelectField"
        });

        CatalogRegistry.Add(new Catalog
        {
            Display = "Справочник программ кап. ремонта",
            Id = "ProgramCr",
            SelectFieldClass = "B4.catalogs.ProgramCrSelectField"
        });
        CatalogRegistry.Add(new Catalog
        {
            Display = "Справочник разрезов финансирования",
            Id = "FinanceSource",
            SelectFieldClass = "B4.catalogs.FinanceSourceSelectField"
        });
        CatalogRegistry.Add(new Catalog
        {
            Display = "Справочник разрезов финансирования",
            Id = "FinanceSource",
            SelectFieldClass = "B4.catalogs.FinanceSourceSelectField"
        });

        CatalogRegistry.Add(new Catalog
        {
            Display = "Справочник муниципальных образований",
            Id = "MunicipalityFormation",
            SelectFieldClass = "B4.catalogs.MunicipalityFormationSelectField"
        });
        CatalogRegistry.Add(new Catalog
        {
            Display = "Справочник муниципальных образований (множественный)",
            Id = "MunicipalityFormationMulti",
            SelectFieldClass = "B4.catalogs.MunicipalityFormationMultiSelectField"
        });

        CatalogRegistry.Add(new Catalog
        {
            Display = "Лицевой счет",
            Id = "PersonalAccount",
            SelectFieldClass = "B4.catalogs.PersonalAccountMultiSelectField"
        });
        CatalogRegistry.Add(new Catalog
        {
            Display = "Дом",
            Id = "RealityObject",
            SelectFieldClass = "B4.catalogs.RealityObjectMultiSelectField"
        });
        CatalogRegistry.Add(new Catalog
        {
            Display = "Юр. лица",
            Id = "JurialContragents",
            SelectFieldClass = "B4.catalogs.JurialContragentsMultiSelectField"
        });

        EnumRegistry.Add(new Enum
        {
            Id = "TypeContractManOrg",
            Display = "Вид управления",
            EnumJsClass = "B4.enums.TypeContractManOrgRealObj"
        });

        EnumRegistry.Add(new Enum
        {
            Id = "PersonalAccountOwnerType",
            Display = "Тип абонентов: Физ.лицо (0); Юр.лицо (1)",
            EnumJsClass = "B4.enums.ReportPersonalAccountType"
        });

        EnumRegistry.Add(new Enum
        {
            Id = "AccountType",
            Display = "Тип счета: Счет рег. оператора (0); Спец счет (1)",
            EnumJsClass = "B4.enums.ReportAccountType"
        });

        CatalogRegistry.Add(new Catalog
        {
            Display = "Программа благоустройства дворов",
            Id = "OutdoorProgram",
            SelectFieldClass = "B4.catalogs.OutdoorProgramSelectField"
        });

        CatalogRegistry.Add(new Catalog
        {
            Id = "DpkrVersion",
            Display = "Версии ДПКР",
            SelectFieldClass = "B4.catalogs.ProgramVersionSelectField"
        });
    }

    private void RegisterExports()
    {
        this.Container.RegisterTransient<IDataExportService, WorksCrExport>("WorksCrExport");
        this.Container.RegisterTransient<IDataExportService, JurJournalBuildContractExport>("JurJournalBuildContractExport");
    }

    private void RegisterDataProviders()
    {
        this.Container.RegisterTransient<IDataProvider, ManyApartmentsHouses>();
        this.Container.RegisterTransient<IDataProvider, ProgramCrControl>();
    }

    private void RegisterNavigations()
    {
        this.Container.RegisterTransient<INavigationProvider, NavigationProvider>();
        this.Container.RegisterTransient<INavigationProvider, ObjectCrMenuProvider>();
        this.Container.RegisterTransient<INavigationProvider, BankStatementMenuProvider>();
        this.Container.RegisterTransient<INavigationProvider, MonitoringSmrMenuProvider>();
        this.Container.RegisterTransient<INavigationProvider, CompetitionMenuProvider>();
        this.Container.RegisterTransient<INavigationProvider, WorksCrMenuProvider>();
        this.Container.RegisterTransient<INavigationProvider, BuildContractClaimWorkMenuProvider>();
        this.Container.RegisterNavigationProvider<RealityObjMenuProvider>();
        this.Container.RegisterNavigationProvider<SpecialObjectCrMenuProvider>();
        this.Container.RegisterNavigationProvider<SpecialMonitoringSmrMenuProvider>();
    }

    private void RegisterServices()
    {
        this.Container.RegisterTransient<IWorksCrService, WorksCrService>();
        this.Container.RegisterTransient<IProtocolCrTypeWorkService, ProtocolCrTypeWorkService>();
        this.Container.RegisterTransient<IJurJournalBuildContractService, JurJournalBuildContractService>();
        this.Container.RegisterTransient<IDefectService, DefectService>();
        this.Container.RegisterTransient<ILawsuitAutoSelector, BuildContractLawsuitAutoSelector>(BuildContractLawsuitAutoSelector.Id);
    }

    private void RegisterViewModels()
    {
        this.Container.RegisterViewModel<Official, OfficialViewModel>();
        this.Container.RegisterViewModel<WorksCrInspection, WorksCrInspectionViewModel>();
        this.Container.RegisterViewModel<ProtocolCrTypeWork, ProtocolCrTypeWorkViewModel>();
        this.Container.RegisterViewModel<ControlDateMunicipalityLimitDate, ControlDateMunicipalityLimitDateViewModel>();

        this.Container.RegisterTransient<IJurJournalType, JurJournalBuildContractType>();
    }

    private void RegisterFormatDataExport()
    {
        ContainerHelper.RegisterFormatDataExportRepository<ObjectCr, FormatDataExportObjectCrRepository>();
    }

    private void RegisterQuartzTasks()
    {
        ApplicationContext.Current.Events
            .GetEvent<AppStartEvent>()
            .Subscribe(e =>
            {
                using (this.Container.BeginScope())
                {
                    var config = this.Container.Resolve<IConfigProvider>().GetConfig();

                    if (!config.AppSettings.GetAs<bool>("Debug"))
                    {
                        var updateTime = this.Container.ResolveRepository<GkhParam>().GetAll()
                            .FirstOrDefault(x => x.Key == "RegisterUpdateTime" && x.Prefix == "BuildContractClaimWork")
                            .Return(x => x.Value)
                            .ToDateTime();

                        TriggerBuilder.Create()
                            .WithDailyTimeIntervalSchedule(
                                x =>
                                    x.StartingDailyAt(TimeOfDay.HourAndMinuteOfDay(updateTime.Hour,
                                        updateTime.Minute)))
                            .StartNow()
                            .ScheduleTask<BuildContractClaimWorkService>();
                    }
                }
            });
    }
}