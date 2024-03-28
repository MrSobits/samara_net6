namespace Bars.Gkh.Gis.Reports
{
    using B4;
    using B4.IoC;
    using B4.Modules.Reports;
    using Reports;
    using SqlManager;
    using SqlManager.Impl;

    public class Module: AssemblyDefinedModule
    {
        public override void Install()
        {
        }

        private void RegisterReport<T>() where T : StimulReportDynamicExcel
        {
            Container.RegisterTransient<IPrintForm, T>(string.Format("Bars.Gkh.Gis.Reports {0}", typeof(T).Name));
        }
    }
}