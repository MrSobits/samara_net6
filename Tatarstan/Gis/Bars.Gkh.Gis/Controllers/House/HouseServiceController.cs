namespace Bars.Gkh.Gis.Controllers.House
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using DomainService.House;

    /// <summary>
    /// Получение услуг дома
    /// </summary>
    public class HouseServiceController : BaseController
    {
        public ActionResult List(BaseParams baseParams)
        {
            var realityObjectId = baseParams.Params.GetAs<long>("realityObjectId");
            var date = baseParams.Params.GetAs<DateTime>("date");
            date = new DateTime(date.Year, date.Month, 1);
            var loadParam = baseParams.GetLoadParam();

            var result =
                (Container.Resolve<IGisHouseService>("GisHouseService").GetHouseServices(realityObjectId, date) ??
                 Container.Resolve<IGisHouseService>("KpHouseService").GetHouseServices(realityObjectId, date))
                .Select(x => new
                {
                    x.Id,
                    x.LsCount,
                    x.Service,
                    x.Supplier,
                    x.Formula
                })
                .AsQueryable()
                .Filter(loadParam, Container)
                .OrderBy(x => x.Service)
                .ThenBy(x => x.Supplier)
                .ThenBy(x => x.Formula);

            return new JsonListResult(result.Order(loadParam).Paging(loadParam).ToList(), result.Count());
        }
    }
}
