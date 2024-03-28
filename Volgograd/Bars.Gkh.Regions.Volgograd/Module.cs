namespace Bars.Gkh.Regions.Volgograd
{
    using B4.Modules.Reports;
    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Regions.Volgograd.Permissions;
    using Bars.Gkh.Report;

    using Castle.MicroKernel.Registration;

    using Bars.B4.IoC;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            Container.Register(
                Component.For<IResourceManifest>().Named("Gkh.Regions.Volgograd resources").ImplementedBy<ResourceManifest>()
                .LifeStyle.Transient);

            Container.Register(Component.For<IPermissionSource>().ImplementedBy<GkhRegionsVolgogradPermissionMap>());

            #region Reports

            Container.Register(
                Component.For<IPrintForm>()
                         .ImplementedBy<RepairPlanning>()
                         .Named("Report Bars.Gkh RepairPlanning")
                         .LifestyleTransient());

            #endregion

            Container.ReplaceComponent<IViewModel<RealityObjectConstructiveElement>>(
                typeof(Bars.Gkh.ViewModel.RealityObjectConstrElViewModel),
                Component.For<IViewModel<RealityObjectConstructiveElement>>().ImplementedBy<Volgograd.ViewModel.RealityObjectConstrElViewModel>());
        }
    }
}