namespace Bars.Gkh.Overhaul.Regions.Saha
{
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Windsor;
    using Bars.Gkh.Entities.RealEstateType;
    using Bars.Gkh.Overhaul.Hmao.DomainService;
    using Bars.Gkh.Overhaul.Hmao.DomainService.Impl;
    using Bars.Gkh.Overhaul.Hmao.ViewModel;
    using Bars.Gkh.Overhaul.Regions.Saha.Controllers;
    using Bars.Gkh.Overhaul.Regions.Saha.DomainService.Impl;
    using Bars.Gkh.Overhaul.Regions.Saha.Entities.RealEstateType;

    using Castle.MicroKernel.Registration;

    public partial class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            Container.Register(
                Component.For<IResourceManifest>()
                         .Named("GkhOverhaul.Regions.Saha resources")
                         .ImplementedBy<ResourceManifest>()
                         .LifeStyle.Transient);

            Container.Register(Component.For<IPermissionSource>().ImplementedBy<OverhaulSahaPermMap>());

            Container.Register(Component.For<INavigationProvider>().ImplementedBy<NavigationProvider>().LifeStyle.Transient);

            Container.Register(Component.For<IClientRouteMapRegistrar>().ImplementedBy<ClientRouteMapRegistrar>().LifestyleTransient());

            Container.ReplaceController<SahaRealEstateTypeRateController>("realestatetyperate");

            Container.ReplaceComponent<IRealEstateTypeRateService>(typeof(RealEstateTypeRateService),
                Component.For<IRealEstateTypeRateService>().ImplementedBy<SahaRealEstateTypeRateService>().LifeStyle.Transient);

            Container.ReplaceComponent<ISubsidyRecordService>(typeof(SubsidyRecordService),
                Component.For<ISubsidyRecordService>().ImplementedBy<SahaSubsidyRecordService>().LifeStyle.Transient);

            Container.ReplaceComponent<IViewModel<RealEstateTypeRate>>(
                typeof(RealEstateTypeRateViewModel),
                Component.For<IViewModel<SahaRealEstateTypeRate>>().ImplementedBy<ViewModel.SahaRealEstateTypeRateViewModel>().LifeStyle.Transient);
        }
    }
}