using Bars.B4;
using Bars.B4.IoC;
using Bars.B4.Windsor;
using Bars.Gkh.Integration.Embir.Import;

namespace Bars.Gkh.Integration.Embir
{
    using Utils;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            Container.RegisterController<ImportEmbirController>();
            Container.RegisterImport<ImportContragentEmbir>();
            Container.RegisterImport<ImportOrgForm>();
            Container.RegisterImport<ImportWallMaterial>();
            Container.RegisterImport<ImportPersonalAccount>();
            Container.RegisterImport<ImportRealityObject>();

            Container.RegisterNavigationProvider<NavigationProvider>();
            Container.RegisterResourceManifest<ResourceManifest>();
            Container.RegisterTransient<IClientRouteMapRegistrar, ClientRouteMapRegistrar>();
            Container.RegisterPermissionMap<EmbirPermissionMap>();
        }
    }
}