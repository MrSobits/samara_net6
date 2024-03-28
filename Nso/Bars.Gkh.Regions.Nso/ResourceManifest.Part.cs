namespace Bars.Gkh.Regions.Nso
{
    using B4;
    using B4.Modules.ExtJs;
    using Enums;

    public partial class ResourceManifest
    {
        protected override void AdditionalInit(IResourceManifestContainer container)
        {
            container.Add("libs/B4/enums/RealityObjectDocumentType.js", new ExtJsEnumResource<RealityObjectDocumentType>("B4.enums.RealityObjectDocumentType"));
        }
    }
}
