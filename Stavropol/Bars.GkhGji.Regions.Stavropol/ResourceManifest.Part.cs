namespace Bars.GkhGji.Regions.Stavropol
{
    using B4;
    using B4.Modules.ExtJs;
    using Enums;

    public partial class ResourceManifest
    {
        protected override void AdditionalInit(IResourceManifestContainer container)
        {
	        container.RegisterExtJsEnum<TypeDefinitionResolPros>();
        }
    }
}