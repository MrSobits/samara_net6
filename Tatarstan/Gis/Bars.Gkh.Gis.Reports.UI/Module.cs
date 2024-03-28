namespace Bars.Gkh.Gis.Reports.UI
{
    using B4;
    using B4.IoC;
    using B4.Windsor;
    using Controller;
    using DomainService;
    using DomainService.Impl;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            Container.RegisterTransient<IResourceManifest, ResourceManifest>();

            Container.RegisterTransient<IReportAreaService, ReportAreaService>();
            Container.RegisterController<ReportAreaController>();

            Container.RegisterTransient<IReportMunicipalityService, ReportMunicipalityService>();
            Container.RegisterController<ReportMunicipalityController>();
        }
    }
}
