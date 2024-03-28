namespace Bars.Gkh.RegOperator.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using B4.DataAccess;
    using B4.IoC;
    using DomainService.RealityObjectAccount;
    using Gkh.Entities;

    public class RealityObjectAccountController : BaseController
    {
        public RealityObjectAccountController(
            IRealityObjectAccountGenerator generator,
            IDomainService<RealityObject> roDomain,
            IRealityObjectAccountService roAccService)
        {
            _generator = generator;
            _roDomain = roDomain;
            _roAccService = roAccService;
        }

        public ActionResult GenerateAccounts(BaseParams baseParams)
        {
            Container.UsingForResolved<IDataTransaction>((c, tr) =>
            {
                try
                {
                    var ros =_roDomain.GetAll();

                    _generator.GenerateChargeAccounts(ros);
                    _generator.GeneratePaymentAccounts(ros);
                    _generator.GenerateSupplierAccounts(ros);

                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                    throw;
                }
            });

            return JsSuccess();
        }

        public ActionResult GetChargeAccount(BaseParams baseParams)
        {
            return new JsonNetResult(_roAccService.GetChargeAccountResult(baseParams));
        }

        public ActionResult GetPaymentAccount(BaseParams baseParams)
        {
            return new JsonNetResult(_roAccService.GetPaymentAccountResult(baseParams));
        }

        public ActionResult GetSupplierAccount(BaseParams baseParams)
        {
            return new JsonNetResult(_roAccService.GetSupplierAccountResult(baseParams));
        }

        public ActionResult GetSubsidyAccount(BaseParams baseParams)
        {
            return new JsonNetResult(_roAccService.GetSubsidyAccountResult(baseParams));
        }

        public ActionResult GetPaymentAccountBySources(BaseParams baseParams)
        {
            return new JsonNetResult(_roAccService.GetPaymentAccountBySources(baseParams));
        }

        private readonly IRealityObjectAccountGenerator _generator;
        private readonly IDomainService<RealityObject> _roDomain;
        private readonly IRealityObjectAccountService _roAccService;
    }
}