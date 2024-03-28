namespace Bars.GkhGji.Regions.Yanao
{
    using Bars.B4;
    using Bars.B4.Modules.ExtJs;
    using Bars.GkhGji.Regions.Yanao.Enums;

    public partial class ResourceManifest
    {
        protected override void AdditionalInit(IResourceManifestContainer container)
        {
            container.Add("libs/B4/enums/TypeDocumentGji.js", new ExtJsEnumResource<TypeDocumentGji>("B4.enums.TypeDocumentGji"));
            container.Add("libs/B4/enums/TypeStage.js", new ExtJsEnumResource<TypeStage>("B4.enums.TypeStage"));
        }
    }
}
