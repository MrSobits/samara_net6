namespace Bars.GkhGji.Regions.Perm
{
    using Bars.B4;
    using Bars.B4.Modules.ExtJs;
    using Bars.GkhGji.Enums;

    public partial class ResourceManifest
    {
        protected override void AdditionalInit(IResourceManifestContainer container)
        {         
            this.RegisterEnums(container);
        }

        private void RegisterEnums(IResourceManifestContainer container)
        {
            container.RegisterExtJsEnum<ControlType>();
            
        }

    }
}