namespace Bars.GkhGji.Regions.Smolensk
{
    using B4;
    using B4.Modules.ExtJs;
    using Enums;

    public partial class ResourceManifest
    {
        protected override void AdditionalInit(IResourceManifestContainer container)
        {
            container.RegisterExtJsEnum<TypePrescriptionCancel>();
            container.RegisterExtJsEnum<TypeDefinitionResolution>();
            if (container.Resources.ContainsKey("~/libs/B4/enums/GisGkhTypeRequest.js"))
            {
                container.Resources.Remove("~/libs/B4/enums/GisGkhTypeRequest.js");
            }
            container.Add("libs/B4/enums/GisGkhTypeRequest.js", new ExtJsEnumResource<GisGkhTypeRequest>("B4.enums.GisGkhTypeRequest"));
        }
    }
}