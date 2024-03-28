namespace Bars.Gkh.RegOperator.Regions.Tyumen
{
    using Bars.B4;
    using Bars.B4.Modules.ExtJs;
    using Bars.Gkh.RegOperator.Regions.Tyumen.Enums;

    public partial class ResourceManifest
    {
        protected override void AdditionalInit(IResourceManifestContainer container)
        {
            container.Add("libs/B4/enums/RequestStatePersonEnum.js", new ExtJsEnumResource<RequestStatePersonEnum>("B4.enums.RequestStatePersonEnum"));
        }
    }
}