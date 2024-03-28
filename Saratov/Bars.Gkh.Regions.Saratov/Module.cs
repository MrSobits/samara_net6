namespace Bars.Gkh.Regions.Saratov
{
    using Bars.B4;
    using Bars.B4.IoC;

    using Castle.MicroKernel.Registration;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            Component.For<IResourceManifest>().ImplementedBy<ResourceManifest>().LifeStyle.Transient.RegisterIn(Container);
        }
    }
}
