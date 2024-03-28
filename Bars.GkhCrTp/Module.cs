namespace Bars.GkhCrTp
{
    using B4;
    using B4.Modules.Reports;
    using Bars.GkhCrTp.Permissions;
    using Bars.GkhCrTp.Report;
    using Castle.MicroKernel.Registration;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            Container.Register(Component.For<IResourceManifest>().Named("GkhCrTp resources").ImplementedBy<ResourceManifest>().LifeStyle.Transient);

            Container.Register(Component.For<IPermissionSource>().ImplementedBy<GkhCrTpPermissionMap>());

            #region Отчеты

            Container.Register(Component.For<IPrintForm>().Named("GkhCrTp Report.SetTwoOutlineBoiler").ImplementedBy<SetTwoOutlineBoiler>().LifeStyle.Transient);
            Container.Register(Component.For<IPrintForm>().Named("GkhCrTp Report.ProgramCrReport").ImplementedBy<ProgramCrReport>().LifeStyle.Transient);
            Container.Register(Component.For<IPrintForm>().Named("GkhCrTp Report.ExcerptFromTechPassportMkd").ImplementedBy<ExcerptFromTechPassportMkd>().LifeStyle.Transient);
            #endregion
        }
    }
}