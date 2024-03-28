namespace Bars.BaseProject
{    
    using Bars.B4;

    public partial class ResourceManifest : ResourceManifestBase
    {

		protected override void BaseInit(IResourceManifestContainer container)
        {  

        }
		        

        private void AddResource(IResourceManifestContainer container, string path)
		{

            container.Add(path, string.Format("Bars.BaseProject.dll/Bars.BaseProject.{0}", path.Replace("/", ".")));
        }
    }
}
