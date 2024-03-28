namespace Bars.Gkh.Gku.Regions.Kamchatka
{    
    using Bars.B4;

    public partial class ResourceManifest : ResourceManifestBase
    {

		protected override void BaseInit(IResourceManifestContainer container)
        {  

            AddResource(container, "libs/B4/controller/GkuExternalLinks.js");
            AddResource(container, "libs/B4/view/FramePanel.js");
            AddResource(container, "content/css/gkhGkuKamchatka.css");
            AddResource(container, "content/img/billing.png");
        }

        private void AddResource(IResourceManifestContainer container, string path)
		{

            container.Add(path, string.Format("Bars.Gkh.Gku.Regions.Kamchatka.dll/Bars.Gkh.Gku.Regions.Kamchatka.{0}", path.Replace("/", ".")));
        }
    }
}
