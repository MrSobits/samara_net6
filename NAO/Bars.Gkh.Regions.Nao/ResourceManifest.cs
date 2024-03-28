namespace Bars.Gkh.Regions.Nao
{    
    using Bars.B4;

    public partial class ResourceManifest : ResourceManifestBase
    {

		protected override void BaseInit(IResourceManifestContainer container)
        {  

            AddResource(container, "libs/B4/aspects/fieldrequirement/RealityObject.js");
            AddResource(container, "libs/B4/aspects/permission/realityobj/Block.js");
            AddResource(container, "libs/B4/controller/ASSberbankClient.js");
            AddResource(container, "libs/B4/model/ASSberbankClient.js");
            AddResource(container, "libs/B4/store/ASSberbankClient.js");
            AddResource(container, "libs/B4/view/assberbank/EditWindow.js");
            AddResource(container, "libs/B4/view/assberbank/Grid.js");
            AddResource(container, "libs/B4/view/realityobj/GeneralParameterContainer.js");
        }

        

        private void AddResource(IResourceManifestContainer container, string path)
		{

            container.Add(path, string.Format("Bars.Gkh.Regions.Nao.dll/Bars.Gkh.Regions.Nao.{0}", path.Replace("/", ".")));
        }
    }
}
