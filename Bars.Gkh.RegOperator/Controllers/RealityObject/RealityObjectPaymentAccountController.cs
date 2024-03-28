namespace Bars.Gkh.RegOperator.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.Gkh.RegOperator.Domain.Repository.RealityObjectAccount;
    using Bars.Gkh.RegOperator.Entities;

    public class RealityObjectPaymentAccountController : B4.Alt.DataController<RealityObjectPaymentAccount>
    {
        private readonly IRealityObjectPaymentAccountRepository _repo;

        public RealityObjectPaymentAccountController(IRealityObjectPaymentAccountRepository repo)
        {
            _repo = repo;
        }

        public ActionResult ListByPersonalAccountOwner(BaseParams @params)
        {
            return new JsonNetResult(_repo.ListByPersonalAccountOwner(@params));
        }
    }
}