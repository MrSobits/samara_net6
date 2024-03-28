namespace Bars.Gkh.Gasu
{     
    using Bars.B4;

    public partial class ResourceManifest : ResourceManifestBase
    {
		protected override void BaseInit(IResourceManifestContainer container)
        {  
            AddResource(container, "libs/B4/controller/GasuIndicator.js");
            AddResource(container, "libs/B4/controller/GasuIndicatorValue.js");
            AddResource(container, "libs/B4/controller/export/ManagementSysImport.js");
            AddResource(container, "libs/B4/controller/export/OverhaulToGasu.js");
            AddResource(container, "libs/B4/model/GasuIndicatorValue.js");
            AddResource(container, "libs/B4/store/GasuIndicator.js");
            AddResource(container, "libs/B4/store/GasuIndicatorValue.js");
            AddResource(container, "libs/B4/view/export/ManagementSysImport.js");
            AddResource(container, "libs/B4/view/export/OverhaulToGasu.js");
            AddResource(container, "libs/B4/view/gasuindicator/EditWindow.js");
            AddResource(container, "libs/B4/view/gasuindicator/Grid.js");
            AddResource(container, "libs/B4/view/gasuindicator/ValueGrid.js");
            AddResource(container, "libs/B4/view/gasuindicator/ValuePanel.js");
        }

        private void AddResource(IResourceManifestContainer container, string path)
		{
            container.Add(path, string.Format("Bars.Gkh.Gasu.dll/Bars.Gkh.Gasu.{0}", path.Replace("/", ".")));
        }
    }
}
