namespace Sobits.GisGkh
{
    using Bars.B4;
    using Bars.B4.Modules.ExtJs;
    using GisGkhLibrary.NsiServiceAsync;
    using Sobits.GisGkh.Enums;

    //using Sobits.GisGkh.Enums;

    public partial class ResourceManifest
    {
        protected override void AdditionalInit(IResourceManifestContainer container)
        {
            container.Add("libs/B4/enums/GisGkhRequestState.js", new ExtJsEnumResource<GisGkhRequestState>("B4.enums.GisGkhRequestState"));
            if (!container.Resources.ContainsKey("~/libs/B4/enums/GisGkhTypeRequest.js"))
            {
                container.Add("libs/B4/enums/GisGkhTypeRequest.js", new ExtJsEnumResource<GisGkhTypeRequest>("B4.enums.GisGkhTypeRequest"));
            }
            container.Add("libs/B4/enums/GisGkhListGroup.js", new ExtJsEnumResource<GisGkhLibrary.NsiCommonAsync.ListGroup>("B4.enums.GisGkhListGroup"));
            container.Add("libs/B4/enums/GisGkhPlanType.js", new ExtJsEnumResource<GisGkhPlanType>("B4.enums.GisGkhPlanType"));
            container.Add("libs/B4/enums/GisGkhExaminationState.js", new ExtJsEnumResource<GisGkhExaminationState>("B4.enums.GisGkhExaminationState"));
            container.Add("libs/B4/enums/GisGkhDataProviderNsiType.js", new ExtJsEnumResource<GisGkhDataProviderNsiType>("B4.enums.GisGkhDataProviderNsiType"));
            container.Add("libs/B4/enums/exportDataProviderNsiItemRequestRegistryNumber.js", new ExtJsEnumResource<exportDataProviderNsiItemRequestRegistryNumber>("B4.enums.exportDataProviderNsiItemRequestRegistryNumber"));
            //container.RegisterExtJsEnum<GisGkhRequestState>();
        }
    }
}