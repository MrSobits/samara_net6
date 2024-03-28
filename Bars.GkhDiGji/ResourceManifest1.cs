namespace Bars.GkhDiGji.Contracts
{    
    using Bars.B4;

    public partial class ResourceManifest : ResourceManifestBase
    {

		protected override void BaseInit(IResourceManifestContainer container)
        {  

            AddResource(container, "libs/B4/aspects/permission/adminresp/State.js");
            AddResource(container, "libs/B4/controller/AdminResp.js");
            AddResource(container, "libs/B4/store/adminresp/ResolutionGjiForSelect.js");
            AddResource(container, "libs/B4/store/adminresp/ResolutionGjiForSelected.js");
            AddResource(container, "libs/B4/view/adminresp/EditPanel.js");

        }

        

        private void AddResource(IResourceManifestContainer container, string path)
		{

            container.Add(path, string.Format("Bars.GkhDiGji.dll/Bars.GkhDiGji.{0}", path.Replace("/", ".")));
        }
    }
}
