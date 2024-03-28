namespace Bars.Gkh.ClaimWork
{
    using Bars.B4;

    public partial class ResourceManifest : ResourceManifestBase
    {
        protected override void BaseInit(IResourceManifestContainer container)
        {  

        AddResource(container, "libs/B4/aspects/ClaimWorkDocument.js");
        AddResource(container, "libs/B4/aspects/CourtClaimEditPanelAspect.js");
        AddResource(container, "libs/B4/aspects/DocumentClwAccountDetailAspect.js");
        AddResource(container, "libs/B4/aspects/permission/Lawsuit.js");
        AddResource(container, "libs/B4/controller/DocumentRegister.js");
        AddResource(container, "libs/B4/controller/JurInstitution.js");
        AddResource(container, "libs/B4/controller/JurJournal.js");
        AddResource(container, "libs/B4/controller/ViolClaimWork.js");
        AddResource(container, "libs/B4/controller/claimwork/ActViolIdentification.js");
        AddResource(container, "libs/B4/controller/claimwork/FlattenedClaimWork.js");
        AddResource(container, "libs/B4/controller/claimwork/Lawsuit.js");
        AddResource(container, "libs/B4/controller/claimwork/LawsuitBuildContract.js");
        AddResource(container, "libs/B4/controller/claimwork/Notification.js");
        AddResource(container, "libs/B4/controller/claimwork/NotificationBuildContract.js");
        AddResource(container, "libs/B4/controller/claimwork/PartialClaimWork.js");
        AddResource(container, "libs/B4/controller/claimwork/Pretension.js");
        AddResource(container, "libs/B4/controller/claimwork/PretensionBuildContract.js");
        AddResource(container, "libs/B4/controller/claimwork/RestructDebt.js");
        AddResource(container, "libs/B4/controller/dict/PetitionToCourt.js");
        AddResource(container, "libs/B4/controller/dict/StateDuty.js");
        AddResource(container, "libs/B4/controller/dict/stateduty/Edit.js");
        AddResource(container, "libs/B4/controller/documentregister/ActViolIdentification.js");
        AddResource(container, "libs/B4/controller/documentregister/Lawsuit.js");
        AddResource(container, "libs/B4/controller/documentregister/Notification.js");
        AddResource(container, "libs/B4/controller/documentregister/Pretension.js");
        AddResource(container, "libs/B4/model/LawsuitReferenceCalculation.js");
        AddResource(container, "libs/B4/model/StateDuty.js");
        AddResource(container, "libs/B4/model/claimwork/ActViolIdentification.js");
        AddResource(container, "libs/B4/model/claimwork/FlattenedClaimWork.js");
        AddResource(container, "libs/B4/model/claimwork/Notification.js");
        AddResource(container, "libs/B4/model/claimwork/Pretension.js");
        AddResource(container, "libs/B4/model/claimwork/RestructDebt.js");
        AddResource(container, "libs/B4/model/claimwork/lawsuit/Court.js");
        AddResource(container, "libs/B4/model/claimwork/lawsuit/CourtOrderClaim.js");
        AddResource(container, "libs/B4/model/claimwork/lawsuit/Doc.js");
        AddResource(container, "libs/B4/model/claimwork/lawsuit/Lawsuit.js");
        AddResource(container, "libs/B4/model/claimwork/lawsuit/LawsuitDocument.js");
        AddResource(container, "libs/B4/model/claimwork/lawsuit/LawsuitIndividualOwnerInfo.js");
        AddResource(container, "libs/B4/model/claimwork/lawsuit/LawsuitLegalOwnerInfo.js");
        AddResource(container, "libs/B4/model/claimwork/lawsuit/LawsuitOwnerInfo.js");
        AddResource(container, "libs/B4/model/claimwork/lawsuit/Petition.js");
        AddResource(container, "libs/B4/model/claimwork/restructdebt/Schedule.js");
        AddResource(container, "libs/B4/model/dict/JurInstitution.js");
        AddResource(container, "libs/B4/model/dict/JurInstitutionRealObj.js");
        AddResource(container, "libs/B4/model/dict/JurInstitutionRo.js");
        AddResource(container, "libs/B4/model/dict/StateDutyPetition.js");
        AddResource(container, "libs/B4/store/LawsuitReferenceCalculation.js");
        AddResource(container, "libs/B4/store/claimwork/ActViolIdentification.js");
        AddResource(container, "libs/B4/store/claimwork/DocumentClwAccountDetail.js");
        AddResource(container, "libs/B4/store/claimwork/FlattenedClaimWork.js");
        AddResource(container, "libs/B4/store/claimwork/Notification.js");
        AddResource(container, "libs/B4/store/claimwork/PartialClaimWork.js");
        AddResource(container, "libs/B4/store/claimwork/Pretension.js");
        AddResource(container, "libs/B4/store/claimwork/RestructDebt.js");
        AddResource(container, "libs/B4/store/claimwork/lawsuit/Court.js");
        AddResource(container, "libs/B4/store/claimwork/lawsuit/Doc.js");
        AddResource(container, "libs/B4/store/claimwork/lawsuit/Lawsuit.js");
        AddResource(container, "libs/B4/store/claimwork/lawsuit/LawsuitDocument.js");
        AddResource(container, "libs/B4/store/claimwork/lawsuit/LawsuitIndividualOwnerInfo.js");
        AddResource(container, "libs/B4/store/claimwork/lawsuit/LawsuitLegalOwnerInfo.js");
        AddResource(container, "libs/B4/store/claimwork/lawsuit/LawsuitOwnerInfo.js");
        AddResource(container, "libs/B4/store/claimwork/lawsuit/Petition.js");
        AddResource(container, "libs/B4/store/claimwork/pretension/DebtPayment.js");
        AddResource(container, "libs/B4/store/claimwork/restructdebt/Schedule.js");
        AddResource(container, "libs/B4/store/dict/JurInstitution.js");
        AddResource(container, "libs/B4/store/dict/JurInstitutionRealObj.js");
        AddResource(container, "libs/B4/store/dict/JurInstitutionRo.js");
        AddResource(container, "libs/B4/store/dict/PetitionToCourt.js");
        AddResource(container, "libs/B4/store/dict/RealObjForJurInst.js");
        AddResource(container, "libs/B4/store/dict/StateDuty.js");
        AddResource(container, "libs/B4/store/dict/StateDutyPetition.js");
        AddResource(container, "libs/B4/view/claimwork/DocumentClwAccountDetailGrid.js");
        AddResource(container, "libs/B4/view/claimwork/DocumentRegisterPanel.js");
        AddResource(container, "libs/B4/view/claimwork/actviolidentification/EditPanel.js");
        AddResource(container, "libs/B4/view/claimwork/actviolidentification/Grid.js");
        AddResource(container, "libs/B4/view/claimwork/buildcontract/lawsuit/CourtClaimInfoPanel.js");
        AddResource(container, "libs/B4/view/claimwork/buildcontract/lawsuit/EditPanel.js");
        AddResource(container, "libs/B4/view/claimwork/buildcontract/lawsuit/MainInfoPanel.js");
        AddResource(container, "libs/B4/view/claimwork/buildcontract/pretension/EditPanel.js");
        AddResource(container, "libs/B4/view/claimwork/buildcontract/pretension/TabPanel.js");
        AddResource(container, "libs/B4/view/claimwork/flattenedclaimwork/EditPanel.js");
        AddResource(container, "libs/B4/view/claimwork/flattenedclaimwork/EditWindow.js");
        AddResource(container, "libs/B4/view/claimwork/flattenedclaimwork/Grid.js");
        AddResource(container, "libs/B4/view/claimwork/lawsuit/CollectionPanel.js");
        AddResource(container, "libs/B4/view/claimwork/lawsuit/CourtClaimInfoPanel.js");
        AddResource(container, "libs/B4/view/claimwork/lawsuit/CourtEditWindow.js");
        AddResource(container, "libs/B4/view/claimwork/lawsuit/CourtGrid.js");
        AddResource(container, "libs/B4/view/claimwork/lawsuit/DocEditWindow.js");
        AddResource(container, "libs/B4/view/claimwork/lawsuit/DocGrid.js");
        AddResource(container, "libs/B4/view/claimwork/lawsuit/DocumentationEditWindow.js");
        AddResource(container, "libs/B4/view/claimwork/lawsuit/DocumentationGrid.js");
        AddResource(container, "libs/B4/view/claimwork/lawsuit/EditPanel.js");
        AddResource(container, "libs/B4/view/claimwork/lawsuit/Grid.js");
        AddResource(container, "libs/B4/view/claimwork/lawsuit/LawsuitOwnerInfoGrid.js");
        AddResource(container, "libs/B4/view/claimwork/lawsuit/LawsuitOwnerInfoWindow.js");
        AddResource(container, "libs/B4/view/claimwork/lawsuit/LawsuitReferenceCalculationGrid.js");
        AddResource(container, "libs/B4/view/claimwork/lawsuit/MainInfoPanel.js");
        AddResource(container, "libs/B4/view/claimwork/notification/EditPanel.js");
        AddResource(container, "libs/B4/view/claimwork/notification/Grid.js");
        AddResource(container, "libs/B4/view/claimwork/partialclaimwork/EditWindow.js");
        AddResource(container, "libs/B4/view/claimwork/partialclaimwork/Grid.js");
        AddResource(container, "libs/B4/view/claimwork/pretension/DebtPaymentGrid.js");
        AddResource(container, "libs/B4/view/claimwork/pretension/EditPanel.js");
        AddResource(container, "libs/B4/view/claimwork/pretension/Grid.js");
        AddResource(container, "libs/B4/view/claimwork/pretension/tabPanel.js");
        AddResource(container, "libs/B4/view/claimwork/restructdebt/AddWindow.js");
        AddResource(container, "libs/B4/view/claimwork/restructdebt/Edit.js");
        AddResource(container, "libs/B4/view/claimwork/restructdebt/MainPanel.js");
        AddResource(container, "libs/B4/view/claimwork/restructdebt/ScheduleGrid.js");
        AddResource(container, "libs/B4/view/dict/petition/PetitionToCourt.js");
        AddResource(container, "libs/B4/view/dict/stateduty/EditPanel.js");
        AddResource(container, "libs/B4/view/dict/stateduty/FormulaPanel.js");
        AddResource(container, "libs/B4/view/dict/stateduty/FormulaSelectWindow.js");
        AddResource(container, "libs/B4/view/dict/stateduty/Grid.js");
        AddResource(container, "libs/B4/view/dict/stateduty/PetitionGrid.js");
        AddResource(container, "libs/B4/view/jurinstitution/EditWindow.js");
        AddResource(container, "libs/B4/view/jurinstitution/Grid.js");
        AddResource(container, "libs/B4/view/jurinstitution/RealObjGrid.js");
        AddResource(container, "libs/B4/view/jurjournal/Panel.js");
        AddResource(container, "libs/B4/view/violclaimwork/Grid.js");
        AddResource(container, "content/css/b4ClwMain.css");
        AddResource(container, "content/img/documentClw.png");
        }

        private void AddResource(IResourceManifestContainer container, string path)
        {

            container.Add(path, string.Format("Bars.Gkh.ClaimWork.dll/Bars.Gkh.ClaimWork.{0}", path.Replace("/", ".")));
        }
    }
}
