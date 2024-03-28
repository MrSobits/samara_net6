namespace Bars.Gkh.Gis.Controllers.PersonalAccount
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using DomainService.PersonalAccount;

    public class PersonalAccountAccrualsController : BaseController
    {
        public ActionResult List(BaseParams baseParams)
        {
           //внутренний идентификатор лицевого счета
            var personalAccountId = baseParams.Params.GetAs<long>("apartmentId");
            var realityObjectId = baseParams.Params.GetAs<long>("realityObjectId");
            var date = new DateTime(baseParams.Params.GetAs<int>("year"), baseParams.Params.GetAs<int>("month"), 1);
            var loadParam = baseParams.GetLoadParam();

            var result =
                (Container.Resolve<IGisPersonalAccountService>("GisPersonalAccount")
                    .GetPersonalAccountAccruals(realityObjectId, personalAccountId, date) ??
                 Container.Resolve<IGisPersonalAccountService>("KpPersonalAccount")
                     .GetPersonalAccountAccruals(realityObjectId, personalAccountId, date))
                     .Where(
                        x => x.BackorderAmount > 0 || x.BalanceIn > 0 || x.BalanceOut > 0 || x.ErcAmount > 0 ||
                             x.RecalcAmount > 0 || x.SupplierAmount > 0 || x.TariffAmount > 0)
                    .Select(x => new
                    {
                        x.Service,
                        x.Supplier,
                        x.BalanceIn,
                        x.ErcAmount,
                        x.RecalcAmount,
                        x.TariffAmount,
                        x.SupplierAmount,
                        x.BackorderAmount,
                        x.BalanceOut,
                        x.ServiceId,
                        x.SupplierId
                    }).AsQueryable().Filter(loadParam, Container).OrderBy(x => x.Service).ThenBy(x => x.Supplier);

            return new JsonListResult(result.Order(loadParam).Paging(loadParam).ToList(), result.Count());
        }
    }
}
