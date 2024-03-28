namespace Bars.Gkh.Regions.Perm
{
    using Bars.B4;
    using Bars.B4.Modules.ExtJs;
    using Bars.Gkh.Enums;

    public partial class ResourceManifest
    {
        protected override void AdditionalInit(IResourceManifestContainer container)
        {
            this.RegisterEnums(container);
        }

        private void RegisterEnums(IResourceManifestContainer container)
        {
            container.RegisterExtJsEnum<TypeManOrgTerminationLicense>();
        }
    }
}