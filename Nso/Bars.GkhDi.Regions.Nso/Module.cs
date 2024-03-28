namespace Bars.GkhDi.Regions.Nso
{
    using B4;
    using B4.IoC;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            Container.RegisterTransient<IResourceManifest, ResourceManifest>();
        }
    }
}