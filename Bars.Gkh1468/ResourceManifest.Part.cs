namespace Bars.Gkh1468
{
    using Bars.B4;
    using Bars.B4.Modules.ExtJs;
    using Bars.Gkh1468.Enums;

    public partial class ResourceManifest
    {
        protected override void AdditionalInit(IResourceManifestContainer container)
        {
            container.Add("libs/B4/enums/TypeRealObj.js", new ExtJsEnumResource<TypeRealObj>("B4.enums.TypeRealObj"));
            container.Add("libs/B4/enums/RespOrgType.js", new ExtJsEnumResource<RespOrgType>("B4.enums.RespOrgType"));
            container.Add("libs/B4/enums/MetaAttributeType.js", new ExtJsEnumResource<MetaAttributeType>("B4.enums.MetaAttributeType"));
            container.Add("libs/B4/enums/PassportType.js", new ExtJsEnumResource<PassportType>("B4.enums.PassportType"));
            container.Add("libs/B4/enums/TypeContract.js", new ExtJsEnumResource<TypeContract>("B4.enums.TypeContract"));
            container.Add("libs/B4/enums/ValueType.js", new ExtJsEnumResource<ValueType>("B4.enums.ValueType"));

            container.Add("RestService.svc", "Bars.Gkh1468.dll/Bars.Gkh1468.Wcf.RestService.svc");
        }
    }
}
