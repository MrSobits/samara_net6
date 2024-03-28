namespace Bars.GkhDi.Regions.Nso
{    
    using Bars.B4;

    public partial class ResourceManifest : ResourceManifestBase
    {

		protected override void BaseInit(IResourceManifestContainer container)
        {  

            AddResource(container, "libs/B4/view/finactivity/DocByYearEditWindow.js");
        }

        

        private void AddResource(IResourceManifestContainer container, string path)
		{

            container.Add(path, string.Format("Bars.GkhDi.Regions.Nso.dll/Bars.GkhDi.Regions.Nso.{0}", path.Replace("/", ".")));
        }
    }
}
