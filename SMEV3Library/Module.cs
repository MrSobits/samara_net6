namespace SMEV3Library
{
    using Bars.B4;
    using Bars.B4.IoC;
    using SMEV3Library.Providers;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            Container.RegisterTransient<IOptionsProvider, ConfigOptionsProvider>();
        }
    }
}