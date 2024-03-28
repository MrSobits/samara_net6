namespace Bars.Gkh.Overhaul.Regions.Saha
{    
    using Bars.B4;

    public partial class ResourceManifest : ResourceManifestBase
    {

		protected override void BaseInit(IResourceManifestContainer container)
        {  

            AddResource(container, "libs/B4/controller/RealEstateTypeRate.js");
            AddResource(container, "libs/B4/model/RealEstateTypeRate.js");
            AddResource(container, "libs/B4/view/realestatetype/RealEstateTypeRateNotLivingAreaGrid.js");
        }

        private void AddResource(IResourceManifestContainer container, string path)
		{

            container.Add(path, string.Format("Bars.Gkh.Overhaul.Regions.Saha.dll/Bars.Gkh.Overhaul.Regions.Saha.{0}", path.Replace("/", ".")));
        }
    }
}
