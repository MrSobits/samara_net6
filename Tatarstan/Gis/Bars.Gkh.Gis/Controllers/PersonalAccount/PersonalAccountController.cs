namespace Bars.Gkh.Gis.Controllers.PersonalAccount
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using DomainService.House;

    public class GisPersonalAccountController : BaseController
    {
        public ActionResult List(BaseParams baseParams)
        {
          var realityObjectId = baseParams.Params.GetAs<long>("realityObjectId");
            var date = baseParams.Params.GetAs<DateTime>("date");
            date = new DateTime(date.Year, date.Month, 1);
            var loadParam = baseParams.GetLoadParam();

            var result =
                (Container.Resolve<IGisHouseService>("GisHouseService").GetHousePersonalAccounts(realityObjectId, date) ??
                 Container.Resolve<IGisHouseService>("KpHouseService").GetHousePersonalAccounts(realityObjectId, date))
                    .Select(x => new 
                    {
                    x.Id,
                    x.RealityObjectId,
                    x.HouseId,
                    x.AccountId,
                    x.PSS,
                    x.PaymentCode,
                    x.ApartmentNumber,
                    x.RoomNumber,
                    x.IsOpen,
                    x.DateBegin,
                    x.DateEnd,
                    Uic = x.PaymentCode
                }).AsQueryable().Filter(loadParam, Container);

            return new JsonListResult(result.Order(loadParam).Paging(loadParam).ToList(), result.Count());
        }
    }
}
