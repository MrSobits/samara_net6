﻿namespace Bars.GkhGji.Regions.BaseChelyabinsk
{    
    using Bars.B4;

    public partial class ResourceManifest : ResourceManifestBase
    {

		protected override void BaseInit(IResourceManifestContainer container)
        {  
            AddResource(container, "libs/B4/DisposalTextValues.js");
            AddResource(container, "libs/B4/GjiTextValuesOverride.js");
            AddResource(container, "libs/B4/aspects/GjiDocumentCreateButton.js");
            AddResource(container, "libs/B4/aspects/GjiDocumentList.js");
            AddResource(container, "libs/B4/aspects/GjiDocumentRegister.js");
            AddResource(container, "libs/B4/aspects/GjiPrescription.js");
            AddResource(container, "libs/B4/aspects/permission/ActRemoval.js");
            AddResource(container, "libs/B4/aspects/permission/AppealCits.js");
            AddResource(container, "libs/B4/aspects/permission/ChelyabinskActCheck.js");
            AddResource(container, "libs/B4/aspects/permission/ChelyabinskDisposal.js");
            AddResource(container, "libs/B4/aspects/permission/ChelyabinskPrescription.js");
            AddResource(container, "libs/B4/controller/ActCheck.js");
            AddResource(container, "libs/B4/controller/ActRemoval.js");
            AddResource(container, "libs/B4/controller/AppealCits.js");
            AddResource(container, "libs/B4/controller/AppealCitsAnswerRegistration.js");
            AddResource(container, "libs/B4/controller/Disposal.js");
            AddResource(container, "libs/B4/controller/EDSInspection.js");
            AddResource(container, "libs/B4/controller/LicenseAction.js");
            AddResource(container, "libs/B4/controller/MkdChangeNotification.js");
            AddResource(container, "libs/B4/controller/MVDLivingPlaceRegistration.js");
            AddResource(container, "libs/B4/controller/MVDPassport.js");
            AddResource(container, "libs/B4/controller/MVDStayingPlaceRegistration.js");
            AddResource(container, "libs/B4/controller/Prescription.js");
            AddResource(container, "libs/B4/controller/ProtocolGji.js");
            AddResource(container, "libs/B4/controller/Reminder.js");
            AddResource(container, "libs/B4/controller/ReminderAppealCits.js");
            AddResource(container, "libs/B4/controller/Resolution.js");
            AddResource(container, "libs/B4/controller/SMEVCertInfo.js");
            AddResource(container, "libs/B4/controller/SMEVComplaints.js");
            AddResource(container, "libs/B4/controller/SMEVComplaintsRequest.js");
            AddResource(container, "libs/B4/controller/SMEVERULReqNumber.js");
            AddResource(container, "libs/B4/controller/TaskCalendar.js");
            AddResource(container, "libs/B4/controller/actremoval/ListPanel.js");
            AddResource(container, "libs/B4/controller/basedisphead/Edit.js");
            AddResource(container, "libs/B4/controller/basejurperson/Edit.js");
            AddResource(container, "libs/B4/controller/baseprosclaim/Edit.js");
            AddResource(container, "libs/B4/controller/basestatement/Edit.js");
            AddResource(container, "libs/B4/controller/dict/SMEVComplaintsDecision.js");
            AddResource(container, "libs/B4/controller/dict/TypeSurveyGji.js");
            AddResource(container, "libs/B4/controller/documentsgjiregister/Protocol197.js");
            AddResource(container, "libs/B4/controller/eds/EDSDocumentRegistry.js");
            AddResource(container, "libs/B4/controller/protocol197/Edit.js");
            AddResource(container, "libs/B4/controller/protocol197/Navigation.js");
            AddResource(container, "libs/B4/controller/protocol197/Protocol197.js");
            AddResource(container, "libs/B4/controller/report/ActReviseInspectionHalfYear.js");
            AddResource(container, "libs/B4/controller/report/ChelyabinskBusinessActivityReport.js");
            AddResource(container, "libs/B4/controller/report/JurPersonInspectionPlan.js");
            AddResource(container, "libs/B4/controller/report/NoActionsMadeListPrescriptions.js");
            AddResource(container, "libs/B4/form/FiasSelectCustomAddress.js");
            AddResource(container, "libs/B4/form/FiasSelectCustomAddressWindow.js");
            AddResource(container, "libs/B4/model/ActCheck.js");
            AddResource(container, "libs/B4/model/ActRemoval.js");
            AddResource(container, "libs/B4/model/AppealCits.js");
            AddResource(container, "libs/B4/model/Disposal.js");
            AddResource(container, "libs/B4/model/MkdChangeNotification.js");
            AddResource(container, "libs/B4/model/MkdChangeNotificationFile.js");
            AddResource(container, "libs/B4/model/Prescription.js");
            AddResource(container, "libs/B4/model/ProtocolGji.js");
            AddResource(container, "libs/B4/model/ResolPros.js");
            AddResource(container, "libs/B4/model/TaskCalendar.js");
            AddResource(container, "libs/B4/model/actcheck/ProvidedDoc.js");
            AddResource(container, "libs/B4/model/actcheck/Violation.js");
            AddResource(container, "libs/B4/model/actremoval/Annex.js");
            AddResource(container, "libs/B4/model/actremoval/Definition.js");
            AddResource(container, "libs/B4/model/actremoval/InspectedPart.js");
            AddResource(container, "libs/B4/model/actremoval/Period.js");
            AddResource(container, "libs/B4/model/actremoval/ProvidedDoc.js");
            AddResource(container, "libs/B4/model/actremoval/Witness.js");
            AddResource(container, "libs/B4/model/appealcits/AppealCitsExecutant.js");
            AddResource(container, "libs/B4/model/appealcits/AppealOrder.js");
            AddResource(container, "libs/B4/model/appealcits/AppealOrderExecutant.js");
            AddResource(container, "libs/B4/model/appealcits/AppealOrderFile.js");
            AddResource(container, "libs/B4/model/appealcits/appealcitsanswerregistration/AppealCitsAnswerRegistration.js");
            AddResource(container, "libs/B4/model/basejurperson/Contragent.js");
            AddResource(container, "libs/B4/model/complaints/ComplaintsFile.js");
            AddResource(container, "libs/B4/model/complaints/Decision.js");
            AddResource(container, "libs/B4/model/complaints/DecisionLS.js");
            AddResource(container, "libs/B4/model/complaints/SMEVComplaints.js");
            AddResource(container, "libs/B4/model/complaints/SMEVComplaintsExecutant.js");
            AddResource(container, "libs/B4/model/complaints/SMEVComplaintsRequest.js");
            AddResource(container, "libs/B4/model/complaints/SMEVComplaintsRequestFile.js");
            AddResource(container, "libs/B4/model/complaints/SMEVComplaintsStep.js");
            AddResource(container, "libs/B4/model/contragent/Contragent.js");
            AddResource(container, "libs/B4/model/disposal/AdminRegulation.js");
            AddResource(container, "libs/B4/model/disposal/DisposalAdditionalDoc.js");
            AddResource(container, "libs/B4/model/disposal/DisposalControlMeasures.js");
            AddResource(container, "libs/B4/model/disposal/DisposalDocConfirm.js");
            AddResource(container, "libs/B4/model/disposal/DisposalVerificationSubject.js");
            AddResource(container, "libs/B4/model/disposal/InspFoundation.js");
            AddResource(container, "libs/B4/model/disposal/InspFoundationCheck.js");
            AddResource(container, "libs/B4/model/disposal/InspFoundCheckNormDocItem.js");
            AddResource(container, "libs/B4/model/disposal/SurveyObjective.js");
            AddResource(container, "libs/B4/model/disposal/SurveyPurpose.js");
            AddResource(container, "libs/B4/model/eds/EDSDocument.js");
            AddResource(container, "libs/B4/model/eds/EDSDocumentRegistry.js");
            AddResource(container, "libs/B4/model/eds/EDSInspection.js");
            AddResource(container, "libs/B4/model/eds/EDSMotivRequst.js");
            AddResource(container, "libs/B4/model/eds/EDSNotice.js");
            AddResource(container, "libs/B4/model/eds/UKDocument.js");
            AddResource(container, "libs/B4/model/licenseaction/LicenseAction.js");
            AddResource(container, "libs/B4/model/licenseaction/LicenseActionFile.js");
            AddResource(container, "libs/B4/model/prescription/BaseDocument.js");
            AddResource(container, "libs/B4/model/prescription/Violation.js");
            AddResource(container, "libs/B4/model/protocol197/Annex.js");
            AddResource(container, "libs/B4/model/protocol197/ArticleLaw.js");
            AddResource(container, "libs/B4/model/protocol197/Definition.js");
            AddResource(container, "libs/B4/model/protocol197/Protocol197.js");
            AddResource(container, "libs/B4/model/protocol197/Violation.js");
            AddResource(container, "libs/B4/model/protocolgji/BaseDocument.js");
            AddResource(container, "libs/B4/model/protocolgji/Violation.js");
            AddResource(container, "libs/B4/model/smev/MVDLivingPlaceRegistration.js");
            AddResource(container, "libs/B4/model/smev/MVDLivingPlaceRegistrationFile.js");
            AddResource(container, "libs/B4/model/smev/MVDPassport.js");
            AddResource(container, "libs/B4/model/smev/MVDPassportFile.js");
            AddResource(container, "libs/B4/model/smev/MVDStayingPlaceRegistration.js");
            AddResource(container, "libs/B4/model/smev/MVDStayingPlaceRegistrationFile.js");
            AddResource(container, "libs/B4/model/smev/SMEVCertInfo.js");
            AddResource(container, "libs/B4/model/smev/SMEVCertInfoFile.js");
            AddResource(container, "libs/B4/model/smev/SMEVERULReqNumber.js");
            AddResource(container, "libs/B4/model/smev/SMEVERULReqNumberFile.js");
            AddResource(container, "libs/B4/store/MkdChangeNotification.js");
            AddResource(container, "libs/B4/store/MkdChangeNotificationFile.js");
            AddResource(container, "libs/B4/store/actcheck/ProvidedDoc.js");
            AddResource(container, "libs/B4/store/actremoval/Annex.js");
            AddResource(container, "libs/B4/store/actremoval/Definition.js");
            AddResource(container, "libs/B4/store/actremoval/InspectedPart.js");
            AddResource(container, "libs/B4/store/actremoval/Period.js");
            AddResource(container, "libs/B4/store/actremoval/ProvidedDoc.js");
            AddResource(container, "libs/B4/store/actremoval/Witness.js");
            AddResource(container, "libs/B4/store/appealcits/AppealCitsExecutant.js");
            AddResource(container, "libs/B4/store/appealcits/AppealOrder.js");
            AddResource(container, "libs/B4/store/appealcits/AppealOrderExecutant.js");
            AddResource(container, "libs/B4/store/appealcits/AppealOrderFile.js");
            AddResource(container, "libs/B4/store/appealcits/appealcitsanswerregistration/AppealCitsAnswerRegistration.js");
            AddResource(container, "libs/B4/store/basejurperson/Contragent.js");
            AddResource(container, "libs/B4/store/complaints/ComplaintsFile.js");
            AddResource(container, "libs/B4/store/complaints/Decision.js");
            AddResource(container, "libs/B4/store/complaints/DecisionLS.js");
            AddResource(container, "libs/B4/store/complaints/SMEVComplaints.js");
            AddResource(container, "libs/B4/store/complaints/SMEVComplaintsExecutant.js");
            AddResource(container, "libs/B4/store/complaints/SMEVComplaintsRequest.js");
            AddResource(container, "libs/B4/store/complaints/SMEVComplaintsRequestFile.js");
            AddResource(container, "libs/B4/store/complaints/SMEVComplaintsStep.js");
            AddResource(container, "libs/B4/store/contragent/ForSelect.js");
            AddResource(container, "libs/B4/store/contragent/ForSelected.js");
            AddResource(container, "libs/B4/store/desktop/ReminderWidget.js");
            AddResource(container, "libs/B4/store/dict/ViolationGjiForTreeSelect.js");
            AddResource(container, "libs/B4/store/disposal/AdminRegulation.js");
            AddResource(container, "libs/B4/store/disposal/DisposalAdditionalDoc.js");
            AddResource(container, "libs/B4/store/disposal/DisposalControlMeasures.js");
            AddResource(container, "libs/B4/store/disposal/DisposalDocConfirm.js");
            AddResource(container, "libs/B4/store/disposal/DisposalVerificationSubject.js");
            AddResource(container, "libs/B4/store/disposal/InspFoundation.js");
            AddResource(container, "libs/B4/store/disposal/InspFoundationCheck.js");
            AddResource(container, "libs/B4/store/disposal/InspFoundCheckNormDocItem.js");
            AddResource(container, "libs/B4/store/disposal/SurveyObjective.js");
            AddResource(container, "libs/B4/store/disposal/SurveyPurpose.js");
            AddResource(container, "libs/B4/store/eds/EDSDocument.js");
            AddResource(container, "libs/B4/store/eds/EDSDocumentRegistry.js");
            AddResource(container, "libs/B4/store/eds/EDSInspection.js");
            AddResource(container, "libs/B4/store/eds/EDSMotivRequst.js");
            AddResource(container, "libs/B4/store/eds/EDSNotice.js");
            AddResource(container, "libs/B4/store/eds/ListDocumentsForPetition.js");
            AddResource(container, "libs/B4/store/eds/UKDocument.js");
            AddResource(container, "libs/B4/store/licenseaction/LicenseAction.js");
            AddResource(container, "libs/B4/store/licenseaction/LicenseActionFile.js");
            AddResource(container, "libs/B4/store/prescription/BaseDocument.js");
            AddResource(container, "libs/B4/store/protocol197/Annex.js");
            AddResource(container, "libs/B4/store/protocol197/ArticleLaw.js");
            AddResource(container, "libs/B4/store/protocol197/Definition.js");
            AddResource(container, "libs/B4/store/protocol197/NavigationMenu.js");
            AddResource(container, "libs/B4/store/protocol197/Protocol197.js");
            AddResource(container, "libs/B4/store/protocol197/Violation.js");
            AddResource(container, "libs/B4/store/protocolgji/BaseDocument.js");
            AddResource(container, "libs/B4/store/reminder/AppealCitsReminder.js");
            AddResource(container, "libs/B4/store/smev/MVDLivingPlaceRegistration.js");
            AddResource(container, "libs/B4/store/smev/MVDLivingPlaceRegistrationAnswer.js");
            AddResource(container, "libs/B4/store/smev/MVDLivingPlaceRegistrationFile.js");
            AddResource(container, "libs/B4/store/smev/MVDPassport.js");
            AddResource(container, "libs/B4/store/smev/MVDPassportAnswer.js");
            AddResource(container, "libs/B4/store/smev/MVDPassportFile.js");
            AddResource(container, "libs/B4/store/smev/MVDStayingPlaceRegistration.js");
            AddResource(container, "libs/B4/store/smev/MVDStayingPlaceRegistrationAnswer.js");
            AddResource(container, "libs/B4/store/smev/MVDStayingPlaceRegistrationFile.js");
            AddResource(container, "libs/B4/store/smev/SMEVCertInfo.js");
            AddResource(container, "libs/B4/store/smev/SMEVCertInfoFile.js");
            AddResource(container, "libs/B4/store/smev/SMEVERULReqNumber.js");
            AddResource(container, "libs/B4/store/smev/SMEVERULReqNumberFile.js");
            AddResource(container, "libs/B4/store/taskcalendar/ListAdmonition.js");
            AddResource(container, "libs/B4/store/taskcalendar/ListAppeals.js");
            AddResource(container, "libs/B4/store/taskcalendar/ListCourt.js");
            AddResource(container, "libs/B4/store/taskcalendar/ListDisposals.js");
            AddResource(container, "libs/B4/store/taskcalendar/ListPrescription.js");
            AddResource(container, "libs/B4/store/taskcalendar/ListProtocols.js");
            AddResource(container, "libs/B4/store/taskcalendar/ListResolPros.js");
            AddResource(container, "libs/B4/store/taskcalendar/ListSOPR.js");
            AddResource(container, "libs/B4/store/view/HeatSeason.js");
            AddResource(container, "libs/B4/view/MkdChangeNotificationEdit.js");
            AddResource(container, "libs/B4/view/MkdChangeNotificationFileEdit.js");
            AddResource(container, "libs/B4/view/MkdChangeNotificationFileGrid.js");
            AddResource(container, "libs/B4/view/MkdChangeNotificationGrid.js");
            AddResource(container, "libs/B4/view/RealityObjectGjiGrid.js");
            AddResource(container, "libs/B4/view/RoomNumsEditWindow.js");
            AddResource(container, "libs/B4/view/TaskCalendar.js");
            AddResource(container, "libs/B4/view/actcheck/ControlListTestWindow.js");
            AddResource(container, "libs/B4/view/actcheck/EditPanel.js");
            AddResource(container, "libs/B4/view/actcheck/Grid.js");
            AddResource(container, "libs/B4/view/actcheck/InspectionResultPanel.js");
            AddResource(container, "libs/B4/view/actcheck/MenuButton.js");
            AddResource(container, "libs/B4/view/actcheck/PeriodEditWindow.js");
            AddResource(container, "libs/B4/view/actcheck/ProvidedDocGrid.js");
            AddResource(container, "libs/B4/view/actcheck/RealityObjectEditPanel.js");
            AddResource(container, "libs/B4/view/actcheck/RealityObjectEditWindow.js");
            AddResource(container, "libs/B4/view/actcheck/RealityObjectGrid.js");
            AddResource(container, "libs/B4/view/actcheck/ViolationEditWindow.js");
            AddResource(container, "libs/B4/view/actcheck/ViolationGrid.js");
            AddResource(container, "libs/B4/view/actremoval/AnnexEditWindow.js");
            AddResource(container, "libs/B4/view/actremoval/AnnexGrid.js");
            AddResource(container, "libs/B4/view/actremoval/DefinitionEditWindow.js");
            AddResource(container, "libs/B4/view/actremoval/DefinitionGrid.js");
            AddResource(container, "libs/B4/view/actremoval/EditPanel.js");
            AddResource(container, "libs/B4/view/actremoval/Grid.js");
            AddResource(container, "libs/B4/view/actremoval/InspectedPartEditWindow.js");
            AddResource(container, "libs/B4/view/actremoval/InspectedPartGrid.js");
            AddResource(container, "libs/B4/view/actremoval/ListPanel.js");
            AddResource(container, "libs/B4/view/actremoval/PeriodEditWindow.js");
            AddResource(container, "libs/B4/view/actremoval/PeriodGrid.js");
            AddResource(container, "libs/B4/view/actremoval/ProvidedDocGrid.js");
            AddResource(container, "libs/B4/view/actremoval/ViolationGrid.js");
            AddResource(container, "libs/B4/view/actremoval/WitnessGrid.js");
            AddResource(container, "libs/B4/view/appealcits/AppealCitsExecutantGrid.js");
            AddResource(container, "libs/B4/view/appealcits/EditWindow.js");
            AddResource(container, "libs/B4/view/appealcits/ExecutantEditWindow.js");
            AddResource(container, "libs/B4/view/appealcits/FilterPanel.js");
            AddResource(container, "libs/B4/view/appealcits/Grid.js");
            AddResource(container, "libs/B4/view/appealcits/GridTopToolBar.js");
            AddResource(container, "libs/B4/view/appealcits/MultiSelectWindowExecutant.js");
            AddResource(container, "libs/B4/view/appealcits/appealcitsanswerregistration/EditWindow.js");
            AddResource(container, "libs/B4/view/appealcits/appealcitsanswerregistration/Grid.js");
            AddResource(container, "libs/B4/view/appealcits/appealcitsanswerregistration/PdfWindow.js");
            AddResource(container, "libs/B4/view/basedefault/Grid.js");
            AddResource(container, "libs/B4/view/basedisphead/Grid.js");
            AddResource(container, "libs/B4/view/baseinscheck/Grid.js");
            AddResource(container, "libs/B4/view/basejurperson/ContragentGrid.js");
            AddResource(container, "libs/B4/view/basejurperson/EditPanel.js");
            AddResource(container, "libs/B4/view/basejurperson/Grid.js");
            AddResource(container, "libs/B4/view/basejurperson/MainInfoTabPanel.js");
            AddResource(container, "libs/B4/view/baselicenseapplicants/Grid.js");
            AddResource(container, "libs/B4/view/baseplanaction/Grid.js");
            AddResource(container, "libs/B4/view/baseprosclaim/MainInfoTabPanel.js");
            AddResource(container, "libs/B4/view/baseprosclaim/MainPanel.js");
            AddResource(container, "libs/B4/view/baseprotocol197/NavigationPanel.js");
            AddResource(container, "libs/B4/view/basestatement/Grid.js");
            AddResource(container, "libs/B4/view/complaints/DecisionDictGrid.js");
            AddResource(container, "libs/B4/view/complaints/DecisionLSGrid.js");
            AddResource(container, "libs/B4/view/complaints/DictEditWindow.js");
            AddResource(container, "libs/B4/view/complaints/EditWindow.js");
            AddResource(container, "libs/B4/view/complaints/ExecutantEditWindow.js");
            AddResource(container, "libs/B4/view/complaints/ExecutantGrid.js");
            AddResource(container, "libs/B4/view/complaints/FileInfoGrid.js");
            AddResource(container, "libs/B4/view/complaints/Grid.js");
            AddResource(container, "libs/B4/view/complaints/StepEditWindow.js");
            AddResource(container, "libs/B4/view/complaints/StepGrid.js");
            AddResource(container, "libs/B4/view/complaintsrequest/EditWindow.js");
            AddResource(container, "libs/B4/view/complaintsrequest/FileInfoGrid.js");
            AddResource(container, "libs/B4/view/complaintsrequest/Grid.js");
            AddResource(container, "libs/B4/view/desktop/portlet/taskcontrol.js");
            AddResource(container, "libs/B4/view/desktop/portlet/taskstate.js");
            AddResource(container, "libs/B4/view/desktop/portlet/tasktable.js");
            AddResource(container, "libs/B4/view/dict/planjurpersongji/Grid.js");
            AddResource(container, "libs/B4/view/dict/typesurveygji/EditWindow.js");
            AddResource(container, "libs/B4/view/dict/typesurveygji/InspFoundationGjiGrid.js");
            AddResource(container, "libs/B4/view/disposal/AdminRegulationGrid.js");
            AddResource(container, "libs/B4/view/disposal/DisposalAdditionalDocGrid.js");
            AddResource(container, "libs/B4/view/disposal/DisposalControlMeasuresGrid.js");
            AddResource(container, "libs/B4/view/disposal/DocConfirmGrid.js");
            AddResource(container, "libs/B4/view/disposal/EditPanel.js");
            AddResource(container, "libs/B4/view/disposal/InspFoundationCheckGrid.js");
            AddResource(container, "libs/B4/view/disposal/InspFoundationCheckPanel.js");
            AddResource(container, "libs/B4/view/disposal/InspFoundationGrid.js");
            AddResource(container, "libs/B4/view/disposal/NormDocItemGrid.js");
            AddResource(container, "libs/B4/view/disposal/ProvidedDocGrid.js");
            AddResource(container, "libs/B4/view/disposal/SubjectVerificationGrid.js");
            AddResource(container, "libs/B4/view/disposal/SurveyObjectiveGrid.js");
            AddResource(container, "libs/B4/view/disposal/SurveyPurposeGrid.js");
            AddResource(container, "libs/B4/view/documentsgjiregister/ActCheckGrid.js");
            AddResource(container, "libs/B4/view/documentsgjiregister/ActRemovalGrid.js");
            AddResource(container, "libs/B4/view/documentsgjiregister/ActSurveyGrid.js");
            AddResource(container, "libs/B4/view/documentsgjiregister/DisposalGrid.js");
            AddResource(container, "libs/B4/view/documentsgjiregister/PrescriptionGrid.js");
            AddResource(container, "libs/B4/view/documentsgjiregister/PresentationGrid.js");
            AddResource(container, "libs/B4/view/documentsgjiregister/Protocol197Grid.js");
            AddResource(container, "libs/B4/view/documentsgjiregister/ProtocolGrid.js");
            AddResource(container, "libs/B4/view/documentsgjiregister/ResolutionGrid.js");
            AddResource(container, "libs/B4/view/eds/DocumentGrid.js");
            AddResource(container, "libs/B4/view/eds/DocumentSignGrid.js");
            AddResource(container, "libs/B4/view/eds/EditWindow.js");
            AddResource(container, "libs/B4/view/eds/Grid.js");
            AddResource(container, "libs/B4/view/eds/MotivRequstGrid.js");
            AddResource(container, "libs/B4/view/eds/NoticeGrid.js");
            AddResource(container, "libs/B4/view/eds/UKDocumentEditWindow.js");
            AddResource(container, "libs/B4/view/eds/UKDocumentGrid.js");
            AddResource(container, "libs/B4/view/heatseason/Grid.js");
            AddResource(container, "libs/B4/view/licenseaction/EditWindow.js");
            AddResource(container, "libs/B4/view/licenseaction/FileInfoGrid.js");
            AddResource(container, "libs/B4/view/licenseaction/Grid.js");
            AddResource(container, "libs/B4/view/mvdlivingplacereg/AnswerGrid.js");
            AddResource(container, "libs/B4/view/mvdlivingplacereg/EditWindow.js");
            AddResource(container, "libs/B4/view/mvdlivingplacereg/FileInfoGrid.js");
            AddResource(container, "libs/B4/view/mvdlivingplacereg/Grid.js");
            AddResource(container, "libs/B4/view/mvdpassport/AnswerGrid.js");
            AddResource(container, "libs/B4/view/mvdpassport/EditWindow.js");
            AddResource(container, "libs/B4/view/mvdpassport/FileInfoGrid.js");
            AddResource(container, "libs/B4/view/mvdpassport/Grid.js");
            AddResource(container, "libs/B4/view/mvdstayingplacereg/AnswerGrid.js");
            AddResource(container, "libs/B4/view/mvdstayingplacereg/EditWindow.js");
            AddResource(container, "libs/B4/view/mvdstayingplacereg/FileInfoGrid.js");
            AddResource(container, "libs/B4/view/mvdstayingplacereg/Grid.js");
            AddResource(container, "libs/B4/view/prescription/BaseDocumentEditWindow.js");
            AddResource(container, "libs/B4/view/prescription/BaseDocumentGrid.js");
            AddResource(container, "libs/B4/view/prescription/EditPanel.js");
            AddResource(container, "libs/B4/view/prescription/MenuButton.js");
            AddResource(container, "libs/B4/view/prescription/RealityObjListPanel.js");
            AddResource(container, "libs/B4/view/prescription/RealityObjViolationGrid.js");
            AddResource(container, "libs/B4/view/prescription/ViolationGrid.js");
            AddResource(container, "libs/B4/view/protocol197/AddWindow.js");
            AddResource(container, "libs/B4/view/protocol197/AnnexEditWindow.js");
            AddResource(container, "libs/B4/view/protocol197/AnnexGrid.js");
            AddResource(container, "libs/B4/view/protocol197/ArticleLawGrid.js");
            AddResource(container, "libs/B4/view/protocol197/DefinitionEditWindow.js");
            AddResource(container, "libs/B4/view/protocol197/DefinitionGrid.js");
            AddResource(container, "libs/B4/view/protocol197/EditPanel.js");
            AddResource(container, "libs/B4/view/protocol197/FilterPanel.js");
            AddResource(container, "libs/B4/view/protocol197/Grid.js");
            AddResource(container, "libs/B4/view/protocol197/MainPanel.js");
            AddResource(container, "libs/B4/view/protocol197/RequisitePanel.js");
            AddResource(container, "libs/B4/view/protocol197/ViolationEditPanel.js");
            AddResource(container, "libs/B4/view/protocol197/ViolationEditWindow.js");
            AddResource(container, "libs/B4/view/protocol197/ViolationGrid.js");
            AddResource(container, "libs/B4/view/protocolgji/BaseDocumentEditWindow.js");
            AddResource(container, "libs/B4/view/protocolgji/BaseDocumentGrid.js");
            AddResource(container, "libs/B4/view/protocolgji/EditPanel.js");
            AddResource(container, "libs/B4/view/protocolgji/RealityObjListPanel.js");
            AddResource(container, "libs/B4/view/protocolgji/RequisitePanel.js");
            AddResource(container, "libs/B4/view/protocolgji/ViolationGrid.js");
            AddResource(container, "libs/B4/view/realityobj/CalculateWindow.js");
            AddResource(container, "libs/B4/view/realityobj/Grid.js");
            AddResource(container, "libs/B4/view/reminder/AppealCitsGrid.js");
            AddResource(container, "libs/B4/view/reminder/FilterPanel.js");
            AddResource(container, "libs/B4/view/reminder/InspectorGrid.js");
            AddResource(container, "libs/B4/view/report/ActReviseInspectionHalfYearPanel.js");
            AddResource(container, "libs/B4/view/report/ChelyabinskBusinessActivityReportPanel.js");
            AddResource(container, "libs/B4/view/report/JurPersonInspectionPlanPanel.js");
            AddResource(container, "libs/B4/view/report/NoActionsMadeListPrescriptionsPanel.js");
            AddResource(container, "libs/B4/view/resolution/RequisitePanel.js");
            AddResource(container, "libs/B4/view/smevcertinfo/EditWindow.js");
            AddResource(container, "libs/B4/view/smevcertinfo/FileInfoGrid.js");
            AddResource(container, "libs/B4/view/smevcertinfo/Grid.js");
            AddResource(container, "libs/B4/view/smeverul/EditWindow.js");
            AddResource(container, "libs/B4/view/smeverul/FileInfoGrid.js");
            AddResource(container, "libs/B4/view/smeverul/Grid.js");
            AddResource(container, "libs/B4/view/taskcalendar/CourtPracticeGrid.js");
            AddResource(container, "libs/B4/view/taskcalendar/DisposalGrid.js");
            AddResource(container, "libs/B4/view/taskcalendar/ProtocolGrid.js");
            AddResource(container, "libs/B4/view/taskcalendar/ResolProsGrid.js");
            AddResource(container, "libs/B4/view/taskcalendar/TaskWindow.js");
            AddResource(container, "content/css/b4GjiChelyabinsk.css");
            AddResource(container, "content/img/mkdChangeNotificationHead.png");
            AddResource(container, "resources/ActCheck.mrt");
            AddResource(container, "resources/ActChectQListAnswer.mrt");
            AddResource(container, "resources/ActControlReviewOfDoc.doc");
            AddResource(container, "resources/ActMonitoringSurveyOfHousing.doc");
            AddResource(container, "resources/ActRemoval.mrt");
            AddResource(container, "resources/ActReviseInspectionHalfYear.xlsx");
            AddResource(container, "resources/BlockGJI_ExecutiveDocProtocol_1.mrt");
            AddResource(container, "resources/BlockGji_MotivatedRequest.mrt");
            AddResource(container, "resources/BlockGJI_Resolution.mrt");
            AddResource(container, "resources/ChelyabinskBusinessActivityReport.xlsx");
            AddResource(container, "resources/ChelyabinskDisposal.mrt");
            AddResource(container, "resources/ChelyabinskPrescription.mrt");
            AddResource(container, "resources/ChelyabinskProtocol.mrt");
            AddResource(container, "resources/DecisionNotification.mrt");
            AddResource(container, "resources/DecisionRequirements.mrt");
            AddResource(container, "resources/DisposalGjiNotification.mrt");
            AddResource(container, "resources/Form1StateHousingInspection.xlsx");
            AddResource(container, "resources/HouseTechPassportReport.xlsx");
            AddResource(container, "resources/JournalAppeals.xlsx");
            AddResource(container, "resources/JurPersonInspectionPlanReport.xlsx");
            AddResource(container, "resources/NoActionsMadeListPrescriptions.xlsx");
            AddResource(container, "resources/PrescriptionOfficialReport.mrt");
            AddResource(container, "resources/stamp2.png");
            AddResource(container, "resources/xslt/egrip.xsl");
            AddResource(container, "resources/xslt/egrul.xsl");





        }
		        

        private void AddResource(IResourceManifestContainer container, string path)
		{

            container.Add(path, string.Format("Bars.GkhGji.Regions.BaseChelyabinsk.dll/Bars.GkhGji.Regions.BaseChelyabinsk.{0}", path.Replace("/", ".")));
        }
    }
}