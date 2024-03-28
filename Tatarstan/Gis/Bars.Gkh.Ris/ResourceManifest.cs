namespace Bars.Gkh.Ris
{    
    using Bars.B4;

    public partial class ResourceManifest : ResourceManifestBase
    {

		protected override void BaseInit(IResourceManifestContainer container)
        {  

            AddResource(container, "libs/B4/controller/integrations/GisIntegrationSettings.js");
            AddResource(container, "libs/B4/view/integrations/gis/SettingsPanel.js");
        }

        private void AddResource(IResourceManifestContainer container, string path)
		{

            container.Add(path, string.Format("Bars.Gkh.Ris.dll/Bars.Gkh.Ris.{0}", path.Replace("/", ".")));
        }
    }
}
