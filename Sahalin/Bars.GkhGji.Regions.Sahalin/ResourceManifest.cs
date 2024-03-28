namespace Bars.GkhGji.Regions.Sahalin
{    
    using Bars.B4;

    public partial class ResourceManifest : ResourceManifestBase
    {

		protected override void BaseInit(IResourceManifestContainer container)
        {  
            AddResource(container, "libs/B4/controller/ProtocolGji.js");
            AddResource(container, "libs/B4/view/protocolgji/RequisitePanel.js");
        }
		        

        private void AddResource(IResourceManifestContainer container, string path)
		{

            container.Add(path, string.Format("Bars.GkhGji.Regions.Sahalin.dll/Bars.GkhGji.Regions.Sahalin.{0}", path.Replace("/", ".")));
        }
    }
}
