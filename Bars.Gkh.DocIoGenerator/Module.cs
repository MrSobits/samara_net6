namespace Bars.Gkh.DocIoGenerator
{
    using B4.Modules.Reports;
    using Bars.B4;
    using Bars.B4.IoC;

    using Castle.MicroKernel.Registration;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            Component
                .For<IReportGenerator>()
                .Named("DocIoGenerator")
                .ImplementedBy<DocIoReportGenerator>()
                .LifeStyle.Transient
                .RegisterIn(Container);
            this.Container.RegisterTransient<IDocIo, DocIo>();
        }
    }
}