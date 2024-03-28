namespace Bars.Gkh.RegOperator.Regions.Tatarstan.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using B4;

    using Bars.Gkh.RegOperator.Regions.Tatarstan.DomainService;
    using Bars.Gkh.RegOperator.Regions.Tatarstan.Entities;

    public class TransferHireController : B4.Alt.DataController<TransferHire>
    {
        public ActionResult Calc(BaseParams baseParams)
        {

            var service = Resolve<ITransferHireService>();
            try
            {
                var result = service.Calc(baseParams);
                return result.Success ? JsSuccess() : JsFailure(result.Message);
            }
            finally 
            {
                Container.Release(service);
            }
            
        }
    }
}