namespace Bars.GkhGji.Regions.Khakasia
{    
    using Bars.B4;

    public partial class ResourceManifest : ResourceManifestBase
    {

		protected override void BaseInit(IResourceManifestContainer container)
        {  

            AddResource(container, "libs/B4/DisposalTextValues.js");
            AddResource(container, "libs/B4/GjiTextValuesOverride.js");
            AddResource(container, "libs/B4/aspects/permission/ActCheck.js");
            AddResource(container, "libs/B4/aspects/permission/AppealCits.js");
            AddResource(container, "libs/B4/aspects/permission/Disposal.js");
            AddResource(container, "libs/B4/aspects/permission/KhakasiaActSurvey.js");
            AddResource(container, "libs/B4/aspects/permission/Resolution.js");
            AddResource(container, "libs/B4/controller/ActCheck.js");
            AddResource(container, "libs/B4/controller/ActRemoval.js");
            AddResource(container, "libs/B4/controller/ActSurvey.js");
            AddResource(container, "libs/B4/controller/AppealCits.js");
            AddResource(container, "libs/B4/controller/BaseDispHead.js");
            AddResource(container, "libs/B4/controller/BaseProsClaim.js");
            AddResource(container, "libs/B4/controller/BaseStatement.js");
            AddResource(container, "libs/B4/controller/Disposal.js");
            AddResource(container, "libs/B4/controller/Prescription.js");
            AddResource(container, "libs/B4/controller/ProtocolGji.js");
            AddResource(container, "libs/B4/controller/Resolution.js");
            AddResource(container, "libs/B4/controller/basedefault/Edit.js");
            AddResource(container, "libs/B4/controller/basedisphead/Edit.js");
            AddResource(container, "libs/B4/controller/baseinscheck/Edit.js");
            AddResource(container, "libs/B4/controller/basejurperson/Edit.js");
            AddResource(container, "libs/B4/controller/baseprosclaim/Edit.js");
            AddResource(container, "libs/B4/controller/basestatement/Edit.js");
            AddResource(container, "libs/B4/controller/dict/LegalReason.js");
            AddResource(container, "libs/B4/controller/dict/SurveySubject.js");
            AddResource(container, "libs/B4/controller/dict/TypeSurveyGji.js");
            AddResource(container, "libs/B4/controller/documentsgjiregister/ActCheck.js");
            AddResource(container, "libs/B4/controller/documentsgjiregister/ActView.js");
            AddResource(container, "libs/B4/controller/report/AdministrativeOffensesJurnalReport.js");
            AddResource(container, "libs/B4/controller/report/AdministrativeOffensesResolution.js");
            AddResource(container, "libs/B4/controller/report/AppealCitsJurnalAccounting.js");
            AddResource(container, "libs/B4/controller/report/PrescriptionRegistrationJournal.js");
            AddResource(container, "libs/B4/controller/report/RegistrationOutgoingDocuments.js");
            AddResource(container, "libs/B4/controller/report/ScheduledInspectionSurveysJournal.js");
            AddResource(container, "libs/B4/controller/resolpros/Edit.js");
            AddResource(container, "libs/B4/model/ActCheck.js");
            AddResource(container, "libs/B4/model/ActSurvey.js");
            AddResource(container, "libs/B4/model/ProtocolGji.js");
            AddResource(container, "libs/B4/model/Resolution.js");
            AddResource(container, "libs/B4/model/actcheck/Violation.js");
            AddResource(container, "libs/B4/model/actremoval/Violation.js");
            AddResource(container, "libs/B4/model/actsurvey/Conclusion.js");
            AddResource(container, "libs/B4/model/appealcits/AppealCitsExecutant.js");
            AddResource(container, "libs/B4/model/appealcits/CheckTimeChange.js");
            AddResource(container, "libs/B4/model/dict/LegalReason.js");
            AddResource(container, "libs/B4/model/dict/SurveySubject.js");
            AddResource(container, "libs/B4/model/dict/TypeSurveyContragentType.js");
            AddResource(container, "libs/B4/model/dict/TypeSurveyGJI.js");
            AddResource(container, "libs/B4/model/dict/TypeSurveyLegalReason.js");
            AddResource(container, "libs/B4/model/disposal/DisposalControlMeasures.js");
            AddResource(container, "libs/B4/model/disposal/DisposalSurveySubject.js");
            AddResource(container, "libs/B4/model/prescription/Violation.js");
            AddResource(container, "libs/B4/model/protocolgji/Definition.js");
            AddResource(container, "libs/B4/model/protocolgji/Violation.js");
            AddResource(container, "libs/B4/model/resolpros/Definition.js");
            AddResource(container, "libs/B4/model/resolution/Definition.js");
            AddResource(container, "libs/B4/model/violationgroup/ViolationGroup.js");
            AddResource(container, "libs/B4/store/actcheck/RealObjForSelect.js");
            AddResource(container, "libs/B4/store/actcheck/ViolationGroup.js");
            AddResource(container, "libs/B4/store/actremoval/ViolationGroup.js");
            AddResource(container, "libs/B4/store/actsurvey/Conclusion.js");
            AddResource(container, "libs/B4/store/appealcits/AppealCitsExecutant.js");
            AddResource(container, "libs/B4/store/appealcits/CheckTimeChange.js");
            AddResource(container, "libs/B4/store/dict/LegalReason.js");
            AddResource(container, "libs/B4/store/dict/LegalReasonForSelect.js");
            AddResource(container, "libs/B4/store/dict/LegalReasonForSelected.js");
            AddResource(container, "libs/B4/store/dict/SurveySubject.js");
            AddResource(container, "libs/B4/store/dict/SurveySubjectForSelect.js");
            AddResource(container, "libs/B4/store/dict/SurveySubjectForSelected.js");
            AddResource(container, "libs/B4/store/dict/TypeSurveyContragentType.js");
            AddResource(container, "libs/B4/store/dict/TypeSurveyLegalReason.js");
            AddResource(container, "libs/B4/store/disposal/DisposalControlMeasures.js");
            AddResource(container, "libs/B4/store/disposal/DisposalSurveySubject.js");
            AddResource(container, "libs/B4/store/prescription/ViolationGroup.js");
            AddResource(container, "libs/B4/store/protocolgji/ViolationGroup.js");
            AddResource(container, "libs/B4/store/resolpros/Definition.js");
            AddResource(container, "libs/B4/store/view/ActView.js");
            AddResource(container, "libs/B4/store/violationgroup/ViolationGroup.js");
            AddResource(container, "libs/B4/store/violationgroup/ViolationGroupForSelect.js");
            AddResource(container, "libs/B4/store/violationgroup/ViolationGroupForSelected.js");
            AddResource(container, "libs/B4/view/actcheck/DefinitionEditWindow.js");
            AddResource(container, "libs/B4/view/actcheck/EditPanel.js");
            AddResource(container, "libs/B4/view/actcheck/RealityObjectEditPanel.js");
            AddResource(container, "libs/B4/view/actcheck/RealityObjectEditWindow.js");
            AddResource(container, "libs/B4/view/actcheck/ViolationGrid.js");
            AddResource(container, "libs/B4/view/actcheck/ViolationGroupEditWindow.js");
            AddResource(container, "libs/B4/view/actcheck/ViolationGroupGrid.js");
            AddResource(container, "libs/B4/view/actremoval/EditPanel.js");
            AddResource(container, "libs/B4/view/actremoval/ViolationGrid.js");
            AddResource(container, "libs/B4/view/actremoval/ViolationGroupEditWindow.js");
            AddResource(container, "libs/B4/view/actremoval/ViolationGroupGrid.js");
            AddResource(container, "libs/B4/view/actsurvey/ConclusionEditWindow.js");
            AddResource(container, "libs/B4/view/actsurvey/ConclusionGrid.js");
            AddResource(container, "libs/B4/view/actsurvey/EditPanel.js");
            AddResource(container, "libs/B4/view/actsurvey/OwnerGrid.js");
            AddResource(container, "libs/B4/view/appealcits/AnswerGrid.js");
            AddResource(container, "libs/B4/view/appealcits/AppealCitsExecutantGrid.js");
            AddResource(container, "libs/B4/view/appealcits/CheckTimeHistory.js");
            AddResource(container, "libs/B4/view/appealcits/EditWindow.js");
            AddResource(container, "libs/B4/view/appealcits/ExecutantEditWindow.js");
            AddResource(container, "libs/B4/view/appealcits/FilterPanel.js");
            AddResource(container, "libs/B4/view/appealcits/Grid.js");
            AddResource(container, "libs/B4/view/appealcits/MultiSelectWindowExecutant.js");
            AddResource(container, "libs/B4/view/basedefault/EditPanel.js");
            AddResource(container, "libs/B4/view/basedisphead/EditPanel.js");
            AddResource(container, "libs/B4/view/baseinscheck/EditPanel.js");
            AddResource(container, "libs/B4/view/basejurperson/EditPanel.js");
            AddResource(container, "libs/B4/view/baseprosclaim/EditPanel.js");
            AddResource(container, "libs/B4/view/basestatement/EditPanel.js");
            AddResource(container, "libs/B4/view/desktop/portlet/TaskTable.js");
            AddResource(container, "libs/B4/view/dict/legalreason/Grid.js");
            AddResource(container, "libs/B4/view/dict/surveysubject/Grid.js");
            AddResource(container, "libs/B4/view/dict/typesurveygji/ContragentTypeGrid.js");
            AddResource(container, "libs/B4/view/dict/typesurveygji/EditWindow.js");
            AddResource(container, "libs/B4/view/dict/typesurveygji/GoalInspGjiGrid.js");
            AddResource(container, "libs/B4/view/dict/typesurveygji/Grid.js");
            AddResource(container, "libs/B4/view/dict/typesurveygji/InspFoundationGjiGrid.js");
            AddResource(container, "libs/B4/view/dict/typesurveygji/KindInspGjiEditWindow.js");
            AddResource(container, "libs/B4/view/dict/typesurveygji/KindInspGjiGrid.js");
            AddResource(container, "libs/B4/view/dict/typesurveygji/TaskInspGjiGrid.js");
            AddResource(container, "libs/B4/view/dict/violationgji/EditWindow.js");
            AddResource(container, "libs/B4/view/dict/violationgji/Grid.js");
            AddResource(container, "libs/B4/view/disposal/DisposalControlMeasuresGrid.js");
            AddResource(container, "libs/B4/view/disposal/DisposalSurveySubjectGrid.js");
            AddResource(container, "libs/B4/view/disposal/EditPanel.js");
            AddResource(container, "libs/B4/view/disposal/ProvidedDocGrid.js");
            AddResource(container, "libs/B4/view/documentsgjiregister/ActSurveyGrid.js");
            AddResource(container, "libs/B4/view/documentsgjiregister/ActViewGrid.js");
            AddResource(container, "libs/B4/view/documentsgjiregister/ResolutionGrid.js");
            AddResource(container, "libs/B4/view/prescription/RealityObjListPanel.js");
            AddResource(container, "libs/B4/view/prescription/ViolationGrid.js");
            AddResource(container, "libs/B4/view/prescription/ViolationGroupEditWindow.js");
            AddResource(container, "libs/B4/view/prescription/ViolationGroupGrid.js");
            AddResource(container, "libs/B4/view/protocolgji/DefinitionEditWindow.js");
            AddResource(container, "libs/B4/view/protocolgji/DefinitionGrid.js");
            AddResource(container, "libs/B4/view/protocolgji/RealityObjListPanel.js");
            AddResource(container, "libs/B4/view/protocolgji/RequisitePanel.js");
            AddResource(container, "libs/B4/view/protocolgji/ViolationGrid.js");
            AddResource(container, "libs/B4/view/protocolgji/ViolationGroupEditWindow.js");
            AddResource(container, "libs/B4/view/protocolgji/ViolationGroupGrid.js");
            AddResource(container, "libs/B4/view/protocolmhc/DefinitionEditWindow.js");
            AddResource(container, "libs/B4/view/report/AdministrativeOffensesJurnalReportPanel.js");
            AddResource(container, "libs/B4/view/report/AdministrativeOffensesResolutionPanel.js");
            AddResource(container, "libs/B4/view/report/AppealCitsJurnalAccountingPanel.js");
            AddResource(container, "libs/B4/view/report/PrescriptionRegistrationJournalPanel.js");
            AddResource(container, "libs/B4/view/report/RegistrationOutgoingDocumentsPanel.js");
            AddResource(container, "libs/B4/view/report/ScheduledInspectionSurveysJournalPanel.js");
            AddResource(container, "libs/B4/view/resolpros/DefinitionEditWindow.js");
            AddResource(container, "libs/B4/view/resolpros/DefinitionGrid.js");
            AddResource(container, "libs/B4/view/resolpros/EditPanel.js");
            AddResource(container, "libs/B4/view/resolution/DefinitionEditWindow.js");
            AddResource(container, "libs/B4/view/resolution/DefinitionGrid.js");
            AddResource(container, "libs/B4/view/resolution/EditPanel.js");
            AddResource(container, "libs/B4/view/resolution/RequisitePanel.js");
        }

        private void AddResource(IResourceManifestContainer container, string path)
		{

            container.Add(path, string.Format("Bars.GkhGji.Regions.Khakasia.dll/Bars.GkhGji.Regions.Khakasia.{0}", path.Replace("/", ".")));
        }
    }
}
