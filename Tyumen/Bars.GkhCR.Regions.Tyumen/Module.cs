using Bars.B4;
using Bars.B4.IoC;
using Bars.B4.Windsor;
using Bars.GkhCr.DomainService.GisGkhRegional;
using Bars.GkhCR.Regions.Tyumen.Controllers;
using Castle.MicroKernel.Registration;

namespace Bars.GkhCR.Regions.Tyumen
{
    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            Container.RegisterController<TypeWorkCrWorksController>();

            Container.RegisterTransient<IResourceManifest, ResourceManifest>("GkhCR.Regions.Tyumen resources");

            Container.ReplaceComponent<IGisGkhRegionalService>(
                typeof(Bars.GkhCr.DomainService.GisGkhRegional.Impl.GisGkhRegionalService),
                Component.For<IGisGkhRegionalService>().ImplementedBy<Bars.GkhCr.Regions.Tymen.DomainService.GisGkhRegional.Impl.GisGkhRegionalService>().LifeStyle.Transient);
        }
    }
}
