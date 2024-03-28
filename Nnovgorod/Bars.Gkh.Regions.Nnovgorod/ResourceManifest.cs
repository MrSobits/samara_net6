namespace Bars.Gkh.Regions.Nnovgorod
{    
    using Bars.B4;

    public partial class ResourceManifest : ResourceManifestBase
    {

		protected override void BaseInit(IResourceManifestContainer container)
        {  

            AddResource(container, "libs/B4/TextValuesOverride.js");
            AddResource(container, "libs/B4/view/dict/zonalinspection/EditWindow.js");
        }
		        

        private void AddResource(IResourceManifestContainer container, string path)
		{

            container.Add(path, string.Format("Bars.Gkh.Regions.Nnovgorod.dll/Bars.Gkh.Regions.Nnovgorod.{0}", path.Replace("/", ".")));
        }
    }
}
