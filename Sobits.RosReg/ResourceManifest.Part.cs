namespace Sobits.RosReg
{
    using Bars.B4;
    using Bars.B4.Modules.ExtJs;

    using Sobits.RosReg.Enums;

    public partial class ResourceManifest
    {
        protected override void AdditionalInit(IResourceManifestContainer container)
        {
            container.Add("libs/B4/enums/ExtractType.js", new ExtJsEnumResource<ExtractType>("B4.enums.ExtractType"));
            container.RegisterExtJsEnum<ExtractType>();
        }
    }
}