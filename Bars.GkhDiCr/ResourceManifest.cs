namespace Bars.GkhDiCr
{    
    using Bars.B4;

    public partial class ResourceManifest : ResourceManifestBase
    {

		protected override void BaseInit(IResourceManifestContainer container)
        {  

            AddResource(container, "libs/B4/controller/CopyCr.js");
            AddResource(container, "libs/B4/model/copycr/RealityObj.js");
            AddResource(container, "libs/B4/store/copycr/RealityObj.js");
            AddResource(container, "libs/B4/store/copycr/RealityObjSelect.js");
            AddResource(container, "libs/B4/store/copycr/RealityObjSelected.js");
            AddResource(container, "libs/B4/view/copycr/EditPanel.js");

        }

        

        private void AddResource(IResourceManifestContainer container, string path)
		{

            container.Add(path, string.Format("Bars.GkhDiCr.dll/Bars.GkhDiCr.{0}", path.Replace("/", ".")));
        }
    }
}
