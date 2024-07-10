namespace Bars.B4.Modules.Analytics.Reports.Web
{    
    using Bars.B4;

	/// <summary>
    /// Манифест ресурсов.
    /// Используется для регистрации ресурсов модуля в общем контейере ресурсов.
    /// </summary>
    public partial class ResourceManifest : ResourceManifestBase
    {
        protected override void BaseInit(IResourceManifestContainer container)
        {	
            this.RegisterResource(container, "libs/B4/controller/BaseReportController.js");
            this.RegisterResource(container, "libs/B4/controller/PrintFormCategory.js");
            this.RegisterResource(container, "libs/B4/controller/al/Kp60Reports.js");
            this.RegisterResource(container, "libs/B4/controller/al/ReportCustom.js");
            this.RegisterResource(container, "libs/B4/controller/al/ReportPanel.js");
            this.RegisterResource(container, "libs/B4/controller/al/StoredReport.js");
            this.RegisterResource(container, "libs/B4/helpers/al/ReportParamFieldBuilder.js");
            this.RegisterResource(container, "libs/B4/model/PrintFormCategory.js");
            this.RegisterResource(container, "libs/B4/model/ReportHistory.js");
            this.RegisterResource(container, "libs/B4/model/al/DataSourceNode.js");
            this.RegisterResource(container, "libs/B4/model/al/Role.js");
            this.RegisterResource(container, "libs/B4/model/al/SqlQueryParam.js");
            this.RegisterResource(container, "libs/B4/store/PrintFormCategory.js");
            this.RegisterResource(container, "libs/B4/store/ReportCustomSelect.js");
            this.RegisterResource(container, "libs/B4/store/ReportHistory.js");
            this.RegisterResource(container, "libs/B4/store/ReportHistoryParam.js");
            this.RegisterResource(container, "libs/B4/view/al/DataSourceAddWindow.js");
            this.RegisterResource(container, "libs/B4/view/al/ExternalLinkWindow.js");
            this.RegisterResource(container, "libs/B4/view/al/Kp60Reports.js");
            this.RegisterResource(container, "libs/B4/view/al/ParameterWindow.js");
            this.RegisterResource(container, "libs/B4/view/al/ReportCustomEdit.js");
            this.RegisterResource(container, "libs/B4/view/al/ReportCustomGrid.js");
            this.RegisterResource(container, "libs/B4/view/al/ReportPanel.js");
            this.RegisterResource(container, "libs/B4/view/al/ReportParamsContainer.js");
            this.RegisterResource(container, "libs/B4/view/al/SqlQueryParamSelectField.js");
            this.RegisterResource(container, "libs/B4/view/al/StoredReportEdit.js");
            this.RegisterResource(container, "libs/B4/view/al/StoredReportGrid.js");
            this.RegisterResource(container, "libs/B4/view/reportHistory/Grid.js");
            this.RegisterResource(container, "libs/B4/view/reportHistory/ParamWindow.js");
            this.RegisterResource(container, "content/ReportPanel.View.css");
            this.RegisterResource(container, "content/img/search.png");
            this.RegisterResource(container, "content/img/tool-sprites.png");
        }             
    }
}
