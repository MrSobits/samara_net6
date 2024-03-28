namespace Bars.Gkh.Regions.Nso
{    
    using Bars.B4;

    public partial class ResourceManifest : ResourceManifestBase
    {

		protected override void BaseInit(IResourceManifestContainer container)
        {  

            AddResource(container, "libs/B4/controller/import/ManOrgRobjectImport.js");
            AddResource(container, "libs/B4/controller/manorg/Edit.js");
            AddResource(container, "libs/B4/controller/manorg/Registry.js");
            AddResource(container, "libs/B4/controller/realityobj/CurentRepair.js");
            AddResource(container, "libs/B4/controller/realityobj/Document.js");
            AddResource(container, "libs/B4/controller/report/LocalGovernment.js");
            AddResource(container, "libs/B4/controller/report/TsjProvidedInfo.js");
            AddResource(container, "libs/B4/model/LocalGovernment.js");
            AddResource(container, "libs/B4/model/dict/ZonalInspection.js");
            AddResource(container, "libs/B4/model/manorg/ManagingOrgRegistry.js");
            AddResource(container, "libs/B4/model/realityobj/Document.js");
            AddResource(container, "libs/B4/store/manorg/ManagingOrgRegistry.js");
            AddResource(container, "libs/B4/store/realityobj/Document.js");
            AddResource(container, "libs/B4/view/dict/zonalinspection/EditWindow.js");
            AddResource(container, "libs/B4/view/dict/zonalinspection/Grid.js");
            AddResource(container, "libs/B4/view/import/ManOrgRobjectImportPanel.js");
            AddResource(container, "libs/B4/view/localgov/AddWindow.js");
            AddResource(container, "libs/B4/view/localgov/EditPanel.js");
            AddResource(container, "libs/B4/view/manorg/EditPanel.js");
            AddResource(container, "libs/B4/view/manorg/RegistryEdit.js");
            AddResource(container, "libs/B4/view/manorg/RegistryGrid.js");
            AddResource(container, "libs/B4/view/realityobj/CurentRepairEditWindow.js");
            AddResource(container, "libs/B4/view/realityobj/CurentRepairGrid.js");
            AddResource(container, "libs/B4/view/realityobj/Grid.js");
            AddResource(container, "libs/B4/view/realityobj/Document/EditWindow.js");
            AddResource(container, "libs/B4/view/realityobj/Document/Grid.js");
            AddResource(container, "libs/B4/view/report/LocalGovernmentReportPanel.js");
            AddResource(container, "libs/B4/view/report/TsjProvidedInfoReportPanel.js");
        }

        

        private void AddResource(IResourceManifestContainer container, string path)
		{

            container.Add(path, string.Format("Bars.Gkh.Regions.Nso.dll/Bars.Gkh.Regions.Nso.{0}", path.Replace("/", ".")));
        }
    }
}
