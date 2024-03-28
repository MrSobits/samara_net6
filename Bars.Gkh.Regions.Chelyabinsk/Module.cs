namespace Bars.Gkh.Regions.Chelyabinsk
{
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Reports;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService.AddressMatching;
    using Bars.Gkh.Regions.Chelyabinsk.DomainService;
    using Bars.Gkh.Regions.Chelyabinsk.Reports;
    using Bars.Gkh.TextValues;
    using Bars.B4.Windsor;
    using Controllers;
    using Entities;
    using Imports;
    using Utils;

    using Castle.MicroKernel.Registration;
    using ViewModel;
    using System;
    using Bars.Gkh.DomainService.GisGkhRegional;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            this.Container.RegisterResources<ResourceManifest>();
            this.Container.Register(Component.For<IPermissionSource>().ImplementedBy<PermissionMap>());
            //this.Container.RegisterTransient<INavigationProvider, RealityObjMenuProvider>();

            this.Container.Register(
                Component.For<IPrintForm>().ImplementedBy<UnderstandingHomeReport>().Named("UnderstandingHomeReport").LifestyleTransient());
            this.Container.ReplaceTransient<IPrintForm, Overhaul.Hmao.Reports.PublishedDpkrByWorkReport, PublishedDpkrByWorkReport>();

            this.Container.RegisterTransient<IImportAddressMatchService, ChesImportAddressMatchService>();

            this.OverrideMenuItems();


            this.RegisterControllers();
            this.RegisterViewModel();
            this.Container.RegisterTransient<IClientRouteMapRegistrar, ClientRouteMapRegistrar>();
            this.Container.RegisterTransient<INavigationProvider, NavigationProvider>("Chelyabinsk menu");
            this.Container.RegisterImport<RosRegExtractImport>();
            this.ReplaceComponents();


        }

        private void ReplaceComponents()
        {
            Container.ReplaceComponent<IGisGkhRegionalService>(
                typeof(Bars.Gkh.DomainService.GisGkhRegional.Impl.GisGkhRegionalService),
                Component.For<IGisGkhRegionalService>().ImplementedBy<Bars.Gkh.Regions.Chelyabinsk.DomainService.GisGkhRegional.Impl.GisGkhRegionalService>().LifeStyle.Transient);
        }

        private void OverrideMenuItems()
        {
            this.Container.UsingForResolved<IMenuItemText>((container, menuItemText) =>
                {
                    menuItemText.Override("Исковое заявление (сведения о собственниках)", "Исковое заявление (сведения о собственниках) 512 ФЗ");
                    menuItemText.Override("Исковое заявление (лицевой счет)", "Исковое заявление (лицевой счет) 512 ФЗ");
                    menuItemText.Override("Исковое заявление", "Исковое заявление 512 ФЗ");
                });
        }

        private void RegisterControllers()
        {
            this.Container.RegisterController<RosRegExtractImportController>();
            this.Container.RegisterController<ExtractPrintController>();
            this.Container.RegisterAltDataController<RosRegExtractBig>();
            this.Container.RegisterAltDataController<RosRegExtractBigOwner>();
        }

        private void RegisterViewModel()
        {
            this.Container.RegisterViewModel<RosRegExtractBig, RosRegExtractBigViewModel>();
            this.Container.RegisterViewModel<RosRegExtractBigOwner, RosRegExtractBigOwnerViewModel>();
        }
    }
}