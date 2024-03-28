namespace Bars.GkhDI.Regions.Samara
{
    using B4.Modules.Reports;
    using Bars.B4;
    using Bars.B4.IoC;
    
    using Bars.GkhDI.Regions.Samara.Report;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            // Регистрируем манифест ресурсов 
            Container.RegisterResourceManifest<ResourceManifest>();

            Container.RegisterTransient<IPrintForm, PercentCalculationReportSamara>("DI Report.PercentCalculationReportSamara");
        }
    }
}