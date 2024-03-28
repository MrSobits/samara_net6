namespace Bars.Gkh.RegOperator.Controllers
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.Gkh.RegOperator.Entities;

    public class CashPaymentCenterPersAccController : B4.Alt.DataController<CashPaymentCenterPersAcc>
    {

        public ActionResult Any()
        {
            return new JsonNetResult(DomainService.GetAll().Any());
        }
    }
}
