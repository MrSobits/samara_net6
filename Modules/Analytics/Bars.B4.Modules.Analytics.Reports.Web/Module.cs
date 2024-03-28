namespace Bars.B4.Modules.Analytics.Reports.Web
{
    using System.IO;

    using Bars.B4.Alt;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Analytics.Reports.Entities;
    using Bars.B4.Modules.Analytics.Reports.Web.Controllers;
    using Bars.B4.Modules.Analytics.Reports.Web.DomainService;
    using Bars.B4.Modules.Analytics.Reports.Web.ViewModels;
    using Bars.B4.Windsor;


    /// <summary>
    /// Модуль
    /// </summary>
    public class Module : AssemblyDefinedModule
    {
        /// <summary>
        /// 
        /// </summary>
        public override void Install()
        {
            // Controller
            this.Container.RegisterController<CodedReportController>();
            this.Container.RegisterController<DataController<ReportCustom>>();
            this.Container.RegisterController<CodedReportManagerController>();
            this.Container.RegisterController<EmptyTemplateController>();
            this.Container.RegisterController<ReportGeneratorController>();
            this.Container.RegisterController<StoredReportController>();
            this.Container.RegisterController<ReportPanelController>();
            this.Container.RegisterController<CatalogRegistryController>();
            this.Container.RegisterController<ExternalReportController>();
            this.Container.RegisterAltDataController<ReportParamGkh>();
            this.Container.RegisterController<Kp60ReportController>();

            // Client routes
            this.Container.RegisterTransient<INavigationProvider, ReportsNavigationProvider>();
            this.Container.RegisterTransient<IClientRouteMapRegistrar, ClientRouteMapRegistrar>();

            // ViewModels
            this.Container.RegisterViewModel<ReportCustom, ReportCustomViewModel>();
            this.Container.RegisterViewModel<StoredReport, StoredReportViewModel>();
            this.Container.RegisterViewModel<ReportParamGkh, ReportParamGkhViewModel>();

            //Services
            this.Container.RegisterTransient<IStimulService, CustomStimulService>(CustomStimulService.Code);
            this.Container.RegisterTransient<IStimulService, StoredStimulService>(StoredStimulService.Code);

            // Регистрируем манифест ресурсов 
            this.Container.RegisterResourceManifest<ResourceManifest>();
        }
    }
}
