namespace Bars.Gkh.Ris
{
    #region usings

    using B4;
    using B4.IoC;
    using B4.Windsor;
    using Bars.B4.Config;
    using Bars.B4.Modules.FileStorage;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities.Bills;
    using Bars.GisIntegration.Base.Entities.DeviceMetering;
    using Bars.GisIntegration.Base.Entities.External.Administration.System;
    using Bars.GisIntegration.Base.Entities.External.Housing.Notif;
    using Bars.GisIntegration.Base.Entities.External.Housing.OKI;
    using Bars.GisIntegration.Base.Entities.HouseManagement;
    using Bars.GisIntegration.Base.Entities.Infrastructure;
    using Bars.GisIntegration.Base.Entities.Nsi;
    using Bars.GisIntegration.Base.Entities.OrgRegistry;
    using Bars.GisIntegration.Base.Entities.Payment;
    using Bars.GisIntegration.Base.Entities.Services;
    using Bars.GisIntegration.Base.Extensions;
    using Bars.GisIntegration.Base.Tasks.PrepareData.DeviceMetering;
    using Bars.GisIntegration.Base.Tasks.PrepareData.DeviceMetering.PrepareAssistants;
    using Bars.GisIntegration.UI.Controllers;
    using Bars.GisIntegration.UI.Service;
    using Bars.Gkh.Gis.Entities.Kp50;
    using Bars.Gkh.Modules.Gkh1468.Entities;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Bars.Gkh.Repair.Entities;
    using Bars.Gkh.Ris.Dictionaries;
    using Bars.Gkh.Ris.Dictionaries.HouseManagement;
    using Bars.Gkh.Ris.Dictionaries.Nsi;
    using Bars.Gkh.Ris.Dictionaries.Services;
    using Bars.Gkh.Ris.DomainService.Impl;
    using Bars.Gkh.Ris.Extractors.Bills;
    using Bars.Gkh.Ris.Extractors.DeviceMetering;
    using Bars.Gkh.Ris.Extractors.HouseManagement.AccountData;
    using Bars.Gkh.Ris.Extractors.HouseManagement.HouseData;
    using Bars.Gkh.Ris.Extractors.HouseManagement.MeteringDeviceData;
    using Bars.Gkh.Ris.Extractors.HouseManagement.NotificationData;
    using Bars.Gkh.Ris.Extractors.HouseManagement.PublicPropertyContractData;
    using Bars.Gkh.Ris.Extractors.HouseManagement.SupplyResourceContract;
    using Bars.Gkh.Ris.Extractors.HouseManagement.VotingProtocolData;
    using Bars.Gkh.Ris.Extractors.Infrastructure;
    using Bars.Gkh.Ris.Extractors.Nsi;
    using Bars.Gkh.Ris.Extractors.OrgRegistry;
    using Bars.Gkh.Ris.Extractors.Payment;
    using Bars.Gkh.Ris.Extractors.Services;
    using Bars.GkhDi.Entities;
    using Castle.MicroKernel.Registration;

    using Gkh.Entities;
    using Gis.Entities.ManOrg;
    using Gis.Entities.ManOrg.Contract;
    using ViewModel.HouseManagement;

    #endregion

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
  

            // маршруты
            this.Container.RegisterTransient<IClientRouteMapRegistrar, ClientRouteMapRegistrar>();

            // ресурсы
            this.Container.RegisterTransient<IResourceManifest, ResourceManifest>();

            // настройки ограничений
            this.Container.Register(Component.For<IPermissionSource>().ImplementedBy<RisPermissionMap>());

            this.Container.RegisterTransient<INavigationProvider, NavigationProvider>();

            //this.Container.RegisterExecutionAction<GenerateContracts>(GenerateContracts.Code);

            this.RegisterControllers();

            this.RegisterViewModels();

            this.RegisterServices();

            this.RegisterDictionaries();

            this.RegisterDataExtractors();

