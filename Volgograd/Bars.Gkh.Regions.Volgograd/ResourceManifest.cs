namespace Bars.Gkh.Regions.Volgograd
{    
    using Bars.B4;

    public partial class ResourceManifest : ResourceManifestBase
    {

		protected override void BaseInit(IResourceManifestContainer container)
        {  

            AddResource(container, "libs/B4/controller/report/RepairPlanning.js");
            AddResource(container, "libs/B4/model/dict/ConstructiveElement.js");
            AddResource(container, "libs/B4/model/dict/MeteringDevice.js");
            AddResource(container, "libs/B4/model/realityobj/ConstructiveElement.js");
            AddResource(container, "libs/B4/view/dict/ConstructiveElement/EditWindow.js");
            AddResource(container, "libs/B4/view/dict/ConstructiveElement/Grid.js");
            AddResource(container, "libs/B4/view/dict/MeteringDevice/EditWindow.js");
            AddResource(container, "libs/B4/view/dict/MeteringDevice/Grid.js");
            AddResource(container, "libs/B4/view/realityobj/ConstructiveElementGrid.js");
            AddResource(container, "libs/B4/view/realityobj/MeteringDeviceEditWindow.js");
            AddResource(container, "libs/B4/view/realityobj/MeteringDeviceGrid.js");
            AddResource(container, "libs/B4/view/report/RepairPlanningPanel.js");

        }

        private void AddResource(IResourceManifestContainer container, string path)
		{
            container.Add(path, string.Format("Bars.Gkh.Regions.Volgograd.dll/Bars.Gkh.Regions.Volgograd.{0}", path.Replace("/", ".")));
        }
    }
}
