using Bars.Gkh.ExecutionAction;
using Bars.Gkh.Overhaul.Regions.Kamchatka.ExecutionAction;

namespace Bars.Gkh.Overhaul.Regions.Kamchatka
{
    using B4;
    using B4.IoC;
    using B4.Modules.Reports;
    using B4.ResourceBundling;
    using B4.Windsor;

    using Castle.MicroKernel.Registration;

    using Gkh.Import;
    using Overhaul.Domain.RealityObjectServices;
    using DomainService;
    using Controllers;
    using Domain.Impl;
    using Overhaul.DomainService;
    using Overhaul.DomainService.Impl;
    using Report;
    using Gkh.Utils;
    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            //Container.RegisterTransient<IObjectCrDpkrDataService, ObjectCrDpkrDataService>();
            Container.ReplaceComponent<IObjectCrDpkrDataService>(
                typeof (Hmao.Domain.Impl.ObjectCrDpkrDataService),
                Component.For<IObjectCrDpkrDataService>()
                    .ImplementedBy<ObjectCrDpkrDataService>().LifestyleTransient());

            Container.ReplaceComponent(
                typeof (B4.Modules.FIAS.IFiasRepository),
                typeof (B4.Modules.FIAS.FiasRepository),
                Component.For<B4.Modules.FIAS.IFiasRepository, ICustomFiasRepository>()
                    .ImplementedBy<CustomFiasRepository>()
                    .LifestyleTransient());

            Container.RegisterExecutionAction<FiasAddressCorrectAction>();
            Container.RegisterExecutionAction<UpdateMunicipalityStageAction>();

            Container.RegisterTransient<IResourceManifest, ResourceManifest>("Gkh.Overhaul.Regions.Kamchatka resources");
            Container.RegisterTransient<INavigationProvider, NavigationProvider>();
            Container.RegisterTransient<IClientRouteMapRegistrar, ClientRouteMapRegistrar>();
            Container.RegisterExecutionAction<DeleteDoubleRecordInVersionAction>();
            Container.RegisterExecutionAction<UpdateVersionSumAction>();
            Container.RegisterTransient<IGkhImport, Import.KamchatkaRealtyObject.RealtyObjectImport>();

            Container.RegisterController<OverhaulExternalLinksController>();
            Container.RegisterController<RealtyObjectDataController>();

            Container.RegisterTransient<IRealtyObjectDataService, RealtyObjectDataService>();
            Container.RegisterTransient<IPrintForm, RealtyObjectDataReport>("RealtyObjectDataReport");
            Container.RegisterTransient<IPrintForm, ProgramCrByDpkrForm1Report>("ProgramCrByDpkrForm1Report");
            Container.RegisterTransient<IPrintForm, ProgramCrByDpkrForm2Report>("ProgramCrByDpkrForm2Report");
            Container.RegisterTransient<IPrintForm, TurnoverBalanceSheet>("TurnoverBalanceSheet");
            Container.RegisterPermissionMap<PermissionMap>();

            var bundler = Container.Resolve<IResourceBundler>();

            bundler.RegisterCssBundle("b4-all", new[]
                {
                    "~/content/css/gkhOvrhlKamchatka.css"
                });
        }
    }
}