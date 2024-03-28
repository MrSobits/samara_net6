namespace Bars.Gkh.Regions.Tomsk
{     
    using Bars.B4;

    public partial class ResourceManifest : ResourceManifestBase
    {
		protected override void BaseInit(IResourceManifestContainer container)
        {  
            AddResource(container, "libs/B4/controller/Contragent.js");
            AddResource(container, "libs/B4/controller/contragent/Edit.js");
            AddResource(container, "libs/B4/model/administration/Operator.js");
            AddResource(container, "libs/B4/model/manorg/contract/Owners.js");
            AddResource(container, "libs/B4/view/administration/operator/EditWindow.js");
            AddResource(container, "libs/B4/view/contragent/AddWindow.js");
            AddResource(container, "libs/B4/view/contragent/EditPanel.js");
            AddResource(container, "libs/B4/view/dict/zonalinspection/EditWindow.js");
            AddResource(container, "libs/B4/view/manorg/contract/OwnersEditWindow.js");
        }

        private void AddResource(IResourceManifestContainer container, string path)
		{
            container.Add(path, string.Format("Bars.Gkh.Regions.Tomsk.dll/Bars.Gkh.Regions.Tomsk.{0}", path.Replace("/", ".")));
        }
    }
}
