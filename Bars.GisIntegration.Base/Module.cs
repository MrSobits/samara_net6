namespace Bars.GisIntegration.Base
{
    using System;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Windsor;
    using Bars.GisIntegration.Base.BillsAsync;
    using Bars.GisIntegration.Base.CapitalRepair;
    using Bars.GisIntegration.Base.Controllers;
    using Bars.GisIntegration.Base.DeviceMeteringAsync;
    using Bars.GisIntegration.Base.Dictionaries;
    using Bars.GisIntegration.Base.Dictionaries.Impl;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Entities.GisRole;
    using Bars.GisIntegration.Base.Export;
    using Bars.GisIntegration.Base.Exporters;
    using Bars.GisIntegration.Base.Exporters.Bills;
    using Bars.GisIntegration.Base.Exporters.CapitalRepair;
    using Bars.GisIntegration.Base.Exporters.DeviceMetering;
    using Bars.GisIntegration.Base.Exporters.HouseManagement;
    using Bars.GisIntegration.Base.Exporters.Infrastructure;
    using Bars.GisIntegration.Base.Exporters.Inspection;
    using Bars.GisIntegration.Base.Exporters.Nsi;
    using Bars.GisIntegration.Base.Exporters.OrgRegistry;
    using Bars.GisIntegration.Base.Exporters.OrgRegistryCommon;
    using Bars.GisIntegration.Base.Exporters.Payment;
    using Bars.GisIntegration.Base.Exporters.Services;
    using Bars.GisIntegration.Base.Extensions;
    using Bars.GisIntegration.Base.File;
    using Bars.GisIntegration.Base.File.Impl;
    using Bars.GisIntegration.Base.GisServiceProvider;
    using Bars.GisIntegration.Base.GisServiceProvider.Bills;
    using Bars.GisIntegration.Base.GisServiceProvider.CapitalRepair;
    using Bars.GisIntegration.Base.GisServiceProvider.DeviceMetering;
    using Bars.GisIntegration.Base.GisServiceProvider.File;
    using Bars.GisIntegration.Base.GisServiceProvider.HouseManagement;
    using Bars.GisIntegration.Base.GisServiceProvider.Infrastructure;
    using Bars.GisIntegration.Base.GisServiceProvider.Inspection;
    using Bars.GisIntegration.Base.GisServiceProvider.Nsi;
    using Bars.GisIntegration.Base.GisServiceProvider.NsiCommon;
    using Bars.GisIntegration.Base.GisServiceProvider.OrgRegistry;
    using Bars.GisIntegration.Base.GisServiceProvider.OrgRegistryCommon;
    using Bars.GisIntegration.Base.GisServiceProvider.Payment;
    using Bars.GisIntegration.Base.GisServiceProvider.Services;
    using Bars.GisIntegration.Base.HouseManagementAsync;
    using Bars.GisIntegration.Base.InfrastructureAsync;
    using Bars.GisIntegration.Base.InspectionAsync;
    using Bars.GisIntegration.Base.Interceptors;
    using Bars.GisIntegration.Base.NsiAsync;
    using Bars.GisIntegration.Base.NsiCommon;
    using Bars.GisIntegration.Base.Package;
    using Bars.GisIntegration.Base.Package.Impl;
    using Bars.GisIntegration.Base.PaymentAsync;
    using Bars.GisIntegration.Base.Service;
    using Bars.GisIntegration.Base.Service.Impl;
    using Bars.GisIntegration.Base.ServicesAsync;
    using Bars.GisIntegration.Base.Tasks.PrepareData.Bills;
    using Bars.GisIntegration.Base.Tasks.PrepareData.CapitalRepair;
    using Bars.GisIntegration.Base.Tasks.PrepareData.DeviceMetering;
    using Bars.GisIntegration.Base.Tasks.PrepareData.HouseManagement;
    using Bars.GisIntegration.Base.Tasks.PrepareData.Infrastructure;
    using Bars.GisIntegration.Base.Tasks.PrepareData.Inspection;
    using Bars.GisIntegration.Base.Tasks.PrepareData.Nsi;
    using Bars.GisIntegration.Base.Tasks.PrepareData.OrgRegistry;
    using Bars.GisIntegration.Base.Tasks.PrepareData.OrgRegistryCommon;
    using Bars.GisIntegration.Base.Tasks.PrepareData.Payment;
    using Bars.GisIntegration.Base.Tasks.PrepareData.Services;
    using Bars.GisIntegration.Base.Tasks.SendData.Bills;
    using Bars.GisIntegration.Base.Tasks.SendData.CapitalRepair;
    using Bars.GisIntegration.Base.Tasks.SendData.DeviceMetering;
    using Bars.GisIntegration.Base.Tasks.SendData.HouseManagement;
    using Bars.GisIntegration.Base.Tasks.SendData.Infrastructure;
    using Bars.GisIntegration.Base.Tasks.SendData.Inspection;
    using Bars.GisIntegration.Base.Tasks.SendData.Nsi;
    using Bars.GisIntegration.Base.Tasks.SendData.OrgRegistry;
    using Bars.GisIntegration.Base.Tasks.SendData.OrgRegistryCommon;
    using Bars.GisIntegration.Base.Tasks.SendData.Payment;
    using Bars.GisIntegration.Base.Tasks.SendData.Services;
    using Bars.Gkh.Quartz.Scheduler;

    using Castle.MicroKernel.Registration;

    public partial class Module : AssemblyDefinedModule
    {        
        public override void Install()
        {
            this.Container.RegisterTransient<IResourceManifest, ResourceManifest>();

            this.RegisterControllers();

            this.RegisterInterceptors();

            this.RegisterServices();

            this.RegisterTasks();

            this.RegisterServiceProviders();

            this.RegisterExporters();

            this.RegisterExports();
        }

        public void RegisterControllers()
        {
            this.Container.RegisterController<ServiceSettingsController>();
            this.Container.RegisterController<ContextSettingsController>();

            this.Container.RegisterAltDataController<GisRole>();
            this.Container.RegisterAltDataController<RisContragent>();
            this.Container.RegisterAltDataController<GisOperator>();
            this.Container.RegisterAltDataController<FrguFunction>();
        }

        public void RegisterServices()
        {
            this.Container.RegisterTransient<IAttachmentService, AttachmentService>();
            this.Container.RegisterSingleton<ITaskManager, TaskManager>();
           
            this.Container.RegisterTransient<IDictionaryManager, DictionaryManager>();

            this.Container.RegisterTransient<IGisSettingsService, GisSettingsService>();

            this.Container.RegisterSingleton<IPackageManager<TempPackageInfo, Guid>, TempPackageManager<TempPackageInfo>>();
            this.Container.RegisterTransient<IPackageManager<RisPackage, long>, StorablePackageManager<RisPackage>>();

            //this.Container.RegisterSingleton<IFileUploader, SimpleFileUploader>("SimpleFileUploader");
            //this.Container.RegisterSingleton<IFileUploader, FileUploader>("FileUploader");
        }

        public void RegisterInterceptors()
        {
            this.Container.RegisterDomainInterceptor<RisPackage, UserEntityInterceptor<RisPackage>>();
            this.Container.RegisterDomainInterceptor<RisTask, RisTaskInterceptor>();
            this.Container.RegisterDomainInterceptor<GisRole, GisRoleInterceptor>();
            this.Container.RegisterDomainInterceptor<GisOperator, GisOperatorInterceptor>();
        }

        public void RegisterTasks()
        {
            this.Container.RegisterTask<ExportCharterDataTask>();
            this.Container.RegisterTask<ExportContractDataTask>();
            this.Container.RegisterTask<HouseOMSExportDataTask>();
            this.Container.RegisterTask<HouseUOExportDataTask>();
            this.Container.RegisterTask<ExportAdditionalServicesTask>();
            this.Container.RegisterTask<ExportMunicipalServicesTask>();
            this.Container.RegisterTask<ExportOrganizationWorksTask>();
            this.Container.RegisterTask<ExportNotificationDataTask>();

            this.Container.RegisterTask<HouseOMSPrepareDataTask>();
            this.Container.RegisterTask<HouseUOPrepareDataTask>();
            this.Container.RegisterTask<ContractPrepareDataTask>();
            this.Container.RegisterTask<CharterPrepareDataTask>();
            this.Container.RegisterTask<NotificationPrepareDataTask>();

            this.Container.RegisterTask<AdditionalServicesPrepareDataTask>();
            this.Container.RegisterTask<MunicipalServicesPrepareDataTask>();
            this.Container.RegisterTask<OrganizationWorksPrepareDataTask>();

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

            this.Container.RegisterTask<MeteringDeviceValuesExportTask>();
            this.Container.RegisterTask<MeteringDeviceValuesPrepareDataTask>();

            this.Container.RegisterTask<ExportRkiDataTask>();
            this.Container.RegisterTask<RkiPrepareDataTask>();

            this.Container.RegisterTask<ExportSupplyResourceContractTask>();
            this.Container.RegisterTask<SupplyResourceContractPrepareDataTask>();
            this.Container.RegisterTask<CompletedWorkExportTask>();
            this.Container.RegisterTask<CompletedWorkPrepareDataTask>();

            this.Container.RegisterTask<ExportOrgRegistryTask>();
            this.Container.RegisterTask<OrgRegistryPrepareDataTask>();

            this.Container.RegisterTask<PlanPrepareDataTask>();
            this.Container.RegisterTask<PlanImportDataTask>();
            this.Container.RegisterTask<CapitalRepairContractsPrepareDataTask>();
            this.Container.RegisterTask<CapitalRepairContractsExportDataTask>();

            this.Container.RegisterTask<ExportAcknowledgmentTask>();
            this.Container.RegisterTask<HouseRSOExportDataTask>();
            this.Container.RegisterTask<HouseRegOperatorExportDataTask>();
            this.Container.RegisterTask<AcknowledgmentPrepareDataTask>();
            this.Container.RegisterTask<HouseRSOPrepareDataTask>();
            this.Container.RegisterTask<HouseRegOperatorPrepareDataTask>();
            this.Container.RegisterTask<ExportAccountDataTask>();
            
            this.Container.RegisterTask<ExportPaymentDocumentTask>();
            this.Container.RegisterTask<PaymentDocumentPrepareDataTask>();
            this.Container.RegisterTask<NotificationsOfOrderExecutionPrepareDataTask>();
            this.Container.RegisterTask<ExportNotificationsOfOrderExecutionTask>();
            this.Container.RegisterTask<AccountPrepareDataTask>();
            this.Container.RegisterTask<NotificationsOfOrderExecutionCancellationPrepareDataTask>();
            this.Container.RegisterTask<ExportNotificationsOfOrderExecutionCancellationTask>();
            this.Container.RegisterTask<ExportSupplierNotificationsOfOrderExecutionTask>();
            this.Container.RegisterTask<SupplierNotificationsOfOrderExecutionPrepareDataTask>();

            this.Container.RegisterTask<ExportInspectionPlanTask>();
            this.Container.RegisterTask<InspectionPlanPrepareDataTask>();

            this.Container.RegisterTask<ExaminationPrepareDataTask>();
            this.Container.RegisterTask<ExaminationExportTask>();

            this.Container.RegisterTask<InsuranceProductPrepareDataTask>();
            this.Container.RegisterTask<ExportInsuranceProductTask>();
        }

        public void RegisterServiceProviders()
        {
            this.Container.RegisterGisServiceProvider<DeviceMeteringPortTypesAsyncClient, DeviceMeteringAsyncServiceProvider>("DeviceMeteringAsyncServiceProvider");
            this.Container.RegisterGisServiceProvider<InfrastructurePortsTypeAsyncClient, InfrastructureServiceProvider>("InfrastructureServiceProvider");
            this.Container.RegisterGisServiceProvider<NsiPortsTypeAsyncClient, NsiServiceProvider>("NsiServiceProvider");
            this.Container.RegisterGisServiceProvider<OrgRegistryAsync.RegOrgPortsTypeAsyncClient, OrgRegistryServiceProvider>("OrgRegistryServiceProvider");
            this.Container.RegisterGisServiceProvider<ServicesPortsTypeAsyncClient, ServicesServiceProvider>("ServicesServiceProvider");
            this.Container.RegisterGisServiceProvider<OrgRegistryCommonAsync.RegOrgPortsTypeAsyncClient, OrgRegistryCommonServiceProvider>("OrgRegistryCommonServiceProvider");
            this.Container.RegisterGisServiceProvider<NsiPortsTypeClient, NsiCommonServiceProvider>("NsiCommonServiceProvider");
            this.Container.RegisterGisServiceProvider<CapitalRepairAsyncPortClient, CapitalRepairAsyncServiceProvider>("CapitalRepairAsyncServiceProvider");
            this.Container.RegisterGisServiceProvider<HouseManagementPortsTypeAsyncClient, HouseManagementServiceProvider>("HouseManagementServiceProvider");
            this.Container.RegisterGisServiceProvider<BillsPortsTypeAsyncClient, BillsAsyncServiceProvider>("BillsAsyncServiceProvider");
            this.Container.RegisterGisServiceProvider<PaymentPortsTypeAsyncClient, PaymentServiceProvider>("PaymentServiceProvider");
            this.Container.RegisterGisServiceProvider<InspectionPortsTypeAsyncClient, InspectionServiceProvider>("InspectionServiceProvider");

            this.Container.RegisterTransient<IGisServiceProvider, FileServiceProvider>("FileServiceProvider");
        }

        public void RegisterExporters()
        {
            this.Container.RegisterSingleton<IDataExporter, ContractDataExporter>("ContractDataExporter");
            this.Container.RegisterSingleton<IDataExporter, CharterDataExporter>("CharterDataExporter");
            this.Container.RegisterSingleton<IDataExporter, HouseUODataExporter>("HouseUODataExporter");
            this.Container.RegisterSingleton<IDataExporter, HouseOMSDataExporter>("HouseOMSDataExporter");
            this.Container.RegisterSingleton<IDataExporter, AdditionalServicesExporter>("AdditionalServicesExporter");
            this.Container.RegisterSingleton<IDataExporter, MunicipalServicesExporter>("MunicipalServicesExporter");
            this.Container.RegisterSingleton<IDataExporter, OrganizationWorksExporter>("OrganizationWorksExporter");
            this.Container.RegisterSingleton<IDataExporter, MeteringDeviceDataExporter>("MeteringDeviceDataExporter");
            this.Container.RegisterSingleton<IDataExporter, SubsidiaryExporter>("SubsidiaryExporter");
            this.Container.RegisterSingleton<IDataExporter, PublicPropertyContractExporter>("PublicPropertyContractExporter");
            this.Container.RegisterSingleton<IDataExporter, VotingProtocolExporter>("VotingProtocolExporter");
            this.Container.RegisterSingleton<IDataExporter, NotificationDataExporter>("NotificationDataExporter");
            this.Container.RegisterSingleton<IDataExporter, WorkingListExporter>("WorkingListExporter");
            this.Container.RegisterSingleton<IDataExporter, RkiDataExporter>("RkiDataExporter");
            this.Container.RegisterSingleton<IDataExporter, WorkingPlanExporter>("WorkingPlanExporter");
            this.Container.RegisterSingleton<IDataExporter, RisCompletedWorkExporter>("RisCompletedWorkExporter");
            this.Container.RegisterSingleton<IDataExporter, MeteringDeviceValuesExporter>("MeteringDeviceValuesExporter");
            this.Container.RegisterSingleton<IDataExporter, SupplyResourceContractExporter>("SupplyResourceContractExporter");
            this.Container.RegisterSingleton<IDataExporter, OrgRegistryExporter>("OrgRegistryExporter");
            this.Container.RegisterSingleton<IDataExporter, PlanImporter>("PlanImporter");
            this.Container.RegisterSingleton<IDataExporter, CapitalRepairContractsDataExporter>("CapitalRepairContractsDataExporter");
            this.Container.RegisterSingleton<IDataExporter, HouseRSODataExporter>("HouseRSODataExporter");
            this.Container.RegisterSingleton<IDataExporter, HouseRegOperatorDataExporter>("HouseRegOperatorDataExporter");
            this.Container.RegisterSingleton<IDataExporter, AccountDataExporter>("AccountDataExporter");
            this.Container.RegisterSingleton<IDataExporter, AcknowledgmentExporter>("AcknowledgmentExporter");
            this.Container.RegisterSingleton<IDataExporter, PaymentDocumentDataExporter>("PaymentDocumentDataExporter");
            this.Container.RegisterSingleton<IDataExporter, NotificationsOfOrderExecutionExporter>("NotificationsOfOrderExecutionExporter");
            this.Container.RegisterSingleton<IDataExporter, SupplierNotificationsOfOrderExecutionExporter>("SupplierNotificationsOfOrderExecutionExporter");
            this.Container.RegisterSingleton<IDataExporter, NotificationsOfOrderExecutionCancellationExporter>("NotificationsOfOrderExecutionCancellationExporter");
            this.Container.RegisterSingleton<IDataExporter, InspectionPlanExporter>("InspectionPlanExporter");
            this.Container.RegisterSingleton<IDataExporter, ExaminationExporter>("ExaminationExporter");
            this.Container.RegisterSingleton<IDataExporter, InsuranceProductExporter>("InsuranceProductExporter");
        }

        public void RegisterExports()
        {
            this.Container.RegisterTransient<IDataExportService, ValidationResultExport>("ValidationResultExport");
            this.Container.RegisterTransient<IDataExportService, PackagesExport>("PackagesExport");
            this.Container.RegisterTransient<IDataExportService, UploadAttachmentResultExport>("UploadAttachmentResultExport");
        }
    }
}