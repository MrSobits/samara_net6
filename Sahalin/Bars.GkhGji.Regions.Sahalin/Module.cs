using Bars.Gkh.Domain;

namespace Bars.GkhGji.Regions.Sahalin
{
    using B4.Modules.Reports;
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Regions.Nso.Report;
    using Bars.GkhGji.Regions.Sahalin.Report;
    using Bars.GkhGji.Regions.Sahalin.Report.Form1Control;
    using Castle.MicroKernel.Registration;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            Container.RegisterResources<ResourceManifest>();
            Container.ReplaceComponent<IGkhBaseReport>(
                typeof(ProtocolGjiReport),
                Component.For<IGkhBaseReport>().ImplementedBy<ProtocolGjiStimulReport>().LifeStyle.Transient);

            Container.ReplaceComponent<IGkhBaseReport>(
                typeof(Nso.Report.DisposalGji.DisposalGjiNotificationStimulReport),
                Component.For<IGkhBaseReport>().ImplementedBy<Report.Disposal.DisposalGjiNotificationStimulReport>().LifeStyle.Transient);

            Container.ReplaceComponent<IGkhBaseReport>(
                typeof(ActCheckGjiStimulReport),
                Component.For<IGkhBaseReport>().ImplementedBy<Report.ActCheck.ActCheckGjiStimulReport>().LifeStyle.Transient);

            Container.ReplaceComponent<IGkhBaseReport>(
                typeof(PrescriptionGjiStimulReport),
                Component.For<IGkhBaseReport>().ImplementedBy<PrescriptionStimulReport>().LifeStyle.Transient);

            Container.RegisterTransient<IPrintForm, SahalinForm1ControlReport>();
            Container.RegisterTransient<IPermissionSource, SahalinGjiPermissionMap>();

            Container.ReplaceTransient<IGkhBaseReport, NsoDisposalStimulReport, DisposalGjiStimulReport>();
        }
    }
}