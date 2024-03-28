namespace Bars.Gkh.Gis.Controllers.House
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using DomainService.House;


    /// <summary>
    /// Получение параметров дома
    /// </summary>
    public class HouseParamController : BaseController
    {
        public ActionResult List(BaseParams baseParams)
        {
            var realityObjectId = baseParams.Params.GetAs<long>("realityObjectId");
            var date = baseParams.Params.GetAs<DateTime>("date");
            date = new DateTime(date.Year, date.Month, 1);
            var loadParam = baseParams.GetLoadParam();

            var result =
                (Container.Resolve<IGisHouseService>("GisHouseService").GetHouseParams(realityObjectId, date) ??
                 Container.Resolve<IGisHouseService>("KpHouseService").GetHouseParams(realityObjectId, date))
                    .Where(x => x.DateBegin <= date && x.DateEnd >= date)
                    .Select(x => new
                    {
                        x.Id,
                        x.Name,
                        x.ValPrm
                    }).AsQueryable().Filter(loadParam, Container);

            return new JsonListResult(result.Order(loadParam).Paging(loadParam).ToList(), result.Count());
        }
    }
}
