namespace Bars.GisIntegration.Smev
{    
    using Bars.B4;

    public partial class ResourceManifest : ResourceManifestBase
    {

		protected override void BaseInit(IResourceManifestContainer container)
        {  


			this.AdditionalInit(container);
        }

        private void AddResource(IResourceManifestContainer container, string path)
		{

            container.Add(path, string.Format("Bars.GisIntegration.CapitalRepair.dll/Bars.GisIntegration.Smev.{0}", path.Replace("/", ".")));
        }
    }
}
