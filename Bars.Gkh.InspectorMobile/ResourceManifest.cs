namespace Bars.Gkh.InspectorMobile
{
    using Bars.B4;

    public partial class ResourceManifest : ResourceManifestBase
    {
        protected override void BaseInit(IResourceManifestContainer container)
        {
            this.AddResource(container, "libs/B4/controller/MpRole.js");
            this.AddResource(container, "libs/B4/model/MpRole.js");
            this.AddResource(container, "libs/B4/store/MpRole.js");
            this.AddResource(container, "libs/B4/store/MpRoleForSelected.js");
            this.AddResource(container, "libs/B4/ux/config/MpRole.js");
            this.AddResource(container, "libs/B4/view/MpRoleGrid.js");
        }

        private void AddResource(IResourceManifestContainer container, string path)
        {
            container.Add(path, string.Format("Bars.Gkh.InspectorMobile.dll/Bars.Gkh.InspectorMobile.{0}", path.Replace("/", ".")));
        }
    }
}
