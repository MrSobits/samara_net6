namespace Bars.Gkh.Regions.Yanao
{    
    using Bars.B4;

    public partial class ResourceManifest : ResourceManifestBase
    {

		protected override void BaseInit(IResourceManifestContainer container)
        {  


        }

        private void AddResource(IResourceManifestContainer container, string path)
		{
            container.Add(path, string.Format("Bars.Gkh.Regions.Yanao.dll/Bars.Gkh.Regions.Yanao.{0}", path.Replace("/", ".")));
        }
    }
}
