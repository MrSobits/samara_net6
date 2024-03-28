namespace Bars.GkhRf
{    
    using Bars.B4;

    public partial class ResourceManifest : ResourceManifestBase
    {

		protected override void BaseInit(IResourceManifestContainer container)
        {  

            AddResource(container, "libs/B4/aspects/GkhManyGridEditWindow.js");
            AddResource(container, "libs/B4/aspects/permission/Payment.js");
            AddResource(container, "libs/B4/aspects/permission/RequestTransferRf.js");
            AddResource(container, "libs/B4/aspects/permission/TransferRf.js");
            AddResource(container, "libs/B4/controller/ContractRf.js");
            AddResource(container, "libs/B4/controller/LimitCheck.js");
            AddResource(container, "libs/B4/controller/Payment.js");
            AddResource(container, "libs/B4/controller/PaymentNavigation.js");
            AddResource(container, "libs/B4/controller/RequestTransferRf.js");
            AddResource(container, "libs/B4/controller/TransferRf.js");
            AddResource(container, "libs/B4/controller/report/AllocationFundsToPeopleInCr.js");
            AddResource(container, "libs/B4/controller/report/AnalisysProgramCr.js");
            AddResource(container, "libs/B4/controller/report/CalculationsBetweenGisuByManOrg.js");
            AddResource(container, "libs/B4/controller/report/CitizenFundsIncomeExpense.js");
            AddResource(container, "libs/B4/controller/report/ContractsAvailabilityWithGisu.js");
            AddResource(container, "libs/B4/controller/report/CorrectionOfLimits.js");
            AddResource(container, "libs/B4/controller/report/CountOfRequestInRf.js");
            AddResource(container, "libs/B4/controller/report/CreatedRealtyObject.js");
            AddResource(container, "libs/B4/controller/report/CrPaymentInformation.js");
            AddResource(container, "libs/B4/controller/report/ExistHouseInContractAndPayment.js");
            AddResource(container, "libs/B4/controller/report/GisuOrderReport.js");
            AddResource(container, "libs/B4/controller/report/GisuRealObjContract.js");
            AddResource(container, "libs/B4/controller/report/InformationCashReceivedRegionalFund.js");
            AddResource(container, "libs/B4/controller/report/InformationRepublicProgramCr.js");
            AddResource(container, "libs/B4/controller/report/InformationRepublicProgramCrPart2.js");
            AddResource(container, "libs/B4/controller/report/JournalAcPayment.js");
            AddResource(container, "libs/B4/controller/report/ListHousesByProgramCr.js");
            AddResource(container, "libs/B4/controller/report/PaymentCrMonthByRealObj.js");
            AddResource(container, "libs/B4/controller/report/PeopleFundsTransferToGisuInfo.js");
            AddResource(container, "libs/B4/controller/report/ProgramCrOwnersSpending.js");
            AddResource(container, "libs/B4/model/ContractRF.js");
            AddResource(container, "libs/B4/model/LimitCheck.js");
            AddResource(container, "libs/B4/model/Payment.js");
            AddResource(container, "libs/B4/model/TransferRF.js");
            AddResource(container, "libs/B4/model/contractrf/Object.js");
            AddResource(container, "libs/B4/model/limitcheck/FinSource.js");
            AddResource(container, "libs/B4/model/Payment/Item.js");
            AddResource(container, "libs/B4/model/transferrf/Funds.js");
            AddResource(container, "libs/B4/model/transferrf/PersonalAccount.js");
            AddResource(container, "libs/B4/model/transferrf/RealityObject.js");
            AddResource(container, "libs/B4/model/transferrf/RecObj.js");
            AddResource(container, "libs/B4/model/transferrf/Record.js");
            AddResource(container, "libs/B4/model/transferrf/Request.js");
            AddResource(container, "libs/B4/store/ContractRF.js");
            AddResource(container, "libs/B4/store/ContractRfByManOrg.js");
            AddResource(container, "libs/B4/store/LimitCheck.js");
            AddResource(container, "libs/B4/store/Payment.js");
            AddResource(container, "libs/B4/store/TransferRF.js");
            AddResource(container, "libs/B4/store/contractrf/ActualRealObjForSelect.js");
            AddResource(container, "libs/B4/store/contractrf/ObjectIn.js");
            AddResource(container, "libs/B4/store/contractrf/ObjectOut.js");
            AddResource(container, "libs/B4/store/limitcheck/FinSource.js");
            AddResource(container, "libs/B4/store/Payment/BuildingCurrentRepair.js");
            AddResource(container, "libs/B4/store/Payment/BuildingRepair.js");
            AddResource(container, "libs/B4/store/Payment/CR.js");
            AddResource(container, "libs/B4/store/Payment/CR185.js");
            AddResource(container, "libs/B4/store/Payment/ElectricalRepair.js");
            AddResource(container, "libs/B4/store/Payment/HeatingRepair.js");
            AddResource(container, "libs/B4/store/Payment/HireRegFund.js");
            AddResource(container, "libs/B4/store/Payment/NavigationMenu.js");
            AddResource(container, "libs/B4/store/Payment/SanitaryEngineeringRepair.js");
            AddResource(container, "libs/B4/store/realityobj/ByManOrgAndContractDate.js");
            AddResource(container, "libs/B4/store/transferrf/Funds.js");
            AddResource(container, "libs/B4/store/transferrf/MunicipalityForSelect.js");
            AddResource(container, "libs/B4/store/transferrf/MunicipalityForSelected.js");
            AddResource(container, "libs/B4/store/transferrf/PersonalAccount.js");
            AddResource(container, "libs/B4/store/transferrf/RealObjForSelect.js");
            AddResource(container, "libs/B4/store/transferrf/RealObjForSelected.js");
            AddResource(container, "libs/B4/store/transferrf/RecObj.js");
            AddResource(container, "libs/B4/store/transferrf/Record.js");
            AddResource(container, "libs/B4/store/transferrf/Request.js");
            AddResource(container, "libs/B4/view/contractrf/EditWindow.js");
            AddResource(container, "libs/B4/view/contractrf/Grid.js");
            AddResource(container, "libs/B4/view/contractrf/ObjectGridIn.js");
            AddResource(container, "libs/B4/view/contractrf/ObjectGridOut.js");
            AddResource(container, "libs/B4/view/limitcheck/EditWindow.js");
            AddResource(container, "libs/B4/view/limitcheck/FinSourceGrid.js");
            AddResource(container, "libs/B4/view/limitcheck/Grid.js");
            AddResource(container, "libs/B4/view/Payment/AddWindow.js");
            AddResource(container, "libs/B4/view/Payment/Grid.js");
            AddResource(container, "libs/B4/view/Payment/ImportWindow.js");
            AddResource(container, "libs/B4/view/Payment/ItemEditWindow.js");
            AddResource(container, "libs/B4/view/Payment/ItemGrid.js");
            AddResource(container, "libs/B4/view/Payment/NavigationPanel.js");
            AddResource(container, "libs/B4/view/report/AllocationFundsToPeopleInCrPanel.js");
            AddResource(container, "libs/B4/view/report/AnalisysProgramCr.js");
            AddResource(container, "libs/B4/view/report/CalculationsBetweenGisuByManOrgPanel.js");
            AddResource(container, "libs/B4/view/report/CitizenFundsIncomeExpense.js");
            AddResource(container, "libs/B4/view/report/ContractsAvailabilityWithGisuPanel.js");
            AddResource(container, "libs/B4/view/report/CorrectionOfLimitsPanel.js");
            AddResource(container, "libs/B4/view/report/CountOfRequestInRfPanel.js");
            AddResource(container, "libs/B4/view/report/CreatedRealtyObjectPanel.js");
            AddResource(container, "libs/B4/view/report/CrPaymentInformationPanel.js");
            AddResource(container, "libs/B4/view/report/ExistHouseInContractAndPaymentPanel.js");
            AddResource(container, "libs/B4/view/report/GisuOrderPanel.js");
            AddResource(container, "libs/B4/view/report/GisuRealObjContractPanel.js");
            AddResource(container, "libs/B4/view/report/InformationCashReceivedRegionalFundPanel.js");
            AddResource(container, "libs/B4/view/report/InformationRepublicProgramCrPart2Panel.js");
            AddResource(container, "libs/B4/view/report/JournalAcPaymentPanel.js");
            AddResource(container, "libs/B4/view/report/ListHousesByProgramCrPanel.js");
            AddResource(container, "libs/B4/view/report/PaymentCrMonthByRealObjPanel.js");
            AddResource(container, "libs/B4/view/report/PeopleFundsTransferToGisuInfoPanel.js");
            AddResource(container, "libs/B4/view/report/ProgramCrOwnersSpendingPanel.js");
            AddResource(container, "libs/B4/view/report/RepublicProgramCrPanel.js");
            AddResource(container, "libs/B4/view/transferrf/EditWindow.js");
            AddResource(container, "libs/B4/view/transferrf/FundsGrid.js");
            AddResource(container, "libs/B4/view/transferrf/Grid.js");
            AddResource(container, "libs/B4/view/transferrf/RecObjGrid.js");
            AddResource(container, "libs/B4/view/transferrf/RecordEditWindow.js");
            AddResource(container, "libs/B4/view/transferrf/RecordGrid.js");
            AddResource(container, "libs/B4/view/transferrf/RequestEditWindow.js");
            AddResource(container, "libs/B4/view/transferrf/RequestGrid.js");
            AddResource(container, "libs/B4/view/transferrf/RequestPanel.js");

            AddResource(container, "resources/AllocationFundsToPeopleInCr.xlsx");
            AddResource(container, "resources/AnalisysProgramCr.xlsx");
            AddResource(container, "resources/CalculationsBetweenGisuByManOrg.xlsx");
            AddResource(container, "resources/CitizenFundsIncomeExpense.xlsx");
            AddResource(container, "resources/ContractsAvailabilityWithGisu.xlsx");
            AddResource(container, "resources/CorrectionOfLimits.xlsx");
            AddResource(container, "resources/CreatedRealtyObject.xlsx");
            AddResource(container, "resources/CrPaymentInformation.xlsx");
            AddResource(container, "resources/ExistHouseInContractAndPayment.xlsx");
            AddResource(container, "resources/GisuOrdersReport.xlsx");
            AddResource(container, "resources/GisuRObjectContract.xlsx");
            AddResource(container, "resources/GisuTransferRecordForm.xlsx");
            AddResource(container, "resources/InformationCashReceivedRegionalFund.xlsx");
            AddResource(container, "resources/InformationRepublicProgramCr.xlsx");
            AddResource(container, "resources/InformationRepublicProgramCrPart2.xlsx");
            AddResource(container, "resources/ListHousesByProgramCr.xlsx");
            AddResource(container, "resources/PaymentCrMonthByRealObj.xlsx");
            AddResource(container, "resources/PeopleFundsTransferToGisuInfo.xlsx");
            AddResource(container, "resources/ProgramCrOwnersSpendingReport.xlsx");
            AddResource(container, "resources/RegFond_RequestsCount.xlsx");
            AddResource(container, "resources/RequestTransferForm.xlsx");

            AddResource(container, "content/css/b4RfMain.css");
            AddResource(container, "content/img/contractRf.png");
            AddResource(container, "content/img/payment.png");
            AddResource(container, "content/img/requestTransferRf.png");
            AddResource(container, "content/img/transferRf.png");
        }

        

        private void AddResource(IResourceManifestContainer container, string path)
		{

            container.Add(path, string.Format("Bars.GkhRf.dll/Bars.GkhRf.{0}", path.Replace("/", ".")));
        }
    }
}
