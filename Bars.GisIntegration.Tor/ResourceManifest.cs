﻿namespace Bars.GisIntegration.Tor
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

            container.Add(path, string.Format("Bars.GisIntegration.Tor.dll/Bars.GisIntegration.Tor.{0}", path.Replace("/", ".")));
        }
    }
}
