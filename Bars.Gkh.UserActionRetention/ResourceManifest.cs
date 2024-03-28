namespace Bars.Gkh.UserActionRetention
{    
    using Bars.B4;

    public partial class ResourceManifest : ResourceManifestBase
    {

		protected override void BaseInit(IResourceManifestContainer container)
        {  

            AddResource(container, "libs/B4/controller/UserActionRetention.js");
            AddResource(container, "libs/B4/model/AuditLogMap.js");
            AddResource(container, "libs/B4/model/LogEntity.js");
            AddResource(container, "libs/B4/model/LogEntityProperty.js");
            AddResource(container, "libs/B4/model/UserLogin.js");
            AddResource(container, "libs/B4/store/AuditLogMap.js");
            AddResource(container, "libs/B4/store/AuditLogMapForSelect.js");
            AddResource(container, "libs/B4/store/AuditLogMapForSelected.js");
            AddResource(container, "libs/B4/store/LogEntity.js");
            AddResource(container, "libs/B4/store/LogEntityProperty.js");
            AddResource(container, "libs/B4/store/UserLogin.js");
            AddResource(container, "libs/B4/store/UserLoginForSelect.js");
            AddResource(container, "libs/B4/store/UserLoginForSelected.js");
            AddResource(container, "libs/B4/view/logentity/FilterPanel.js");
            AddResource(container, "libs/B4/view/logentity/Grid.js");
            AddResource(container, "libs/B4/view/logentity/Panel.js");
            AddResource(container, "libs/B4/view/logentityproperty/DetailWindow.js");
            AddResource(container, "libs/B4/view/logentityproperty/Grid.js");
        }

        private void AddResource(IResourceManifestContainer container, string path)
		{

            container.Add(path, string.Format("Bars.Gkh.UserActionRetention.dll/Bars.Gkh.UserActionRetention.{0}", path.Replace("/", ".")));
        }
    }
}
