namespace Bars.Gkh.Gis.Controllers.PersonalAccount
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using DomainService.PersonalAccount;

    public class PersonalAccountServiceController : BaseController
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
                    .GetPersonalAccountServices(realityObjectId, personalAccountId, date) ??
                 Container.Resolve<IGisPersonalAccountService>("KpPersonalAccount")
                     .GetPersonalAccountServices(realityObjectId, personalAccountId, date))
                    .Select(x => new
                    {
                        x.Service,
                        x.Supplier,
                        x.Tariff
                    }).AsQueryable().Filter(loadParam, Container).OrderBy(x => x.Service).ThenBy(x => x.Supplier);

            return new JsonListResult(result.Order(loadParam).Paging(loadParam).ToList(), result.Count());
        }
    }
}
