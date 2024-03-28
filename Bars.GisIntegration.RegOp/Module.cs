namespace Bars.GisIntegration.RegOp
{
    using B4.Windsor;
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.GisIntegration.Base.Entities.Bills;
    using Bars.GisIntegration.Base.Entities.HouseManagement;
    using Bars.GisIntegration.Base.Entities.Payment;
    using Bars.GisIntegration.RegOp.DataExtractors.AccountData;
    using Bars.GisIntegration.RegOp.DataExtractors.Bills;
    using Bars.GisIntegration.RegOp.DataExtractors.HouseData;
    using Bars.GisIntegration.RegOp.DataExtractors.Payment;
    using Bars.GisIntegration.RegOp.Dictionaries.HouseManagement;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities;
    using Base.DataExtractors;
    using Base.Extensions;
    using Castle.MicroKernel.Registration;
    using Dictionaries.Nsi;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            // ресурсы
            this.Container.RegisterTransient<IResourceManifest, ResourceManifest>();

            // настройки ограничений
            this.Container.Register(Component.For<IPermissionSource>().ImplementedBy<PermissionMap>());

            this.RegisterControllers();

            this.RegisterServices();

            this.RegisterDictionaries();

            this.RegisterDataExtractors();

            this.RegisterDataSelectors();
        }

        public void RegisterControllers()
        {
        }

        public void RegisterServices()
        {
        }

        public void RegisterDictionaries()
        {
            this.Container.RegisterDictionary<ServiceWorkPurposeDictionary>();
            this.Container.RegisterDictionary<PremisesCharacteristicDictionary>();
            this.Container.RegisterDictionary<HouseManagementTypeDictionary>();
            this.Container.RegisterDictionary<OlsonTZDictionary>();
            this.Container.RegisterDictionary<OverhaulFormingKindDictionary>();
            this.Container.RegisterDictionary<HouseStateDictionary>();
            this.Container.RegisterDictionary<OwnerDocumentTypeDictionary>();
            this.Container.RegisterDictionary<MunicipalServiceDictionary>(); 
            this.Container.RegisterDictionary<LiftTypeDictionary>();
        }

        public void RegisterDataSelectors()
        {
            this.Container.RegisterTransient<IDataSelector<RealityObject>, RisHouseDataExtractor>("HouseDataSelector");
            this.Container.RegisterTransient<IDataSelector<Entrance>, EntranceDataExtractor>("EntranceDataSelector");
            this.Container.RegisterTransient<IDataSelector<Room>, NonResidentialPremisesDataExtractor>("NonResidentialPremisesDataSelector");
            this.Container.RegisterTransient<IDataSelector<Room>, ResidentialPremisesDataExtractor>("ResidentialPremisesDataSelector");
            this.Container.RegisterTransient<IDataSelector<NotificationOfOrderExecution>, NotificationOfOrderExecutionCancellationDataSelector>("NotificationOfOrderExecutionCancellationDataSelector");
        }

        public void RegisterDataExtractors()
        {
            this.Container.RegisterTransient<IDataExtractor<RisEntrance>, EntranceDataExtractor>("EntranceDataExtractor");
            this.Container.RegisterTransient<IDataExtractor<NonResidentialPremises>, NonResidentialPremisesDataExtractor>("NonResidentialPremisesDataExtractor");
            this.Container.RegisterTransient<IDataExtractor<ResidentialPremises>, ResidentialPremisesDataExtractor>("ResidentialPremisesDataExtractor");
            this.Container.RegisterTransient<IDataExtractor<RisHouse>, RisHouseDataExtractor>("RisHouseDataExtractor");
            this.Container.RegisterTransient<IDataExtractor<RisAcknowledgment>, AcknowledgmentDataExtractor>("AcknowledgmentDataExtractor");
            this.Container.RegisterTransient<IDataExtractor<RisAccount>, AccountDataExtractor>("AccountDataExtractor");
            this.Container.RegisterTransient<IDataExtractor<RisCapitalRepairDebt>, RisCapitalRepairDebtDataExtractor>("RisCapitalRepairDebtDataExtractor");
            this.Container.RegisterTransient<IDataExtractor<RisCapitalRepairCharge>, RisCapitalRepairChargeDataExtractor>("RisCapitalRepairChargeDataExtractor");
            this.Container.RegisterTransient<IDataExtractor<RisPaymentDocument>, RisPaymentDocumentDataExtractor>("RisPaymentDocumentDataExtractor");
            this.Container.RegisterTransient<IDataExtractor<NotificationOfOrderExecution>, NotificationOfOrderExecutionExtractor>("NotificationOfOrderExecutionExtractor");
            this.Container.RegisterTransient<IDataExtractor<RisLift>, LiftDataExtractor>("LiftDataExtractor");
            this.Container.RegisterTransient<IDataExtractor<LivingRoom>, LivingRoomExtractor>("LivingRoomExtractor");
        }
    }
}
