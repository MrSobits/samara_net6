namespace Bars.Gkh.Gis.Controllers.PersonalAccount
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using DomainService.PersonalAccount;

    public class PersonalAccountParamController : BaseController
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
                    .GetPersonalAccountParams(realityObjectId, personalAccountId, date) ??
                 Container.Resolve<IGisPersonalAccountService>("KpPersonalAccount")
                     .GetPersonalAccountParams(realityObjectId, personalAccountId, date))
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
