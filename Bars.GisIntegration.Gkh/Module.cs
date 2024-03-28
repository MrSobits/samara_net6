namespace Bars.GisIntegration.Gkh
{
    using Bars.B4;
    using Bars.B4.Config;
    using Bars.B4.Events;
    using Bars.B4.IoC;
    using Bars.B4.Windsor;
    using Bars.GisIntegration.Base.B4Events.Payloads;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Service;
    using Bars.GisIntegration.Base.Tasks.SendData.OrgRegistryCommon;
    using Bars.GisIntegration.Gkh.ConfigSections;
    using Bars.GisIntegration.Gkh.Controllers;
    using Bars.GisIntegration.Gkh.DataExtractors.OrgRegistryCommon;
    using Bars.GisIntegration.Gkh.ExecutionAction;
    using Bars.GisIntegration.Gkh.Security;
    using Bars.GisIntegration.Gkh.Service;
    using Bars.GisIntegration.Gkh.Service.Impl;
    using Bars.GisIntegration.UI.Service;
    using B4;
    using B4.Events;
    using B4.IoC;
    using B4.Modules.FileStorage;

    using Bars.GisIntegration.Base.Extensions;
    using Bars.GisIntegration.Gkh.DataExtractors.HouseManagement.CharterData;
    using Bars.GisIntegration.Gkh.Dictionaries.HouseManagement;

    using Base.B4Events.Payloads;
    using Base.DataExtractors;
    using Base.Entities.HouseManagement;
    using Base.Service;
    using Base.Tasks.SendData.OrgRegistryCommon;
    using ConfigSections;
    using DataExtractors.HouseManagement.ContractData;
    using DataExtractors.OrgRegistryCommon;
    using ExecutionAction;
    using Service.Impl;
    using UI.Service;

    using Bars.B4.Config;
    using Bars.B4.Windsor;
    using Bars.GisIntegration.Gkh.Controllers;
    using Bars.GisIntegration.Gkh.Security;
    using Bars.Gkh.B4Events;
    using Bars.Gkh.Entities;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.Ris.Extractors.HouseManagement.ContractData;
    using Bars.Gkh.Utils;

    using Castle.MicroKernel.Registration;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            Settings.Init(this.Container.Resolve<IConfigProvider>());

            // настройки ограничений
            this.Container.Register(Component.For<IPermissionSource>().ImplementedBy<RisPermissionMap>());

            // маршруты
            this.Container.RegisterTransient<IClientRouteMapRegistrar, ClientRouteMapRegistrar>();

            // навигация
            this.Container.RegisterTransient<INavigationProvider, NavigationProvider>();

            this.Container.RegisterGkhConfig<GisIntegrationConfig>();

            Component.For<Base.GisIntegrationConfig>().UsingFactoryMethod(
                () =>
                    {
                        var gkhConfig = this.Container.GetGkhConfig<GisIntegrationConfig>();
                        return new Base.GisIntegrationConfig
                        {
                            Login = gkhConfig.Login,
                            Password = gkhConfig.Password,
                            SingXml = gkhConfig.SingXml,
                            UseLoginCredentials = gkhConfig.UseLoginCredentials
                        };
                    }).LifestyleTransient().RegisterIn(this.Container);

            this.Container.RegisterExecutionAction<GisIntegrationConfigsCreateAction>();

            this.ForwardEvents();

            this.RegisterControllers();

            this.RegisterServices();

            this.RegisterDataSelectors();

            this.RegisterDataExtractors();

            this.RegisterDictionaries();
        }

        public void RegisterControllers()
        {
            this.Container.RegisterController<RisSettingsController>();
        }

        public void RegisterServices()
        {
            this.Container.RegisterTransient<IDataSupplierProvider, DataSupplierProvider>();

            this.Container.RegisterTransient<IOrgRegistryService, OrgRegistryService>();
            this.Container.RegisterTransient<ICapitalRepairService, CapitalRepairService>();
            this.Container.RegisterTransient<IHouseManagementService, HouseManagementService>();
            this.Container.RegisterTransient<IServicesService, ServicesService>();
            this.Container.RegisterTransient<IInfrastructureService, InfrastructureService>();
            this.Container.RegisterTransient<IPaymentService, PaymentService>();
            this.Container.RegisterTransient<IBillsService, BillsService>();

            this.Container.Register(Component.For<CrossAuthentification>().LifestyleTransient());
            this.Container.Register(Component.For<RisManager>().LifestyleTransient());
        }

        public void RegisterDataSelectors()
        {
            this.Container.RegisterTransient<IDataSelector<ContragentProxy>, ContragentSelector>("ContragentSelector");
            this.Container.RegisterTransient<IDataSelector<ManOrgBaseContract>, ContractDataExtractor>("ContractDataSelector");
            this.Container.RegisterTransient<IDataSelector<ManOrgBaseContract>, ContractObjectDataExtractor>("ContractObjectDataSelector");
            this.Container.RegisterTransient<IDataSelector<FileInfo>, ContractAttachmentExtractor>("ContractAttachmentSelector");
            this.Container.RegisterTransient<IDataSelector<FileInfo>, OwnersProtocolMeetingOwnerExtractor>("OwnersProtocolMeetingOwnerSelector");
            this.Container.RegisterTransient<IDataSelector<FileInfo>, OwnersProtocolOkExtractor>("OwnersProtocolOkSelector");
            this.Container.RegisterTransient<IDataSelector<FileInfo>, JskTsjProtocolMeetingOwnerExtractor>("JskTsjProtocolMeetingOwnerSelector");

            this.Container.RegisterTransient<IDataSelector<ManOrgJskTsjContract>, CharterExtractor>("CharterSelector");
        }

        public void RegisterDataExtractors()
        {
            this.Container.RegisterTransient<IDataExtractor<RisContract>, ContractDataExtractor>("ContractDataExtractor");
            this.Container.RegisterTransient<IDataExtractor<ContractObject>, ContractObjectDataExtractor>("ContractObjectDataExtractor");
            this.Container.RegisterTransient<IDataExtractor<RisContractAttachment>, ContractAttachmentExtractor>("ContractAttachmentExtractor");
            this.Container.RegisterTransient<IDataExtractor<ProtocolMeetingOwner>, OwnersProtocolMeetingOwnerExtractor>("OwnersProtocolMeetingOwnerExtractor");
            this.Container.RegisterTransient<IDataExtractor<RisProtocolOk>, OwnersProtocolOkExtractor>("OwnersProtocolOkExtractor");
            this.Container.RegisterTransient<IDataExtractor<ProtocolMeetingOwner>, JskTsjProtocolMeetingOwnerExtractor>("JskTsjProtocolMeetingOwnerExtractor");

            this.Container.RegisterTransient<IDataExtractor<Charter>, CharterExtractor>("CharterExtractor");
        }

        public void RegisterDictionaries()
        {
            this.Container.RegisterDictionary<ContractBaseDictionary>();
        }

        public void ForwardEvents()
        {
            var eventAggregator = this.Container.Resolve<IEventAggregator>();

            eventAggregator.GetEvent<SessionStartEvent>().Subscribe(
                gkhEvent =>
                    {
                        var gisEvent = eventAggregator.GetEvent<Base.B4Events.SessionStartEvent>();
                        gisEvent.Publish(new SessionStartEventArgs(gkhEvent.SessionId));
                    });

            eventAggregator.GetEvent<SessionEndEvent>().Subscribe(
                gkhEvent =>
                {
                    var gisEvent = eventAggregator.GetEvent<Base.B4Events.SessionEndEvent>();
                    gisEvent.Publish(new SessionEndEventArgs(gkhEvent.SessionId));
                });
        }
    }
}