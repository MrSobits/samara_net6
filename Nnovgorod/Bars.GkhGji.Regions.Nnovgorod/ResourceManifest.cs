namespace Bars.GkhGji.Regions.Nnovgorod
{    
    using Bars.B4;

    public partial class ResourceManifest : ResourceManifestBase
    {

		protected override void BaseInit(IResourceManifestContainer container)
        {  

            AddResource(container, "libs/B4/DisposalTextValues.js");
            AddResource(container, "libs/B4/GjiTextValuesOverride.js");
            AddResource(container, "libs/B4/aspects/permission/ActCheck.js");
            AddResource(container, "libs/B4/aspects/permission/Resolution.js");
            AddResource(container, "libs/B4/controller/ActCheck.js");
            AddResource(container, "libs/B4/controller/AppealCits.js");
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
            AddResource(container, "libs/B4/controller/dict/TypeSurveyGji.js");
            AddResource(container, "libs/B4/controller/report/AdministrativeOffensesJurnalReport.js");
            AddResource(container, "libs/B4/controller/report/AdministrativeOffensesResolution.js");
            AddResource(container, "libs/B4/controller/report/AppealCitsJurnalAccounting.js");
            AddResource(container, "libs/B4/controller/report/PrescriptionRegistrationJournal.js");
            AddResource(container, "libs/B4/controller/report/RegistrationOutgoingDocuments.js");
            AddResource(container, "libs/B4/controller/report/ScheduledInspectionSurveysJournal.js");
            AddResource(container, "libs/B4/model/ActCheck.js");
            AddResource(container, "libs/B4/model/ProtocolGji.js");
            AddResource(container, "libs/B4/model/Resolution.js");
            AddResource(container, "libs/B4/model/actcheck/Violation.js");
            AddResource(container, "libs/B4/model/actremoval/Violation.js");
            AddResource(container, "libs/B4/model/appealcits/CheckTimeChange.js");
            AddResource(container, "libs/B4/model/disposal/DisposalControlMeasures.js");
            AddResource(container, "libs/B4/model/prescription/Violation.js");
            AddResource(container, "libs/B4/model/protocolgji/Violation.js");
            AddResource(container, "libs/B4/store/appealcits/CheckTimeChange.js");
            AddResource(container, "libs/B4/store/disposal/DisposalControlMeasures.js");
            AddResource(container, "libs/B4/view/actcheck/EditPanel.js");
            AddResource(container, "libs/B4/view/actcheck/RealityObjectEditPanel.js");
            AddResource(container, "libs/B4/view/actcheck/RealityObjectEditWindow.js");
            AddResource(container, "libs/B4/view/actcheck/ViolationGrid.js");
            AddResource(container, "libs/B4/view/actremoval/ViolationGrid.js");
            AddResource(container, "libs/B4/view/appealcits/AnswerEditWindow.js");
            AddResource(container, "libs/B4/view/appealcits/CheckTimeHistory.js");
            AddResource(container, "libs/B4/view/appealcits/EditWindow.js");
            AddResource(container, "libs/B4/view/appealcits/Grid.js");
            AddResource(container, "libs/B4/view/appealcits/RealityObjectGrid.js");
            AddResource(container, "libs/B4/view/basedefault/EditPanel.js");
            AddResource(container, "libs/B4/view/basedisphead/EditPanel.js");
            AddResource(container, "libs/B4/view/baseinscheck/EditPanel.js");
            AddResource(container, "libs/B4/view/basejurperson/EditPanel.js");
            AddResource(container, "libs/B4/view/baseprosclaim/EditPanel.js");
            AddResource(container, "libs/B4/view/basestatement/EditPanel.js");
            AddResource(container, "libs/B4/view/desktop/portlet/TaskTable.js");
            AddResource(container, "libs/B4/view/dict/typesurveygji/GoalInspGjiGrid.js");
            AddResource(container, "libs/B4/view/dict/typesurveygji/InspFoundationGjiGrid.js");
            AddResource(container, "libs/B4/view/dict/typesurveygji/KindInspGjiEditWindow.js");
            AddResource(container, "libs/B4/view/dict/typesurveygji/KindInspGjiGrid.js");
            AddResource(container, "libs/B4/view/dict/typesurveygji/TaskInspGjiGrid.js");
            AddResource(container, "libs/B4/view/disposal/ControlMeasuresEditWindow.js");
            AddResource(container, "libs/B4/view/disposal/DisposalControlMeasures.js");
            AddResource(container, "libs/B4/view/disposal/EditPanel.js");
            AddResource(container, "libs/B4/view/documentsgjiregister/ResolutionGrid.js");
            AddResource(container, "libs/B4/view/prescription/ViolationGrid.js");
            AddResource(container, "libs/B4/view/protocolgji/RequisitePanel.js");
            AddResource(container, "libs/B4/view/protocolgji/ViolationGrid.js");
            AddResource(container, "libs/B4/view/report/AdministrativeOffensesJurnalReportPanel.js");
            AddResource(container, "libs/B4/view/report/AdministrativeOffensesResolutionPanel.js");
            AddResource(container, "libs/B4/view/report/AppealCitsJurnalAccountingPanel.js");
            AddResource(container, "libs/B4/view/report/PrescriptionRegistrationJournalPanel.js");
            AddResource(container, "libs/B4/view/report/RegistrationOutgoingDocumentsPanel.js");
            AddResource(container, "libs/B4/view/report/ScheduledInspectionSurveysJournalPanel.js");
            AddResource(container, "libs/B4/view/resolution/EditPanel.js");
            AddResource(container, "libs/B4/view/resolution/RequisitePanel.js");
        }
		        

        private void AddResource(IResourceManifestContainer container, string path)
		{

            container.Add(path, string.Format("Bars.GkhGji.Regions.Nnovgorod.dll/Bars.GkhGji.Regions.Nnovgorod.{0}", path.Replace("/", ".")));
        }
    }
}
