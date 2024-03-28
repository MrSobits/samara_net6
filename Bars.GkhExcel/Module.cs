namespace Bars.GkhExcel
{
    using Bars.B4;

    using Castle.MicroKernel.Registration;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
          this.Container.Register(Component.For<IGkhExcelProvider>().Named("ExcelEngineProvider").ImplementedBy<GkhExcelProvider>().LifeStyle.Transient);
        }
    }
}