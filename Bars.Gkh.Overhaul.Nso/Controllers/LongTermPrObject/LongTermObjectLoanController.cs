namespace Bars.Gkh.Overhaul.Nso.Controllers
{
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using B4.Modules.FileStorage;
    using DomainService;
    using Entities;

    public class LongTermObjectLoanController : FileStorageDataController<LongTermObjectLoan>
    {
        public ILongTermObjectLoanService Service { get; set; }

        public ActionResult ListRegister(BaseParams baseParams)
        {
            var result = (ListDataResult)Service.ListRegister(baseParams);
            return new JsonListResult((IList) result.Data, result.TotalCount);
        }
    }
}