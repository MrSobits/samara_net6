namespace Bars.Gkh.Gis.Reports.UI
{
    using B4;

    using Bars.B4.Modules.ExtJs;

    using Enums;
    using Gis.Reports.Enums;

    public partial class ResourceManifest
    {
        protected override void AdditionalInit(IResourceManifestContainer container)
        {
            //container.RegisterExtJsEnum<TypeReportCollection>();
            container.Add("libs/B4/enums/TypeReportCollection.js",
                          new ExtJsEnumResource<TypeReportCollection>("B4.enums.TypeReportCollection"));

            container.RegisterExtJsEnum<ReportService>();
        }
    }
}