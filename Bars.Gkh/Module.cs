namespace Bars.Gkh
{
    using System;
    using System.Collections.Specialized;
    using System.Globalization;
    using System.Linq;
    using System.Threading;

    using AutoMapper;

    using B4;
    using B4.Application;
    using B4.DataAccess;
    using B4.Events;
    using B4.Filter;
    using B4.Filter.Impl;
    using B4.IoC;
    using B4.Modules.Analytics.Data;
    using B4.Modules.DataExport.Domain;
    using B4.Modules.FIAS;
    using B4.Modules.FileStorage;
    using B4.Modules.FileStorage.DomainService;
    using B4.Modules.NH.Events;
    using B4.Modules.NHibernateChangeLog;
    using B4.Modules.Quartz;
    using B4.Modules.Reports;
    using B4.Modules.Security;
    using B4.Modules.States;
    using B4.Windsor;
    
    using Bars.B4.Migrations;
    using Bars.B4.Modules.Analytics.Reports;
    using Bars.B4.Modules.FileSystemStorage;
    using Bars.B4.Modules.Security.Extensions;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.B4Events;
    using Bars.Gkh.ClaimWork.Entities;
    using Bars.Gkh.Config;
    using Bars.Gkh.Config.Impl;
    using Bars.Gkh.ConfigSections.Administration;
    using Bars.Gkh.ConfigSections.ClaimWork;
    using Bars.Gkh.ConfigSections.PostalService;
    using Bars.Gkh.ConfigSections.RegOperator;
    using Bars.Gkh.ConfigSections.RegOperator.Administration;
    using Bars.Gkh.Controllers;
    using Bars.Gkh.Controllers.Administration;
    using Bars.Gkh.Controllers.Config;
    using Bars.Gkh.Controllers.Dict;
    using Bars.Gkh.Controllers.Dict.Multipurpose;
    using Bars.Gkh.Controllers.Dict.RealEstateType;
    using Bars.Gkh.Controllers.EfficiencyRating;
    using Bars.Gkh.Controllers.Licensing;
    using Bars.Gkh.Controllers.ManOrg.ManOrgContract;
    using Bars.Gkh.Controllers.MetaValueConstructor;
    using Bars.Gkh.Controllers.RealityObj;
    using Bars.Gkh.Controllers.Suggestion;
    using Bars.Gkh.Controllers.SystemInfo;
    using Bars.Gkh.DataProviders;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Domain.Cache;
    using Bars.Gkh.Domain.Impl;
    using Bars.Gkh.Domain.ParameterVersioning;
    using Bars.Gkh.Domain.ParameterVersioning.Maps;
    using Bars.Gkh.Domain.PaymentDocumentNumber;
    using Bars.Gkh.Domain.PaymentDocumentNumber.Impl;
    using Bars.Gkh.Domain.RoleFilterRestriction;
    using Bars.Gkh.Domain.RoleFilterRestriction.Impl;
    using Bars.Gkh.Domain.TableLocker;
    using Bars.Gkh.Domain.TableLocker.Impl;
    using Bars.Gkh.DomainEvent.Events;
    using Bars.Gkh.DomainEvent.Handlers;
    using Bars.Gkh.DomainEvent.Infrastructure;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.DomainService.AddressMatching;
    using Bars.Gkh.DomainService.Administration;
    using Bars.Gkh.DomainService.Administration.Impl;
    using Bars.Gkh.DomainService.Config;
    using Bars.Gkh.DomainService.Config.Impl;
    using Bars.Gkh.DomainService.Dict;
    using Bars.Gkh.DomainService.Dict.Impl;
    using Bars.Gkh.DomainService.Documentation;
    using Bars.Gkh.DomainService.Documentation.Impl;
    using Bars.Gkh.DomainService.EfficiencyRating;
    using Bars.Gkh.DomainService.EfficiencyRating.Impl;
    using Bars.Gkh.DomainService.GkhParam;
    using Bars.Gkh.DomainService.GkhParam.Impl;
    using Bars.Gkh.DomainService.Impl;
    using Bars.Gkh.DomainService.MetaValueConstructor;
    using Bars.Gkh.DomainService.Multipurpose;
    using Bars.Gkh.DomainService.Multipurpose.Impl;
    using Bars.Gkh.DomainService.Permission;
    using Bars.Gkh.DomainService.Permission.Impl;
    using Bars.Gkh.DomainService.RealityObjectOutdoor;
    using Bars.Gkh.DomainService.RealityObjectOutdoor.Impl;
    using Bars.Gkh.DomainService.RealityObjImage;
    using Bars.Gkh.DomainService.TechPassport;
    using Bars.Gkh.DomainService.TechPassport.Impl;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Administration.ExecutionAction;
    using Bars.Gkh.Entities.Administration.FormatDataExport;
    using Bars.Gkh.Entities.Administration.Notification;
    using Bars.Gkh.Entities.Administration.SystemDataTransfer;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Entities.Dicts.ContractService;
    using Bars.Gkh.Entities.Dicts.Multipurpose;
    using Bars.Gkh.Entities.EfficiencyRating;
    using Bars.Gkh.Entities.Hcs;
    using Bars.Gkh.Entities.HousingInspection;
    using Bars.Gkh.Entities.Licensing;
    using Bars.Gkh.Entities.ManOrg;
    using Bars.Gkh.Entities.RealityObj;
    using Bars.Gkh.Entities.Suggestion;
    using Bars.Gkh.ExecutionAction.ExecutionActionResolver;
    using Bars.Gkh.ExecutionAction.ExecutionActionResolver.Impl;
    using Bars.Gkh.ExecutionAction.ExecutionActionScheduler;
    using Bars.Gkh.ExecutionAction.ExecutionActionScheduler.Impl;
    using Bars.Gkh.ExecutionAction.Impl;
    using Bars.Gkh.Export;
    using Bars.Gkh.FormatDataExport.Domain;
    using Bars.Gkh.FormatDataExport.Domain.Impl;
    using Bars.Gkh.FormatDataExport.Scheduler;
    using Bars.Gkh.FormatDataExport.Scheduler.Impl;
    using Bars.Gkh.FormatDataExport.Tasks;
    using Bars.Gkh.Formulas;
    using Bars.Gkh.Formulas.Impl;
    using Bars.Gkh.GeneralState;
    using Bars.Gkh.Import;
    using Bars.Gkh.Import.ElevatorsImport;
    using Bars.Gkh.Import.FiasHelper;
    using Bars.Gkh.Import.Fund;
    using Bars.Gkh.Import.FundRealtyObjects;
    using Bars.Gkh.Import.FundRealtyObjects.ExtraDataImport.Impl;
    using Bars.Gkh.Import.Impl;
    using Bars.Gkh.Import.ImportOktmo;
    using Bars.Gkh.Import.Organization;
    using Bars.Gkh.Import.Organization.Impl;
    using Bars.Gkh.Import.ReformGkh;
    using Bars.Gkh.Import.RoImport;
    using Bars.Gkh.ImportExport;
    using Bars.Gkh.ImportExport.Impl;
    using Bars.Gkh.Interceptors;
    using Bars.Gkh.Interceptors.Dict;
    using Bars.Gkh.Interceptors.EfficiencyRating;
    using Bars.Gkh.Interceptors.Licensing;
    using Bars.Gkh.Interceptors.ManOrg;
    using Bars.Gkh.Interceptors.MetaValueConstructor;
    using Bars.Gkh.Interceptors.RealEstateType;
    using Bars.Gkh.Interceptors.RealityObjectOutdoor;
    using Bars.Gkh.Interceptors.ServOrg;
    using Bars.Gkh.Log;
    using Bars.Gkh.Log.Impl;
    using Bars.Gkh.LogMap.Provider;
    using Bars.Gkh.MetaValueConstructor.DataFillers;
    using Bars.Gkh.MetaValueConstructor.DomainModel;
    using Bars.Gkh.MetaValueConstructor.FormulaValidating;
    using Bars.Gkh.Modules.ClaimWork.Controller;
    using Bars.Gkh.Modules.ClaimWork.Controllers;
    using Bars.Gkh.Modules.ClaimWork.DomainService;
    using Bars.Gkh.Modules.ClaimWork.DomainService.Impl;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using Bars.Gkh.Modules.ClaimWork.Interceptors;
    using Bars.Gkh.Modules.ClaimWork.Repository;
    using Bars.Gkh.Modules.ClaimWork.Repository.Impl;
    using Bars.Gkh.Modules.Gkh1468.Entities;
    using Bars.Gkh.Modules.Gkh1468.Entities.ContractPart;
    using Bars.Gkh.Modules.Gkh1468.Entities.Dict;
    using Bars.Gkh.Modules.Gkh1468.Interceptors;
    using Bars.Gkh.Modules.Gkh1468.ViewModel;
    using Bars.Gkh.Navigation;
    using Bars.Gkh.PassportProvider;
    using Bars.Gkh.Permissions;
    using Bars.Gkh.Quartz;
    using Bars.Gkh.Report;
    using Bars.Gkh.Report.Licensing;
    using Bars.Gkh.Report.TechPassportSections;
    using Bars.Gkh.Repositories;
    using Bars.Gkh.Repositories.Impl;
    using Bars.Gkh.SchedulerTasks;
    using Bars.Gkh.StateChanges;
    using Bars.Gkh.TextValues;
    using Bars.Gkh.TextValues.Impl;
    using Bars.Gkh.Utils;
    using Bars.Gkh.Utils.AddressPattern;
    using Bars.Gkh.Utils.PerformanceLogging;
    using Bars.Gkh.ViewModel;
    using Bars.Gkh.ViewModel.Administration;
    using Bars.Gkh.ViewModel.Dict;
    using Bars.Gkh.ViewModel.EfficiencyRating;
    using Bars.Gkh.ViewModel.Licensing;
    using Bars.Gkh.ViewModel.ManOrg;
    using Bars.Gkh.ViewModel.MetaValueConstructor;
    using Bars.Gkh.ViewModel.RealityObject;
    using Bars.Gkh.ViewModel.Suggestion;

    using Castle.MicroKernel.Registration;

    using Entities.EmergencyObj;
    using Entities.RealEstateType;

    using global::Quartz;

    using Bars.Gkh.Migrations;
    using FiasController = Bars.Gkh.Controllers.FiasController;
    using Bars.Gkh.MigrationManager;
    using Bars.Gkh.MigrationManager.Interceptors;
    using Bars.Gkh.Modules.ClaimWork.ViewModel;
    using Bars.Gkh.Nhibernate;
    using Bars.Gkh.Services.Override;
    using Bars.Gkh.SystemDataTransfer;
    using Bars.Gkh.SystemDataTransfer.Caching;
    using Bars.Gkh.SystemDataTransfer.Domain;
    using Bars.Gkh.SystemDataTransfer.Meta;
    using Bars.Gkh.SystemDataTransfer.Tasks;
    using Bars.Gkh.ViewModel.HousingInspection;
    using Bars.Gkh.TechnicalPassport;
    using Bars.Gkh.TechnicalPassport.Impl;
    using Bars.Gkh.ViewModel.EntityHistory;

    using global::Quartz.Impl;
    using Controllers.HousingInspection;
    using Bars.Gkh.DomainService.ContragentClw.Impl;
    using Bars.Gkh.Services.Impl;
    using Bars.Gkh.Services.ServiceContracts;
    using Bars.Gkh.Entities.Administration;
    using Bars.Gkh.Services.ServiceContracts.Mail;

    using Castle.Windsor;

    using Microsoft.AspNetCore.Builder;

    using Refit;
    using Bars.Gkh.Entities.Administration.PrintCertHistory;

    /// <summary>
    /// Класс модуля
    /// </summary>
    public partial class Module : AssemblyDefinedModule, IAspNetCoreApplicationConfigurator
    {
        /// <summary>
        /// Метод инициализации модуля
        /// </summary>
        public override void Install()
        {
            this.SetDefaultCulture();

            this.Container.RegisterSingleton<IInterceptor<IMigrationManager>, ExecutionActionInterceptor>();
            this.Container.RegisterSingleton<IInterceptor<IMigrationManager>, RestartCalcServerInterceptor>();

            this.Container.Register(Component
                .For<IMigrationManager>()
                .ImplementedBy<B4.Modules.ECM7.MigrationManager>()
                .IsDefault()
                .Named(ProxyGenerator.MigrationManagerRegistrationKey)
                .UsingFactoryMethod((kernel, context) =>
                {
                    // достаём регистрацию из модуля (старую)
                    var manager = this.Container.Kernel
                        .GetHandlers(typeof(IMigrationManager))
                        .First(x => x.ComponentModel.Name != ProxyGenerator.MigrationManagerRegistrationKey)
                        .Resolve(context) as IMigrationManager;

                    // оборачиваем её в прокси
                    return ProxyGenerator.GetProxy(manager);
                }));


            this.Container.RegisterTransient<IFormulaService, FormulaService>();
            this.RegisterConfigs();
            this.Container.RegisterTransient<IFormulaTranslator, RussianFormulaTranslator>();
            this.Container.RegisterTransient<IProcessLog, ProcessLog>();

            this.Container.Register(Component.For<GkhCache>().ImplementedBy<GkhCache>().LifeStyle.Scoped());

            this.Container.ReplaceComponent(
                typeof(IFiasRepository),
                typeof(FiasRepository),
                Component.For<IFiasRepository, IGkhCustomFiasRepository>().ImplementedBy<GkhCustomFiasRepository>().LifestyleTransient());

            this.Container.RegisterController<FormPermissionController>();

            this.Container.RegisterTransient<IActionExecuteHandler, SessionTimeoutActionHandler>();
            this.Container.RegisterTransient<IX509CertificateValidator, X509ChainValidator>();
            this.Container.RegisterTransient<IFileService, FileService>();
            this.Container.RegisterTransient<IEntityExportProvider, EntityExportProvider>();
            this.Container.RegisterTransient<IImportExportProvider, ImportExportProvider>();
            this.Container.RegisterTransient<IImportExportLogger, ImportExportLogger>();
            this.Container.RegisterViewModel<ImportExport.ImportExport, ImportExportViewModel>();

            this.Container.RegisterSingleton<IMenuItemText, MenuItemText>();

            this.Container.RegisterTransient<IResourceManifest, ResourceManifest>("Gkh resources");
            this.Container.RegisterTransient<IStatefulEntitiesManifest, StatefulEntityManifest>("Gkh statefulentity");

            this.Container.Register(Component.For<IGkhUserManager>()
                .Forward<IUserManager>()
                .ImplementedBy<UserManager>()
                .LifeStyle.Scoped());

            this.Container.RegisterAuthenticationServiceHandler<GkhAuthenticationServiceHandler>();
            this.Container.Register(Component.For<IGkhReportProvider>().ImplementedBy<GkhReportProvider>().LifeStyle.Singleton);

            this.Container.Register(Component.For<IGkhReportService>().ImplementedBy<GkhReportService>());
            this.Container.Register(Component.For<IGkhImportService>().ImplementedBy<GkhImportService>());
            this.Container.Register(Component.For<IPermissionSource>().ImplementedBy<GkhPermissionMap>());
            this.Container.Register(Component.For<IFieldRequirementSource>().ImplementedBy<GkhFieldRequirementMap>());
            this.Container.Register(Component.For<IFieldRequirementService>().ImplementedBy<FieldRequirementService>());
            this.Container.Register(Component.For<IFormatDataExportEntityInfoService>().ImplementedBy<FormatDataExportEntityInfoService>());
            this.Container.RegisterTransient<IViewCollection, GkhViewCollection>("GkhViewCollection");

            this.Container.RegisterSingleton<IFormPermssionService, FormPermssionService>();

            // Регистрация класса для получения информации о зависимостях
            this.Container.Register(Component.For<IModuleDependencies>()
                .Named("Bars.Gkh dependencies")
                .LifeStyle.Singleton.UsingFactoryMethod(
                    () => new ModuleDependencies.ModuleDependencies(this.Container).Init()));

            this.Container.RegisterTransient<ILogImport, CsvLogImport>();
            this.Container.RegisterTransient<ILogImportManager, LogImportManager>();
            this.Container.RegisterTransient<IClientRouteMapRegistrar, ClientRouteMapRegistrar>();
            this.Container.RegisterTransient<IAddressPattern, AddressPattern>();

            // Заменяем фильтр для string.Contains
            this.Container.ReplaceComponent<IStringContainsOperationProvider>(typeof(StringContainsOperationProvider),
                Component.For<IStringContainsOperationProvider>().ImplementedBy<StringIgnoreSpaceContainsProvider>());

            if (!this.Container.HasComponent<IStringEndsWithOperationProvider>())
            {
                this.Container.Register(Component.For(typeof(IStringEndsWithOperationProvider))
                    .ImplementedBy(typeof(StringEndsWithOperationProvider))
                    .LifestyleTransient());
            }

            if (!this.Container.HasComponent<IStringEqOperationProvider>())
            {
                this.Container.Register(Component.For(typeof(IStringEqOperationProvider))
                    .ImplementedBy(typeof(StringEqOperationProvider))
                    .LifestyleTransient());
            }

            if (!this.Container.HasComponent<IStringNeqProvider>())
            {
                this.Container.Register(Component.For(typeof(IStringNeqProvider))
                    .ImplementedBy(typeof(StringNeqOperationProvider))
                    .LifestyleTransient());
            }

            if (!this.Container.HasComponent<IStringStartsWithOperationProvider>())
            {
                this.Container.Register(Component.For(typeof(IStringStartsWithOperationProvider))
                    .ImplementedBy(typeof(StringStartsWithOperationProvider))
                    .LifestyleTransient());
            }

            if (!this.Container.HasComponent<IGuidEqOperationProvider>())
            {
                this.Container.Register(Component.For(typeof(IGuidEqOperationProvider))
                    .ImplementedBy(typeof(GuidEqOperationProvider))
                    .LifestyleTransient());
            }

            if (!this.Container.HasComponent<IGuidNeqProvider>())
            {
                this.Container.Register(Component.For(typeof(IGuidNeqProvider))
                    .ImplementedBy(typeof(GuidNeqOperationProvider))
                    .LifestyleTransient());
            }

            this.Container.Register(
                Component.For<IPrintForm>().ImplementedBy<AdviceMkdReport>().Named("Report Bars.Gkh AdviceMKD").LifestyleTransient(),
                Component.For<IPrintForm>().ImplementedBy<RoomAreaControlReport>().Named("Report Bars.Gkh RoomAreaControl").LifestyleTransient(),
                Component.For<IPrintForm>().ImplementedBy<InformationByFloors>().Named("Report Bars.Gkh InformationByFloors").LifestyleTransient(),
                Component.For<IPrintForm>().ImplementedBy<NoteByReestrHousesReport>().Named("RF Report.NoteByReestrHouses").LifestyleTransient(),
                Component.For<IPrintForm>().ImplementedBy<ReferenceOnGroundsAccident>().Named("Report Bars.Gkh ReferenceOnGroundsAccident").LifestyleTransient(),
                Component.For<IPrintForm>().ImplementedBy<InformationOnApartments>().Named("Report Bars.Gkh Report.InformationOnApartments").LifestyleTransient(),
                Component.For<IPrintForm>().ImplementedBy<InformationOnUseBuildings>().Named("Report Bars.Gkh InformationOnUseBuildings").LifestyleTransient(),
                Component.For<IPrintForm>().ImplementedBy<InformationByEmergencyObjectsReport>().Named("Report Bars.Gkh InformationByEmergencyObjectsReport").LifestyleTransient(),
                Component.For<IPrintForm>().ImplementedBy<ReferenceByYearConstruction>().Named("Report Bars.Gkh ReferenceByYearConstruction").LifestyleTransient(),
                Component.For<IPrintForm>().ImplementedBy<ControlActivityDatOfContractByUK>().Named("Report Bars.Gkh ControlActivityDatOfContractByUK").LifestyleTransient(),
                Component.For<IPrintForm>().ImplementedBy<ReferenceWallMaterial>().Named("Report Bars.Gkh ReferenceWallMaterial").LifestyleTransient(),
                Component.For<IPrintForm>().ImplementedBy<MakingProtocolsOwnersControlReport>().Named("Report Bars.Gkh MakingProtocolsOwnersControl").LifestyleTransient(),
                Component.For<IPrintForm>().ImplementedBy<ContragentReport>().Named("Report Bars.Gkh Contragents").LifestyleTransient());

            this.Container.RegisterTransient<IGkhBaseReport, QualificationCertificateReport>("QualificationCertificate");
            this.Container.RegisterTransient<IGkhBaseReport, PersonRequestToExamReport>("PersonRequestToExamReport");
            this.Container.RegisterTransient<IGkhBaseReport, LicenseApplicationPrimaryReport>();
            this.Container.RegisterTransient<IGkhBaseReport, LicenseApplicationJurPersonReport>();
            this.Container.RegisterTransient<IGkhBaseReport, LicenseRenewalApplicationReport>();
            this.Container.RegisterTransient<IGkhBaseReport, LicenseDuplicateApplicationReport>();
            this.Container.RegisterTransient<IGkhBaseReport, NotificationOfAdmissionExamReport>("NotificationOfAdmissionExamReport");
            this.Container.RegisterTransient<IGkhBaseReport, NotificationAboutResultsQualificationExaminationReport>("NotificationAboutResultsQualificationExaminationReport");
            this.Container.RegisterTransient<IGkhBaseReport, QualificationExaminationResultsReport>("QualificationExaminationResultsReport");

            this.Container.RegisterTransient<IGkhBaseReport, CitizenSuggestionReport>();

            this.Container.ReplaceComponent<IFileManager>(typeof(FileSystemFileManager),
                Component.For<IFileManager>().ImplementedBy<Gkh.FileManager.FileSystemFileManager>());

            var autoSuggestionsProcessing = ApplicationContext.Current.Configuration.AppSettings.GetAs("AutoSuggestionsProcessing", () => false);
            if (autoSuggestionsProcessing)
            {
                ApplicationContext
                    .Current
                    .Events
                    .GetEvent<AppStartEvent>()
                    .Subscribe(e => TriggerBuilder
                        .Create()
                        .WithDailyTimeIntervalSchedule(x => x.WithIntervalInHours(24))
                        .StartNow()
                        .ScheduleTask<SuggestionsProcessingTask>());

                ApplicationContext
                    .Current
                    .Events
                    .GetEvent<AppStartEvent>()
                    .Subscribe(e => TriggerBuilder
                        .Create()
                        .WithDailyTimeIntervalSchedule(x => x.WithIntervalInHours(24))
                        .StartNow()
                        .ScheduleTask<SuggestionsClosingTask>());
            }

            // Переопределение Bars.B4.Modules.Quartz.WindsorJobListener, который безбожно писал в Info.log всякую чепух
            
            // TODO: quartz
            // this.Container.Register(Component.For<IScheduler>().IsDefault().Named(nameof(GkhWindsorJobListener)).UsingFactoryMethod((k, cc) =>
            // {
            //     var scheduler = k.Resolve<ISchedulerFactory>().GetScheduler();
            //     scheduler.ListenerManager.AddJobListener(new GkhWindsorJobListener(this.Container));
            //     return scheduler;
            // }));

            this.RegisterBundlers();

            this.Container.Register(Component.For<IOrganizationFormImportService>().ImplementedBy<OrganizationFormImportService>().LifeStyle.Transient);

            // Справочники
            this.Container.Register(Component.For<IDomainServiceInterceptor<MultipurposeGlossary>>().ImplementedBy<GlossaryInterceptor>().LifestyleTransient());
            this.Container.Register(Component.For<IDomainServiceInterceptor<MultipurposeGlossaryItem>>().ImplementedBy<GlossaryItemInterceptor>().LifestyleTransient());
            this.Container.Register(Component.For<IDomainServiceInterceptor<ManOrgLicenseRequest>>().ImplementedBy<ManOrgLicenseRequestInterceptor>().LifestyleTransient());
            this.Container.Register(Component.For<IDomainServiceInterceptor<ManOrgLicense>>().ImplementedBy<ManOrgLicenseInterceptor>().LifestyleTransient());
            this.Container.Register(Component.For<IDomainServiceInterceptor<NormativeDoc>>().ImplementedBy<NormativeDocInterceptor>().LifestyleTransient());
            this.Container.RegisterTransient<IMenuModificator, EmptyMenuModificator>();

            this.RegisterController();
            this.RegisterDomainInterceptors();
            this.RegisterDomainServices();
            this.RegisterExports();
            this.RegisterImports();
            this.RegisterNavigations();
            this.RegisterServices();
            this.RegisterViewModels();
            this.RegisterExecuteActions();
            this.RegisterReports();
            this.RegisterAuditLogMap();
            this.RegisterStateChangeRules();
            this.RegisterCodedReports();
            this.RegisterTaskExecutors();
            this.RegisterTasks();
            this.RegisterDomainEventHandlers();
            this.DataTransferConfiguration();

            this.RegisterNoServiceFilterRoles();

            this.RegisterRepositories();

            this.Container.RegisterTransient<IVersionedEntity, RealObjConditionHouseVersionMap>();

            this.RegisterWcfRiaServices();

            this.Container.RegisterTransient<IPerformanceLogger, PerformanceLogger>();
            this.Container.RegisterTransient<IPerformanceLogsCollector, SystemLogsCollector>("SystemLogsCollector");
            this.Container.RegisterSingleton<IPerformanceLoggerFactory, PerformanceLoggerFactory>();

            this.ExecuteAutoMapperConfiguration();

            this.RegisterExecutionActionScheduler();

            this.RegisterTehPasport();

            this.RegisterFormatDataExport();

            this.CheckMigration();
            this.RegisterNhGenerators();

            this.Container.RegisterJsCompressor<JsCommentCutter>("Gkh");
        }

        /// <inheritdoc />
        public void ConfigureApplication(IApplicationBuilder builder)
        {
            this.RegisterSignalR(builder);
        }

        private void SetDefaultCulture()
        {
            var culture = new CultureInfo("ru-RU");
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        private void RegisterRepositories()
        {
            this.Container.Register(
                Component.For<IStateRepository>().ImplementedBy<StateRepository>().LifeStyle.Scoped(),
                Component.For<ILawsuitRepository>().ImplementedBy<LawsuitRepository>().LifeStyle.Scoped());
        }

        private void RegisterConfigs()
        {
            this.Container.RegisterTransient<IGkhConfigStorageBackend, DatabaseConfigStorageBackend>();
            this.Container.RegisterSingleton<IGkhConfigProvider, GkhConfigProvider>();

            var eventAggregator = this.Container.Resolve<IEventAggregator>();
            eventAggregator.GetEvent<NhStartEvent>().Subscribe(a =>
            {
                var p = ApplicationContext.Current.Container.Resolve<IGkhConfigProvider>();
                p.LoadConfiguration();
            });
            eventAggregator.GetEvent<AppStartEvent>().Subscribe(a =>
            {
                var p = ApplicationContext.Current.Container.Resolve<IGkhConfigProvider>();
                p.CompleteMapping();

                ApplicationContext.Current.Container.Resolve<IEventAggregator>().GetEvent<GkhInitializedEvent>().Publish(new AppEventArgsBase { Date = DateTime.Now });
            });

            this.Container.RegisterTransient<IPermissionSource, GkhConfigPermissionMap>();

            this.Container.RegisterGkhConfig<AdministrationConfig>();
            this.Container.RegisterGkhConfig<ConfigSections.General.GeneralConfig>();
            this.Container.RegisterGkhConfig<ClaimWorkConfig>();
            this.Container.RegisterGkhConfig<RegOperatorConfig>();
            this.Container.RegisterGkhConfig<RegOperatorLogsConfig>();
            this.Container.RegisterGkhConfig<PostalServiceConfig>();

            this.Container.Register(
                Component.For<PaymentDocumentNumberRuleBase>().ImplementedBy<PaymentDocumentNumberRuleBackSlash>().LifestyleSingleton(),
                Component.For<PaymentDocumentNumberRuleBase>().ImplementedBy<PaymentDocumentNumberRuleDash>().LifestyleSingleton(),
                Component.For<PaymentDocumentNumberRuleBase>().ImplementedBy<PaymentDocumentNumberRuleDot>().LifestyleSingleton(),
                Component.For<PaymentDocumentNumberRuleBase>().ImplementedBy<PaymentDocumentNumberRuleSlash>().LifestyleSingleton(),
                Component.For<PaymentDocumentNumberRuleBase>().ImplementedBy<PaymentDocumentNumberRuleSpace>().LifestyleSingleton());
        }

        private void RegisterCodedReports()
        {
            this.Container.RegisterTransient<IDataProvider, RegionNameDataProvider>();
            this.Container.RegisterTransient<IDataProvider, QualificationCertificateData>();
            this.Container.RegisterTransient<ICodedReport, CitizenSuggestionPortalReport>("CitizenSuggestionPortalReport");
        }

        private void RegisterController()
        {
            this.Container.ReplaceController<ChangeLogController>("changeLog");
            this.Container.RegisterController<CountCacheController>();
            this.Container.RegisterController<Controllers.GkhPermissionController>();
            this.Container.ReplaceController<StatePermissionController>("statepermission");
            this.Container.ReplaceController<StateController>("state");
            this.Container.ReplaceController<StateTransferRuleController>("StateTransferRule");
            this.Container.ReplaceController<StateTransferController>("StateTransfer");

            this.Container.RegisterAltDataController<PrintCertHistory>();

            this.Container.RegisterController<FilePreviewController>();

            this.Container.RegisterController<PaymentDocumentNumberController>();
            this.Container.RegisterController<FormulaController>();
            this.Container.RegisterController<GkhConfigController>();

            this.Container.RegisterController<SystemVersionInfoController>();
            this.Container.RegisterController<CertificateController>();
            this.Container.RegisterController<ImportExportController>();
            this.Container.RegisterController<GkhParamController>();

            this.Container.RegisterController<MultipurposeGlossaryItemController>();
            this.Container.RegisterAltDataController<MultipurposeGlossary>();
            this.Container.RegisterController<GkhTasksController>();
            this.Container.RegisterController<TaskStatusController>();

            // Administration
            this.Container.RegisterController<OperatorController>();
            this.Container.RegisterAltDataController<OperatorContragent>();
            this.Container.RegisterController<MenuController>();
            this.Container.RegisterController<TemplateReplacementController>();
            this.Container.RegisterController<InstructionController>();
            this.Container.RegisterAltDataController<FieldRequirement>();
            this.Container.RegisterController<FieldsRequirementController>();
            this.Container.RegisterController<ExportController>();
            this.Container.RegisterController<FileStorageDataController<LogOperation>>();
            this.Container.RegisterAltDataController<DataTransferIntegrationSession>();
            this.Container.RegisterController<DataTransferIntegrationController>();
            this.Container.RegisterController<LocalAdminRoleController>();
            this.Container.RegisterController<FormatDataExportEntityInfoController>();
            this.Container.RegisterAltDataController<FormatDataExportEntity>();
            ContainerHelper.RegisterGkhDataController<ExecutionActionTask>();
            ContainerHelper.RegisterGkhDataController<ExecutionActionResult>();
            ContainerHelper.RegisterGkhDataController<NotifyMessage>();
            ContainerHelper.RegisterGkhDataController<NotifyPermission>();
            ContainerHelper.RegisterGkhDataController<NotifyStats>();
            ContainerHelper.RegisterGkhDataController<EmailMessage>();

            // BelayManOrgActivity
            this.Container.RegisterController<BelayManOrgActivityController>();

            // BelayOrg
            this.Container.RegisterController<BelayOrganizationController>();

            // BelayPolicy
            this.Container.RegisterController<BelayPolicyController>();
            this.Container.RegisterController<BelayPolicyRiskController>();
            this.Container.RegisterController<BelayPolicyMkdController>();
            this.Container.RegisterController<FileStorageDataController<BelayPolicyEvent>>();
            this.Container.RegisterController<FileStorageDataController<BelayPolicyPayment>>();

            // Builder
            this.Container.RegisterController<BuilderController>();
            this.Container.RegisterController<FileStorageDataController<BuilderDocument>>();
            this.Container.RegisterController<FileStorageDataController<BuilderFeedback>>();
            this.Container.RegisterController<FileStorageDataController<BuilderProductionBase>>();
            this.Container.RegisterController<FileStorageDataController<BuilderSroInfo>>();
            this.Container.RegisterController<FileStorageDataController<BuilderTechnique>>();
            this.Container.RegisterController<FileStorageDataController<BuilderWorkforce>>();
            this.Container.RegisterAltDataController<BuilderLoan>();
            this.Container.RegisterAltDataController<BuilderLoanRepayment>();

            // ClaimWork
            this.Container.RegisterController<ClaimWorkDocumentController>();
            this.Container.RegisterController<ClaimWorkReportController>();

            // Contragent
            this.Container.RegisterController<ContragentController>();
            this.Container.RegisterAltDataController<ContragentContact>();
            this.Container.RegisterAltDataController<ContragentBank>();
            this.Container.RegisterController<ContragentMunicipalityController>();
            this.Container.RegisterController<HousingInspectionMunicipalityController>();
            this.Container.RegisterFileStorageDataController<ActivityStage>();
            this.Container.RegisterAltDataController<HousingInspection>();
            this.Container.RegisterController<ContragentAdditionRoleController>();

            // Dict
            this.Container.RegisterController<WorkController>();
            this.Container.RegisterController<MunicipalityController>();
            this.Container.RegisterController<ConstructiveElementController>();
            this.Container.RegisterController<OrganizationFormController>();
            this.Container.RegisterController<InspectorController>();
            this.Container.RegisterController<CapitalGroupController>();
            this.Container.RegisterController<ConstructiveElementGroupController>();
            this.Container.RegisterController<UnitMeasureController>();
            this.Container.RegisterController<ZonalInspectionController>();
            this.Container.RegisterController<BuilderDocumentTypeController>();
            this.Container.RegisterController<ResettlementProgramController>();

            this.Container.RegisterAltDataController<LicenseProvidedDoc>();
            this.Container.RegisterAltDataController<BuildingFeature>();
            this.Container.RegisterAltDataController<Institutions>();
            this.Container.RegisterAltDataController<MunicipalitySourceFinancing>();
            this.Container.RegisterAltDataController<EmerObjResettlementProgram>();
            this.Container.RegisterAltDataController<InterlocutorInformation>();
            this.Container.RegisterAltDataController<WorkKindCurrentRepair>();
            this.Container.RegisterAltDataController<MeteringDevice>();
            this.Container.RegisterAltDataController<Position>();
            this.Container.RegisterAltDataController<RoofingMaterial>();
            this.Container.RegisterAltDataController<TypeOwnership>();
            this.Container.RegisterAltDataController<TypeService>();
            this.Container.RegisterAltDataController<WallMaterial>();
            this.Container.RegisterAltDataController<Specialty>();
            this.Container.RegisterAltDataController<Period>();
            this.Container.RegisterAltDataController<KindEquipment>();
            //this.Container.RegisterAltDataController<NormativeDoc>();
            this.Container.RegisterAltDataController<NormativeDocItem>();
            this.Container.RegisterController<NormativeDocController>();
            this.Container.RegisterAltDataController<FurtherUse>();
            this.Container.RegisterAltDataController<ReasonInexpedient>();
            this.Container.RegisterAltDataController<ResettlementProgramSource>();
            this.Container.RegisterAltDataController<BelayOrgKindActivity>();
            this.Container.RegisterAltDataController<ZonalInspectionMunicipality>();
            this.Container.RegisterAltDataController<ZonalInspectionInspector>();
            this.Container.RegisterAltDataController<KindRisk>();
            this.Container.RegisterAltDataController<TypeProject>();
            this.Container.RegisterAltDataController<InspectorSubscription>();
            this.Container.RegisterAltDataController<ProblemPlace>();
            this.Container.RegisterAltDataController<FiasOktmo>();
            this.Container.RegisterAltDataController<MunicipalityFiasOktmo>();
            this.Container.RegisterAltDataController<ContentRepairMkdWork>();
            this.Container.RegisterAltDataController<ContragentRole>();
            this.Container.RegisterAltDataController<RiskCategory>();
            this.Container.RegisterAltDataController<TypeFloor>();

            this.Container.ReplaceController<FiasController>("fias");
            this.Container.RegisterController<EntranceController>();

            this.Container.RegisterAltDataController<TypeLift>();
            this.Container.RegisterAltDataController<ModelLift>();
            this.Container.RegisterAltDataController<CabinLift>();
            this.Container.RegisterAltDataController<TypeLiftShaft>();
            this.Container.RegisterAltDataController<TypeLiftDriveDoors>();
            this.Container.RegisterAltDataController<TypeLiftMashineRoom>();
            this.Container.RegisterAltDataController<CentralHeatingStation>();
            this.Container.RegisterAltDataController<TypeInformationNpa>();
            this.Container.RegisterAltDataController<TypeNpa>();
            this.Container.RegisterAltDataController<TypeNormativeAct>();
            this.Container.RegisterAltDataController<IdentityDocumentType>();

            // EmergencyObj
            this.Container.RegisterController<EmergencyObjectController>();
            this.Container.RegisterController<EmergencyObjectDocumentsController>();

            // Import
            this.Container.RegisterController<ImportLogController>();
            this.Container.RegisterAltDataController<AddressMatch>();
            ContainerHelper.RegisterGkhDataController<FormatDataExportTask>(this.Container);
            ContainerHelper.RegisterGkhDataController<FormatDataExportResult>(this.Container);
            ContainerHelper.RegisterGkhDataController<FormatDataExportRemoteResult>(this.Container);
            ContainerHelper.RegisterGkhDataController<FormatDataExportInfo>(this.Container);

            // LocalGov
            this.Container.RegisterController<LocalGovernmentController>();
            this.Container.RegisterController<LocalGovernmentWorkModeController>();

            // PaymentAgent
            this.Container.RegisterController<PaymentAgentController>();

            // ContragentClw
            this.Container.RegisterController<ContragentClwController>();
            this.Container.RegisterAltDataController<ContragentClwMunicipality>();

            // ManOrg
            this.Container.RegisterController<ManagingOrganizationController>();
            this.Container.RegisterController<ManagingOrgWorkModeController>();
            this.Container.RegisterController<ManOrgContractOwnersController>();
            this.Container.RegisterController<ManOrgJskTsjContractController>();
            this.Container.RegisterController<ManOrgContractTransferController>();
            this.Container.RegisterController<ManagingOrgRealityObjectController>();
            this.Container.RegisterController<ManOrgBaseContractController>();
            this.Container.RegisterController<FileStorageDataController<ManagingOrgDocumentation>>();

            this.Container.RegisterAltDataController<ManagingOrgClaim>();
            this.Container.RegisterAltDataController<ManagingOrgService>();
            this.Container.RegisterAltDataController<ManagingOrgMembership>();
            this.Container.RegisterAltDataController<ManOrgContractRelation>();
            this.Container.RegisterAltDataController<ManOrgContractRealityObject>();
            this.Container.RegisterController<ManagingOrgMunicipalityController>();
            this.Container.RegisterController<FileStorageDataController<ManagingOrgRegistry>>();

            // PoliticAuth
            this.Container.RegisterController<PoliticAuthorityController>();
            this.Container.RegisterController<PoliticAuthorityWorkModeController>();

            // RealityObject
            this.Container.RegisterController<RealityObjectController>();
            this.Container.RegisterController<RealityObjectBuildingFeatureController>();
            this.Container.RegisterController<RealityObjectProtocolController>();
            this.Container.RegisterController<RealityObjectCouncillorsController>();
            this.Container.RegisterController<RealityObjectBelayPolicyController>();

            this.Container.RegisterController<FileStorageDataController<RealityObjectDirectManagContract>>();
            this.Container.RegisterController<FileStorageDataController<RealityObjectServiceOrg>>();
            this.Container.RegisterController<RealityObjectImageController>();
            this.Container.RegisterController<FileStorageDataController<RealityObjectLand>>();

            this.Container.RegisterController<FileStorageDataController<RealityObjectResOrg>>();
            this.Container.RegisterAltDataController<HouseAccountCharge>();
            this.Container.RegisterAltDataController<RealityObjectApartInfo>();
            this.Container.RegisterAltDataController<RealityObjectConstructiveElement>();
            this.Container.RegisterAltDataController<RealityObjectCurentRepair>();
            this.Container.RegisterAltDataController<RealityObjectHouseInfo>();
            this.Container.RegisterAltDataController<RealityObjectBlock>();
            this.Container.RegisterAltDataController<RealityObjectMeteringDevice>();

            this.Container.RegisterAltDataController<HouseAccount>();
            this.Container.RegisterAltDataController<HouseMeterReading>();
            this.Container.RegisterController<HouseInfoOverviewController>();
            this.Container.RegisterAltDataController<HouseOverallBalance>();
            this.Container.RegisterAltDataController<MeterReading>();
            this.Container.RegisterController<RoomController>();
            this.Container.RegisterController<RealityObjectFieldsController>();
            this.Container.RegisterAltDataController<MeteringDevicesChecks>();

            this.Container.RegisterAltDataController<RealityObjectLift>();
            this.Container.RegisterAltDataController<RealityObjectLiftSum>();
            this.Container.RegisterFileStorageDataController<RealityObjectTechnicalMonitoring>();

            this.Container.RegisterController<RealityObjectOutdoorController>();
            // Scripts
            this.Container.RegisterController<GkhScriptsController>();

            // ServiceOrg
            this.Container.RegisterController<ServiceOrganizationController>();
            this.Container.RegisterController<ServiceOrgServiceController>();
            this.Container.RegisterController<ServiceOrgRealityObjectController>();
            this.Container.RegisterController<FileStorageDataController<ServiceOrgDocumentation>>();
            this.Container.RegisterController<FileStorageDataController<ServiceOrgContract>>();
            this.Container.RegisterController<ServiceOrgMunicipalityController>();
            this.Container.RegisterAltDataController<CommunalResource>();
            this.Container.RegisterAltDataController<StopReason>();
            this.Container.RegisterAltDataController<PublicServiceOrgContractService>();
            this.Container.RegisterAltDataController<ManagementContractService>();
            this.Container.RegisterAltDataController<AgreementContractService>();
            this.Container.RegisterAltDataController<AdditionalContractService>();
            this.Container.RegisterAltDataController<CommunalContractService>();

            this.Container.RegisterAltDataController<PublicOrgServiceQualityLevel>();
            this.Container.RegisterAltDataController<PublicServiceOrgTemperatureInfo>();
            this.Container.RegisterAltDataController<TypeCustomer>();

            this.Container.RegisterAltDataController<RsoAndServicePerformerContract>();
            this.Container.RegisterAltDataController<IndividualOwnerContract>();
            this.Container.RegisterAltDataController<JurPersonOwnerContract>();
            this.Container.RegisterAltDataController<BudgetOrgContract>();
            this.Container.RegisterAltDataController<FuelEnergyResourceContract>();
            this.Container.RegisterAltDataController<BaseContractPart>();

            // Suggestion
            this.Container.RegisterController<TransitionBusinessProcessController>();
            this.Container.RegisterController<ExpiredSuggetsionClosingController>();
            this.Container.RegisterAltDataController<Rubric>();
            this.Container.RegisterAltDataController<Transition>();
            this.Container.RegisterAltDataController<CitizenSuggestionHistory>();
            this.Container.RegisterController<CitizenSuggestionController>();
            this.Container.RegisterFileStorageDataController<SuggestionComment>();
            this.Container.RegisterFileStorageDataController<CitizenSuggestionFiles>();
            this.Container.RegisterFileStorageDataController<SuggestionCommentFiles>();
            this.Container.RegisterAltDataController<CategoryPosts>();
            this.Container.RegisterController<MessageSubjectController>();

            // SupplyResOrg
            this.Container.RegisterController<SupplyResourceOrgController>();
            this.Container.RegisterController<SupplyResourceOrgMunicipalityController>();
            this.Container.RegisterController<SupplyResourceOrgRealtyObjectController>();
            this.Container.RegisterController<SupplyResourceOrgServiceController>();
            this.Container.RegisterController<FileStorageDataController<SupplyResourceOrgDocumentation>>();

            // Person
            this.Container.RegisterController<PersonController>();
            this.Container.RegisterAltDataController<PersonPlaceWork>();
            this.Container.RegisterController<FileStorageDataController<PersonDisqualificationInfo>>();
            this.Container.RegisterController<FileStorageDataController<PersonQualificationCertificate>>();
            this.Container.RegisterFileStorageDataController<QualificationDocument>();
            this.Container.RegisterAltDataController<TechnicalMistake>();
            this.Container.RegisterController<PersonRequestToExamController>();

            // Лицезия УО
            this.Container.RegisterController<ManOrgLicenseController>();
            this.Container.RegisterController<FileStorageDataController<ManOrgLicenseDoc>>();
            this.Container.RegisterController<ManOrgLicenseRequestController>();
            this.Container.RegisterAltDataController<ManOrgRequestPerson>();
            this.Container.RegisterController<FileStorageDataController<ManOrgRequestProvDoc>>();
            this.Container.RegisterController<FileStorageDataController<ManOrgRequestAnnex>>();
            this.Container.RegisterAltDataController<ManOrgLicensePerson>();

            this.Container.RegisterController<GkhReportController>(); //контроллер отчетов
            this.Container.RegisterController<GkhImportController>();

            this.Container.RegisterController<ExecutionActionController>();
            this.Container.RegisterController<GkhParamsController>();
            this.Container.RegisterController<DocumentationController>();
            this.Container.RegisterController<GkhStateTransferController>();
            this.Container.RegisterController<RealEstateTypeController>();

            this.Container.RegisterController<InstructionGroupController>();
            this.Container.RegisterAltDataController<InstructionGroupRole>();

            // Log
            this.Container.RegisterAltDataController<EntityLogLight>();
            this.Container.RegisterController<ParametersController>();

            this.Container.RegisterController<TableLockController>();
            this.Container.RegisterAltDataController<AnnexToAppealForLicenseIssuance>();

            this.Container.RegisterController<DataMetaInfoController>();

            this.Container.RegisterAltDataController<EfficiencyRatingPeriod>();
            this.Container.RegisterController<BaseDataValueController>();
            this.Container.RegisterController<ManagingOrganizationEfficiencyRatingController>();
            this.Container.RegisterController<EfficiencyRatingAnaliticsGraphController>();

            this.Container.RegisterController<FormatDataExportController>();

            this.Container.RegisterController<FormGovernmentServiceController>();
            this.Container.RegisterAltDataController<GovernmenServiceDetail>();

            this.Container.RegisterController<RegionCodeController>();
            this.Container.RegisterFileStorageDataController<TechnicalCustomer>();

            this.Container.RegisterController<HousingFundMonitoringPeriodController>();

            this.Container.RegisterAltDataController<LicenseRegistrationReason>();
            this.Container.RegisterAltDataController<LicenseRejectReason>();
            this.Container.RegisterAltDataController<HousingFundMonitoringInfo>();

            this.Container.RegisterAltDataController<GeneralStateHistory>();

            this.Container.RegisterController<EntityChangeLogController>();

            ContainerHelper.RegisterGkhDataController<EntityHistoryInfo>();
            ContainerHelper.RegisterGkhDataController<EntityHistoryField>();

            this.Container.RegisterAltDataController<BaseHouseEmergency>();
            this.Container.RegisterAltDataController<TypesHeatSource>();
            this.Container.RegisterAltDataController<TypeInterHouseHeatingSystem>();
            this.Container.RegisterAltDataController<TypesHeatedAppliances>();
            this.Container.RegisterAltDataController<NetworkAndRiserMaterials>();
            this.Container.RegisterAltDataController<NetworkInsulationMaterials>();
            this.Container.RegisterAltDataController<TypesWaterDisposalMaterial>();
            this.Container.RegisterAltDataController<FoundationMaterials>();
            this.Container.RegisterAltDataController<TypesWindowMaterials>();
            this.Container.RegisterAltDataController<TypesBearingPartRoof>();
            this.Container.RegisterAltDataController<WarmingLayersAttics>();
            this.Container.RegisterAltDataController<MaterialRoof>();
            this.Container.RegisterAltDataController<FacadeDecorationMaterials>();
            this.Container.RegisterAltDataController<TypesExternalFacadeInsulation>();
            this.Container.RegisterAltDataController<TypesExteriorWalls>();
            this.Container.RegisterAltDataController<WaterDispensers>();
            this.Container.RegisterAltDataController<CategoryConsumersEqualPopulation>();
            this.Container.RegisterAltDataController<EnergyEfficiencyClasses>();
        }

        private void RegisterDomainInterceptors()
        {
            // Administration
            this.Container.Register(
                Component.For<IDomainServiceInterceptor<Operator>>().ImplementedBy<OperatorServiceInterceptor>().LifeStyle.Transient,
                Component.For<IDomainServiceInterceptor<State>>().ImplementedBy<StateServiceInterceptor>().LifeStyle.Transient,
                Component.For<IDomainServiceInterceptor<Instruction>>().ImplementedBy<InstructionServiceInterceptor>().LifeStyle.Transient);

            this.Container.RegisterDomainInterceptor<InstructionGroup, InstructionGroupInterceptor>();
            this.Container.RegisterDomainInterceptor<Inspector, InspectorInterceptor>();
            this.Container.RegisterDomainInterceptor<ConstructiveElementGroup, ConstructiveElementGroupInterceptor>();
            this.Container.RegisterDomainInterceptor<Period, PeriodInterceptor>();
            this.Container.RegisterDomainInterceptor<Work, WorkInterceptor>();
            this.Container.RegisterDomainInterceptor<ViolClaimWork, ViolClaimWorkInterceptor>();
            this.Container.RegisterDomainInterceptor<ZonalInspection, ZonalInspectionInterceptor>();
            this.Container.RegisterDomainInterceptor<LicenseProvidedDoc, LicenseProvidedDocInterceptor>();
            this.Container.RegisterDomainInterceptor<CapitalGroup, CapitalGroupServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<TypeProject, TypeProjectInterceptor>();
            this.Container.RegisterDomainInterceptor<WorkKindCurrentRepair, WorkKindCurrentRepairInterceptor>();
            this.Container.RegisterDomainInterceptor<FormatDataExportTask, FormatDataExportTaskInterceptor>();
            this.Container.RegisterDomainInterceptor<ExecutionActionTask, ExecutionActionTaskInterceptor>();
            this.Container.RegisterDomainInterceptor<Role, RoleInterceptor>();
            this.Container.RegisterDomainInterceptor<NotifyMessage, NotifyMessageInterceptor>();

            // BelayManOrgActivity
            this.Container.RegisterDomainInterceptor<BelayManOrgActivity, BelayManOrgActivityServiceInterceptor>();

            // BelayOrg
            this.Container.RegisterDomainInterceptor<BelayOrganization, BelayOrgServiceInterceptor>();

            // BelayPolicy
            this.Container.RegisterDomainInterceptor<BelayPolicy, BelayPolicyInterceptor>();

            // Builder
            this.Container.RegisterDomainInterceptor<Builder, BuilderServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<BuilderLoan, BuilderLoanServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<Entrance, EntranceInterceptor>();

            // Contragent
            this.Container.RegisterDomainInterceptor<Contragent, ContragentServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<ContragentContact, ContragentContactServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<ActivityStage, ActivityStageInterceptor>();

            // Dict
            this.Container.RegisterDomainInterceptor<BelayOrgKindActivity, BelayOrgKindActivityServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<ConstructiveElement, ConstructiveElementServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<FurtherUse, FurtherUseServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<KindEquipment, KindEquipmentServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<KindRisk, KindRiskServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<MeteringDevice, MeteringDeviceServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<OrganizationForm, OrganizationFormServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<Position, PositionServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<Specialty, SpecialtyServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<ReasonInexpedient, ReasonInexpedientServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<ResettlementProgram, ResettlementProgramServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<ResettlementProgramSource, ResettlementProgramSourceInterceptor>();
            this.Container.RegisterDomainInterceptor<RoofingMaterial, RoofingMaterialServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<TypeOwnership, TypeOwnershipServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<TypeService, TypeServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<WallMaterial, WallMaterialServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<WorkKindCurrentRepair, WorkKindCurrentRepairServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<FiasOktmo, FiasOktmoInterceptor>();
            this.Container.RegisterDomainInterceptor<CategoryPosts, CategoryPostsInterceptor>();
            this.Container.RegisterDomainInterceptor<BuilderDocumentType, BuilderDocumentTypeInterceptor>();
            this.Container.RegisterDomainInterceptor<CentralHeatingStation, CentralHeatingStationServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<RiskCategory, RiskCategoryInterceptor>();
            this.Container.RegisterDomainInterceptor<TypeFloor, TypeFloorInterceptor>();

            // EmergencyObj
            this.Container.RegisterDomainInterceptor<EmergencyObject, EmergencyObjectServiceInterceptor>();

            // Hcs
            this.Container.RegisterDomainInterceptor<HouseAccountCharge, HouseAccountChargeInterceptor>();
            this.Container.RegisterDomainInterceptor<HouseOverallBalance, HouseOverallBalanceInterceptor>();
            this.Container.RegisterDomainInterceptor<MeterReading, MeterReadingInterceptor>();
            this.Container.RegisterDomainInterceptor<HouseAccount, HouseAccountInterceptor>();

            // LocalGov
            this.Container.RegisterDomainInterceptor<LocalGovernment, LocalGovServiceInterceptor>();

            // PaymentAgent
            this.Container.RegisterDomainInterceptor<PaymentAgent, PaymentAgentInterceptor>();

            // ContragentClw
            this.Container.RegisterDomainInterceptor<ContragentClw, ContragentClwInterceptor>();

            // ManOrg
            this.Container.RegisterDomainInterceptor<ManagingOrganization, ManOrgInterceptor>();
            this.Container.RegisterDomainInterceptor<ManOrgBaseContract, ManOrgBaseContractInterceptor>();
            this.Container.RegisterDomainInterceptor<ManOrgJskTsjContract, ManOrgJskTsjContractInterceptor>();
            this.Container.RegisterDomainInterceptor<ManOrgContractOwners, ManOrgContractOwnersInterceptor>();
            this.Container.RegisterDomainInterceptor<ManOrgContractTransfer, ManOrgContractTransferInterceptor>();
            this.Container.RegisterDomainInterceptor<ManOrgContractRealityObject, ManOrgContractRealityObjectInterceptor>();

            // PoliticAuth
            this.Container.RegisterDomainInterceptor<PoliticAuthority, PoliticAuthorityServiceInterceptor>();

            // RealityObject
            this.Container.RegisterDomainInterceptor<RealityObject, RealityObjectInterceptor>();
            this.Container.RegisterDomainInterceptor<RealityObjectDirectManagContract, RealityObjectDirectManagContractInterceptor>();
            this.Container.RegisterDomainInterceptor<RealityObjectImage, RealityObjectImageInterceptor>();

            this.Container.RegisterDomainInterceptor<RealityObjectOutdoor, RealityObjectOutdoorInterceptor>();
            // ServOrg
            this.Container.RegisterDomainInterceptor<ServiceOrganization, ServOrgServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<RealEstateTypeCommonParam, RealEstateTypeCommonParamInterceptor>();
            this.Container.RegisterDomainInterceptor<ServiceOrgContract, ServiceOrgContractInterceptor>();
            this.Container.RegisterDomainInterceptor<PublicServiceOrgContractService, PublicServiceOrgContractServiceInterceptor>();

            this.Container.RegisterDomainInterceptor<BaseContractPart, ContractPartInterceptor<BaseContractPart>>();
            this.Container.RegisterDomainInterceptor<IndividualOwnerContract, ContractPartInterceptor<IndividualOwnerContract>>();
            this.Container.RegisterDomainInterceptor<JurPersonOwnerContract, ContractPartInterceptor<JurPersonOwnerContract>>();
            this.Container.RegisterDomainInterceptor<RsoAndServicePerformerContract, ContractPartInterceptor<RsoAndServicePerformerContract>>();

            // SupplyResOrg
            this.Container.RegisterDomainInterceptor<SupplyResourceOrg, SupplyResourceOrgServiceInterceptor>();

            // RealEstateType
            this.Container.RegisterDomainInterceptor<RealEstateType, RealEstateTypeInterceptor>();

            this.Container.RegisterDomainInterceptor<UnitMeasure, UnitMeasureInterceptor>();

            this.Container.RegisterDomainInterceptor<Person, PersonServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<PersonDisqualificationInfo, PersonDisqualificationInfoInterceptor>();
            this.Container.RegisterDomainInterceptor<PersonPlaceWork, PersonPlaceWorkInterceptor>();
            this.Container.RegisterDomainInterceptor<PersonQualificationCertificate, PersonQualificationCertificateInterceptor>();

            this.Container.RegisterDomainInterceptor<CitizenSuggestion, CitizenSuggestionServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<SuggestionComment, SuggestionCommentServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<ProblemPlace, ProblemPlaceInterceptor>();
            this.Container.RegisterDomainInterceptor<Municipality, MunicipalityServiceInterceptor>();
            this.Container.RegisterDomainInterceptor<Room, RoomInterceptor>();
            this.Container.RegisterDomainInterceptor<PersonRequestToExam, PersonRequestToExamInterceptor>();
            this.Container.RegisterDomainInterceptor<BaseClaimWork, BaseClaimWorkInterceptor>();
            this.Container.RegisterDomainInterceptor<PetitionToCourtType, PetitionToCourtTypeInterceptor>();

            this.Container.RegisterDomainInterceptor<BuildingFeature, BuildingFeatureInterceptor>();
            
            this.Container.RegisterDomainInterceptor<DataMetaInfo, DataMetaInfoInterceptor>();
            this.Container.RegisterDomainInterceptor<BaseDataValue, BaseDataValueInterceptor>();
            this.Container.RegisterDomainInterceptor<EfficiencyRatingPeriod, EfficiencyRatingPeriodInterceptor>();
            this.Container.RegisterDomainInterceptor<ManagingOrganizationEfficiencyRating, ManagingOrganizationEfficiencyRatingInterceptor>();

            this.Container.RegisterDomainInterceptor<FormGovernmentService, FormGovernmentServiceInterceptor>();

            this.Container.RegisterDomainInterceptor<HousingFundMonitoringPeriod, HousingFundMonitoringPeriodInterceptor>();
        }

        private void RegisterDomainServices()
        {
            // Administration
            this.Container.RegisterDomainService<Instruction, FileStorageDomainService<Instruction>>();
            this.Container.RegisterDomainService<TemplateReplacement, FileStorageDomainService<TemplateReplacement>>();
            this.Container.RegisterDomainService<RealityObjectDirectManagContract, FileStorageDomainService<RealityObjectDirectManagContract>>();
            this.Container.RegisterDomainService<Operator, FileStorageDomainService<Operator>>();
            this.Container.RegisterDomainService<Role, RoleDomainService>();
            this.Container.RegisterDomainService<NotifyMessage, NotifyMessageDomainService>();

            // BelayManOrgActivity
            this.Container.RegisterDomainService<BelayManOrgActivity, BelayManOrgActivityDomainService>();

            // BelayOrg
            this.Container.RegisterDomainService<BelayOrganization, BelayOrganizationDomainService>();

            // BelayPolicy
            this.Container.RegisterTransient<IDomainService<BelayPolicyEvent>, FileStorageDomainService<BelayPolicyEvent>>();
            this.Container.RegisterTransient<IDomainService<BelayPolicyPayment>, FileStorageDomainService<BelayPolicyPayment>>();

            // Builder
            this.Container.RegisterTransient<IDomainService<Builder>, FileStorageDomainService<Builder>>();
            this.Container.RegisterTransient<IDomainService<BuilderDocument>, FileStorageDomainService<BuilderDocument>>();
            this.Container.RegisterTransient<IDomainService<BuilderFeedback>, FileStorageDomainService<BuilderFeedback>>();
            this.Container.RegisterTransient<IDomainService<BuilderProductionBase>, FileStorageDomainService<BuilderProductionBase>>();
            this.Container.RegisterTransient<IDomainService<BuilderSroInfo>, FileStorageDomainService<BuilderSroInfo>>();
            this.Container.RegisterTransient<IDomainService<BuilderTechnique>, FileStorageDomainService<BuilderTechnique>>();
            this.Container.RegisterTransient<IDomainService<BuilderWorkforce>, FileStorageDomainService<BuilderWorkforce>>();

            // Contragent
            this.Container.RegisterTransient<IDomainService<Contragent>, ContragentDomainService>();
            this.Container.RegisterFileStorageDomainService<ActivityStage>();

            // EmergencyObject
            this.Container.RegisterTransient<IDomainService<EmergencyObject>, FileStorageDomainService<EmergencyObject>>();
            this.Container.RegisterTransient<IDomainService<EmergencyObjectDocuments>, FileStorageDomainService<EmergencyObjectDocuments>>();

            // LocalGov
            this.Container.RegisterDomainService<LocalGovernment, LocalGovernmentDomainService>();

            // ManOrg
            this.Container.RegisterDomainService<ManagingOrganization, ManOrgDomainService>();
            this.Container.RegisterDomainService<ManOrgBaseContract, FileStorageDomainService<ManOrgBaseContract>>();
            this.Container.RegisterDomainService<ManOrgContractOwners, FileStorageDomainService<ManOrgContractOwners>>();
            this.Container.RegisterDomainService<ManOrgJskTsjContract, FileStorageDomainService<ManOrgJskTsjContract>>();
            this.Container.RegisterDomainService<ManOrgContractTransfer, FileStorageDomainService<ManOrgContractTransfer>>();
            this.Container.RegisterDomainService<ManagingOrgDocumentation, FileStorageDomainService<ManagingOrgDocumentation>>();
            this.Container.RegisterDomainService<ManagingOrgRegistry, FileStorageDomainService<ManagingOrgRegistry>>();

            // PoliticAuth
            this.Container.RegisterDomainService<PoliticAuthority, PoliticAuthorityDomainService>();

            // RealityObject
            this.Container.RegisterTransient<IDomainService<RealityObject>, RealityObjectDomainService>();
            this.Container.RegisterTransient<IDomainService<RealityObjectImage>, RealityObjectImageDomainService>();
            this.Container.RegisterTransient<IDomainService<RealityObjectLand>, FileStorageDomainService<RealityObjectLand>>();
            this.Container.RegisterTransient<IDomainService<RealityObjectProtocol>, FileStorageDomainService<RealityObjectProtocol>>();
            this.Container.RegisterTransient<IDomainService<RealityObjectServiceOrg>, FileStorageDomainService<RealityObjectServiceOrg>>();
            this.Container.RegisterTransient<IRealityObjectFieldsService, RealityObjectFieldsService>();
            this.Container.RegisterFileStorageDomainService<RealityObjectTechnicalMonitoring>();

            // ServOrg
            this.Container.RegisterDomainService<ServiceOrgDocumentation, FileStorageDomainService<ServiceOrgDocumentation>>();

            // SupplyResOrg
            this.Container.RegisterDomainService<SupplyResourceOrgDocumentation, FileStorageDomainService<SupplyResourceOrgDocumentation>>();
            this.Container.RegisterDomainService<RealityObjectResOrg, FileStorageDomainService<RealityObjectResOrg>>();

            // Person
            this.Container.RegisterDomainService<PersonDisqualificationInfo, FileStorageDomainService<PersonDisqualificationInfo>>();
            this.Container.RegisterDomainService<PersonQualificationCertificate, FileStorageDomainService<PersonQualificationCertificate>>();
            this.Container.RegisterDomainService<PersonRequestToExam, FileStorageDomainService<PersonRequestToExam>>();
            this.Container.RegisterFileStorageDomainService<QualificationDocument>();

            // Лицензия УО
            this.Container.RegisterDomainService<ManOrgLicenseRequest, FileStorageDomainService<ManOrgLicenseRequest>>();
            this.Container.RegisterDomainService<ManOrgLicenseDoc, FileStorageDomainService<ManOrgLicenseDoc>>();
            this.Container.RegisterDomainService<ManOrgRequestProvDoc, FileStorageDomainService<ManOrgRequestProvDoc>>();
            this.Container.RegisterDomainService<ManOrgRequestAnnex, FileStorageDomainService<ManOrgRequestAnnex>>();
            this.Container.RegisterDomainService<Municipality, MunicipalityDomainService>();
            this.Container.RegisterDomainService<CitizenSuggestion, CitizenSuggestionDomainService>();
            this.Container.RegisterDomainService<SuggestionComment, SuggestionCommentDomainService>();
            this.Container.RegisterDomainService<CitizenSuggestionFiles, FileStorageDomainService<CitizenSuggestionFiles>>();
            this.Container.RegisterDomainService<Operator, OperatorDomainService>();
            this.Container.RegisterFileStorageDomainService<TechnicalCustomer>();
            this.Container.RegisterFileStorageDomainService<Room>();

            this.Container.RegisterDomainService<FormatDataExportTask, FormatDataExportTaskDomainService>();
            this.Container.RegisterDomainService<ExecutionActionTask, ExecutionActionTaskDomainService>();
        }

        private void RegisterExports()
        {
            this.Container.RegisterTransient<IDataExportService, ContragentDataExport>("ContragentDataExport");
            this.Container.RegisterTransient<IDataExportService, RealityObjectDataExport>("RealityObjectDataExport");
            this.Container.RegisterTransient<IDataExportService, ManagingOrganizationDataExport>("ManagingOrganizationDataExport");
            this.Container.RegisterTransient<IDataExportService, ServiceOrganizationDataExport>("ServiceOrganizationDataExport");
            this.Container.RegisterTransient<IDataExportService, LocalGovernmentDataExport>("LocalGovernmentDataExport");
            this.Container.RegisterTransient<IDataExportService, BuilderDataExport>("BuilderDataExport");
            this.Container.RegisterTransient<IDataExportService, BelayOrganizationDataExport>("BelayOrganizationDataExport");
            this.Container.RegisterTransient<IDataExportService, SupplyResourceOrgDataExport>("SupplyResourceOrgDataExport");
            this.Container.RegisterTransient<IDataExportService, EmergencyObjectDataExport>("EmergencyObjectDataExport");
            this.Container.RegisterTransient<IDataExportService, BelayManOrgActivityDataExport>("BelayManOrgActivityDataExport");
            this.Container.RegisterTransient<IDataExportService, PoliticAuthorityDataExport>("PoliticAuthorityDataExport");
            this.Container.RegisterTransient<IDataExportService, ConstructiveElementGroupDataExport>("ConstructiveElementGroupDataExport");
            this.Container.RegisterTransient<IDataExportService, ConstructiveElementDataExport>("ConstructiveElementDataExport");
            this.Container.RegisterTransient<IDataExportService, PaymentAgentDataExport>("PaymentAgentDataExport");
            this.Container.RegisterTransient<IDataExportService, PersonDataExport>("PersonDataExport");
            this.Container.RegisterTransient<IDataExportService, PersonRequestToExamDataExport>("PersonRequestToExamDataExport");
            this.Container.RegisterTransient<IDataExportService, UpdateRoTypesPreviewExport>("UpdateRoTypesPreviewExport");
            this.Container.RegisterTransient<IDataExportService, RealityObjectOutdoorDataExport>("RealityObjectOutdoorDataExport");
        }

        private void RegisterImports()
        {
            this.Container.RegisterImport<ImportGku>(ImportGku.Id);
            this.Container.RegisterImport<ImportBilling>(ImportBilling.Id);
            this.Container.RegisterImport<FundRealtyObjectsImport>(FundRealtyObjectsImport.Id);
            this.Container.RegisterImport<RoImport>(RoImport.Id);
            this.Container.RegisterImport<ImportOktmo>(ImportOktmo.Id);
            this.Container.RegisterImport<MunicipalityFiasOktmoImport>(MunicipalityFiasOktmoImport.Id);
            this.Container.RegisterImport<ImportOperator>(ImportOperator.Id);
            this.Container.RegisterImport<OrganizationImport>();
            this.Container.RegisterImport<BillingGkuDataImport>();

            this.Container.RegisterTransient<IBillingFileImporter, AddressUidImporter>();

            this.Container.RegisterTransient<IExtraDataImport, Part1>();
            this.Container.RegisterTransient<IExtraDataImport, Part2>();
            this.Container.RegisterTransient<IExtraDataImport, Part3>();
            this.Container.RegisterTransient<IExtraDataImport, Part5>();

            this.Container.RegisterTransient<IOrganizationImportHelper, ManagingOrganizationImportHelper>();
            this.Container.RegisterTransient<IOrganizationImportHelper, SupplyResourceOrgImportHelper>();
            this.Container.RegisterTransient<IOrganizationImportHelper, ServiceOrganizationImportHelper>();
            this.Container.RegisterTransient<IOrganizationImportHelper, DirectManagementImportHelper>();
            this.Container.RegisterTransient<IOrganizationImportCommonHelper, OrganizationImportCommonHelper>();
        }

        private void RegisterNavigations()
        {
            this.Container.RegisterTransient<INavigationProvider, NavigationProvider>("Gkh navigation");
            this.Container.RegisterTransient<INavigationProvider, ContragentMenuProvider>("Contragent navigation");
            this.Container.RegisterTransient<INavigationProvider, ManOrgMenuProvider>("ManOrg navigation");
            this.Container.RegisterTransient<INavigationProvider, ServOrgMenuProvider>("ServOrg navigation");
            this.Container.RegisterTransient<INavigationProvider, RealityObjMenuProvider>("RealityObject navigation");
            this.Container.RegisterTransient<INavigationProvider, BuilderMenuProvider>("Builder navigation");
            this.Container.RegisterTransient<INavigationProvider, LocalGovernmentMenuProvider>("LocalGov navigation");
            this.Container.RegisterTransient<INavigationProvider, SupplyResourceOrgMenuProvider>("SupplyResourceOrg navigation");
            this.Container.RegisterTransient<INavigationProvider, PoliticAuthorityMenuProvider>("PoliticAuth navigation");
            this.Container.RegisterTransient<INavigationProvider, BelayPolicyMenuProvider>("BelayPolicy navigation");
            this.Container.RegisterTransient<INavigationProvider, EmergencyObjMenuProvider>("EmergencyObj navigation");
            this.Container.RegisterTransient<INavigationProvider, BelayOrgMenuProvider>("BelayOrg navigation");
            this.Container.RegisterTransient<INavigationProvider, PaymentAgentMenuProvider>("PaymentAgent navigation");
            this.Container.RegisterTransient<INavigationProvider, PersonMenuProvider>("Person navigation");
            this.Container.RegisterTransient<INavigationProvider, ManOrgLicenseMenuProvider>("ManOrgLicense navigation");
            this.Container.RegisterTransient<INavigationProvider, HousingInspectionMenuProvider>("HousingInspection navigation");
            this.Container.RegisterTransient<INavigationProvider, RealityObjectOutdoorMenuProvider>("RealityObjectOutdoor navigation");
            this.Container.RegisterTransient<INavigationProvider, ContragentClwMenuProvider>();
        }

        private void RegisterServices()
        {
            // Administration
            this.Container.RegisterTransient<IInstructionService, InstructionService>();
            this.Container.RegisterTransient<IInstructionGroupService, InstructionGroupService>();
            this.Container.RegisterTransient<IOperatorService, OperatorService>();
            this.Container.RegisterSingleton<ISystemVersionService, SystemVersionService>();
            this.Container.RegisterTransient<ILocalAdminRoleService, LocalAdminRoleService>();
            this.Container.RegisterTransient<IFilterPermissionService, FilterPermissionService>();
            this.Container.RegisterTransient<IEntityChangeLog, EntityChangeLog>();
            this.Container.RegisterTransient<IInheritEntityChangeLog, ContragentContactChangeLog>(ContragentContactChangeLog.Id);

            this.Container.RegisterSingleton<INotifyService, NotifyService>();

            // BelayPolicy
            this.Container.RegisterTransient<IBelayPolicyService, BelayPolicyService>();
            this.Container.RegisterTransient<IBelayPolicyMkdService, BelayPolicyMkdService>();
            this.Container.RegisterTransient<IBelayPolicyRiskService, BelayPolicyRiskService>();
            this.Container.RegisterTransient<IRoomService, RoomService>();

            // ClaimWork
            this.Container.RegisterTransient<IClaimWorkDocumentProvider, ClaimWorkDocumentProvider>();
            this.Container.RegisterTransient<ILawsuitInfoService, LawsuitInfoService>();

            // Contragent
            this.Container.RegisterTransient<IContragentService, ContragentService>();
            this.Container.RegisterTransient<IContragentListForTypeJurOrg, GkhContragentListForTypeJurOrg>();
            this.Container.RegisterTransient<IContragentAdditionRoleService, ContragentAdditionRoleService>();

            // Dict
            this.Container.RegisterTransient<IZonalInspectionService, ZonalInspectionService>();
            this.Container.RegisterTransient<ITemplateReplacementService, TemplateReplacementService>();
            this.Container.RegisterTransient<IWorkService, WorkService>();
            this.Container.RegisterTransient<IEmergencyObjectService, EmergencyObjectService>();
            this.Container.RegisterTransient<IMunicipalityService, MunicipalityService>();
            this.Container.RegisterTransient<IPoliticAuthorityService, PoliticAuthorityService>();
            this.Container.RegisterTransient<IPoliticAuthorityWorkModeService, PoliticAuthorityWorkModeService>();
            this.Container.RegisterTransient<IInspectorService, InspectorService>();
            this.Container.RegisterTransient<IUnitMeasureService, UnitMeasureService>();
            this.Container.RegisterTransient<IBuilderDocumentTypeService, BuilderDocumentTypeService>();
            this.Container.RegisterTransient<IResettlementProgramService, ResettlementProgramService>();

            // LocalGov
            this.Container.RegisterTransient<ILocalGovernmentService, LocalGovernmentService>();
            this.Container.RegisterTransient<ILocalGovernmentWorkModeService, LocalGovernmentWorkModeService>();

            // PaymentAgent
            this.Container.RegisterTransient<IPaymentAgentService, PaymentAgentService>();

            // ContragentClw
            this.Container.RegisterTransient<IContragentClwService, ContragentClwService>();

            // ManOrg
            this.Container.RegisterTransient<IManagingOrgWorkModeService, ManagingOrgWorkModeService>();
            this.Container.RegisterTransient<IManagingOrgRealityObjectService, ManagingOrgRealityObjectService>();
            this.Container.RegisterTransient<IManOrgContractTransferService, ManOrgContractTransferService>();
            this.Container.RegisterTransient<IManOrgContractOwnersService, ManOrgContractOwnersService>();
            this.Container.RegisterTransient<IManOrgJskTsjContractService, ManOrgJskTsjContractService>();
            this.Container.RegisterTransient<IManagingOrgMunicipalityService, ManagingOrgMunicipalityService>();

            this.Container.RegisterTransient<IHousingInspectionMunicipalityService, HousingInspectionMunicipalityService>();

            // RealityObject
            this.Container.RegisterTransient<IRealityObjectService, RealityObjectService>();
            this.Container.RegisterTransient<IRealityObjectCouncillorsService, RealityObjectCouncillorsService>();
            this.Container.RegisterTransient<IRealityObjectProtocolService, RealityObjectProtocolService>();
            this.Container.RegisterService<IRealityObjectManOrgService, RealityObjectManOrgService>();

            this.Container.RegisterTransient<IRealityObjectOutdoorService, RealityObjectOutdoorService>();

            // Scripts
            this.Container.RegisterTransient<IGkhScriptService, GkhScriptService>();

            // ServOrg
            this.Container.RegisterTransient<IServiceOrgServService, ServiceOrgServService>();
            this.Container.RegisterTransient<IServiceOrgRealityObjectService, ServiceOrgRealityObjectService>();
            this.Container.RegisterTransient<IServiceOrgMunicipalityService, ServiceOrgMunicipalityService>();

            // SupplyResOrg
            this.Container.RegisterTransient<ISupplyResourceOrgServService, SupplyResourceOrgServService>();
            this.Container.RegisterTransient<ISupplyResourceOrgMunicipalityService, SupplyResourceOrgMunicipalityService>();
            this.Container.RegisterTransient<ISupplyResourceOrgRealtyObjectService, SupplyResourceOrgRealtyObjectService>();

            this.Container.RegisterTransient<IPersonService, PersonService>();
            this.Container.RegisterTransient<IManOrgLicenseRequestService, ManOrgLicenseRequestService>();
            this.Container.RegisterTransient<IManorgLicenceApplicantsProvider, ManorgLicenceApplicantsProvider>();
            this.Container.RegisterTransient<IManOrgLicenseService, ManOrgLicenseService>();

            this.Container.RegisterTransient<IHouseInfoOverviewService, HouseInfoOverviewService>();
            this.Container.RegisterTransient<ICitizenSuggestionService, CitizenSuggestionService>();
            this.Container.RegisterTransient<ICitizenSuggestionReportService, CitizenSuggestionReportService>();

            this.Container.RegisterTransient<IVersionedEntityService, VersionedEntityService>();
            this.Container.RegisterTransient<IFiasHelper, FiasHelper>();

            this.Container.RegisterTransient<IGkhParams, ConfigGkhParams>();

            this.Container.RegisterTransient<IDocumentationService, DocumentationService>();
            this.Container.RegisterTransient<IGkhRuleChangeStatusService, GkhRuleChangeStatusService>();

            this.Container.RegisterTransient<IBackForwardIterator<RealityObject>, BackForwardIterator<RealityObject>>();

            this.Container.Register(Component.For(typeof(IBlobPropertyService<,>)).ImplementedBy(typeof(BlobPropertyService<,>)).LifestyleTransient());

            this.Container.RegisterTransient<IGkhParamService, GkhParamService>();

            this.Container.RegisterTransient<IGkhConfigService, GkhConfigService>();
            this.Container.RegisterTransient<IManagingOrganizationService, ManagingOrganizationService>();

            this.Container.RegisterTransient<IMultipurposeGlossaryItemService, MultipurposeGlossaryItemService>();
            //Эта имплементация регистрируется как Fallback для того, чтобы любая регистрация этого же сервиса в регионах перекрывала текущую
            this.Container.Register(Component.For<IImportedAddressService>().ImplementedBy<BaseImportedAddressService>().LifestyleTransient().IsFallback());

            this.Container.RegisterTransient<ITechPassportElementDescriptorService, TechPassportElementDescriptorService>();

            this.Container.RegisterTransient<ITableLocker, TableLocker>();
            this.Container.RegisterTransient<IBatchTableLocker, BatchTableLocker>();
            this.Container.RegisterTransient<ITableLockService, TableLockService>();
            this.Container.RegisterTransient<INhibernateConfigModifier, TableLockNhConfigModifier>();
            this.Container.RegisterTransient<IEmergencyObjectDocumentsService, EmergencyObjectDocumentsService>();

            // Конструктор и рейтинг эффективности УО и вся архитектура
            this.Container.RegisterTransient<IDataMetaInfoService, DataMetaInfoService>();
            this.Container.RegisterTransient<IDataFillerService, DataFillerService>();

            this.Container.RegisterSingleton<IConstructorDataFillerMap, GkhConstructorDataFillerMap>();
            this.Container.RegisterSingleton<IDataFillerProvider, DataFillerProvider>();
            this.Container.UsingForResolved<IEventAggregator>((container, eventAggregator) =>
            {
                eventAggregator.GetEvent<AppStartEvent>().Subscribe<RegisterDataFillerMapEventHandler>();
            });

            this.Container.RegisterTransient<IFormulaValiudator, FactorFormulaValidator>("FactorFormulaValidator");
            this.Container.RegisterTransient<IFormulaValiudator, CoefficientFormulaValidator>("CoefficientFormulaValidator");
            this.Container.RegisterTransient<IFormulaValiudator, AttributeFormulaValudator>("AttributeFormulaValudator");

            this.Container.RegisterTransient<IDataValueService, IEfficiencyRatingService, EfficiencyRatingDataValueService>("EfficiencyRatingMetaValueService");
            this.Container.RegisterTransient<IEfficiencyRatingAnaliticsGraphService, EfficiencyRatingAnaliticsGraphService>();

            this.Container.RegisterSingleton<ITehPassportCacheService, TehPassportCacheService>();
            this.Container.RegisterService<IFormatDataExportService, FormatDataExportService>();
            this.Container.RegisterService<IRegionCodeService, RegionCodeService>();

            this.Container.RegisterService<IHousingFundMonitoringService, HousingFundMonitoringService>();

            this.Container.RegisterTransient<IUserInfoProvider, UserInfoProvider>();
            this.Container.RegisterTransient<IAddressMatcher, AddressMatcher>();
            //GeneralState
            this.Container.RegisterSingleton<IGeneralStateProvider, GeneralStateProvider>();
            this.Container.UsingForResolved<IEventAggregator>((container, eventAggregator) =>
            {
                eventAggregator.GetEvent<AppStartEvent>().Subscribe<RegisterGeneralStateManifestEventHandler>();
            });

            this.Container.RegisterSessionScoped<IGeneralStateHistoryManager, GeneralStateHistoryManager>();

            this.Container.RegisterTransient<IGeneralStateHistoryService, GeneralStateHistoryService>();

            this.Container.RegisterTransient<IServiceOverride, Services.Override.ServiceOverride>();
            
            this.Container.RegisterTransient<IPostalService, PostalService>();

            this.Container.RegisterTransient<IReflectionHelperService, ReflectionHelperService>();

            this.Container.RegisterTransient<ICryptoService, CryptoService>();

            Container.Register(Component
                .For<ICryptographyJcpApi>()
                .UsingFactoryMethod(_ =>
                    RestService.For<ICryptographyJcpApi>(ApplicationContext.Current.Configuration.AppSettings.GetAs<string>("JcpApiUri")))
                .LifeStyle.Scoped());
        }

        private void RegisterViewModels()
        {
            // Administration
            this.Container.RegisterViewModel<PrintCertHistory, PrintCertHistoryViewModel>();
            this.Container.RegisterViewModel<InstructionGroup, InstructionGroupViewModel>();
            this.Container.RegisterViewModel<InstructionGroupRole, InstructionGroupRoleViewModel>();
            this.Container.RegisterViewModel<Instruction, InstructionViewModel>();
            this.Container.RegisterViewModel<Operator, OperatorViewModel>();
            this.Container.RegisterViewModel<Role, RoleViewModel>();
            this.Container.RegisterViewModel<TemplateReplacement, TemplateReplacementViewModel>();
            this.Container.RegisterViewModel<LogImport, LogImportViewModel>();
            this.Container.RegisterViewModel<FieldRequirement, FieldRequirementViewModel>();
            this.Container.RegisterViewModel<OrganizationForm, OrganizationFormViewModel>();
            this.Container.RegisterViewModel<ExecutionActionTask, ExecutionActionTaskViewModel>();
            this.Container.RegisterViewModel<ExecutionActionResult, ExecutionActionResultViewModel>();
            this.Container.RegisterViewModel<NotifyMessage, NotifyMessageViewModel>();
            this.Container.RegisterViewModel<NotifyStats, NotifyStatsViewModel>();
            this.Container.RegisterViewModel<NotifyPermission, NotifyPermissionViewModel>();
            this.Container.RegisterViewModel<EmailMessage, EmailMessageViewModel>();

            // BelayManOrgActivity
            this.Container.RegisterViewModel<BelayManOrgActivity, BelayManOrgActivityViewModel>();

            // BelayOrg
            this.Container.RegisterViewModel<BelayOrganization, BelayOrganizationViewModel>();

            // BelayPolicy
            this.Container.RegisterViewModel<BelayPolicy, BelayPolicyViewModel>();
            this.Container.RegisterViewModel<BelayPolicyMkd, BelayPolicyMkdViewModel>();
            this.Container.RegisterViewModel<BelayPolicyRisk, BelayPolicyRiskViewModel>();
            this.Container.RegisterViewModel<BelayPolicyEvent, BelayPolicyEventViewModel>();
            this.Container.RegisterViewModel<BelayPolicyPayment, BelayPolicyPaymentViewModel>();

            // Builder
            this.Container.RegisterViewModel<Builder, BuilderViewModel>();
            this.Container.RegisterViewModel<BuilderDocument, BuilderDocumentViewModel>();
            this.Container.RegisterViewModel<BuilderFeedback, BuilderFeedbackViewModel>();
            this.Container.RegisterViewModel<BuilderProductionBase, BuilderProductionBaseViewModel>();
            this.Container.RegisterViewModel<BuilderSroInfo, BuilderSroInfoViewModel>();
            this.Container.RegisterViewModel<BuilderTechnique, BuilderTechniqueViewModel>();
            this.Container.RegisterViewModel<BuilderWorkforce, BuilderWorkforceViewModel>();
            this.Container.RegisterViewModel<BuilderLoan, BuilderLoanViewModel>();
            this.Container.RegisterViewModel<BuilderLoanRepayment, BuilderLoanRepaymentViewModel>();

            // Contragent
            this.Container.RegisterViewModel<Contragent, ContragentViewModel>();
            this.Container.RegisterViewModel<ContragentBank, ContragentBankViewModel>();
            this.Container.RegisterViewModel<ContragentContact, ContragentContactViewModel>();
            this.Container.RegisterViewModel<ContragentMunicipality, ContragentMunicipalityViewModel>();
            this.Container.RegisterViewModel<HousingInspection, HousingInspectionViewModel>();
            this.Container.RegisterViewModel<HousingInspectionMunicipality, HousingInspectionMunicipalityViewModel>();

            // Dict
            this.Container.RegisterViewModel<ConstructiveElement, ConstructiveElementViewModel>();
            this.Container.RegisterViewModel<Inspector, InspectorViewModel>();
            this.Container.RegisterViewModel<ZonalInspection, ZonalInspectionViewModel>();
            this.Container.RegisterViewModel<InspectorSubscription, InspectorSubscriptionViewModel>();
            this.Container.RegisterViewModel<Institutions, InstitutionsViewModel>();
            this.Container.RegisterViewModel<Municipality, MunicipalityViewModel>();
            this.Container.RegisterViewModel<MunicipalitySourceFinancing, MunicipalitySourceFinancingViewModel>();
            this.Container.RegisterViewModel<ResettlementProgram, ResettlementProgramViewModel>();
            this.Container.RegisterViewModel<Work, WorkViewModel>();
            this.Container.RegisterViewModel<WorkKindCurrentRepair, WorkKindCurrentRepairViewModel>();
            this.Container.RegisterViewModel<ZonalInspectionInspector, ZonalInspectionInspectorViewModel>();
            this.Container.RegisterViewModel<ZonalInspectionMunicipality, ZonalInspectionMunicipalityViewModel>();
            this.Container.RegisterViewModel<Period, PeriodViewModel>();
            this.Container.RegisterViewModel<FiasOktmo, FiasOktmoViewModel>();
            this.Container.RegisterViewModel<MunicipalityFiasOktmo, MunicipalityFiasOktmoViewModel>();
            this.Container.RegisterViewModel<NormativeDocItem, NormativeDocItemViewModel>();
            this.Container.RegisterViewModel<TypeOwnership, TypeOwnershipViewModel>();
            this.Container.RegisterViewModel<ContentRepairMkdWork, ContentRepairMkdWorkViewModel>();
            this.Container.RegisterViewModel<ManagementContractService, ManagementContractServiceViewModel>();
            this.Container.RegisterViewModel<CommunalContractService, CommunalContractServiceViewModel>();
            this.Container.RegisterViewModel<AgreementContractService, AgreementContractServiceViewModel>();
            this.Container.RegisterViewModel<AdditionalContractService, AdditionalContractServiceViewModel>();
            this.Container.RegisterViewModel<CentralHeatingStation, CentralHeatingStationViewModel>();
            this.Container.RegisterViewModel<ActivityStage, ActivityStageViewModel>();

            // EmergencyObject
            this.Container.RegisterViewModel<EmergencyObject, EmergencyObjectViewModel>();
            this.Container.RegisterViewModel<EmerObjResettlementProgram, EmerObjResettlementProgramViewModel>();

            // Hcs
            this.Container.RegisterViewModel<HouseAccount, HouseAccountViewModel>();
            this.Container.RegisterViewModel<HouseAccountCharge, HouseAccountChargeViewModel>();
            this.Container.RegisterViewModel<MeterReading, MeterReadingViewModel>();
            this.Container.RegisterViewModel<HouseMeterReading, HouseMeterReadingViewModel>();
            this.Container.RegisterViewModel<HouseOverallBalance, HouseOverallBalanceViewModel>();

            // LocalGov
            this.Container.RegisterViewModel<LocalGovernmentWorkMode, LocalGovernmentWorkModeViewModel>();
            this.Container.RegisterViewModel<LocalGovernment, LocalGovernmentViewModel>();

            // PaymentAgent
            this.Container.RegisterViewModel<PaymentAgent, PaymentAgentViewModel>();

            // ContragentClw
            this.Container.RegisterViewModel<ContragentClw, ContragentClwViewModel>();
            this.Container.RegisterViewModel<ContragentClwMunicipality, ContragentClwMunicipalityViewModel>();

            // ManOrg
            this.Container.RegisterViewModel<ManagingOrganization, ManOrgViewModel>();
            this.Container.RegisterViewModel<ManagingOrgClaim, ManagingOrgClaimViewModel>();
            this.Container.RegisterViewModel<ManagingOrgWorkMode, ManOrgWorkModeViewModel>();
            this.Container.RegisterViewModel<ManagingOrgService, ManagingOrgServiceViewModel>();
            this.Container.RegisterViewModel<ManOrgBaseContract, ManOrgBaseContractViewModel>();
            this.Container.RegisterViewModel<ManagingOrgMembership, ManagingOrgMembershipViewModel>();
            this.Container.RegisterViewModel<ManOrgContractRelation, ManOrgContractRelationViewModel>();
            this.Container.RegisterViewModel<ManOrgContractTransfer, ManOrgContractTransferViewModel>();
            this.Container.RegisterViewModel<ManagingOrgDocumentation, ManagingOrgDocumentationViewModel>();
            this.Container.RegisterViewModel<ManagingOrgRealityObject, ManagingOrgRealityObjectViewModel>();
            this.Container.RegisterViewModel<ManagingOrgMunicipality, ManagingOrgMunicipalityViewModel>();
            this.Container.RegisterViewModel<RealityObjectDirectManagContract, RealityObjectDirectManagContractViewModel>();
            this.Container.RegisterViewModel<ManagingOrgRegistry, ManagingOrgRegistryViewModel>();
            this.Container.RegisterViewModel<Entrance, EntranceViewModel>();

            // PoliticAuth
            this.Container.RegisterViewModel<PoliticAuthority, PoliticAuthorityViewModel>();
            this.Container.RegisterViewModel<PoliticAuthorityWorkMode, PoliticAuthorityWorkModeViewModel>();

            // RealityObject
            this.Container.RegisterViewModel<RealityObject, RealityObjectViewModel>();
            this.Container.RegisterViewModel<RealityObjectImage, RealityObjectImageViewModel>();
            this.Container.RegisterViewModel<RealityObjectLand, RealityObjectLandViewModel>();
            this.Container.RegisterViewModel<RealityObjectApartInfo, RealityObjectApartInfoViewModel>();
            this.Container.RegisterViewModel<RealityObjectCouncillors, RealityObjectCouncillorsViewModel>();
            this.Container.RegisterViewModel<RealityObjectConstructiveElement, RealityObjectConstrElViewModel>();
            this.Container.RegisterViewModel<RealityObjectHouseInfo, RealityObjectHouseInfoViewModel>();
            this.Container.RegisterViewModel<RealityObjectMeteringDevice, RealityObjectMeteringDeviceViewModel>();
            this.Container.RegisterViewModel<RealityObjectCurentRepair, RealityObjectCurentRepairViewModel>();
            this.Container.RegisterViewModel<RealityObjectResOrg, RealityObjectResOrgViewModel>();
            this.Container.RegisterViewModel<RealityObjectServiceOrg, RealityObjectServiceOrgViewModel>();
            this.Container.RegisterViewModel<Room, RoomViewModel>();
            this.Container.RegisterViewModel<RealityObjectBlock, RealityObjectBlockViewModel>();
            this.Container.RegisterViewModel<RealityObjectBuildingFeature, RealityObjectBuildingFeatureViewModel>();
            this.Container.RegisterViewModel<MeteringDevicesChecks, MeteringDevicesChecksViewModel>();
            this.Container.RegisterViewModel<RealityObjectTechnicalMonitoring, RealityObjectTechnicalMonitoringViewModel>();


            this.Container.RegisterViewModel<RealityObjectLift, RealityObjectLiftViewModel>();
            this.Container.RegisterViewModel<RealityObjectLiftSum, RealityObjectLiftSumViewModel>();
            this.Container.RegisterViewModel<RealityObjectOutdoor, RealityObjectOutdoorViewModel>();

            // ServOrg
            this.Container.RegisterViewModel<ServiceOrgDocumentation, ServiceOrgDocumentationViewModel>();
            this.Container.RegisterViewModel<ServiceOrganization, ServiceOrgViewModel>();
            this.Container.RegisterViewModel<ServiceOrgService, ServiceOrgServViewModel>();
            this.Container.RegisterViewModel<ServiceOrgRealityObject, ServiceOrgRealityObjectViewModel>();
            this.Container.RegisterViewModel<ServiceOrgContract, ServiceOrgContractViewModel>();
            this.Container.RegisterViewModel<ServiceOrgMunicipality, ServiceOrgMunicipalityViewModel>();
            this.Container.RegisterViewModel<ServiceDictionary, ServiceDictionaryViewModel>();
            this.Container.RegisterViewModel<PublicServiceOrgContractService, PublicServiceOrgContractServiceViewModel>();
            this.Container.RegisterViewModel<PublicOrgServiceQualityLevel, PublicOrgServiceQualityLevelViewModel>();
            this.Container.RegisterViewModel<PublicServiceOrgTemperatureInfo, PublicServiceOrgTemperatureInfoViewModel>();

            // SupplyResOrg
            this.Container.RegisterViewModel<SupplyResourceOrg, SupplyResourceOrgViewModel>();
            this.Container.RegisterViewModel<SupplyResourceOrgService, SupplyResourceOrgServViewModel>();
            this.Container.RegisterViewModel<SupplyResourceOrgMunicipality, SupplyResourceOrgMunicipalityViewModel>();
            this.Container.RegisterViewModel<SupplyResourceOrgRealtyObject, SupplyResourceOrgRealtyObjViewModel>();
            this.Container.RegisterViewModel<SupplyResourceOrgDocumentation, SupplyResourceOrgDocumentationViewModel>();

            // Person
            this.Container.RegisterViewModel<Person, PersonViewModel>();
            this.Container.RegisterViewModel<PersonQualificationCertificate, PersonQualificationCertificateViewModel>();
            this.Container.RegisterViewModel<PersonPlaceWork, PersonPlaceWorkViewModel>();
            this.Container.RegisterViewModel<PersonDisqualificationInfo, PersonDisqualificationInfoViewModel>();
            this.Container.RegisterViewModel<PersonRequestToExam, PersonRequestToExamViewModel>();
            this.Container.RegisterViewModel<QualificationDocument, QualificationDocumentViewModel>();
            this.Container.RegisterViewModel<TechnicalMistake, TechnicalMistakeViewModel>();

            // Лицензия УО
            this.Container.RegisterViewModel<ManOrgLicenseRequest, ManOrgLicenseRequestViewModel>();
            this.Container.RegisterViewModel<ManOrgRequestPerson, ManOrgRequestPersonViewModel>();
            this.Container.RegisterViewModel<ManOrgRequestProvDoc, ManOrgRequestProvDocViewModel>();
            this.Container.RegisterViewModel<ManOrgRequestAnnex, ManOrgRequestAnnexViewModel>();
            this.Container.RegisterViewModel<ManOrgLicenseDoc, ManOrgLicenseDocViewModel>();
            this.Container.RegisterViewModel<ManOrgLicense, ManOrgLicenseViewModel>();
            this.Container.RegisterViewModel<ManOrgLicensePerson, ManOrgLicensePersonViewModel>();

            this.Container.RegisterViewModel<CitizenSuggestion, CitizenSuggestionViewModel>();
            this.Container.RegisterViewModel<Transition, TransitionViewModel>();
            this.Container.RegisterViewModel<CitizenSuggestionHistory, CitizenSuggestionHistoryViewModel>();
            this.Container.RegisterViewModel<CitizenSuggestionFiles, CitizenSuggestionFileViewModel>();
            this.Container.RegisterViewModel<SuggestionComment, SuggestionCommentViewModel>();
            this.Container.RegisterViewModel<SuggestionCommentFiles, SuggestionCommentFileViewModel>();
            this.Container.RegisterViewModel<MultipurposeGlossaryItem, MultipurposeGlossaryItemViewModel>();

            this.Container.RegisterViewModel<EntityLogLight, EntityLogLightViewModel>();
            this.Container.RegisterViewModel<LogOperation, LogOperationViewModel>();

            this.Container.RegisterViewModel<DataMetaInfo, DataMetaInfoViewModel>();

            this.Container.RegisterViewModel<ManagingOrganizationEfficiencyRating, ManagingOrganizationEfficiencyRatingViewModel>();
            this.Container.RegisterViewModel<EfficiencyRatingPeriod, EfficiencyRatingPeriodViewModel>();
            this.Container.RegisterViewModel<EfficiencyRatingAnaliticsGraph, EfficiencyRatingAnaliticsGraphViewModel>();

            this.Container.RegisterViewModel<GovernmenServiceDetail, GovernmenServiceDetailViewModel>();
            this.Container.RegisterViewModel<TechnicalCustomer, TechnicalCustomerViewModel>();

            this.Container.RegisterViewModel<HousingFundMonitoringInfo, HousingFundMonitoringInfoViewModel>();

            this.Container.RegisterViewModel<AddressMatch, AddressMatchViewModel>();

            this.Container.RegisterViewModel<FormatDataExportTask, FormatDataExportTaskViewModel>();
            this.Container.RegisterViewModel<FormatDataExportResult, FormatDataExportResultViewModel>();
            this.Container.RegisterViewModel<FormatDataExportRemoteResult, FormatDataExportRemoteResultViewModel>();
            this.Container.RegisterViewModel<FormatDataExportInfo, FormatDataExportInfoViewModel>();

            this.Container.RegisterViewModel<GeneralStateHistory, GeneralStateHistoryViewModel>();

            this.Container.RegisterViewModel<PretensionClw, PretensionViewModel>();

            this.Container.RegisterViewModel<EntityHistoryInfo, EntityHistoryInfoViewModel>();
            this.Container.RegisterViewModel<EntityHistoryField, EntityHistoryFieldViewModel>();
        }

        private void RegisterExecuteActions()
        {
            // Отключённые действия
            //this.Container.Register(Component.For<IExecutionAction>().ImplementedBy<RoomMigrationAction>().Named(RoomMigrationAction.Code));
            //this.Container.RegisterExecutionAction<MakeManOrgContractFileDuplicateAction>(MakeManOrgContractFileDuplicateAction.Code);
            //this.Container.RegisterExecutionAction<SetMunicipalityTypeAction>(SetMunicipalityTypeAction.Code);
            //this.Container.RegisterExecutionAction<CorrectionMoSettlementArchangelskAction>(CorrectionMoSettlementArchangelskAction.Code);
            //this.Container.RegisterExecutionAction<HouseInfoMigrationAction>(HouseInfoMigrationAction.Code);
            //this.Container.RegisterExecutionAction<CorrectAddressHousingAfterImportOktmoAction>(CorrectAddressHousingAfterImportOktmoAction.Code);
            //this.Container.RegisterExecutionAction<SetContragentMoSettlementAction>(SetContragentMoSettlementAction.Code);

            this.Container.RegisterExecutionAction<TransferSorgRoContractsAction>();
            this.Container.RegisterExecutionAction<FixFiasAddressByRobjectMunicipalityAction>();
            this.Container.RegisterExecutionAction<FixManOrgContractsRealObjectsAction>();
            this.Container.RegisterExecutionAction<AddContragentPermissionAction>();
            this.Container.RegisterExecutionAction<CorrectionMunicipalityAction>();
            this.Container.RegisterExecutionAction<SetParamInvolvedCrToHouseFinalStateAction>();
            this.Container.RegisterExecutionAction<GkhFiasAddressCorrectAction>();
            this.Container.RegisterExecutionAction<RenameDictMultipurposePermissionAction>();
            this.Container.RegisterExecutionAction<AddDbCommentsAction>();
            this.Container.RegisterExecutionAction<RoomAttributesConvertZeroToNullAction>();
            this.Container.RegisterExecutionAction<MigrateBuilderDocumentType>();
            this.Container.RegisterExecutionAction<UpdateMoContragentAction>();
            this.Container.RegisterExecutionAction<FileInfoMoveFolderAction>();
            this.Container.RegisterExecutionAction<MigrateFiasHouseFromGkh>();
            this.Container.RegisterExecutionAction<SetHouseGuidFromFiasHouseAction>();
            this.Container.RegisterExecutionAction<SetIsNotInvolvedCrAction>();
            this.Container.RegisterExecutionAction<AddRegionCodesAction>();
            this.Container.RegisterExecutionAction<CreateDeafultGovernmenServiceDetailGroupAction>();
            this.Container.RegisterExecutionAction<AddEntityImportIdAction>();
            this.Container.RegisterExecutionAction<UpdateFiasAddressStreetParentAction>();
            this.Container.RegisterExecutionAction<GrantForLocalAdminAction>();
            this.Container.RegisterExecutionAction<ClearFileStorageAction>();
            this.Container.RegisterExecutionAction<CheckFiasOktmoAction>();
            this.Container.RegisterExecutionAction<FileStorageAnalyzeAction>();
#if DEBUG
            this.Container.RegisterExecutionAction<TestAction>();
            this.Container.RegisterExecutionAction<TestMandatoryAction>();
#endif
        }

        private void RegisterTasks()
        {
            this.Container.RegisterTransient<ITask, UpdateRoContractTask>();
            this.Container.RegisterTransient<ITask, DataTransferExportTask>();
            this.Container.RegisterTransient<ITask, DataTransferImportTask>();
        }

        private void RegisterReports()
        {
            this.Container.RegisterTransient<IGkhBaseReport, ManOrgLicenseRequestTaxReport>();
            this.Container.RegisterTransient<IGkhBaseReport, ManOrgLicenseRequestMvdReport>();
            this.Container.RegisterTransient<IGkhBaseReport, ManOrgLicenseRequestTreasuryReport>();
            this.Container.RegisterTransient<IGkhBaseReport, ManOrgLicenseRequestOrderReport>();

            this.Container.RegisterTransient<IGkhBaseReport, ManOrgLicenseReport>();

            this.Container.RegisterTransient<IGkhBaseReport, MotivatedProposalForLicensingReport>();
            this.Container.RegisterTransient<IGkhBaseReport, NotificationRefusalToIssueLicenseReport>();
            this.Container.RegisterTransient<IGkhBaseReport, ProtocolMeetingLicensingCommissionReport>();

            this.Container.RegisterTransient<IGkhBaseReport, HousingFundMonitoringInfoReport>();
        }

        private void RegisterAuditLogMap()
        {
            this.Container.RegisterTransient<IAuditLogMapProvider, AuditLogMapProvider>();
        }

        private void RegisterStateChangeRules()
        {
            this.Container.RegisterTransient<IRuleChangeStatus, PersonDisqualificationStateRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, CitizenSuggestionHasAnswerRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, LicenseRequestStateRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, RevocationLicenseStateRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, CitizenSuggestionStateRule>();
            this.Container.RegisterTransient<IRuleChangeStatus, CitizenSuggestionExecutorState>();
            this.Container.RegisterTransient<IRuleChangeStatus, RealityObjectValidateStateRule>();
        }

        private void RegisterNoServiceFilterRoles()
        {
            this.Container.RegisterTransient<INoServiceFilterRole, ProviderServiceNoServiceFilterRoleName>("Поставщик услуг");
        }

        private void ExecuteAutoMapperConfiguration()
        {
            this.Container.RegisterSingleton<IAutoMapperConfigProvider, AutoMapperConfigProvider>();
        }

        private void RegisterExecutionActionScheduler()
        {
            var eventAggregator = this.Container.Resolve<IEventAggregator>();

            using (this.Container.Using(eventAggregator))
            {
                // Регистрация только для веб-приложений
                if (ApplicationContext.Current.GetContextType() == ApplicationContextType.WebApplication)
                {
                    eventAggregator.GetEvent<AppStartEvent>().Subscribe<InitExecutionActionScheduler>();
                    eventAggregator.GetEvent<AppStartEvent>().Subscribe<FormatDataExportSchedulerService>();
#if !DEBUG
                    eventAggregator.GetEvent<AppStartEvent>().Subscribe<InitScheduler>();
#endif
                    // Регистрация планировщика
                    this.Container.RegisterSingleton<IExecutionActionScheduler, ExecutionActionScheduler>(ExecutionActionScheduler.Code);
                    this.Container.RegisterSingleton<IJobListener, ExecutionActionJobListener>(ExecutionActionJobListener.Code);
                    this.Container.RegisterSingleton<IExecutionActionJobStateReporter, ExecutionActionJobStateReporter>(ExecutionActionJobStateReporter.Code);
                    this.Container.RegisterSingleton<IExecutionActionJobBuilder, ExecutionActionJobBuilder>(ExecutionActionJobBuilder.Code);
                    this.Container.RegisterSingleton<IExecutionActionResolver, ExecutionActionResolver>(ExecutionActionResolver.Code);
                    this.Container.RegisterSingleton<IExecutionActionService, ExecutionActionService>();
                    this.Container.RegisterSingleton<IExecutionActionInfoService, ExecutionActionInfoService>();

                    this.Container.RegisterSingleton<IFormatDataExportScheduler, FormatDataExportScheduler>();
                }
            }
        }

        private void RegisterTehPasport()
        {
            this.Container.RegisterResourceManifest<ResourceManifest>("GkhTp resources");
#if DEBUG
            this.Container.RegisterTransient<IPassportProvider, TechPassportXmlProvider>();
#else
            this.Container.RegisterSingleton<IPassportProvider, TechPassportXmlProvider>();
#endif
            this.Container.RegisterController<TechPassportController>();
            this.Container.RegisterController<RealtyObjTypeWorkCrController>();

            this.Container.RegisterTransient<ITehPassportValueService, TehPassportValueService>();

            this.Container.RegisterService<IRealtyObjectTypeWorkService, RealtyObjectTypeWorkService>();
            this.Container.RegisterService<ITechPassportService, TechPassportService>();
            this.Container.RegisterDomainInterceptor<RealityObject, RealityObjectTpInterceptor>();

            this.Container.RegisterTransient<IRealityObjectTpSyncService, RealityObjectTpSyncService>();
            this.Container.RegisterDomainInterceptor<RealityObject, RealityObjectTpSyncInterceptor>();
            this.Container.RegisterDomainInterceptor<TehPassportValue, TpRealityObjectSyncInterceptor>();

            this.Container.Register(Component.For<IPrintForm>().Named("TP Report.TechPassport").ImplementedBy<HouseTechPassportReport>().LifeStyle.Transient);
            this.Container.Register(Component.For<IPrintForm>().Named("TP Report.DevicesByRealityObject").ImplementedBy<DevicesByRealityObject>().LifeStyle.Transient);

            this.Container.Register(Component.For<ITechPassportSectionReport>().Named("TP Report.TechPassport.Section1").ImplementedBy<Section1>().LifeStyle.Transient);
            this.Container.Register(Component.For<ITechPassportSectionReport>().Named("TP Report.TechPassport.Section1_1").ImplementedBy<Section1_1>().LifeStyle.Transient);
            this.Container.Register(Component.For<ITechPassportSectionReport>().Named("TP Report.TechPassport.Section2").ImplementedBy<Section2>().LifeStyle.Transient);
            this.Container.Register(Component.For<ITechPassportSectionReport>().Named("TP Report.TechPassport.Section3").ImplementedBy<Section3>().LifeStyle.Transient);
            this.Container.Register(Component.For<ITechPassportSectionReport>().Named("TP Report.TechPassport.Section4").ImplementedBy<Section4>().LifeStyle.Transient);
            this.Container.Register(Component.For<ITechPassportSectionReport>().Named("TP Report.TechPassport.Section5").ImplementedBy<Section5>().LifeStyle.Transient);
            this.Container.Register(Component.For<ITechPassportSectionReport>().Named("TP Report.TechPassport.Section6").ImplementedBy<Section6>().LifeStyle.Transient);

            this.Container.RegisterTransient<ITechPassportImport, TechPassportImport>();
            this.Container.RegisterTransient<ITechPassportPartImport, Bars.Gkh.Import.Fund.Impl.Part0>(Bars.Gkh.Import.Fund.Impl.Part0.Code);
            this.Container.RegisterTransient<ITechPassportPartImport, Bars.Gkh.Import.Fund.Impl.Part1>(Bars.Gkh.Import.Fund.Impl.Part1.Code);
            this.Container.RegisterTransient<ITechPassportPartImport, Bars.Gkh.Import.Fund.Impl.Part2>(Bars.Gkh.Import.Fund.Impl.Part2.Code);
            this.Container.RegisterTransient<ITechPassportPartImport, Bars.Gkh.Import.Fund.Impl.Part3>(Bars.Gkh.Import.Fund.Impl.Part3.Code);
            this.Container.RegisterTransient<ITechPassportPartImport, Bars.Gkh.Import.Fund.Impl.Part4>(Bars.Gkh.Import.Fund.Impl.Part4.Code);
            this.Container.RegisterTransient<ITechPassportPartImport, Bars.Gkh.Import.Fund.Impl.Part5>(Bars.Gkh.Import.Fund.Impl.Part5.Code);
            this.Container.RegisterTransient<ITechPassportPartImport, Bars.Gkh.Import.Fund.Impl.Part6>(Bars.Gkh.Import.Fund.Impl.Part6.Code);
            this.Container.RegisterTransient<ITechPassportPartImport, Bars.Gkh.Import.Fund.Impl.PartLifts>(Bars.Gkh.Import.Fund.Impl.PartLifts.Code);

            this.Container.RegisterTransient<ITechPassportDataImport, TechPassportDataImport>();

            this.Container.Register(Component.For<IPermissionSource>().ImplementedBy<GkhTpPermissionMap>());
            this.Container.Register(Component.For<IPrintForm>().ImplementedBy<RoTechPassportExport>().Named("Report Bars.GkhTp.RoTechPassportExport").LifestyleTransient());

            this.Container.RegisterExecutionAction<FillTpByRealityObjectData>();

            Component.For<IGkhImport>().ImplementedBy<ElevatorsImport>().Named("Bars.GkhTp.Imports.ElevatorsImport").RegisterIn(this.Container);

            this.Container.RegisterTransient<ITechnicalPassportTransformer, TechnicalPassportTransformer>();
            this.Container.RegisterTransient<ITechnicalPassportConstructor, TechnicalPassportConstructor>();
        }

        private void RegisterTaskExecutors()
        {
            this.Container.RegisterTaskExecutor<FormatDataExportTaskExecutor>(FormatDataExportTaskExecutor.Id);
        }

        private void RegisterDomainEventHandlers()
        {
            Component
                .For<IDomainEventHandler<GeneralStateChangeEvent>>()
                .ImplementedBy<GeneralStateChangeHandler>()
                .LifeStyle.Scoped()
                .RegisterIn(this.Container);
        }

        private void DataTransferConfiguration()
        {
            this.Container.RegisterTransient<IDataTransferCache, DataTransferCache>();
            this.Container.RegisterTransient<IDataTransferProvider, DataTransferProvider>();
            this.Container.RegisterTransient<ISystemIntegrationService, SystemIntegrationService>();
            this.Container.RegisterSingleton<ITransferEntityProvider, TransferEntityProvider>();

            // планировщик для однопоточной работы с импортом/экспортом
            // TODO: quartz
            // Component.For<IScheduler>()
            //     .Named("TransferEntityScheduler")
            //     .UsingFactoryMethod((k, cc) =>
            //     {
            //         var parameters = new NameValueCollection
            //         {
            //             { "quartz.threadPool.threadCount", "1" },
            //             { "quartz.scheduler.instanceName", "TransferEntityScheduler" },
            //             { "quartz.scheduler.instanceId", "TransferEntityScheduler" }
            //         };
            //
            //         var schedulerFactory = new StdSchedulerFactory(parameters);
            //         var scheduler = schedulerFactory.GetScheduler();
            //
            //         if (!scheduler.IsStarted)
            //         {
            //             scheduler.Start();
            //         }
            //
            //         return scheduler;
            //     })
            //     .LifestyleSingleton()
            //     .RegisterIn(this.Container);
        }

        private void CheckMigration()
        {
            if (ApplicationContext.Current.GetContextType() == ApplicationContextType.TaskApplication)
            {
                this.Container.UsingForResolved<IEventAggregator>((container, eventAggregator) =>
                {
                    eventAggregator.GetEvent<AppStartEvent>().Subscribe<MigrationChecker>();
                });
            }
        }

        private void RegisterNhGenerators()
        {
            this.Container.RegisterTransient<INhibernateConfigModifier, NhConfigModifier>();
            this.Container.RegisterMethodHqlGenerator<GetIdGenerator>();
            this.Container.RegisterMethodHqlGenerator<ToNullableGenerator>();

        }
    }

    /// <summary>
    /// Для того что бы не было тормозов на логинке. Будет переделано
    /// </summary>
    public class UpDb : EventHandlerBase<AppStartEventArgs>
    {
        /// <summary>
        /// Обработчик события
        /// </summary>
        /// <param name="args">Параметры события</param>
        public override void OnEvent(AppStartEventArgs args)
        {
            try
            {
                ApplicationContext.Current.Container.Resolve<IRepository<User>>().GetAll().Count();
            }
            catch
            {
                // ignored
            }
        }
    }
}