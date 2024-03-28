namespace Bars.Gkh.Overhaul.Regions.Saratov
{    
    using Bars.B4;

    public partial class ResourceManifest : ResourceManifestBase
    {

		protected override void BaseInit(IResourceManifestContainer container)
        {  

            AddResource(container, "libs/B4/controller/report/RealtyObjectCertificationControl.js");
            AddResource(container, "libs/B4/view/report/RealtyObjectCertificationControlPanel.js");

            AddResource(container, "resources/RealtyObjectCertificationControl.xlsx");
        }

        

        private void AddResource(IResourceManifestContainer container, string path)
		{

            container.Add(path, string.Format("Bars.Gkh.Overhaul.Regions.Saratov.dll/Bars.Gkh.Overhaul.Regions.Saratov.{0}", path.Replace("/", ".")));
        }
    }
}
