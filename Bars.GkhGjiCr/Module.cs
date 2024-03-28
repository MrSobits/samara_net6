namespace Bars.GkhCjiCr
{
    using B4.Modules.Reports;
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.GkhGjiCr;
    using Bars.GkhGjiCr.Permissions;
    using Bars.GkhGjiCr.Report;

    using Castle.MicroKernel.Registration;

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            Component.For<IResourceManifest>().Named("GkhGjiCr resources").ImplementedBy<ResourceManifest>()
            .LifestyleTransient().RegisterIn(Container);

            Component.For<IPermissionSource>().ImplementedBy<GkhGjiCrPermissionMap>()
                .RegisterIn(Container);
            Component.For<IPrintForm>().Named("GjiCr Report.InformationCrByResolution").ImplementedBy<InformationCrByResolution>()
                .LifestyleTransient().RegisterIn(Container);
            Component.For<IPrintForm>().Named("GjiCr Report.Form1StateHousingInspection").ImplementedBy<Form1StateHousingInspection>()
                .LifestyleTransient().RegisterIn(Container);

            Component.For<IPrintForm>().Named("Report.TestReport").ImplementedBy<TestReport>()
                .LifestyleTransient().RegisterIn(Container);
        }
    }
}

