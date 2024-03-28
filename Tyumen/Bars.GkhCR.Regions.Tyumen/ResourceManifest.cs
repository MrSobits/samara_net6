namespace Bars.GkhCR.Regions.Tyumen
{    
    using Bars.B4;

    public partial class ResourceManifest : ResourceManifestBase
    {

		protected override void BaseInit(IResourceManifestContainer container)
        {  

            AddResource(container, "libs/B4/controller/objectcr/TypeWorkCr.js");
            AddResource(container, "libs/B4/model/objectcr/TypeWorkCr.js");
            AddResource(container, "libs/B4/model/objectcr/TypeWorkCrPotentialWorks.js");
            AddResource(container, "libs/B4/model/objectcr/TypeWorkCrWorks.js");
            AddResource(container, "libs/B4/store/objectcr/TypeWorkCrPotentialWorks.js");
            AddResource(container, "libs/B4/store/objectcr/TypeWorkCrWorks.js");
            AddResource(container, "libs/B4/view/objectcr/TypeWorkCREditWindow.js");
            AddResource(container, "libs/B4/view/objectcr/TypeWorkCrPotentialWorksGrid.js");
            AddResource(container, "libs/B4/view/objectcr/TypeWorkCrWorksGrid.js");

			AdditionalInit(container);

        }

        private void AddResource(IResourceManifestContainer container, string path)
		{
            container.Add(path, string.Format("Bars.GkhCR.Regions.Tyumen.dll/Bars.GkhCR.Regions.Tyumen.{0}", path.Replace("/", ".")));
        }
    }
}
