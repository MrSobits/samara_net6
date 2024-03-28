namespace Bars.Gkh.Reforma
{
    using Bars.B4;
    using Bars.B4.Modules.ExtJs;
    using Bars.Gkh.Reforma.Enums;

    public partial class ResourceManifest : ResourceManifestBase
    {
        protected override void AdditionalInit(IResourceManifestContainer container)
        {
            container.Add("libs/B4/enums/ReformaRequestStatus.js", new ExtJsEnumResource<RequestStatus>("B4.enums.ReformaRequestStatus"));
            container.Add("libs/B4/enums/dict/ReportingPeriodState.js", new ExtJsEnumResource<ReportingPeriodStateEnum>("B4.enums.dict.ReportingPeriodState"));

            ResourceManifest.RegisterEnums(container);
        }

        private static void RegisterEnums(IResourceManifestContainer container)
        {
            container.RegisterExtJsEnum<TypeIntegration>();
        }
    }
}
