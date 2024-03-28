namespace Bars.GkhGji.Regions.Samara
{    
    using Bars.B4;

    public partial class ResourceManifest : ResourceManifestBase
    {

		protected override void BaseInit(IResourceManifestContainer container)
        {  

            AddResource(container, "libs/B4/controller/AppealCits.js");
            AddResource(container, "libs/B4/controller/Prescription.js");
            AddResource(container, "libs/B4/controller/report/ControlAppealsExecution.js");
            AddResource(container, "libs/B4/controller/report/Form123Samara.js");
            AddResource(container, "libs/B4/controller/report/PrescriptionViolationRemoval.js");
            AddResource(container, "libs/B4/controller/report/ProtocolResponsibility_2.js");
            AddResource(container, "libs/B4/model/appealcits/Tester.js");
            AddResource(container, "libs/B4/store/appealcits/Tester.js");
            AddResource(container, "libs/B4/view/actcheck/ViolationGrid.js");
            AddResource(container, "libs/B4/view/appealcits/EditWindow.js");
            AddResource(container, "libs/B4/view/appealcits/Grid.js");
            AddResource(container, "libs/B4/view/appealcits/TesterGrid.js");
            AddResource(container, "libs/B4/view/disposal/MenuButton.js");
            AddResource(container, "libs/B4/view/prescription/CancelEditGeneralInfoTab.js");
            AddResource(container, "libs/B4/view/prescription/CancelEditViolCancelTab.js");
            AddResource(container, "libs/B4/view/prescription/CancelEditWindow.js");
            AddResource(container, "libs/B4/view/prescription/CancelGrid.js");
            AddResource(container, "libs/B4/view/prescription/ViolationGrid.js");
            AddResource(container, "libs/B4/view/report/ControlAppealsExecutionPanel.js");
            AddResource(container, "libs/B4/view/report/Form123SamaraPanel.js");
            AddResource(container, "libs/B4/view/report/PrescriptionViolationRemovalPanel.js");
            AddResource(container, "libs/B4/view/report/ProtocolResponsibility_2Panel.js");

            AddResource(container, "resources/ControlAppealsExecution.xlsx");
            AddResource(container, "resources/Form123Samara.xlsx");
            AddResource(container, "resources/JournalAppeals.xlsx");
            AddResource(container, "resources/PrescriptionViolationRemoval.xlsx");
            AddResource(container, "resources/ProtocolResponsibility_2.xlsx");
        }

        

        private void AddResource(IResourceManifestContainer container, string path)
		{

            container.Add(path, string.Format("Bars.GkhGji.Regions.Samara.dll/Bars.GkhGji.Regions.Samara.{0}", path.Replace("/", ".")));
        }
    }
}
