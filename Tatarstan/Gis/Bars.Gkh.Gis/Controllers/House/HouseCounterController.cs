namespace Bars.Gkh.Gis.Controllers.House
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using DomainService.House;
    using Entities.House;


    /// <summary>
    /// Получение показаний приборов учета дома
    /// </summary>
    public class HouseCounterController : BaseController
    {
        public ActionResult List(BaseParams baseParams)
        {
            var realityObjectId = baseParams.Params.GetAs<long>("realityObjectId");
            var date = baseParams.Params.GetAs<DateTime>("date");
            date = new DateTime(date.Year, date.Month, 1);
            var loadParam = baseParams.GetLoadParam();

            var result =
                (Container.Resolve<IGisHouseService>("GisHouseService").GetHouseCounterValues(realityObjectId, date) ??
                 Container.Resolve<IGisHouseService>("KpHouseService").GetHouseCounterValues(realityObjectId, date))
                    .Select(x => new HouseCounter
                    {
                        Id = x.Id,
                        HouseId = x.HouseId,
                        Service = x.Service,
                        CounterNumber = x.CounterNumber,
                        StatementDate = x.StatementDate,
                        CounterValue = x.CounterValue,
                        PrevStatementDate = x.PrevStatementDate,
                        PrevCounterValue = x.PrevCounterValue,
                        CounterType = x.CounterType
                    }).AsQueryable().Filter(loadParam, Container);

            return new JsonListResult(result.Order(loadParam).Paging(loadParam).ToList(), result.Count());
        }
    }
}
