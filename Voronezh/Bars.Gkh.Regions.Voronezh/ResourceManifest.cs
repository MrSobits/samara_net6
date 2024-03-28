namespace Bars.Gkh.Regions.Voronezh
{
    using Bars.B4;

    /// <summary>
    /// Манифест ресурсов.
    /// Используется для регистрации ресурсов модуля в общем контейере ресурсов.
    /// </summary>
    public partial class ResourceManifest : ResourceManifestBase
    {
        protected override void BaseInit(IResourceManifestContainer container)
        {

            this.RegisterResource(container, "libs/B4/aspects/RosRegExtractAspect.js");
            this.RegisterResource(container, "libs/B4/controller/DateAreaOwner.js");
            this.RegisterResource(container, "libs/B4/controller/OwnerAccountRoomComparison.js");
            this.RegisterResource(container, "libs/B4/controller/RosRegExtract.js");
            this.RegisterResource(container, "libs/B4/controller/RosRegExtractDesc.js");
            this.RegisterResource(container, "libs/B4/controller/claimwork/EditIndividualClaimWork.js");
            this.RegisterResource(container, "libs/B4/controller/claimwork/IndividualClaimWork.js");
            this.RegisterResource(container, "libs/B4/controller/claimwork/Lawsuit.js");
            this.RegisterResource(container, "libs/B4/controller/claimwork/PartialClaimWork.js");
            this.RegisterResource(container, "libs/B4/controller/dict/ZonalInspection.js");
            this.RegisterResource(container, "libs/B4/controller/Import/DateAreaOwner2.js");
            this.RegisterResource(container, "libs/B4/controller/regop/personal_account/Debtor.js");
            this.RegisterResource(container, "libs/B4/form/FiasSelectAddress.js");
            this.RegisterResource(container, "libs/B4/form/FiasSelectAddressWindow.js");
            this.RegisterResource(container, "libs/B4/model/AccountsForComparsion.js");
            this.RegisterResource(container, "libs/B4/model/DateAreaOwner.js");
            this.RegisterResource(container, "libs/B4/model/LawsuitReferenceCalculation.js");
            this.RegisterResource(container, "libs/B4/model/OwnerAccountRoomComparison.js");
            this.RegisterResource(container, "libs/B4/model/RosRegExtract.js");
            this.RegisterResource(container, "libs/B4/model/RosRegExtractDesc.js");
            this.RegisterResource(container, "libs/B4/model/RosRegExtractGov.js");
            this.RegisterResource(container, "libs/B4/model/RosRegExtractOrg.js");
            this.RegisterResource(container, "libs/B4/model/RosRegExtractOwner.js");
            this.RegisterResource(container, "libs/B4/model/RosRegExtractPers.js");
            this.RegisterResource(container, "libs/B4/model/RosRegExtractReg.js");
            this.RegisterResource(container, "libs/B4/model/RosRegExtractRight.js");
            this.RegisterResource(container, "libs/B4/model/claimwork/LawSuitDebtWorkSSP.js");
            this.RegisterResource(container, "libs/B4/model/claimwork/LawSuitDebtWorkSSPDocument.js");
            this.RegisterResource(container, "libs/B4/model/claimwork/LawsuitOwnerRepresentative.js");
            this.RegisterResource(container, "libs/B4/model/claimwork/lawsuit/LawsuitAddInfo.js");
            this.RegisterResource(container, "libs/B4/model/claimwork/lawsuit/LawsuitOwnerInfoAddInfo.js");
            this.RegisterResource(container, "libs/B4/model/dict/ZonalInspectionPrefix.js");
            this.RegisterResource(container, "libs/B4/model/regop/personal_account/Debtor.js");
            this.RegisterResource(container, "libs/B4/model/viewrosregextract/ViewRosRegExtractGov.js");
            this.RegisterResource(container, "libs/B4/model/viewrosregextract/ViewRosRegExtractOrg.js");
            this.RegisterResource(container, "libs/B4/model/viewrosregextract/ViewRosRegExtractPers.js");
            this.RegisterResource(container, "libs/B4/store/AccountsForComparsion.js");
            this.RegisterResource(container, "libs/B4/store/DateAreaOwner.js");
            this.RegisterResource(container, "libs/B4/store/LawsuitReferenceCalculation.js");
            this.RegisterResource(container, "libs/B4/store/OwnerAccountRoomComparison.js");
            this.RegisterResource(container, "libs/B4/store/RosRegExtract.js");
            this.RegisterResource(container, "libs/B4/store/RosRegExtractDesc.js");
            this.RegisterResource(container, "libs/B4/store/RosRegExtractGov.js");
            this.RegisterResource(container, "libs/B4/store/RosRegExtractOrg.js");
            this.RegisterResource(container, "libs/B4/store/RosRegExtractPers.js");
            this.RegisterResource(container, "libs/B4/store/claimwork/LawSuitDebtWorkSSP.js");
            this.RegisterResource(container, "libs/B4/store/claimwork/LawSuitDebtWorkSSPDocument.js");
            this.RegisterResource(container, "libs/B4/store/claimwork/LawsuitOwnerInfoByDocId.js");
            this.RegisterResource(container, "libs/B4/store/claimwork/LawsuitOwnerRepresentative.js");
            this.RegisterResource(container, "libs/B4/store/claimwork/lawsuit/LawsuitAddInfo.js");
            this.RegisterResource(container, "libs/B4/store/claimwork/lawsuit/LawsuitOwnerInfoAddInfo.js");
            this.RegisterResource(container, "libs/B4/store/dict/ZonalInspectionPrefix.js");
            this.RegisterResource(container, "libs/B4/store/viewrosregextract/ViewRosRegExtractGov.js");
            this.RegisterResource(container, "libs/B4/store/viewrosregextract/ViewRosRegExtractOrg.js");
            this.RegisterResource(container, "libs/B4/store/viewrosregextract/ViewRosRegExtractPers.js");
            this.RegisterResource(container, "libs/B4/view/RosRegExtract.js");
            this.RegisterResource(container, "libs/B4/view/claimwork/LawSuitDebtWorkSSPAddWindow.js");
            this.RegisterResource(container, "libs/B4/view/claimwork/LawSuitDebtWorkSSPDocumentEditWindow.js");
            this.RegisterResource(container, "libs/B4/view/claimwork/LawSuitDebtWorkSSPDocumentGrid.js");
            this.RegisterResource(container, "libs/B4/view/claimwork/LawSuitDebtWorkSSPEditWindow.js");
            this.RegisterResource(container, "libs/B4/view/claimwork/LawSuitDebtWorkSSPGrid.js");
            this.RegisterResource(container, "libs/B4/view/claimwork/LawsuitOwnerRepresentativeGrid.js");
            this.RegisterResource(container, "libs/B4/view/claimwork/LawsuitOwnerRepresentativeWindow.js");
            this.RegisterResource(container, "libs/B4/view/claimwork/lawsuit/CollectionPanel.js");
            this.RegisterResource(container, "libs/B4/view/claimwork/lawsuit/CourtClaimInfoPanel.js");
            this.RegisterResource(container, "libs/B4/view/claimwork/lawsuit/CourtEditWindow.js");
            this.RegisterResource(container, "libs/B4/view/claimwork/lawsuit/CourtGrid.js");
            this.RegisterResource(container, "libs/B4/view/claimwork/lawsuit/EditPanel.js");
            this.RegisterResource(container, "libs/B4/view/claimwork/lawsuit/LawsuitOwnerInfoGrid.js");
            this.RegisterResource(container, "libs/B4/view/claimwork/lawsuit/LawsuitOwnerInfoWindow.js");
            this.RegisterResource(container, "libs/B4/view/claimwork/lawsuit/LawsuitReferenceCalculationGrid.js");
            this.RegisterResource(container, "libs/B4/view/claimwork/lawsuit/MainInfoPanel.js");
            this.RegisterResource(container, "libs/B4/view/dateareaowner/EditWindow.js");
            this.RegisterResource(container, "libs/B4/view/dateareaowner/Grid.js");
            this.RegisterResource(container, "libs/B4/view/dict/zonalInspection/EditWindow.js");
            this.RegisterResource(container, "libs/B4/view/dict/zonalInspection/PrefixPanel.js");
            this.RegisterResource(container, "libs/B4/view/Import/DateAreaOwner.js");
            this.RegisterResource(container, "libs/B4/view/jurinstitution/EditWindow.js");
            this.RegisterResource(container, "libs/B4/view/owneraccountroomcomparison/EditWindow.js");
            this.RegisterResource(container, "libs/B4/view/owneraccountroomcomparison/Grid.js");
            this.RegisterResource(container, "libs/B4/view/regop/personal_account/DebtorGrid.js");
            this.RegisterResource(container, "libs/B4/view/rosregextract/EditWindow.js");
            this.RegisterResource(container, "libs/B4/view/rosregextract/GovGrid.js");
            this.RegisterResource(container, "libs/B4/view/rosregextract/Grid.js");
            this.RegisterResource(container, "libs/B4/view/rosregextract/OrgGrid.js");
            this.RegisterResource(container, "libs/B4/view/rosregextract/PersonEditWindow.js");
            this.RegisterResource(container, "libs/B4/view/rosregextract/PersonGrid.js");

        }
    }
}