            this.RegisterDataSelectors();
        }

        public void RegisterControllers()
        {
            this.Container.RegisterAltDataController<RisContract>();
        }

        public void RegisterViewModels()
        {
            this.Container.RegisterViewModel<RisContract, RisContractViewModel>();
        }

        public void RegisterServices()
        {
            this.Container.RegisterTransient<INsiService, NsiService>();

            this.Container.RegisterTransient<IMeteringDeviceBasePrepareAssistant<RisMeteringDeviceControlValue>, RisMeteringDeviceControlValueAssistant>();
            this.Container.RegisterTransient<IMeteringDeviceBasePrepareAssistant<RisMeteringDeviceCurrentValue>, RisMeteringDeviceCurrentValueAssistant>();
            this.Container.RegisterTransient<IMeteringDeviceBasePrepareAssistant<RisMeteringDeviceVerificationValue>, RisMeteringDeviceVerificationValueAssistant>();
        }

        public void RegisterDictionaries()
        {
            this.Container.RegisterDictionary<ChargeableMunicipalResourceDictionary>();
            this.Container.RegisterDictionary<FormingFondTypeDictionary>();
            this.Container.RegisterDictionary<HouseManagementTypeDictionary>();
            this.Container.RegisterDictionary<HouseStateDictionary>();
            this.Container.RegisterDictionary<MunicipalServiceDictionary>();
            this.Container.RegisterDictionary<OlsonTZDictionary>();
            this.Container.RegisterDictionary<OverhaulFormingKindDictionary>();
            this.Container.RegisterDictionary<OwnerDocumentTypeDictionary>();
            this.Container.RegisterDictionary<OwnersMeetingDecisionTypeDictionary>();
            this.Container.RegisterDictionary<PositionDictionary>();
            this.Container.RegisterDictionary<PremisesCharacteristicDictionary>();
            this.Container.RegisterDictionary<ProjectTypeDictionary>();
            this.Container.RegisterDictionary<PurposeDictionary>();
            this.Container.RegisterDictionary<RoomsNumDictionary>();
            this.Container.RegisterDictionary<StopReasonDictionary>();
            this.Container.RegisterDictionary<SupplyResContractBaseDictionary>();
            this.Container.RegisterDictionary<UnitMeasureDictionary>();
            this.Container.RegisterDictionary<ContentRepairMkdWorkDictionary>();
            this.Container.RegisterDictionary<ServiceWorkPurposeDictionary>();
            this.Container.RegisterDictionary<ServiceTypeDictionary>();
            this.Container.RegisterDictionary<KindCheckDictionary>();
            this.Container.RegisterDictionary<MunicipalResourceDictionary>();
            this.Container.RegisterDictionary<TypeContractManOrgDictionary>();
            this.Container.RegisterDictionary<LiftTypeDictionary>();
        }

        public void RegisterDataSelectors()
        {
            this.Container.RegisterTransient<IDataSelector<RealityObject>, RisHouseDataExtractor>("HouseDataSelector");
            this.Container.RegisterTransient<IDataSelector<ManOrgBilWorkService>, OrganizationWorksDataExtractor>("OrganizationWorksDataSelector");
            
            this.Container.RegisterTransient<IDataSelector<InfoAboutUseCommonFacilities>, PublicPropertyContractExtractor>("PublicPropertyContractSelector");
            this.Container.RegisterTransient<IDataSelector<NotifAddress>, NotificationAddresseeExtractor>("NotificationAddresseeSelector");
            this.Container.RegisterTransient<IDataSelector<RepairObject>, WorkListExtractor>("WorkListSelector");
            this.Container.RegisterTransient<IDataSelector<NotificationOfOrderExecution>, NotificationOfOrderExecutionCancellationDataSelector>(nameof(NotificationOfOrderExecutionCancellationDataSelector));
            this.Container.RegisterTransient<IDataSelector<BilServiceDictionary>, AdditionalServicesDataExtractor>("AdditionalServicesSelector");

            //пока не используются
            this.Container.RegisterTransient<IDataSelector<Entrance>, EntranceDataExtractor>("EntranceDataSelector");
            this.Container.RegisterTransient<IDataSelector<Room>, NonResidentialPremisesDataExtractor>("NonResidentialPremisesDataSelector");
            this.Container.RegisterTransient<IDataSelector<Room>, ResidentialPremisesDataExtractor>("ResidentialPremisesDataSelector");
            this.Container.RegisterTransient<IDataSelector<Contragent>, SubsidiaryDataExtractor>("SubsidiaryDataSelector");
            this.Container.RegisterTransient<IDataSelector<FileInfo>, PublicPropertyContractAttachmentExtractor>("PublicPropertyContractAttachmentSelector");
            this.Container.RegisterTransient<IDataSelector<FileInfo>, TrustDocAttachmentExtractor>("TrustDocAttachmentSelector");
            this.Container.RegisterTransient<IDataSelector<PropertyOwnerProtocols>, VotingProtocolExtractor>("VotingProtocolSelector");
            this.Container.RegisterTransient<IDataSelector<BasePropertyOwnerDecision>, DecisionExtractor>("DecisionSelector");
            this.Container.RegisterTransient<IDataSelector<FileInfo>, VotingProtocolAttachmentExtractor>("VotingProtocolAttachmentSelector");
            this.Container.RegisterTransient<IDataSelector<Notif>, NotificationDataExtractor>("NotificationDataSelector");
            this.Container.RegisterTransient<IDataSelector<NotifDoc>, NotificationAttachmentExtractor>("NotificationAttachmentLinkSelector");
            this.Container.RegisterTransient<IDataSelector<RepairWork>, WorkListItemExtractor>("WorkListItemSelector");
            this.Container.RegisterTransient<IDataSelector<OkiObject>, RkiItemDataExtractor>("RkiItemDataSelector");
            this.Container.RegisterTransient<IDataSelector<RepairObject>, WorkingPlanExtractor>("WorkingPlanSelector");
            this.Container.RegisterTransient<IDataSelector<RepairWork>, WorkPlanItemExtractor>("WorkPlanItemSelector");
            this.Container.RegisterTransient<IDataSelector<PublicServiceOrgContract>, SupplyResourceContractExtractor>("SupplyResourceContractSelector");
            this.Container.RegisterTransient<IDataSelector<PerformedRepairWorkAct>, RisCompletedWorkExtractor>("RisCompletedWorkSelector");
            this.Container.RegisterTransient<IDataSelector<PublicServiceOrgContractService>, SupResContractServiceResourceExtractor>("SupResContractServiceResourceSelector");
            this.Container.RegisterTransient<IDataSelector<PublicOrgServiceQualityLevel>, SupResContractSubjectOtherQualityExtractor>("SupResContractSubjectOtherQualitySelector");
            this.Container.RegisterTransient<IDataSelector<PublicServiceOrgTemperatureInfo>, SupResContractTemperatureChartExtractor>("SupResContractTemperatureChartSelector");
            this.Container.RegisterTransient<IDataSelector<PublicServiceOrgContractService>, SupResContractSubjectExtractor>("SupResContractSubjectSelector");
            this.Container.RegisterTransient<IDataSelector<FileInfo>, SupResContractAttachmentExtractor>("SupResContractAttachmentSelector");
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
            this.Container.RegisterTransient<IDataExtractor<RisSubsidiary>, SubsidiaryDataExtractor>("SubsidiaryDataExtractor");
            this.Container.RegisterTransient<IDataExtractor<RisPublicPropertyContract>, PublicPropertyContractExtractor>("PublicPropertyContractExtractor");
            this.Container.RegisterTransient<IDataExtractor<RisContractAttachment>, PublicPropertyContractAttachmentExtractor>("PublicPropertyContractAttachmentExtractor");
            this.Container.RegisterTransient<IDataExtractor<RisTrustDocAttachment>, TrustDocAttachmentExtractor>("TrustDocAttachmentExtractor");
            this.Container.RegisterTransient<IDataExtractor<RisVotingProtocol>, VotingProtocolExtractor>("VotingProtocolExtractor");
            this.Container.RegisterTransient<IDataExtractor<RisDecisionList>, DecisionExtractor>("DecisionExtractor");
            this.Container.RegisterTransient<IDataExtractor<RisVotingProtocolAttachment>, VotingProtocolAttachmentExtractor>("VotingProtocolAttachmentExtractor");
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
            this.Container.RegisterTransient<IDataExtractor<RisLift>, LiftDataExtractor>("LiftDataExtractor");
            this.Container.RegisterTransient<IDataExtractor<InsuranceProduct>, InsuranceProductExtractor>("InsuranceProductExtractor");
        }
    }
}
