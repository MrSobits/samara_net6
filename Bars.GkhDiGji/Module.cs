namespace Bars.GkhDiGji.Contracts
{
    using B4;

    using Bars.GkhDiGji.Permissions;

    using Castle.MicroKernel.Registration;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            Container.Register(
                Component.For<IResourceManifest>().Named("Bars.GkhDiGji resources").ImplementedBy<ResourceManifest>()
                .LifeStyle.Transient);

            Container.Register(Component.For<IPermissionSource>().ImplementedBy<GkhDiGjiPermissionMap>());
        }
    }
}

