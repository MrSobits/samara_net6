namespace Bars.B4.Modules.FIAS
{    
    using System.IO;

    using Bars.B4;

    public partial class ResourceManifest : ResourceManifestBase
    {
        protected override void BaseInit(IResourceManifestContainer container)
        {  
            AddResource(container, "libs/B4/aspects/FiasTreeEditWindow.js");
            AddResource(container, "libs/B4/controller/FiasController.js");
            AddResource(container, "libs/B4/form/FiasSelectAddress.js");
            AddResource(container, "libs/B4/form/FiasSelectAddressWindow.js");
            AddResource(container, "libs/B4/model/Fias.js");
            AddResource(container, "libs/B4/model/FiasAddress.js");
            AddResource(container, "libs/B4/store/FiasStreet.js");
            AddResource(container, "libs/B4/view/Fias/EditWindow.js");
            AddResource(container, "libs/B4/view/Fias/Panel.js");
            AddResource(container, "libs/B4/view/Fias/StreetGrid.js");
        }
        
		private void AddResource(IResourceManifestContainer container, string path)
        {
            var modifiedPath = TransformFilePathForEmbeddedResource(path);
            container.Add(path, string.Format("Bars.B4.Modules.FIAS.dll/Bars.B4.Modules.FIAS.{0}", modifiedPath));
        }
    }
}
