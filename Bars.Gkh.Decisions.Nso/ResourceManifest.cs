namespace Bars.Gkh.Decisions.Nso
{    
    using Bars.B4;

    public partial class ResourceManifest : ResourceManifestBase
    {

		protected override void BaseInit(IResourceManifestContainer container)
        {  

            AddResource(container, "libs/B4/controller/DecisionNotification.js");
            AddResource(container, "libs/B4/controller/realityobj/DecisionHistory.js");
            AddResource(container, "libs/B4/controller/realityobj/DecisionProtocol.js");
            AddResource(container, "libs/B4/controller/realityobj/ExistingSolutions.js");
            AddResource(container, "libs/B4/controller/realityobj/GovDecisionProtocol.js");
            AddResource(container, "libs/B4/controller/report/FundFormationReport.js");
            AddResource(container, "libs/B4/controller/report/RepairAbovePaymentReport.js");
            AddResource(container, "libs/B4/model/DecisionNotification.js");
            AddResource(container, "libs/B4/model/ExistingSolutionsModel.js");
            AddResource(container, "libs/B4/model/MethodFormFund.js");
            AddResource(container, "libs/B4/model/MonthlyFeeAmountDecHistory.js");
            AddResource(container, "libs/B4/model/RealityObjectDecisionProtocol.js");
            AddResource(container, "libs/B4/store/DecisionNotification.js");
            AddResource(container, "libs/B4/store/MethodFormFundForSelect.js");
            AddResource(container, "libs/B4/store/MethodFormFundForSelected.js");
            AddResource(container, "libs/B4/store/MonthlyFeeAmountDecHistory.js");
            AddResource(container, "libs/B4/store/MunicipalityByParent.js");
            AddResource(container, "libs/B4/store/realityobj/GovDecision.js");
            AddResource(container, "libs/B4/store/realityobj/decisionhistory/JobYears.js");
            AddResource(container, "libs/B4/store/realityobj/decisionhistory/Tree.js");
            AddResource(container, "libs/B4/store/realityobj/decision_protocol/Decision.js");
            AddResource(container, "libs/B4/store/realityobj/decision_protocol/Protocol.js");
            AddResource(container, "libs/B4/view/DecisionNotificationGrid.js");
            AddResource(container, "libs/B4/view/decision/AccountOwner.js");
            AddResource(container, "libs/B4/view/decision/CreditOrganization.js");
            AddResource(container, "libs/B4/view/decision/CrFundFormingMethod.js");
            AddResource(container, "libs/B4/view/decision/CrMonthlyFee.js");
            AddResource(container, "libs/B4/view/decision/CrWorkTime.js");
            AddResource(container, "libs/B4/view/decision/MinimalCrFundSize.js");
            AddResource(container, "libs/B4/view/decision/MkdManage.js");
            AddResource(container, "libs/B4/view/decision/TransferFundOnSpecAcc.js");
            AddResource(container, "libs/B4/view/realityobj/ExistingSolutionsPanel.js");
            AddResource(container, "libs/B4/view/realityobj/MonthlyFeeHistoryGrid.js");
            AddResource(container, "libs/B4/view/realityobj/decision/JobYear.js");
            AddResource(container, "libs/B4/view/realityobj/decision/MonthlyFee.js");
            AddResource(container, "libs/B4/view/realityobj/decision/PenaltyDelay.js");
            AddResource(container, "libs/B4/view/realityobj/decision/Protocol.js");
            AddResource(container, "libs/B4/view/realityobj/decisionhistory/JobYearsGrid.js");
            AddResource(container, "libs/B4/view/realityobj/decisionhistory/MainPanel.js");
            AddResource(container, "libs/B4/view/realityobj/decisionhistory/TreePanel.js");
            AddResource(container, "libs/B4/view/realityobj/decision_protocol/DecisionEdit.js");
            AddResource(container, "libs/B4/view/realityobj/decision_protocol/DecisionGrid.js");
            AddResource(container, "libs/B4/view/realityobj/decision_protocol/DecisionWindow.js");
            AddResource(container, "libs/B4/view/realityobj/decision_protocol/NskDecisionAddConfirm.js");
            AddResource(container, "libs/B4/view/realityobj/decision_protocol/NskDecisionAddConfirmNotif.js");
            AddResource(container, "libs/B4/view/realityobj/decision_protocol/NskDecisionEdit.js");
            AddResource(container, "libs/B4/view/realityobj/decision_protocol/ProtocolEdit.js");
            AddResource(container, "libs/B4/view/realityobj/decision_protocol/ProtocolGrid.js");
            AddResource(container, "libs/B4/view/realityobj/govdecisionprotocol/MainGrid.js");
            AddResource(container, "libs/B4/view/realityobj/govdecisionprotocol/Window.js");
            AddResource(container, "libs/B4/view/report/FundFormationPanel.js");
            AddResource(container, "libs/B4/view/report/RepairAbovePaymentReportPanel.js");

        }

        

        private void AddResource(IResourceManifestContainer container, string path)
		{

            container.Add(path, string.Format("Bars.Gkh.Decisions.Nso.dll/Bars.Gkh.Decisions.Nso.{0}", path.Replace("/", ".")));
        }
    }
}
