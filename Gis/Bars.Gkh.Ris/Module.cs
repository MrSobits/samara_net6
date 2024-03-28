namespace Bars.Gkh.Ris
{
    #region usings

    using B4;
    using B4.IoC;
    using B4.Windsor;
    using B4.Modules.DataExport.Domain;
    using Bars.Gkh.Ris.Integration.HouseManagement.DataExtractors.AccountData;
    using Bars.B4.Config;
    using Bars.Gkh.Ris.Entities.External.Housing.House;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Gis.Entities.Kp50;
    using Bars.Gkh.Modules.Gkh1468.Entities;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Bars.Gkh.Repair.Entities;
    using Bars.Gkh.Ris.Entities.External.Housing.OKI;
    using Bars.Gkh.Ris.Entities.Infrastructure;
    using Bars.Gkh.Ris.Entities.DeviceMetering;
    using Bars.Gkh.Ris.Entities.Inspection;
    using Bars.Gkh.Ris.Entities.Bills;
    using Bars.Gkh.Ris.Entities.External.Administration.System;
    using Bars.Gkh.Ris.DeviceMeteringAsync;
    using Bars.Gkh.Ris.Integration.HouseManagement.DataExtractors.MeteringDeviceData;
    using Bars.Gkh.Ris.Entities.OrgRegistry;
    using Bars.Gkh.Ris.Entities.Payment;
    using Bars.Gkh.Ris.Entities.Services;
    using Bars.Gkh.Ris.GisServiceProvider.OrgRegistry;
    using Bars.Gkh.Ris.InfrastructureAsync;
    using Bars.Gkh.Ris.InspectionAsync;
    using Bars.Gkh.Ris.Integration.Bills.DataExtractors;
    using Bars.Gkh.Ris.Integration.DeviceMetering;
    using Bars.Gkh.Ris.Integration.DeviceMetering.DataExtractors;
    using Bars.Gkh.Ris.Integration.DeviceMetering.Exporters;
    using Bars.Gkh.Ris.Integration.DeviceMetering.PrepareAssistants;
    using Bars.Gkh.Ris.Integration.Bills.Extractors.OpenOrgPaymentPeriod;
    using Bars.Gkh.Ris.Integration.HouseManagement.DataExtractors.CharterData;
    using Bars.Gkh.Ris.Integration.HouseManagement.DataExtractors.HouseData;
    using Bars.Gkh.Ris.Integration.HouseManagement.DataExtractors.PublicPropertyContractData;
    using Bars.Gkh.Ris.Integration.HouseManagement.DataExtractors.VotingProtocolData;
    using Bars.Gkh.Ris.Integration.Inspection.DataExtractors.InspectionPlan;
    using Bars.Gkh.Ris.Integration.HouseManagement.DataExtractors.NotificationData;
    using Bars.Gkh.Ris.Integration.HouseManagement.DataExtractors.SupplyResourceContract;
    using Bars.Gkh.Ris.Integration.Infrastructure.DataExtractors.RkiData;
    using Bars.Gkh.Ris.Integration.Infrastructure.Exporters;
    using Bars.Gkh.Ris.Integration.Inspection.DataExtractors.Examination;
    using Bars.Gkh.Ris.Integration.Inspection.Exporters;
    using Bars.Gkh.Ris.Integration.Inspection.Tasks;
    using Bars.Gkh.Ris.Integration.OrgRegistry.DataExtractors;
    using Bars.Gkh.Ris.Integration.OrgRegistry.Exporters;
    using Bars.Gkh.Ris.Integration.OrgRegistryCommon.DataExtractors;
    using Bars.Gkh.Ris.Integration.Services.Exporters;
    using Bars.Gkh.Ris.Tasks;
    using Bars.Gkh.Ris.Tasks.Bills.Acknowledgment;
    using Bars.Gkh.Ris.Tasks.HouseManagement.ContractData;
    using Bars.Gkh.Ris.Tasks.HouseManagement.HouseData;
    using Bars.Gkh.Ris.Tasks.Nsi.AdditionalServices;
    using Bars.Gkh.Ris.Tasks.Nsi.MunicipalServices;
    using Bars.Gkh.Ris.Tasks.Nsi.OrganizationWorks;
    using Bars.Gkh.Ris.Tasks.OrgRegistryCommon.DataProvider;
    using Bars.Gkh.Ris.Integration.Payment.DataExtractors;
    using Bars.Gkh.Ris.Integration.Payment.DataSelectors;
    using Bars.Gkh.Ris.Integration.Payment.Exporters;
    using Bars.Gkh.Ris.Integration.Services.Tasks;
    using Bars.Gkh.Ris.Security;
    using Bars.Gkh.Ris.Tasks.HouseManagement.AccountData;
    using Bars.Gkh.Ris.Tasks.HouseManagement.MeteringDeviceData;
    using Bars.Gkh.Ris.Tasks.OrgRegistry;
    using Bars.Gkh.Ris.ServicesAsync;
    using Bars.Gkh.Ris.Tasks.Bills.ImportPaymentDocumentData;
    using Bars.Gkh.Ris.Tasks.DeviceMetering.MeteringDeviceValues;
    using Bars.Gkh.Ris.Tasks.Bills.OpenOrgPaymentPeriod;
    using Bars.Gkh.Ris.Tasks.HouseManagement.CharterData;
    using Bars.Gkh.Ris.Tasks.HouseManagement.PublicPropertyContractData;
    using Bars.Gkh.Ris.Tasks.HouseManagement.VotingProtocol;
    using Bars.Gkh.Ris.Tasks.Inspection.InspectionPlan;
    using Bars.Gkh.Ris.Tasks.HouseManagement.NotificationData;
    using Bars.Gkh.Ris.Tasks.HouseManagement.SupplyResourceContract;
    using Bars.Gkh.Ris.Tasks.Infrastructure.Rki;
    using Bars.Gkh.Ris.Tasks.OrgRegistryCommon.OrgRegistry;
    using Bars.Gkh.Ris.Tasks.Payment;
    using Bars.Gkh.Ris.Tasks.Payment.NotificationsOfOrderExecutionCancellation;
    using Bars.Gkh.Ris.Tasks.Payment.SupplierNotificationsOfOrderExecution;
    using Bars.Gkh.Ris.Tasks.Services;
    using Bars.Gkh.Ris.ViewModel.Protocol;
    using Bars.Gkh.Ris.ViewModel.Result;

    using BillsAsync;
    using Bars.Gkh.Ris.ViewModel.Task;
    using Bars.GkhDi.Entities;
    using Bars.GkhGji.Entities;

    using Castle.MicroKernel.Registration;
    using ConfigSections;
    using Controllers;
    using DomainService.GisIntegration;
    using DomainService.GisIntegration.Impl;
    using Entities;
    using Entities.GisIntegration;
    using Entities.GisIntegration.Ref;
    using Entities.HouseManagement;
    using Entities.Nsi;
    using ExecutionAction;
    using Export;
    using GisServiceProvider;
    using GisServiceProvider.Bills;
    using GisServiceProvider.DeviceMetering;
    using GisServiceProvider.HouseManagement;
    using GisServiceProvider.Infrastructure;
    using GisServiceProvider.Inspection;
    using GisServiceProvider.Nsi;
    using GisServiceProvider.OrgRegistryCommon;
    using GisServiceProvider.Payment;
    using GisServiceProvider.Services;
    using Gkh.Entities;
    using Gis.Entities.ManOrg;
    using Gis.Entities.ManOrg.Contract;
    using Gkh.ExecutionAction;
    using HouseManagementAsync;
    using Integration;
    using Integration.Bills.Exporters;
    using Integration.FileService;
    using Integration.FileService.Impl;
    using Integration.HouseManagement.DataExtractors;
    using Integration.HouseManagement.DataExtractors.ContractData;
    using Integration.HouseManagement.Exporters;
    using Integration.Inspection.DataExtractors;
    using Integration.Nsi;
    using Integration.Nsi.DataExtractors;
    using Integration.Nsi.DictionaryAction;
    using Integration.Nsi.DictionaryAction.HouseManagement;
    using Integration.Nsi.DictionaryAction.Inspection;
    using Integration.Nsi.DictionaryAction.Nsi;
    using Integration.Nsi.DictionaryAction.Services;
    using Integration.Nsi.Exporters;
    using Integration.OrgRegistryCommon.Exporters;
    using Integration.Services.DataExtractors;
    using Interceptors;
    using Interceptors.GisIntegration;
    using NsiAsync;
    using PaymentAsync;
    using Quartz.Scheduler;
    using Utils;
    using ViewModel;
    using ViewModel.GisIntegration;
    using ViewModel.HouseManagement;

    #endregion

    public partial class Module : AssemblyDefinedModule
    {        
        public override void Install()
        {
            Settings.Init(this.Container.Resolve<IConfigProvider>());

            // маршруты
            this.Container.RegisterTransient<IClientRouteMapRegistrar, ClientRouteMapRegistrar>();

            // ресурсы
            this.Container.RegisterTransient<IResourceManifest, ResourceManifest>();

            // настройки ограничений
            this.Container.Register(Component.For<IPermissionSource>().ImplementedBy<RisPermissionMap>());

            this.Container.RegisterTransient<INavigationProvider, NavigationProvider>();

            this.Container.RegisterGkhConfig<GisIntegrationConfig>();
            this.Container.RegisterTransient<IExecutionAction, GisIntegrationConfigCreateAction>(GisIntegrationConfigCreateAction.Code);

            //this.Container.RegisterTransient<IExecutionAction, GenerateContracts>(GenerateContracts.Code);

            this.RegisterControllers();

            this.RegisterInterceptors();

            this.RegisterViewModels();

            this.RegisterServices();

            this.RegisterDataExtractors();

            this.RegisterDataSelectors();

            this.RegisterBundlers();

            this.RegisterTasks();

            this.RegisterServiceProviders();

            this.RegisterExporters();

            this.RegisterExports();
        }

        public void RegisterControllers()
        {
            this.Container.RegisterAltDataController<GisDict>();
            this.Container.RegisterAltDataController<GisDictRef>();
            this.Container.RegisterController<GisIntegrationController>();
            this.Container.RegisterController<RisSettingsController>();
            this.Container.RegisterController<TaskTreeController>();
            this.Container.RegisterController<HouseManagementController>();
            this.Container.RegisterController<InspectionServiceController>();
            this.Container.RegisterController<NsiController>();
            this.Container.RegisterController<OrgRegistryController>();
            this.Container.RegisterAltDataController<RisContract>();
            this.Container.RegisterController<ServicesController>();
            this.Container.RegisterController<InfrastructureController>();
            this.Container.RegisterController<PaymentServiceController>();
            this.Container.RegisterController<BillsController>();
        }

        public void RegisterViewModels()
        {
            this.Container.RegisterViewModel<GisDict, GisDictViewModel>();
            this.Container.RegisterViewModel<GisDictRef, GisDictRefViewModel>();
            this.Container.RegisterTransient<ITreeViewModel, TaskTreeViewModel>("TaskTreeViewModel");
            this.Container.RegisterViewModel<RisContract, RisContractViewModel>();
            this.Container.RegisterTransient<ITriggerProtocolViewModel, TriggerProtocolViewModel>();
            this.Container.RegisterTransient<IObjectProcessingResultViewModel, ObjectProcessingResultViewModel>();
        }

        public void RegisterServices()
        {
            this.Container.Register(Component.For<CrossAuthentification>().LifestyleTransient());
            this.Container.Register(Component.For<RisManager>().LifestyleTransient());

            this.Container.RegisterTransient<IMeteringDeviceBasePrepareAssistant<RisMeteringDeviceControlValue>, RisMeteringDeviceControlValueAssistant>();
            this.Container.RegisterTransient<IMeteringDeviceBasePrepareAssistant<RisMeteringDeviceCurrentValue>, RisMeteringDeviceCurrentValueAssistant>();
            this.Container.RegisterTransient<IMeteringDeviceBasePrepareAssistant<RisMeteringDeviceVerificationValue>, RisMeteringDeviceVerificationValueAssistant>();

            this.Container.RegisterTransient<IGisIntegrationService, GisIntegrationService>();
            this.Container.RegisterTransient<IFileUploadService, FileUploadService>();
            this.Container.RegisterTransient<IHouseManagementService, HouseManagementService>();
            this.Container.RegisterTransient<IInspectionService, InspectionService>();
            this.Container.RegisterTransient<IOrgRegistryService, OrgRegistryService>();
            this.Container.RegisterTransient<INsiService, NsiService>();
            this.Container.RegisterTransient<ITaskTreeService, TaskTreeService>();
            this.Container.RegisterTransient<ITaskManager, TaskManager>();
            this.Container.RegisterTransient<IServicesService, ServicesService>();
            this.Container.RegisterTransient<IInfrastructureService, InfrastructureService>();
            this.Container.RegisterTransient<IPaymentService, PaymentService>();
            this.Container.RegisterTransient<IBillsService, BillsService>();

            // регистрация словарей
            this.Container.RegisterTransient<IGisIntegrDictAction, TypeContractManOrgDictAction>();
            this.Container.RegisterTransient<IGisIntegrDictAction, ConditionHouseDictAction>();
            this.Container.RegisterTransient<IGisIntegrDictAction, KindCheckDictAction>();
            this.Container.RegisterTransient<IGisIntegrDictAction, ContractBaseDictAction>();
            this.Container.RegisterTransient<IGisIntegrDictAction, ServiceTypeDictAction>();
            this.Container.RegisterTransient<IGisIntegrDictAction, MunicipalServiceDictAction>();
            this.Container.RegisterTransient<IGisIntegrDictAction, MunicipalResourceDictAction>();
            this.Container.RegisterTransient<IGisIntegrDictAction, RoomsNumDictAction>();
            this.Container.RegisterTransient<IGisIntegrDictAction, PremisesCharacteristicDictAction>();
            this.Container.RegisterTransient<IGisIntegrDictAction, PositionDictAction>();
            this.Container.RegisterTransient<IGisIntegrDictAction, PurposeDictAction>();
            this.Container.RegisterTransient<IGisIntegrDictAction, ServiceWorkPurposeDictAction>();
            this.Container.RegisterTransient<IGisIntegrDictAction, ContentRepairMkdWorkDictAction>();
            this.Container.RegisterTransient<IGisIntegrDictAction, FormingFondTypeDictAction>();
            this.Container.RegisterTransient<IGisIntegrDictAction, OwnersMeetingDecisionTypeDictAction>();
            this.Container.RegisterTransient<IGisIntegrDictAction, ChargeableMunicipalResourceDictAction>();
            this.Container.RegisterTransient<IGisIntegrDictAction, OwnerDocumentTypeDictAction>();
            this.Container.RegisterTransient<IGisIntegrDictAction, ExaminationBaseDictAction>();
            this.Container.RegisterTransient<IGisIntegrDictAction, ExaminationFormDictAction>();
            this.Container.RegisterTransient<IGisIntegrDictAction, StopReasonDictAction>();
            this.Container.RegisterTransient<IGisIntegrDictAction, UnitMeasureDictAction>();
            this.Container.RegisterTransient<IGisIntegrDictAction, SupplyResContractBaseDictAction>();
            this.Container.RegisterTransient<IGisIntegrDictAction, OlsonTZDictAction>();
            this.Container.RegisterTransient<IGisIntegrDictAction, OverhaulFormingKindDictAction>();
            this.Container.RegisterTransient<IGisIntegrDictAction, HouseManagementTypeDictAction>();
        }

        public void RegisterExtractors()
        {
           // this.Container.RegisterTransient<IGisIntegrationDataExtractor, ImportPublicPropertyContractDataExtractor>("ImportPublicPropertyContractDataExtractor");

            //this.Container.RegisterTransient<IDataExtractor<RisHouse, RealityObject>, RisHouseDataExtractor>("RisHouseDataExtractor");
            //this.Container.RegisterTransient<IDataExtractor<RisEntrance, Entrance>, EntranceDataExtractor>("EntranceDataExtractor");
            //this.Container.RegisterTransient<IDataExtractor<NonResidentialPremises, Room>, NonResidentialPremisesDataExtractor>("NonResidentialPremisesDataExtractor");
            //this.Container.RegisterTransient<IDataExtractor<ResidentialPremises, Room>, ResidentialPremisesDataExtractor>("ResidentialPremisesDataExtractor");
            //this.Container.RegisterTransient<IDataExtractor<RisOrganizationWork, ManOrgBilWorkService>, OrganizationWorksDataExtractor>("OrganizationWorksDataExtractor");
            //this.Container.RegisterTransient<IDataExtractor<RisContract, ManOrgBaseContract>, ContractDataExtractor>("ContractDataExtractor");
            //this.Container.RegisterTransient<IDataExtractor<ContractObject, ManOrgBaseContract>, ContractObjectDataExtractor>("ContractObjectDataExtractor");
            //this.Container.RegisterTransient<IDataExtractor<HouseManService, ContractOwnersCommService>, OwnersHouseManServiceExtractor>("OwnersHouseManServiceExtractor");
            //this.Container.RegisterTransient<IDataExtractor<HouseManService, JskTsjContractCommService>, JskTsjHouseManServiceExtractor>("JskTsjHouseManServiceExtractor");
            //this.Container.RegisterTransient<IDataExtractor<AddService, ContractOwnersAddService>, OwnersAddServiceExtractor>("OwnersAddServiceExtractor");
            //this.Container.RegisterTransient<IDataExtractor<AddService, JskTsjContractAddService>, JskTsjAddServiceExtractor>("JskTsjAddServiceExtractor");
            //this.Container.RegisterTransient<IDataExtractor<RisContractAttachment, FileInfo>, ContractAttachmentExtractor>("ContractAttachmentExtractor");
            //this.Container.RegisterTransient<IDataExtractor<ProtocolMeetingOwner, FileInfo>, OwnersProtocolMeetingOwnerExtractor>("OwnersProtocolMeetingOwnerExtractor");
            //this.Container.RegisterTransient<IDataExtractor<ProtocolMeetingOwner, FileInfo>, JskTsjProtocolMeetingOwnerExtractor>("JskTsjProtocolMeetingOwnerExtractor");
            //this.Container.RegisterTransient<IDataExtractor<RisProtocolOk, FileInfo>, OwnersProtocolOkExtractor>("OwnersProtocolOkExtractor");
            //this.Container.RegisterTransient<IDataExtractor<RisSubsidiary, Contragent>, SubsidiaryDataExtractor>("SubsidiaryDataExtractor");
            //this.Container.RegisterTransient<IDataExtractor<Charter, ManOrgJskTsjContract>, CharterExtractor>("CharterExtractor");
            //this.Container.RegisterTransient<IDataExtractor<RisPublicPropertyContract, InfoAboutUseCommonFacilities>, PublicPropertyContractExtractor>("PublicPropertyContractExtractor");
            //this.Container.RegisterTransient<IDataExtractor<RisContractAttachment, FileInfo>, PublicPropertyContractAttachmentExtractor>("PublicPropertyContractAttachmentExtractor");
            //this.Container.RegisterTransient<IDataExtractor<RisTrustDocAttachment, FileInfo>, TrustDocAttachmentExtractor>("TrustDocAttachmentExtractor");
            //this.Container.RegisterTransient<IDataExtractor<RisVotingProtocol, PropertyOwnerProtocols>, VotingProtocolExtractor>("VotingProtocolExtractor");
            //this.Container.RegisterTransient<IDataExtractor<RisDecisionList, BasePropertyOwnerDecision>, DecisionExtractor>("DecisionExtractor");
            //this.Container.RegisterTransient<IDataExtractor<RisVotingProtocolAttachment, FileInfo>, VotingProtocolAttachmentExtractor>("VotingProtocolAttachmentExtractor");
            //this.Container.RegisterTransient<IDataExtractor<Examination, BaseJurPerson>, ExaminationExtractor>("ExaminationExtractor");
            //this.Container.RegisterTransient<IDataExtractor<InspectionPlan, PlanJurPersonGji>, InspectionPlanExtractor>("InspectionPlanExtractor");
            //this.Container.RegisterTransient<IDataExtractor<RisNotification, Notif>, NotificationDataExtractor>("NotificationDataExtractor");
            //this.Container.RegisterTransient<IDataExtractor<RisNotificationAddressee, NotifAddress>, NotificationAddresseeExtractor>("NotificationAddresseeExtractor");
            //this.Container.RegisterTransient<IDataExtractor<RisNotificationAttachment, NotifDoc>, NotificationAttachmentExtractor>("NotificationAttachmentLinkExtractor");
        }

        public void RegisterDataSelectors()
        {
            this.Container.RegisterTransient<IDataSelector<ContragentProxy>, ContragentSelector>("ContragentSelector");
            this.Container.RegisterTransient<IDataSelector<RealityObject>, RisHouseDataExtractor>("HouseDataSelector");
            this.Container.RegisterTransient<IDataSelector<ManOrgBilWorkService>, OrganizationWorksDataExtractor>("OrganizationWorksDataSelector");
            this.Container.RegisterTransient<IDataSelector<ManOrgBaseContract>, ContractDataExtractor>("ContractDataSelector");
            this.Container.RegisterTransient<IDataSelector<ManOrgJskTsjContract>, CharterExtractor>("CharterSelector");
            this.Container.RegisterTransient<IDataSelector<InfoAboutUseCommonFacilities>, PublicPropertyContractExtractor>("PublicPropertyContractSelector");
            this.Container.RegisterTransient<IDataSelector<PlanJurPersonGji>, InspectionPlanExtractor>("InspectionPlanSelector");
            this.Container.RegisterTransient<IDataSelector<NotifAddress>, NotificationAddresseeExtractor>("NotificationAddresseeSelector");
            this.Container.RegisterTransient<IDataSelector<RepairObject>, WorkListExtractor>("WorkListSelector");
            this.Container.RegisterTransient<IDataSelector<NotificationOfOrderExecution>, NotificationOfOrderExecutionCancellationDataSelector>(nameof(NotificationOfOrderExecutionCancellationDataSelector));
            this.Container.RegisterTransient<IDataSelector<InspectionGji>, InspectionSelector>("InspectionSelector");
            this.Container.RegisterTransient<IDataSelector<Disposal>, DisposalSelector>("DisposalSelector");
            this.Container.RegisterTransient<IDataSelector<BilServiceDictionary>, AdditionalServicesDataExtractor>("AdditionalServicesSelector");

            //пока не используются
            this.Container.RegisterTransient<IDataSelector<Entrance>, EntranceDataExtractor>("EntranceDataSelector");
            this.Container.RegisterTransient<IDataSelector<Room>, NonResidentialPremisesDataExtractor>("NonResidentialPremisesDataSelector");
            this.Container.RegisterTransient<IDataSelector<Room>, ResidentialPremisesDataExtractor>("ResidentialPremisesDataSelector");
            this.Container.RegisterTransient<IDataSelector<ManOrgBaseContract>, ContractObjectDataExtractor>("ContractObjectDataSelector");
            this.Container.RegisterTransient<IDataSelector<ContractOwnersCommService>, OwnersHouseManServiceExtractor>("OwnersHouseManServiceSelector");
            this.Container.RegisterTransient<IDataSelector<JskTsjContractCommService>, JskTsjHouseManServiceExtractor>("JskTsjHouseManServiceSelector");
            this.Container.RegisterTransient<IDataSelector<ContractOwnersAddService>, OwnersAddServiceExtractor>("OwnersAddServiceSelector");
            this.Container.RegisterTransient<IDataSelector<JskTsjContractAddService>, JskTsjAddServiceExtractor>("JskTsjAddServiceSelector");
            this.Container.RegisterTransient<IDataSelector<FileInfo>, ContractAttachmentExtractor>("ContractAttachmentSelector");
            this.Container.RegisterTransient<IDataSelector<FileInfo>, OwnersProtocolMeetingOwnerExtractor>("OwnersProtocolMeetingOwnerSelector");
            this.Container.RegisterTransient<IDataSelector<FileInfo>, JskTsjProtocolMeetingOwnerExtractor>("JskTsjProtocolMeetingOwnerSelector");
            this.Container.RegisterTransient<IDataSelector<FileInfo>, OwnersProtocolOkExtractor>("OwnersProtocolOkSelector");
            this.Container.RegisterTransient<IDataSelector<Contragent>, SubsidiaryDataExtractor>("SubsidiaryDataSelector");
            this.Container.RegisterTransient<IDataSelector<FileInfo>, PublicPropertyContractAttachmentExtractor>("PublicPropertyContractAttachmentSelector");
            this.Container.RegisterTransient<IDataSelector<FileInfo>, TrustDocAttachmentExtractor>("TrustDocAttachmentSelector");
            this.Container.RegisterTransient<IDataSelector<PropertyOwnerProtocols>, VotingProtocolExtractor>("VotingProtocolSelector");
            this.Container.RegisterTransient<IDataSelector<BasePropertyOwnerDecision>, DecisionExtractor>("DecisionSelector");
            this.Container.RegisterTransient<IDataSelector<FileInfo>, VotingProtocolAttachmentExtractor>("VotingProtocolAttachmentSelector");
            this.Container.RegisterTransient<IDataSelector<BaseJurPerson>, InspectionPlanExaminationExtractor>("ExaminationSelector");
            this.Container.RegisterTransient<IDataSelector<Notif>, NotificationDataExtractor>("NotificationDataSelector");
            this.Container.RegisterTransient<IDataSelector<NotifDoc>, NotificationAttachmentExtractor>("NotificationAttachmentLinkSelector");
            this.Container.RegisterTransient<IDataSelector<RepairWork>, WorkListItemExtractor>("WorkListItemSelector");
            this.Container.RegisterTransient<IDataSelector<OkiObject>, RkiItemDataExtractor>("RkiItemDataSelector");
            this.Container.RegisterTransient<IDataSelector<RepairObject>, WorkingPlanExtractor>("WorkingPlanSelector");
            this.Container.RegisterTransient<IDataSelector<RepairWork>, WorkPlanItemExtractor>("WorkPlanItemSelector");
            this.Container.RegisterTransient<IDataSelector<RealObjPublicServiceOrg>, SupplyResourceContractExtractor>("SupplyResourceContractSelector");
            this.Container.RegisterTransient<IDataSelector<PerformedRepairWorkAct>, RisCompletedWorkExtractor>("RisCompletedWorkSelector");
            this.Container.RegisterTransient<IDataSelector<RealObjPublicServiceOrgService>, SupResContractServiceResourceExtractor>("SupResContractServiceResourceSelector");
            this.Container.RegisterTransient<IDataSelector<PublicOrgServiceQualityLevel>, SupResContractSubjectOtherQualityExtractor>("SupResContractSubjectOtherQualitySelector");
            this.Container.RegisterTransient<IDataSelector<PublicServiceOrgTemperatureInfo>, SupResContractTemperatureChartExtractor>("SupResContractTemperatureChartSelector");
            this.Container.RegisterTransient<IDataSelector<RealObjPublicServiceOrgService>, SupResContractSubjectExtractor>("SupResContractSubjectSelector");
            this.Container.RegisterTransient<IDataSelector<FileInfo>, SupResContractAttachmentExtractor>("SupResContractAttachmentSelector");
            this.Container.RegisterTransient<IDataSelector<ReportPeriod>, OpenOrgPaymentPeriodExtractor>("OpenOrgPaymentPeriodSelector");   
        }

        public void RegisterDataExtractors()
        { 
            this.Container.RegisterTransient<IGisIntegrationDataExtractor, MunicipalServicesDataExtractor>("MunicipalServicesDataExtractor");

            this.Container.RegisterTransient<IDataExtractor<RisAdditionalService>, AdditionalServicesDataExtractor>("AdditionalServicesDataExtractor");
            this.Container.RegisterTransient<IDataExtractor<RisHouse>, RisHouseDataExtractor>("RisHouseDataExtractor");
            this.Container.RegisterTransient<IDataExtractor<RisEntrance>, EntranceDataExtractor>("EntranceDataExtractor");
            this.Container.RegisterTransient<IDataExtractor<NonResidentialPremises>, NonResidentialPremisesDataExtractor>("NonResidentialPremisesDataExtractor");
            this.Container.RegisterTransient<IDataExtractor<ResidentialPremises>, ResidentialPremisesDataExtractor>("ResidentialPremisesDataExtractor");
            this.Container.RegisterTransient<IDataExtractor<RisOrganizationWork>, OrganizationWorksDataExtractor>("OrganizationWorksDataExtractor");
            this.Container.RegisterTransient<IDataExtractor<RisContract>, ContractDataExtractor>("ContractDataExtractor");
            this.Container.RegisterTransient<IDataExtractor<ContractObject>, ContractObjectDataExtractor>("ContractObjectDataExtractor");
            this.Container.RegisterTransient<IDataExtractor<HouseManService>, OwnersHouseManServiceExtractor>("OwnersHouseManServiceExtractor");
            this.Container.RegisterTransient<IDataExtractor<HouseManService>, JskTsjHouseManServiceExtractor>("JskTsjHouseManServiceExtractor");
            this.Container.RegisterTransient<IDataExtractor<AddService>, OwnersAddServiceExtractor>("OwnersAddServiceExtractor");
            this.Container.RegisterTransient<IDataExtractor<AddService>, JskTsjAddServiceExtractor>("JskTsjAddServiceExtractor");
            this.Container.RegisterTransient<IDataExtractor<RisContractAttachment>, ContractAttachmentExtractor>("ContractAttachmentExtractor");
            this.Container.RegisterTransient<IDataExtractor<ProtocolMeetingOwner>, OwnersProtocolMeetingOwnerExtractor>("OwnersProtocolMeetingOwnerExtractor");
            this.Container.RegisterTransient<IDataExtractor<ProtocolMeetingOwner>, JskTsjProtocolMeetingOwnerExtractor>("JskTsjProtocolMeetingOwnerExtractor");
            this.Container.RegisterTransient<IDataExtractor<RisProtocolOk>, OwnersProtocolOkExtractor>("OwnersProtocolOkExtractor");
            this.Container.RegisterTransient<IDataExtractor<RisSubsidiary>, SubsidiaryDataExtractor>("SubsidiaryDataExtractor");
            this.Container.RegisterTransient<IDataExtractor<Charter>, CharterExtractor>("CharterExtractor");
            this.Container.RegisterTransient<IDataExtractor<RisPublicPropertyContract>, PublicPropertyContractExtractor>("PublicPropertyContractExtractor");
            this.Container.RegisterTransient<IDataExtractor<RisContractAttachment>, PublicPropertyContractAttachmentExtractor>("PublicPropertyContractAttachmentExtractor");
            this.Container.RegisterTransient<IDataExtractor<RisTrustDocAttachment>, TrustDocAttachmentExtractor>("TrustDocAttachmentExtractor");
            this.Container.RegisterTransient<IDataExtractor<RisVotingProtocol>, VotingProtocolExtractor>("VotingProtocolExtractor");
            this.Container.RegisterTransient<IDataExtractor<RisDecisionList>, DecisionExtractor>("DecisionExtractor");
            this.Container.RegisterTransient<IDataExtractor<RisVotingProtocolAttachment>, VotingProtocolAttachmentExtractor>("VotingProtocolAttachmentExtractor");
            this.Container.RegisterTransient<IDataExtractor<Examination>, InspectionPlanExaminationExtractor>("InspectionPlanExaminationExtractor");
            this.Container.RegisterTransient<IDataExtractor<InspectionPlan>, InspectionPlanExtractor>("InspectionPlanExtractor");
            this.Container.RegisterTransient<IDataExtractor<RisNotification>, NotificationDataExtractor>("NotificationDataExtractor");
            this.Container.RegisterTransient<IDataExtractor<RisNotificationAddressee>, NotificationAddresseeExtractor>("NotificationAddresseeExtractor");
            this.Container.RegisterTransient<IDataExtractor<RisNotificationAttachment>, NotificationAttachmentExtractor>("NotificationAttachmentLinkExtractor");
            this.Container.RegisterTransient<IDataExtractor<WorkList>, WorkListExtractor>("WorkListExtractor");
            this.Container.RegisterTransient<IDataExtractor<WorkListItem>, WorkListItemExtractor>("WorkListItemExtractor");
            this.Container.RegisterTransient<IDataExtractor<RisAccount>, AccountDataExtractor>("AccountDataExtractor");
            this.Container.RegisterTransient<IDataExtractor<RisMeteringDeviceData>, MeteringDeviceDataExtractor>("MeteringDeviceDataExtractor");
            this.Container.RegisterTransient<IDataExtractor<RisMeteringDeviceAccount>, MeteringDeviceAccountExtractor>("MeteringDeviceAccountExtractor");
            this.Container.RegisterTransient<IDataExtractor<RisMeteringDeviceLivingRoom>, MeteringDeviceLivingRoomExtractor>("MeteringDeviceLivingRoomExtractor");
            this.Container.RegisterTransient<IDataExtractor<WorkingPlan>, WorkingPlanExtractor>("WorkingPlanExtractor");
            this.Container.RegisterTransient<IDataExtractor<WorkPlanItem>, WorkPlanItemExtractor>("WorkPlanItemExtractor");
            this.Container.RegisterTransient<IDataExtractor<SupplyResourceContract>, SupplyResourceContractExtractor>("SupplyResourceContractExtractor");
            this.Container.RegisterTransient<IDataExtractor<SupResContractServiceResource>, SupResContractServiceResourceExtractor>("SupResContractServiceResourceExtractor");
            this.Container.RegisterTransient<IDataExtractor<SupResContractSubjectOtherQuality>, SupResContractSubjectOtherQualityExtractor>("SupResContractSubjectOtherQualityExtractor");
            this.Container.RegisterTransient<IDataExtractor<SupResContractTemperatureChart>, SupResContractTemperatureChartExtractor>("SupResContractTemperatureChartExtractor");
            this.Container.RegisterTransient<IDataExtractor<SupResContractSubject>, SupResContractSubjectExtractor>("SupResContractSubjectExtractor");
            this.Container.RegisterTransient<IDataExtractor<SupResContractAttachment>, SupResContractAttachmentExtractor>("SupResContractAttachmentExtractor");
            this.Container.RegisterTransient<IDataExtractor<OrgPaymentPeriod>, OpenOrgPaymentPeriodExtractor>("OpenOrgPaymentPeriodExtractor");
            
            this.Container.RegisterTransient<IDataExtractor<RisAttachmentsEnergyEfficiency>, RkiAttachmentEnergyEfficiencyExtractor>("RkiAttachmentEnergyEfficiencyExtractor");
            this.Container.RegisterTransient<IDataExtractor<RisRkiAttachment>, RkiAttachmentExtractor>("RkiAttachmentExtractor");
            this.Container.RegisterTransient<IDataExtractor<RisRkiCommunalService>, RkiCommunalServiceExtractor>("RkiCommunalServiceExtractor");
            this.Container.RegisterTransient<IDataExtractor<RisRkiItem>, RkiItemDataExtractor>("RkiItemDataExtractor");
            this.Container.RegisterTransient<IDataExtractor<RisNetPieces>, RkiNetPiecesExtractor>("RkiNetPiecesExtractor");
            this.Container.RegisterTransient<IDataExtractor<RisRkiReceiver>, RkiReceiverExtractor>("RkiReceiverExtractor");
            this.Container.RegisterTransient<IDataExtractor<RisResource>, RkiResourceExtractor>("RkiResourceExtractor");
            this.Container.RegisterTransient<IDataExtractor<RisRkiSource>, RkiSourceExtractor>("RkiSourceExtractor");
            this.Container.RegisterTransient<IDataExtractor<RisTransportationResources>, RkiTransportationResourceExtractor>("RkiTransportationResourceExtractor");

            this.Container.RegisterTransient<IDataExtractor<RisMeteringDeviceControlValue>, MeteringDeviceValuesBaseDataExtractor<RisMeteringDeviceControlValue>>("MeteringDeviceValuesBaseDataExtractor.RisMeteringDeviceControlValue");
            this.Container.RegisterTransient<IDataExtractor<RisMeteringDeviceCurrentValue>, MeteringDeviceValuesBaseDataExtractor<RisMeteringDeviceCurrentValue>>("MeteringDeviceValuesBaseDataExtractor.RisMeteringDeviceCurrentValue");
            this.Container.RegisterTransient<IDataExtractor<RisMeteringDeviceVerificationValue>, MeteringDeviceValuesBaseDataExtractor<RisMeteringDeviceVerificationValue>>("MeteringDeviceValuesBaseDataExtractor.RisMeteringDeviceVerificationValue");

            this.Container.RegisterTransient<IDataExtractor<NotificationOfOrderExecution>, NotificationOfOrderExecutionExtractor>("NotificationOfOrderExecutionExtractor");
            this.Container.RegisterTransient<IDataExtractor<RisCompletedWork>, RisCompletedWorkExtractor>("RisCompletedWorkExtractor");

            this.Container.RegisterTransient<IDataExtractor<RisPaymentDocument>, RisPaymentDocumentDataExtractor>("RisPaymentDocumentDataExtractor");
            this.Container.RegisterTransient<IDataExtractor<RisAdditionalServiceChargeInfo>, RisAdditionalServiceChargeInfoDataExtractor>("RisAdditionalServiceChargeInfoDataExtractor");
            this.Container.RegisterTransient<IDataExtractor<RisAdditionalServiceExtChargeInfo>, RisAdditionalServiceExtChargeInfoDataExtractor>("RisAdditionalServiceExtChargeInfoDataExtractor");
            this.Container.RegisterTransient<IDataExtractor<RisHousingServiceChargeInfo>, RisHousingServiceChargeInfoDataExtractor>("RisHousingServiceChargeInfoDataExtractor");
            this.Container.RegisterTransient<IDataExtractor<RisMunicipalServiceChargeInfo>, RisMunicipalServiceChargeInfoDataExtractor>("RisMunicipalServiceChargeInfoDataExtractor");
            this.Container.RegisterTransient<IDataExtractor<RisTechService>, RisTechServiceDataExtractor>("RisTechServiceDataExtractor");
            this.Container.RegisterTransient<IDataExtractor<RisAcknowledgment>, AcknowledgmentDataExtractor>("AcknowledgmentDataExtractor");
            this.Container.RegisterTransient<IDataExtractor<LivingRoom>, LivingRoomExtractor>("LivingRoomExtractor");

            this.Container.RegisterTransient<IDataExtractor<Examination>, ExaminationExtractor>("ExaminationExtractor");
            this.Container.RegisterTransient<IDataExtractor<Precept>, PreceptExtractor>("PreceptExtractor");
            this.Container.RegisterTransient<IDataExtractor<Offence>, OffenceExtractor>("OffenceExtractor");
        }

        public void RegisterInterceptors()
        {
            this.Container.RegisterDomainInterceptor<GisDict, GisDictInterceptor>();
            this.Container.RegisterDomainInterceptor<RisPackage, UserEntityInterceptor<RisPackage>>();
            this.Container.RegisterDomainInterceptor<RisTask, UserEntityInterceptor<RisTask>>();
        }

        public void RegisterTasks()
        {
            this.Container.RegisterTask<ExportAcknowledgmentTask>();
            this.Container.RegisterTask<ExportDataProviderTask>();
            this.Container.RegisterTask<ExportCharterDataTask>();
            this.Container.RegisterTask<ExportContractDataTask>();
            this.Container.RegisterTask<HouseOMSExportDataTask>();
            this.Container.RegisterTask<HouseRSOExportDataTask>();
            this.Container.RegisterTask<HouseUOExportDataTask>();
            this.Container.RegisterTask<ExportAdditionalServicesTask>();
            this.Container.RegisterTask<ExportMunicipalServicesTask>();
            this.Container.RegisterTask<ExportOrganizationWorksTask>();
            this.Container.RegisterTask<ExportOrgRegistryTask>();
            this.Container.RegisterTask<ExportNotificationDataTask>();

            this.Container.RegisterTask<HouseOMSPrepareDataTask>();
            this.Container.RegisterTask<HouseRSOPrepareDataTask>();
            this.Container.RegisterTask<HouseUOPrepareDataTask>();
            this.Container.RegisterTask<ContractPrepareDataTask>();
            this.Container.RegisterTask<CharterPrepareDataTask>();
            this.Container.RegisterTask<NotificationPrepareDataTask>();

            this.Container.RegisterTask<AdditionalServicesPrepareDataTask>();
            this.Container.RegisterTask<MunicipalServicesPrepareDataTask>();
            this.Container.RegisterTask<OrganizationWorksPrepareDataTask>();

            this.Container.RegisterTask<OrgRegistryPrepareDataTask>();
            this.Container.RegisterTask<DataProviderPrepareDataTask>();

            this.Container.RegisterTask<Tasks.Bills.Acknowledgment.AcknowledgmentPrepareDataTask>();
            this.Container.RegisterTask<AccountPrepareDataTask>();
            this.Container.RegisterTask<ExportAccountDataTask>();

            this.Container.RegisterTask<ExportMeteringDeviceDataTask>();
            this.Container.RegisterTask<MeteringDevicePrepareDataTask>();

            this.Container.RegisterTask<SubsidiaryPrepareDataTask>();
            this.Container.RegisterTask<SubsidiaryExportTask>();

            this.Container.RegisterTask<ExportPublicPropertyContractDataTask>();            
            this.Container.RegisterTask<PublicPropertyContractPrepareDataTask>();

            this.Container.RegisterTask<ExportVotingProtocolTask>();
            this.Container.RegisterTask<VotingProtocolPrepareDataTask>();

            this.Container.RegisterTask<WorkingPlanExportTask>();
            this.Container.RegisterTask<WorkingPlanPrepareDataTask>();

            this.Container.RegisterTask<WorkingListExportTask>();
            this.Container.RegisterTask<WorkingListPrepareDataTask>();

            this.Container.RegisterTask<ExportInspectionPlanTask>();
            this.Container.RegisterTask<InspectionPlanPrepareDataTask>();

            this.Container.RegisterTask<MeteringDeviceValuesExportTask>();
            this.Container.RegisterTask<MeteringDeviceValuesPrepareDataTask>();

            this.Container.RegisterTask<ExportRkiDataTask>();
            this.Container.RegisterTask<RkiPrepareDataTask>();

            this.Container.RegisterTask<ExportNotificationsOfOrderExecutionTask>();
            this.Container.RegisterTask<NotificationsOfOrderExecutionPrepareDataTask>();
            this.Container.RegisterTask<ExportSupplierNotificationsOfOrderExecutionTask>();
            this.Container.RegisterTask<SupplierNotificationsOfOrderExecutionPrepareDataTask>();

            this.Container.RegisterTask<ExportSupplyResourceContractTask>();
            this.Container.RegisterTask<SupplyResourceContractPrepareDataTask>();
            this.Container.RegisterTask<CompletedWorkExportTask>();
            this.Container.RegisterTask<CompletedWorkPrepareDataTask>();

            this.Container.RegisterTask<PaymentDocumentPrepareDataTask>();
            this.Container.RegisterTask<ExportPaymentDocumentTask>();
            this.Container.RegisterTask<ExportNotificationsOfOrderExecutionCancellationTask>();
            this.Container.RegisterTask<NotificationsOfOrderExecutionCancellationPrepareDataTask>();
            this.Container.RegisterTask<ExportOpenOrgPaymentPeriodTask>();
            this.Container.RegisterTask<OpenOrgPaymentPeriodPrepareDataTask>();

            this.Container.RegisterTask<ExaminationPrepareDataTask>();
            this.Container.RegisterTask<ExaminationExportTask>();
        }

        public void RegisterServiceProviders()
        {
            this.Container.RegisterTransient<IGisServiceProvider<BillsPortsTypeAsyncClient>, BillsAsyncServiceProvider>("BillsAsyncServiceProvider");
            this.Container.RegisterTransient<IGisServiceProvider<DeviceMeteringPortTypesAsyncClient>, DeviceMeteringAsyncServiceProvider>("DeviceMeteringAsyncServiceProvider");
            this.Container.RegisterTransient<IGisServiceProvider<HouseManagementPortsTypeAsyncClient>, HouseManagementServiceProvider>("HouseManagementServiceProvider");
            this.Container.RegisterTransient<IGisServiceProvider<InfrastructurePortsTypeAsyncClient>, InfrastructureServiceProvider>("InfrastructureServiceProvider");
            this.Container.RegisterTransient<IGisServiceProvider<InspectionPortsTypeAsyncClient>, InspectionServiceProvider>("InspectionServiceProvider");
            this.Container.RegisterTransient<IGisServiceProvider<NsiPortsTypeAsyncClient>, NsiServiceProvider>("NsiServiceProvider");
            this.Container.RegisterTransient<IGisServiceProvider<OrgRegistryAsync.RegOrgPortsTypeAsyncClient>, OrgRegistryServiceProvider>("OrgRegistryServiceProvider");
            this.Container.RegisterTransient<IGisServiceProvider<OrgRegistryCommonAsync.RegOrgPortsTypeAsyncClient>, OrgRegistryCommonServiceProvider>("OrgRegistryCommonServiceProvider");
            this.Container.RegisterTransient<IGisServiceProvider<PaymentPortsTypeAsyncClient>, PaymentServiceProvider>("PaymentServiceProvider");
            this.Container.RegisterTransient<IGisServiceProvider<ServicesPortsTypeAsyncClient>, ServicesServiceProvider>("ServicesServiceProvider");
        }

        public void RegisterExporters()
        {
            this.Container.RegisterSingleton<IDataExporter, ContractDataExporter>("ContractDataExporter");
            this.Container.RegisterSingleton<IDataExporter, AcknowledgmentExporter>("AcknowledgmentExporter");
            this.Container.RegisterSingleton<IDataExporter, DataProviderExporter>("DataProviderExporter");
            this.Container.RegisterSingleton<IDataExporter, CharterDataExporter>("CharterDataExporter");
            this.Container.RegisterSingleton<IDataExporter, HouseUODataExporter>("HouseUODataExporter");
            this.Container.RegisterSingleton<IDataExporter, HouseOMSDataExporter>("HouseOMSDataExporter");
            this.Container.RegisterSingleton<IDataExporter, AdditionalServicesExporter>("AdditionalServicesExporter");
            this.Container.RegisterSingleton<IDataExporter, MunicipalServicesExporter>("MunicipalServicesExporter");
            this.Container.RegisterSingleton<IDataExporter, HouseRSODataExporter>("HouseRSODataExporter");
            this.Container.RegisterSingleton<IDataExporter, OrganizationWorksExporter>("OrganizationWorksExporter");
            this.Container.RegisterSingleton<IDataExporter, OrgRegistryExporter>("OrgRegistryExporter");
            this.Container.RegisterSingleton<IDataExporter, MeteringDeviceDataExporter>("MeteringDeviceDataExporter");
            this.Container.RegisterSingleton<IDataExporter, SubsidiaryExporter>("SubsidiaryExporter");
            this.Container.RegisterSingleton<IDataExporter, AccountDataExporter>("AccountDataExporter");
            this.Container.RegisterSingleton<IDataExporter, PublicPropertyContractExporter>("PublicPropertyContractExporter");
            this.Container.RegisterSingleton<IDataExporter, VotingProtocolExporter>("VotingProtocolExporter");
            this.Container.RegisterSingleton<IDataExporter, InspectionPlanExporter>("InspectionPlanExporter");
            this.Container.RegisterSingleton<IDataExporter, NotificationDataExporter>("NotificationDataExporter");
            this.Container.RegisterSingleton<IDataExporter, WorkingListExporter>("WorkingListExporter");
            this.Container.RegisterSingleton<IDataExporter, RkiDataExporter>("RkiDataExporter");
            this.Container.RegisterSingleton<IDataExporter, WorkingPlanExporter>("WorkingPlanExporter");
            this.Container.RegisterSingleton<IDataExporter, RisCompletedWorkExporter>("RisCompletedWorkExporter");
            this.Container.RegisterSingleton<IDataExporter, MeteringDeviceValuesExporter>("MeteringDeviceValuesExporter");
            this.Container.RegisterSingleton<IDataExporter, NotificationsOfOrderExecutionExporter>("NotificationsOfOrderExecutionExporter");
            this.Container.RegisterSingleton<IDataExporter, SupplierNotificationsOfOrderExecutionExporter>("SupplierNotificationsOfOrderExecutionExporter");
            this.Container.RegisterSingleton<IDataExporter, SupplyResourceContractExporter>("SupplyResourceContractExporter");
            this.Container.RegisterSingleton<IDataExporter, PaymentDocumentDataExporter>("PaymentDocumentDataExporter");
            this.Container.RegisterSingleton<IDataExporter, NotificationsOfOrderExecutionCancellationExporter>("NotificationsOfOrderExecutionCancellationExporter");
            this.Container.RegisterSingleton<IDataExporter, OpenOrgPaymentPeriodExporter>("OpenOrgPaymentPeriodExporter");
            this.Container.RegisterSingleton<IDataExporter, ExaminationExporter>("ExaminationExporter");
        }

        public void RegisterExports()
        {
            this.Container.RegisterTransient<IDataExportService, ValidationResultExport>("ValidationResultExport");
            this.Container.RegisterTransient<IDataExportService, PackagesExport>("PackagesExport");
        }
    }
}