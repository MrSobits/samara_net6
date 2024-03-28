namespace Bars.GkhGji.Regions.Archangelsk
{    
    using Bars.B4;

    public partial class ResourceManifest : ResourceManifestBase
    {

		protected override void BaseInit(IResourceManifestContainer container)
        {  

            AddResource(container, "libs/B4/controller/AppealCits.js");
            AddResource(container, "libs/B4/controller/Prescription.js");
            AddResource(container, "libs/B4/model/appealcits/AppealCitsExecutant.js");
            AddResource(container, "libs/B4/store/appealcits/AppealCitsExecutant.js");
            AddResource(container, "libs/B4/view/actcheck/ViolationGrid.js");
            AddResource(container, "libs/B4/view/appealcits/AppealCitsExecutantGrid.js");
            AddResource(container, "libs/B4/view/appealcits/EditWindow.js");
            AddResource(container, "libs/B4/view/appealcits/ExecutantEditWindow.js");
            AddResource(container, "libs/B4/view/appealcits/Grid.js");
            AddResource(container, "libs/B4/view/appealcits/MultiSelectWindowExecutant.js");
            AddResource(container, "libs/B4/view/prescription/CancelEditGeneralInfoTab.js");
            AddResource(container, "libs/B4/view/prescription/CancelEditViolCancelTab.js");
            AddResource(container, "libs/B4/view/prescription/CancelEditWindow.js");
            AddResource(container, "libs/B4/view/prescription/CancelGrid.js");
            AddResource(container, "libs/B4/view/prescription/ViolationGrid.js");
        }

        

        private void AddResource(IResourceManifestContainer container, string path)
		{

            container.Add(path, string.Format("Bars.GkhGji.Regions.Archangelsk.dll/Bars.GkhGji.Regions.Archangelsk.{0}", path.Replace("/", ".")));
        }
    }
}
