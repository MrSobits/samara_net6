namespace Bars.GkhGji.Regions.Perm
{
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Regions.Perm.Permissions;
    using Bars.GkhGji.Regions.Perm.Report.ActCheckGji;

    using Castle.MicroKernel.Registration;

    using ManorgLicenceApplicantsProvider = Bars.Gkh.Regions.Perm.DomainService.ManorgLicenceApplicantsProvider;

    public class Module : AssemblyDefinedModule
    {
        /// <summary>
        /// Метод инициализации модуля
        /// </summary>
        public override void Install()
        {
            this.Container.RegisterResources<ResourceManifest>();
            this.Container.Register(Component.For<IPermissionSource>().ImplementedBy<PermGjiPermissionMap>());

            this.Container.ReplaceTransient<IManorgLicenceApplicantsProvider, Bars.Gkh.DomainService.ManorgLicenceApplicantsProvider, ManorgLicenceApplicantsProvider>();

            this.RegisterReports();
        }

        private void RegisterReports()
        {
            this.Container.RegisterTransient<IGkhBaseReport, MotivatedProposalForLicenseReport>();
            this.Container.RegisterTransient<IGkhBaseReport, MotivatedProposalForRefusalLicenseReport>();
        }
    }
}