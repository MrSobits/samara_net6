namespace Bars.GkhDiCr
{
    using B4;

    using Bars.B4.Windsor;
    using Bars.GkhDi.GroupAction;
    using Bars.GkhDiCr.Controllers;
    using Bars.GkhDiCr.GroupAction;
    using Bars.GkhDiCr.Permissions;

    using Castle.MicroKernel.Registration;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            Container.Register(Component.For<IResourceManifest>().Named("GkhDiCr resources").ImplementedBy<ResourceManifest>().LifeStyle.Transient);

            Container.RegisterPermissionMap<GkhDiCrPermissionMap>();

            // Сущности
            Container.RegisterController<RealityObjCopyCrController>();

            // Групповые операции
            Container.Register(Component.For<IDiGroupAction>().ImplementedBy<DiCopyCrServiceGroupAction>());
        }
    }
}

