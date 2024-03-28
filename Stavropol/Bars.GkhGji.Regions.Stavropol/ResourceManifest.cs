namespace Bars.GkhGji.Regions.Stavropol
{    
    using Bars.B4;

    public partial class ResourceManifest : ResourceManifestBase
    {

		protected override void BaseInit(IResourceManifestContainer container)
        {  

            AddResource(container, "libs/B4/aspects/permission/ActCheck.js");
            AddResource(container, "libs/B4/aspects/permission/ProtocolGji.js");
            AddResource(container, "libs/B4/aspects/permission/Resolution.js");
            AddResource(container, "libs/B4/controller/ActCheck.js");
            AddResource(container, "libs/B4/controller/Prescription.js");
            AddResource(container, "libs/B4/controller/ProtocolGji.js");
            AddResource(container, "libs/B4/controller/Resolution.js");
            AddResource(container, "libs/B4/controller/resolpros/Edit.js");
            AddResource(container, "libs/B4/model/ActCheck.js");
            AddResource(container, "libs/B4/model/ResolPros.js");
            AddResource(container, "libs/B4/model/actcheck/Definition.js");
            AddResource(container, "libs/B4/model/protocolgji/Definition.js");
            AddResource(container, "libs/B4/model/resolpros/Definition.js");
            AddResource(container, "libs/B4/model/resolution/Definition.js");
            AddResource(container, "libs/B4/store/resolpros/Definition.js");
            AddResource(container, "libs/B4/view/actcheck/DefinitionEditWindow.js");
            AddResource(container, "libs/B4/view/actcheck/EditPanel.js");
            AddResource(container, "libs/B4/view/actcheck/ViolationGrid.js");
            AddResource(container, "libs/B4/view/appealcits/BaseStatementAddWindow.js");
            AddResource(container, "libs/B4/view/prescription/CancelEditGeneralInfoTab.js");
            AddResource(container, "libs/B4/view/prescription/CancelEditViolCancelTab.js");
            AddResource(container, "libs/B4/view/prescription/CancelEditWindow.js");
            AddResource(container, "libs/B4/view/prescription/CancelGrid.js");
            AddResource(container, "libs/B4/view/prescription/ViolationGrid.js");
            AddResource(container, "libs/B4/view/protocolgji/DefinitionEditWindow.js");
            AddResource(container, "libs/B4/view/protocolgji/DefinitionGrid.js");
            AddResource(container, "libs/B4/view/resolpros/AddWindow.js");
            AddResource(container, "libs/B4/view/resolpros/DefinitionEditWindow.js");
            AddResource(container, "libs/B4/view/resolpros/DefinitionGrid.js");
            AddResource(container, "libs/B4/view/resolpros/EditPanel.js");
            AddResource(container, "libs/B4/view/resolution/DefinitionEditWindow.js");

        }

        private void AddResource(IResourceManifestContainer container, string path)
		{
            container.Add(path, string.Format("Bars.GkhGji.Regions.Stavropol.dll/Bars.GkhGji.Regions.Stavropol.{0}", path.Replace("/", ".")));
        }
    }
}
