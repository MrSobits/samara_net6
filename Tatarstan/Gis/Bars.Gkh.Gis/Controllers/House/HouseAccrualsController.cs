namespace Bars.Gkh.Gis.Controllers.House
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using DomainService.House;


    /// <summary>
    /// Получение начислений дома
    /// </summary>
    public class HouseAccrualsController : BaseController
    {
        public ActionResult List(BaseParams baseParams)
        {
            var realityObjectId = baseParams.Params.GetAs<long>("realityObjectId");
            var date = baseParams.Params.GetAs<DateTime>("date");
            date = new DateTime(date.Year, date.Month, 1);
            var loadParam = baseParams.GetLoadParam();

            var result =
                (Container.Resolve<IGisHouseService>("GisHouseService").GetHouseAccruals(realityObjectId, date) ??
                 Container.Resolve<IGisHouseService>("KpHouseService").GetHouseAccruals(realityObjectId, date))
                     .Select(x => new
                     {
                         x.Service,
                         x.BalanceIn,
                         x.ErcAmount,
                         x.RecalcAmount,
                         x.TariffAmount,
                         x.SupplierAmount,
                         x.BackorderAmount,
                         x.BalanceOut
                     }).AsQueryable().Filter(loadParam, Container).OrderBy(x => x.Service);

            return new JsonListResult(result.Order(loadParam).Paging(loadParam).ToList(), result.Count());
        }
    }
}
