namespace Bars.GisIntegration.Smev
{
    using Bars.B4;
    using Bars.B4.Events;
    using Bars.B4.IoC;
    using Bars.B4.Windsor;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Exporters;
    using Bars.GisIntegration.Base.Extensions;
    using Bars.GisIntegration.Smev.ConfigSections;
    using Bars.GisIntegration.Smev.Controllers;
    using Bars.GisIntegration.Smev.DomainService;
    using Bars.GisIntegration.Smev.DomainService.Impl;
    using Bars.GisIntegration.Smev.Entity;
    using Bars.GisIntegration.Smev.EventHandlers;
    using Bars.GisIntegration.Smev.Exporters;
    using Bars.GisIntegration.Smev.Extractors;
    using Bars.GisIntegration.Smev.SmevExchangeService;
    using Bars.GisIntegration.Smev.Tasks.PrepareData;
    using Bars.GisIntegration.Smev.Tasks.SendData;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Gis.RabbitMQ;
    using Bars.Gkh.Quartz.Scheduler;
    using Bars.Gkh.Smev3;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Decision;
    using Smev3MessageHandler = MessageHandlers.Smev3MessageHandler;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            this.Container.ReplaceTransient<IDomainServiceInterceptor<RisTask>, Base.Interceptors.RisTaskInterceptor, Interceptors.RisTaskInterceptor>();

            this.Container.RegisterTransient<IResourceManifest, ResourceManifest>();
            this.Container.RegisterGkhConfig<SmevIntegrationConfig>();
            this.Container.RegisterGkhConfig<ErknmIntegrationConfig>();
            this.RegisterControllers();
            this.RegisterServices();
            this.RegisterServiceProviders();
            this.RegisterDataExporters();
            this.RegisterTasks();
            this.RegisterDictionaries();
            this.RegisterDataExtractors();
            this.RegisterDataSelectors();
            this.RegisterMessageHandlers();
            this.SubscribeOnEvents();
        }

        private void RegisterServiceProviders()
        {
            this.Container.RegisterGisServiceProvider<ServiceConsumerClient, SmevServiceProvider>(nameof(SmevServiceProvider));
        }
        
        private void RegisterDataExporters()
        {
            this.Container.RegisterTransient<IDataExporter, DisposalExporter>(nameof(DisposalExporter));
            this.Container.RegisterTransient<IDataExporter, ProsecutorOfficesExport>(nameof(ProsecutorOfficesExport));
            this.Container.RegisterTransient<IDataExporter, DisposalCorrectionExporter>(nameof(DisposalCorrectionExporter));
            this.Container.RegisterTransient<IDataExporter, KnmExporter>(nameof(KnmExporter));
            this.Container.RegisterTransient<IDataExporter, KnmCorrectionExporter>(nameof(KnmCorrectionExporter));
        }

        private void RegisterTasks()
        {
            this.Container.RegisterTask<DisposalPrepareDataTask>();
            this.Container.RegisterTask<DisposalSendDataTask>();

            this.Container.RegisterTask<KnmPrepareDataTask>();
            this.Container.RegisterTask<KnmCorrectionPrepareDataTask>();
            this.Container.RegisterTask<KnmSendDataTask>();

            this.Container.RegisterTask<ProsecutorOfficePrepareDataTask>();
            this.Container.RegisterTask<ProsecutorOfficeSendDataTask>();
        }

        private void RegisterControllers()
        {
            this.Container.RegisterController<BaseIntegrationController>();
            this.Container.RegisterController<ErknmIntegrationController>();
            this.Container.RegisterController<ErpIntegrationController>();
            this.Container.RegisterAltDataController<FileMetadata>();
        }

        private void RegisterServices()
        {
            this.Container.RegisterTransient<IBaseIntegrationService, BaseIntegrationService>();
            this.Container.RegisterTransient<IErknmIntegrationService, ErknmIntegrationService>();
            this.Container.RegisterTransient<IErpIntegrationService, ErpIntegrationService>();
            this.Container.RegisterTransient<IAttachmentManager, AttachmentManager>();
        }

        private void RegisterDictionaries()
        {

        }

        private void RegisterDataSelectors()
        {
            
        }

        private void RegisterMessageHandlers()
        {
            this.Container.RegisterTransient<IMessageHandler<Smev3Response>, Smev3MessageHandler>();
        }

        private void SubscribeOnEvents()
        {
            this.Container.Resolve<IEventAggregator>().GetEvent<AppStartEvent>().Subscribe<QueueSubscriber>();
        }

        public void RegisterDataExtractors()
        {
            this.Container.RegisterTransient<IDataExtractor<TatarstanDisposal>, DisposalDataExtractor>("DisposalDataExtractor");
            this.Container.RegisterTransient<IDataExtractor<ProsecutorOfficeDict>, ProsecutorOfficesDataExtractor>("ProsecutorOfficiesDataExtractor");
            this.Container.RegisterTransient<IDataExtractor<Decision>, DecisionDataExtractor>("DecisionDataExtractor");
        }
    }
}