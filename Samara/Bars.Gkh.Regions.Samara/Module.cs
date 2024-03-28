namespace Bars.Gkh.Regions.Samara
{
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.Regions.Samara.Navigation;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            Container.RegisterTransient<INavigationProvider, RealityObjMenuProvider>(); 
        }
    }
}
