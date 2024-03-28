namespace Bars.Gkh.Overhaul.Regions.Saratov
{
    using B4.Modules.Reports;
    using Bars.B4;
    using Bars.B4.IoC;
    
    using Bars.Gkh.Overhaul.Regions.Saratov.Reports;

    using Castle.MicroKernel.Registration;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            Component.For<IResourceManifest>().ImplementedBy<ResourceManifest>().LifeStyle.Transient.RegisterIn(Container);
            Container.Register(Component.For<IPermissionSource>().ImplementedBy<PermissionMap>());

            #region Reports

            Container.RegisterTransient<IPrintForm, RealtyObjectCertificationControl>("RealtyObjectCertificationControl");

            #endregion
        }
    }
}
