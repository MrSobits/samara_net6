namespace Bars.Gkh.RegOperator.Controllers.PersonalAccount.PayDoc
{
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;

    internal class AccountPaymentInfoSnapshotController : BaseController
    {
        private readonly IDomainService<AccountPaymentInfoSnapshot> _dmn;
        private readonly IViewModel<AccountPaymentInfoSnapshot> _vm;

        public AccountPaymentInfoSnapshotController(
            IDomainService<AccountPaymentInfoSnapshot> dmn,
            IViewModel<AccountPaymentInfoSnapshot> vm)
        {
            _dmn = dmn;
            _vm = vm;
        }

        public ActionResult List(BaseParams prms)
        {
            return Js(_vm.List(_dmn, prms));
        }
    }
}