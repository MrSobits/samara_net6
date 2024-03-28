namespace Bars.GkhGji.Regions.Khakasia
{
    using B4;
    using B4.Modules.ExtJs;
    using Enums;

    public partial class ResourceManifest
    {
        protected override void AdditionalInit(IResourceManifestContainer container)
        {
            container.Add("libs/B4/enums/TypeDefinitionResolPros.js", new ExtJsEnumResource<TypeDefinitionResolPros>("B4.enums.TypeDefinitionResolPros"));
            container.Add("libs/B4/enums/TypeDefinitionResolution.js", new ExtJsEnumResource<TypeDefinitionResolution>("B4.enums.TypeDefinitionResolution"));
        }
    }
}