namespace Bars.GisIntegration.UI
{
    using System;
    using System.Collections.Generic;

    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.Exporters;
    using Bars.GisIntegration.Base.Exporters.Bills;
    using Bars.GisIntegration.Base.Exporters.CapitalRepair;
    using Bars.GisIntegration.Base.Exporters.DeviceMetering;
    using Bars.GisIntegration.Base.Exporters.HouseManagement;
    using Bars.GisIntegration.Base.Exporters.Infrastructure;
    using Bars.GisIntegration.Base.Exporters.Inspection;
    using Bars.GisIntegration.Base.Exporters.Nsi;
    using Bars.GisIntegration.Base.Exporters.OrgRegistryCommon;
    using Bars.GisIntegration.Base.Exporters.Payment;
    using Bars.GisIntegration.Base.Exporters.Services;
    using Bars.GisIntegration.Base.Tasks.SendData.Payment;

    public class ExporterWizardRefs
    {
        private const string DefaultWizard = "B4.view.wizard.preparedata.Wizard";

        private readonly Dictionary<Type, string> refs = new Dictionary<Type, string>();

        public ExporterWizardRefs()
        {
            this.Register<AcknowledgmentExporter>("B4.view.wizard.export.acknowledgmentexporter.ExportAcknowledgmentDataWizard");
            this.Register<PaymentDocumentDataExporter>("B4.view.wizard.export.paymentdocumentdata.ExportPaymentDocumentDataWizard");
            this.Register<CapitalRepairContractsDataExporter>("B4.view.wizard.export.capitalrepair.CapitalRepairContractsWizard");
            this.Register<PlanImporter>("B4.view.wizard.export.capitalRepairPlan.ImportCapitalRepairPlanWizard");
            this.Register<MeteringDeviceValuesExporter>("B4.view.wizard.export.devicemetering.ExportMeteringDeviceValuesDataWizard");
            this.Register<AccountDataExporter>("B4.view.wizard.export.accountData.ExportAccountDataWizard");
            this.Register<CharterDataExporter>("B4.view.wizard.export.charterData.ExportCharterDataWizard");
            this.Register<ContractDataExporter>("B4.view.wizard.export.contractData.ExportContractDataWizard");
            this.Register<HouseOMSDataExporter>("B4.view.wizard.export.houseData.ExportHouseDataWizard");
            this.Register<HouseRegOperatorDataExporter>("B4.view.wizard.export.houseData.ExportHouseDataWizard");
            this.Register<HouseRSODataExporter>("B4.view.wizard.export.houseData.ExportHouseDataWizard");
            this.Register<HouseUODataExporter>("B4.view.wizard.export.houseData.ExportHouseDataWizard");
            this.Register<MeteringDeviceDataExporter>("B4.view.wizard.export.meteringdevicedata.ExportMeteringDeviceDataWizard");
            this.Register<NotificationDataExporter>("B4.view.wizard.export.notificationData.ExportNotificationDataWizard");
            this.Register<SupplyResourceContractExporter>("B4.view.wizard.export.supplyresourcecontract.ExportSupplyResourceContractWizard");
            this.Register<VotingProtocolExporter>("B4.view.wizard.export.votingProtocol.ExportVotingProtocolaWizard");
            this.Register<RkiDataExporter>("B4.view.wizard.export.rkiData.ExportRkiDataWizard");
            this.Register<ExaminationExporter>("B4.view.wizard.export.examination.ExportExaminationDataWizard");
            this.Register<InspectionPlanExporter>("B4.view.wizard.export.inspectionplan.ExportInspectionPlanWizard");
            this.Register<AdditionalServicesExporter>("B4.view.wizard.export.additionalservices.ExportAdditionalServicesWizard");
            this.Register<MunicipalServicesExporter>("B4.view.wizard.export.municipalservices.ExportMunicipalServicesWizard");
            this.Register<OrganizationWorksExporter>("B4.view.wizard.export.organizationworks.ExportOrganizationWorksWizard");
            this.Register<OrgRegistryExporter>("B4.view.wizard.export.orgregistry.ImportOrgRegistryWizard");
            this.Register<NotificationsOfOrderExecutionCancellationExporter>("B4.view.wizard.export.notificationsoforderexecutioncancellation.NotificationsOfOrderExecutionCancellationDataWizard");
            this.Register<NotificationsOfOrderExecutionExporter>("B4.view.wizard.export.notificationoforderexecution.NotificationsOfOrderExecutionWizard");
            this.Register<SupplierNotificationsOfOrderExecutionExporter>("B4.view.wizard.export.suppliernotificationoforderexecution.SupplierNotificationsOfOrderExecutionWizard");
            this.Register<WorkingListExporter>("B4.view.wizard.export.workinglistdata.ExportWorkingListDataWizard");
            this.Register<WorkingPlanExporter>("B4.view.wizard.export.workingplan.ExportWorkingPlanDataWizard");
        }

        private void Register<TExporter>(string wizard) where TExporter : IDataExporter
        {
            this.refs[typeof(TExporter)] = wizard;
        }

        public string Get(object exporter)
        {
            if (exporter == null)
            {
                return null;
            }

            var type = exporter.GetType();

            return this.refs.Get(type, defValue: ExporterWizardRefs.DefaultWizard);
        }
    }
}