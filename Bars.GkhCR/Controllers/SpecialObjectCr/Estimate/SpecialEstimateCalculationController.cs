namespace Bars.GkhCr.Controllers
{
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage;
    using Bars.GkhCr.DomainService;
    using Bars.GkhCr.Entities;

    using Microsoft.AspNetCore.Mvc;

    public class SpecialEstimateCalculationController : FileStorageDataController<SpecialEstimateCalculation>
    {
        public ActionResult ListEstimateRegisterDetail(BaseParams baseParams)
        {
            var service = this.Container.Resolve<ISpecialEstimateCalculationService>();
            using (this.Container.Using(service))
            {
                return new JsonNetResult(service.ListEstimateRegisterDetail(baseParams));
            }
        }
    }
}