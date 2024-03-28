namespace Bars.Gkh.Gku
{
    using Bars.B4;
    using Bars.B4.Modules.ExtJs;
    using Bars.Gkh.Gku.Enums;

    public partial class ResourceManifest
    {
        protected override void AdditionalInit(IResourceManifestContainer container)
        {
            container.Add("libs/B4/enums/ServiceKindGku.js", new ExtJsEnumResource<ServiceKindGku>("B4.enums.ServiceKindGku"));
        }
    }
}