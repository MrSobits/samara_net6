namespace Bars.Gkh.Regions.Saratov
{    
    using Bars.B4;

    public partial class ResourceManifest : ResourceManifestBase
    {

		protected override void BaseInit(IResourceManifestContainer container)
        {  

            AddResource(container, "libs/B4/view/realityobj/EditPanel.js");
        }

        

        private void AddResource(IResourceManifestContainer container, string path)
		{

            container.Add(path, string.Format("Bars.Gkh.Regions.Saratov.dll/Bars.Gkh.Regions.Saratov.{0}", path.Replace("/", ".")));
        }
    }
}
