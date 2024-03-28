namespace Bars.B4.Modules.Analytics.Reports.Web
{
    using Bars.B4;
    using Bars.B4.Modules.Analytics.Reports.Entities;
    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Analytics.Reports.Params;
    using Bars.B4.Modules.ExtJs;

    public partial class ResourceManifest
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="container"></param>
        protected override void AdditionalInit(IResourceManifestContainer container)
        {
            var reportCustom = new ExtJsModelResource<ReportCustom>("B4.model.al.ReportCustom");
            var metaData = reportCustom.GetModelMeta();
            metaData.Controller("ReportCustom");
            metaData.AddProperty<string>("ReportName");
            container.Add("libs/B4/model/al/ReportCustom.js", reportCustom);

            var storedReport = new ExtJsModelResource<StoredReport>("B4.model.al.StoredReport");
            var storedReportMetaData = storedReport.GetModelMeta();
            storedReportMetaData.Controller("StoredReport");
            storedReportMetaData.Exclude(x => x.DataSources);
            storedReportMetaData.AddProperty("HasParams", typeof(bool));
            storedReportMetaData.AddProperty("Token", typeof(string));
            storedReportMetaData.AddProperty<string>("DataSourcesNames");
            storedReportMetaData.AddProperty<string>("DataSourcesIds");
            storedReportMetaData.GetField("ForAll").DefaultValue = true;
            container.Add("libs/B4/model/al/StoredReport.js", storedReport);

            var paramModel = new ExtJsModelResource<ReportParamGkh>("B4.model.al.ReportParam");
            paramModel.GetModelMeta().Controller("ReportParamGkh");
            paramModel.GetModelMeta().AddProperty<Catalog>("Catalog");
            paramModel.GetModelMeta().AddProperty<Enum>("Enum");
            paramModel.GetModelMeta().AddProperty<string>("Id");
            container.Add("libs/B4/model/al/ReportParam.js", paramModel);

            container.Add("libs/B4/enums/al/ReportType.js", new ExtJsEnumResource<StoredReportType>("B4.enums.al.ReportType"));
            container.Add("libs/B4/enums/al/ReportEncoding.js", new ExtJsEnumResource<ReportEncoding>("B4.enums.al.ReportEncoding"));
            container.Add("libs/B4/enums/al/ReportPrintFormat.js", new ExtJsEnumResource<ReportPrintFormat>("B4.enums.al.ReportPrintFormat"));

            RegisterResource(container, "Views/StimulDesigner/Index.cshtml");
        }
    }
}
