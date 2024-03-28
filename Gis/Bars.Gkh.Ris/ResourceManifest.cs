namespace Bars.Gkh.Ris
{    
    using Bars.B4;

    public partial class ResourceManifest : ResourceManifestBase
    {

		protected override void BaseInit(IResourceManifestContainer container)
        {  

            AddResource(container, "libs/B4/controller/integrations/ExternalRis.js");
            AddResource(container, "libs/B4/controller/integrations/GisIntegration.js");
            AddResource(container, "libs/B4/controller/integrations/GisIntegrationSettings.js");
            AddResource(container, "libs/B4/cryptopro/jsxmlsigner.js");
            AddResource(container, "libs/B4/cryptopro/xadessigner.js");
            AddResource(container, "libs/B4/model/ObjectProcessingResult.js");
            AddResource(container, "libs/B4/model/PrepareDataProtocolRecord.js");
            AddResource(container, "libs/B4/model/RisCertificate.js");
            AddResource(container, "libs/B4/model/RisPackage.js");
            AddResource(container, "libs/B4/model/SendDataProtocolRecord.js");
            AddResource(container, "libs/B4/model/TaskTreeNode.js");
            AddResource(container, "libs/B4/model/ValidationResult.js");
            AddResource(container, "libs/B4/model/integrations/bills/Acknowledgment.js");
            AddResource(container, "libs/B4/model/integrations/bills/OpenOrgPaymentPeriod.js");
            AddResource(container, "libs/B4/model/integrations/gis/Dict.js");
            AddResource(container, "libs/B4/model/integrations/gis/DictRef.js");
            AddResource(container, "libs/B4/model/integrations/gis/Method.js");
            AddResource(container, "libs/B4/model/integrations/houseManagement/Charter.js");
            AddResource(container, "libs/B4/model/integrations/houseManagement/Contract.js");
            AddResource(container, "libs/B4/model/integrations/houseManagement/House.js");
            AddResource(container, "libs/B4/model/integrations/houseManagement/Notification.js");
            AddResource(container, "libs/B4/model/integrations/houseManagement/PublicPropertyContract.js");
            AddResource(container, "libs/B4/model/integrations/houseManagement/SupplyResourceContract.js");
            AddResource(container, "libs/B4/model/integrations/infrastructure/Rki.js");
            AddResource(container, "libs/B4/model/integrations/inspection/Examination.js");
            AddResource(container, "libs/B4/model/integrations/inspection/Plan.js");
            AddResource(container, "libs/B4/model/integrations/nsi/AdditionalService.js");
            AddResource(container, "libs/B4/model/integrations/nsi/MunicipalService.js");
            AddResource(container, "libs/B4/model/integrations/nsi/OrganizationWork.js");
            AddResource(container, "libs/B4/model/integrations/orgRegistry/Contragent.js");
            AddResource(container, "libs/B4/model/integrations/payment/NotificationOfOrderExecution.js");
            AddResource(container, "libs/B4/model/integrations/services/CompletedWork.js");
            AddResource(container, "libs/B4/model/integrations/services/RepairObject.js");
            AddResource(container, "libs/B4/model/integrations/services/RepairProgram.js");
            AddResource(container, "libs/B4/store/ObjectProcessingResult.js");
            AddResource(container, "libs/B4/store/PrepareDataProtocol.js");
            AddResource(container, "libs/B4/store/RisCertificate.js");
            AddResource(container, "libs/B4/store/RisPackage.js");
            AddResource(container, "libs/B4/store/SendDataProtocol.js");
            AddResource(container, "libs/B4/store/ValidationResult.js");
            AddResource(container, "libs/B4/store/integrations/bills/Acknowledgment.js");
            AddResource(container, "libs/B4/store/integrations/bills/OpenOrgPaymentPeriod.js");
            AddResource(container, "libs/B4/store/integrations/gis/Dict.js");
            AddResource(container, "libs/B4/store/integrations/gis/DictList.js");
            AddResource(container, "libs/B4/store/integrations/gis/DictRecordList.js");
            AddResource(container, "libs/B4/store/integrations/gis/DictRef.js");
            AddResource(container, "libs/B4/store/integrations/gis/Method.js");
            AddResource(container, "libs/B4/store/integrations/gis/TaskTreeStore.js");
            AddResource(container, "libs/B4/store/integrations/houseManagement/Charter.js");
            AddResource(container, "libs/B4/store/integrations/houseManagement/Contract.js");
            AddResource(container, "libs/B4/store/integrations/houseManagement/House.js");
            AddResource(container, "libs/B4/store/integrations/houseManagement/Notification.js");
            AddResource(container, "libs/B4/store/integrations/houseManagement/PublicPropertyContract.js");
            AddResource(container, "libs/B4/store/integrations/houseManagement/SupplyResourceContract.js");
            AddResource(container, "libs/B4/store/integrations/infrastructure/Rki.js");
            AddResource(container, "libs/B4/store/integrations/inspection/Examination.js");
            AddResource(container, "libs/B4/store/integrations/inspection/Plan.js");
            AddResource(container, "libs/B4/store/integrations/nsi/AdditionalService.js");
            AddResource(container, "libs/B4/store/integrations/nsi/MunicipalService.js");
            AddResource(container, "libs/B4/store/integrations/nsi/OrganizationWork.js");
            AddResource(container, "libs/B4/store/integrations/orgRegistry/Contragent.js");
            AddResource(container, "libs/B4/store/integrations/payment/NotificationOfOrderExecution.js");
            AddResource(container, "libs/B4/store/integrations/services/CompletedWork.js");
            AddResource(container, "libs/B4/store/integrations/services/RepairObject.js");
            AddResource(container, "libs/B4/store/integrations/services/RepairProgram.js");
            AddResource(container, "libs/B4/view/integrations/ExternalRis.js");
            AddResource(container, "libs/B4/view/integrations/gis/DictGrid.js");
            AddResource(container, "libs/B4/view/integrations/gis/DictRefGrid.js");
            AddResource(container, "libs/B4/view/integrations/gis/DictRefSelectWindow.js");
            AddResource(container, "libs/B4/view/integrations/gis/DictRefWindow.js");
            AddResource(container, "libs/B4/view/integrations/gis/DictWindow.js");
            AddResource(container, "libs/B4/view/integrations/gis/PackageDataPreviewWindow.js");
            AddResource(container, "libs/B4/view/integrations/gis/Panel.js");
            AddResource(container, "libs/B4/view/integrations/gis/PrepareDataResultWindow.js");
            AddResource(container, "libs/B4/view/integrations/gis/ProtocolWindow.js");
            AddResource(container, "libs/B4/view/integrations/gis/SendDataResultWindow.js");
            AddResource(container, "libs/B4/view/integrations/gis/SettingsPanel.js");
            AddResource(container, "libs/B4/view/integrations/gis/TaskTree.js");
            AddResource(container, "libs/B4/view/integrations/gis/ValidationResultGrid.js");
            AddResource(container, "libs/B4/view/wizard/WizardBaseStepFrame.js");
            AddResource(container, "libs/B4/view/wizard/WizardFinishStepFrame.js");
            AddResource(container, "libs/B4/view/wizard/WizardStartStepFrame.js");
            AddResource(container, "libs/B4/view/wizard/WizardWindow.js");
            AddResource(container, "libs/B4/view/wizard/export/accountData/AccountDataParametersStepFrame.js");
            AddResource(container, "libs/B4/view/wizard/export/accountData/ExportAccountDataWizard.js");
            AddResource(container, "libs/B4/view/wizard/export/acknowledgmentexporter/AcknowledgmentDataParametersStepFrame.js");
            AddResource(container, "libs/B4/view/wizard/export/acknowledgmentexporter/ExportAcknowledgmentDataWizard.js");
            AddResource(container, "libs/B4/view/wizard/export/additionalservices/AdditionalServicesParametersStepFrame.js");
            AddResource(container, "libs/B4/view/wizard/export/additionalservices/ExportAdditionalServicesWizard.js");
            AddResource(container, "libs/B4/view/wizard/export/charterData/CharterDataParametersStepFrame.js");
            AddResource(container, "libs/B4/view/wizard/export/charterData/ExportCharterDataWizard.js");
            AddResource(container, "libs/B4/view/wizard/export/completedWork/CompletedWorkParametersStepFrame.js");
            AddResource(container, "libs/B4/view/wizard/export/completedWork/ExportCompletedWorkWizard.js");
            AddResource(container, "libs/B4/view/wizard/export/contractData/ContractDataParametersStepFrame.js");
            AddResource(container, "libs/B4/view/wizard/export/contractData/ExportContractDataWizard.js");
            AddResource(container, "libs/B4/view/wizard/export/dataprovider/ExportDataProviderWizard.js");
            AddResource(container, "libs/B4/view/wizard/export/dataprovider/ExportDataProviderXmlPreviewStepFrame.js");
            AddResource(container, "libs/B4/view/wizard/export/devicemetering/ExportMeteringDeviceValuesDataWizard.js");
            AddResource(container, "libs/B4/view/wizard/export/devicemetering/ExportMeteringDeviceValuesPreviewStepFrame.js");
            AddResource(container, "libs/B4/view/wizard/export/examination/ExaminationDataParametersStepFrame.js");
            AddResource(container, "libs/B4/view/wizard/export/examination/ExportExaminationDataWizard.js");
            AddResource(container, "libs/B4/view/wizard/export/houseData/ExportHouseDataWizard.js");
            AddResource(container, "libs/B4/view/wizard/export/houseData/HouseDataParametersStepFrame.js");
            AddResource(container, "libs/B4/view/wizard/export/inspectionplan/ExportInspectionPlanWizard.js");
            AddResource(container, "libs/B4/view/wizard/export/inspectionplan/InspectionPlanParametersStepFrame.js");
            AddResource(container, "libs/B4/view/wizard/export/meteringdevicedata/ExportMeteringDeviceDataWizard.js");
            AddResource(container, "libs/B4/view/wizard/export/meteringdevicedata/MeteringDeviceDataParametersStepFrame.js");
            AddResource(container, "libs/B4/view/wizard/export/municipalservices/ExportMunicipalServicesWizard.js");
            AddResource(container, "libs/B4/view/wizard/export/municipalservices/MunicipalServicesParametersStepFrame.js");
            AddResource(container, "libs/B4/view/wizard/export/notificationData/ExportNotificationDataWizard.js");
            AddResource(container, "libs/B4/view/wizard/export/notificationData/NotificationDataParametersStepFrame.js");
            AddResource(container, "libs/B4/view/wizard/export/notificationoforderexecution/NotificationsOfOrderExecutionParametersStepFrame.js");
            AddResource(container, "libs/B4/view/wizard/export/notificationoforderexecution/NotificationsOfOrderExecutionWizard.js");
            AddResource(container, "libs/B4/view/wizard/export/notificationsoforderexecutioncancellation/NotificationsOfOrderExecutionCancellationDataParametersStepFrame.js");
            AddResource(container, "libs/B4/view/wizard/export/notificationsoforderexecutioncancellation/NotificationsOfOrderExecutionCancellationDataWizard.js");
            AddResource(container, "libs/B4/view/wizard/export/openOrgPaymentPeriod/openOrgPaymentPeriodParametersStepFrame.js");
            AddResource(container, "libs/B4/view/wizard/export/openOrgPaymentPeriod/OpenOrgPaymentPeriodWizard.js");
            AddResource(container, "libs/B4/view/wizard/export/organizationworks/ExportOrganizationWorksWizard.js");
            AddResource(container, "libs/B4/view/wizard/export/organizationworks/OrganizationWorksParametersStepFrame.js");
            AddResource(container, "libs/B4/view/wizard/export/orgregistry/ImportOrgRegistryWizard.js");
            AddResource(container, "libs/B4/view/wizard/export/orgregistry/OrgRegistryParametersStepFrame.js");
            AddResource(container, "libs/B4/view/wizard/export/paymentdocumentdata/ExportPaymentDocumentDataWizard.js");
            AddResource(container, "libs/B4/view/wizard/export/paymentdocumentdata/PaymentDocumentDataParametersStepFrame.js");
            AddResource(container, "libs/B4/view/wizard/export/publicPropertyContractData/ExportPublicPropertyContractDataWizard.js");
            AddResource(container, "libs/B4/view/wizard/export/publicPropertyContractData/PublicPropertyContractDataParametersStepFrame.js");
            AddResource(container, "libs/B4/view/wizard/export/rkiData/ExportRkiDataWizard.js");
            AddResource(container, "libs/B4/view/wizard/export/rkiData/RkiDataParametersStepFrame.js");
            AddResource(container, "libs/B4/view/wizard/export/suppliernotificationoforderexecution/SupplierNotificationsOfOrderExecutionParametersStepFrame.js");
            AddResource(container, "libs/B4/view/wizard/export/suppliernotificationoforderexecution/SupplierNotificationsOfOrderExecutionWizard.js");
            AddResource(container, "libs/B4/view/wizard/export/supplyresourcecontract/ExportSupplyResourceContractWizard.js");
            AddResource(container, "libs/B4/view/wizard/export/supplyresourcecontract/SupplyResourceContractParametersStepFrame.js");
            AddResource(container, "libs/B4/view/wizard/export/votingProtocol/ExportVotingProtocolaWizard.js");
            AddResource(container, "libs/B4/view/wizard/export/votingProtocol/VotingProtocolParametersStepFrame.js");
            AddResource(container, "libs/B4/view/wizard/export/workinglistdata/ExportWorkingListDataWizard.js");
            AddResource(container, "libs/B4/view/wizard/export/workinglistdata/WorkingListDataParametersStepFrame.js");
            AddResource(container, "libs/B4/view/wizard/export/workingplan/ExportWorkingPlanDataWizard.js");
            AddResource(container, "libs/B4/view/wizard/export/workingplan/WorkingPlanDataParametersStepFrame.js");
            AddResource(container, "libs/B4/view/wizard/preparedata/FinishStepFrame.js");
            AddResource(container, "libs/B4/view/wizard/preparedata/ParametersStepFrame.js");
            AddResource(container, "libs/B4/view/wizard/preparedata/StartStepFrame.js");
            AddResource(container, "libs/B4/view/wizard/preparedata/Wizard.js");
            AddResource(container, "libs/B4/view/wizard/senddata/FinishStepFrame.js");
            AddResource(container, "libs/B4/view/wizard/senddata/PackageGrid.js");
            AddResource(container, "libs/B4/view/wizard/senddata/StartStepFrame.js");
            AddResource(container, "libs/B4/view/wizard/senddata/ValidationResultStepFrame.js");
            AddResource(container, "libs/B4/view/wizard/senddata/Wizard.js");
            AddResource(container, "libs/B4/view/wizard/senddata/XmlPreviewStepFrame.js");

            AddResource(container, "content/css/risMain.css");
            AddResource(container, "content/icon/arrow-180.png");
            AddResource(container, "content/icon/arrow.png");
            AddResource(container, "content/icon/cross.png");
            AddResource(container, "content/icon/prepareData.png");
            AddResource(container, "content/icon/send.png");
            AddResource(container, "content/icon/sign_send.png");
            AddResource(container, "content/icon/task.png");
            AddResource(container, "content/icon/view.png");
            AddResource(container, "content/icon/wand.png");
            AddResource(container, "content/img/apply.png");
            AddResource(container, "content/img/error.png");
            AddResource(container, "content/img/warning.png");
            AddResource(container, "content/img/wizard.png");
        }

        private void AddResource(IResourceManifestContainer container, string path)
		{

            container.Add(path, string.Format("Bars.Gkh.Ris.dll/Bars.Gkh.Ris.{0}", path.Replace("/", ".")));
        }
    }
}
