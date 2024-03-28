namespace Bars.Gkh.Overhaul.Nso.Controllers
{
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using DomainService;
    using Entities;

    public class AccountOperationController : B4.Alt.DataController<AccountOperation>
    {
        public IAccountOperationService Service { get; set; }

        public ActionResult ListNoPaging(BaseParams baseParams)
        {
            var list = (ListDataResult)Container.Resolve<IAccountOperationService>().ListNoPaging(baseParams);
            return new JsonListResult((IList) list.Data, list.TotalCount);
        }
    }
}