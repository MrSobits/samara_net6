namespace Bars.GkhCr.Regions.Tatarstan
{
    using B4;

    using Bars.B4.Modules.ExtJs;
    using Bars.GkhCr.Regions.Tatarstan.Enum;

    public partial class ResourceManifest
    {
        protected override void AdditionalInit(IResourceManifestContainer container)
        {
            this.RegisterEnums(container);
        }

        private void RegisterEnums(IResourceManifestContainer container)
        {
            container.RegisterExtJsEnum<DpkrProgramType>();
        }
    }
}