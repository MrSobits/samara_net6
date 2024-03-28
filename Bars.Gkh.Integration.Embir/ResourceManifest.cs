namespace Bars.Gkh.Integration.Embir
{    
    using Bars.B4;

    public partial class ResourceManifest : ResourceManifestBase
    {

		protected override void BaseInit(IResourceManifestContainer container)
        {  

            AddResource(container, "libs/B4/controller/import/Embir.js");
            AddResource(container, "libs/B4/store/import/EmbirLog.js");
            AddResource(container, "libs/B4/view/import/EmbirLogGrid.js");
            AddResource(container, "libs/B4/view/import/EmbirPanel.js");

        }

        

        private void AddResource(IResourceManifestContainer container, string path)
		{

            container.Add(path, string.Format("Bars.Gkh.Integration.Embir.dll/Bars.Gkh.Integration.Embir.{0}", path.Replace("/", ".")));
        }
    }
}
