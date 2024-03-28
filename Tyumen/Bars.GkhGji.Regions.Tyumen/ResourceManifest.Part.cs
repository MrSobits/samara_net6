namespace Bars.GkhGji.Regions.Tyumen
{
    using Bars.B4;
    using Bars.B4.Modules.ExtJs;
    using Bars.GkhGji.Regions.Tyumen.Enums;
    using Bars.GkhGji.Regions.Tyumen.Enums.Egrn;

    public partial class ResourceManifest
    {
        protected override void AdditionalInit(IResourceManifestContainer container)
        {
            container.Add("libs/B4/enums/TypeDocumentGji.js", new ExtJsEnumResource<TypeDocumentGji>("B4.enums.TypeDocumentGji"));
            container.Add("libs/B4/enums/TypeStage.js", new ExtJsEnumResource<TypeStage>("B4.enums.TypeStage"));
            container.Add("libs/B4/enums/OMSNoticeResult.js", new ExtJsEnumResource<OMSNoticeResult>("B4.enums.OMSNoticeResult"));
            if (container.Resources.ContainsKey("~/libs/B4/enums/GisGkhTypeRequest.js"))
            {
                container.Resources.Remove("libs/B4/enums/GisGkhTypeRequest.js");
            }
            container.Add("libs/B4/enums/GisGkhTypeRequest.js", new ExtJsEnumResource<GisGkhTypeRequest>("B4.enums.GisGkhTypeRequest"));
            container.Add("libs/B4/enums/RequestDataType.js", new ExtJsEnumResource<RequestDataType>("B4.enums.RequestDataType"));
            container.Add("libs/B4/enums/RequestType.js", new ExtJsEnumResource<RequestType>("B4.enums.RequestType"));
            container.Add("libs/B4/enums/RequestState.js", new ExtJsEnumResource<RequestState>("B4.enums.RequestState"));
            container.Add("libs/B4/enums/SMEVFileType.js", new ExtJsEnumResource<SMEVFileType>("B4.enums.SMEVFileType"));
        }
    }
}
