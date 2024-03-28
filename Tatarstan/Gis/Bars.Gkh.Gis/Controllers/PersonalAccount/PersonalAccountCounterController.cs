namespace Bars.Gkh.Gis.Controllers.PersonalAccount
{
    using System;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using DomainService.PersonalAccount;
    using Entities.PersonalAccount;

    public class PersonalAccountCounterController : BaseController
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
                    .GetPersonalAccountCounterValues(realityObjectId, personalAccountId, date) ??
                 Container.Resolve<IGisPersonalAccountService>("KpPersonalAccount")
                     .GetPersonalAccountCounterValues(realityObjectId, personalAccountId, date))
                   .Select(x => new PersonalAccountCounter
                {
                    Id = x.Id,
                    ApartmentId = x.ApartmentId,
                    Service = x.Service,
                    CounterNumber = x.CounterNumber,
                    StatementDate = x.StatementDate,
                    CounterValue = x.CounterValue,
                    PrevStatementDate = x.PrevStatementDate,
                    PrevCounterValue = x.PrevCounterValue,
                    CounterType = x.CounterType,
                }).AsQueryable().Filter(loadParam, Container);

            return new JsonListResult(result.Order(loadParam).Paging(loadParam).ToList(), result.Count());
        }
    }
}
