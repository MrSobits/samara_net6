namespace Bars.GkhOverhaulTp.Regions.Chelyabinsk
{
    using B4.Modules.Reports;
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.Utils;
    using Bars.GkhOverhaulTp.Regions.Chelyabinsk.ExecutionAction;
    using Bars.GkhOverhaulTp.Regions.Chelyabinsk.Reports;

    using Castle.MicroKernel.Registration;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            Container.RegisterTransient<IResourceManifest, ResourceManifest>();
            Container.Register(Component.For<IPermissionSource>().ImplementedBy<PermissionMap>());
            this.Container.RegisterExecutionAction<CopyStructElementsFromTpAction>();
            this.Container.RegisterExecutionAction<UnnamedMultipleStructElementRenameAction>();
            this.Container.RegisterExecutionAction<RobjectPropertiesUpdateAction>();

            Container.Register(Component.For<IPrintForm>().ImplementedBy<AreaDataSheetReport>().Named("AreaDataSheetReport").LifestyleTransient());
        }
    }
}