namespace Bars.Gkh.Regions.Msk
{
    using B4.Modules.Reports;
    using B4.Modules.Tasks.Common.Service;

    using Bars.B4;

    using Bars.B4.IoC;
    using Bars.Gkh.Import;
    using Bars.Gkh.Regions.Msk.Import.CommonRealtyObjectImport;
    using Bars.Gkh.Regions.Msk.Services;
    using Bars.Gkh.Regions.Msk.Services.Impl;
    using Castle.MicroKernel.Registration;
    using Import;
    using Reports;

    using Utils;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            Container.RegisterNavigationProvider<NavigationProvider>();
            Container.RegisterPermissionMap<PermissionMap>();
            Container.RegisterTransient<IService, Service>();
            Container.RegisterTransient<IClientRouteMapRegistrar, ClientRouteMapRegistrar>();
            Container.RegisterTransient<IResourceManifest, ResourceManifest>("Gkh.Regions.Msk resources");
            Container.RegisterImport<MskDpkrImport>();
            Container.RegisterImport<MskCeoStateForServiceImport>();
            Container.RegisterImport<MskOverhaulImport>();
            Container.RegisterImport<MskCeoStateImport>();
            Container.RegisterImport<MskCeoPointImport>();
            Container.RegisterImport<RoImport>();

            Container.RegisterTransient<IPrintForm, TypeWorkReport>("TypeWorkReport");
        }
    }
}
