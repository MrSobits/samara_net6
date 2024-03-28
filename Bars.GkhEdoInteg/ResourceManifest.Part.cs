namespace Bars.GkhEdoInteg
{
    using B4;

    using Bars.B4.Modules.ExtJs;
    using Bars.GkhEdoInteg.Enums;

    public partial class ResourceManifest
    {
        protected override void AdditionalInit(IResourceManifestContainer container)
        {
            container.Add("libs/B4/enums/ActionIntegrationRow.js", new ExtJsEnumResource<ActionIntegrationRow>("B4.enums.ActionIntegrationRow"));
        }
    }
}
