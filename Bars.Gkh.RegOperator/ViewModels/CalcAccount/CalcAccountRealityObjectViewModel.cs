namespace Bars.Gkh.RegOperator.ViewModels
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.Utils;

    public class CalcAccountRealityObjectViewModel : BaseViewModel<CalcAccountRealityObject>
    {
        public override IDataResult List(IDomainService<CalcAccountRealityObject> domainService, BaseParams baseParams)
        {
            var accountId = baseParams.Params.GetAsId("accId");
            var showAll = baseParams.Params.GetAs<bool>("showAll");

            return domainService.GetAll()
                .Where(x => x.Account.Id == accountId)
                .WhereIf(!showAll, x => x.DateStart <= DateTime.Today)
                .WhereIf(!showAll, x => !x.DateEnd.HasValue || x.DateEnd > DateTime.Today)
                .Select(x => new
                {
                    x.Id,
                    x.RealityObject.Address,
                    StartDate = x.DateStart,
                    EndDate = x.DateEnd,
                })
                .ToListDataResult(this.GetLoadParam(baseParams));
        }
    }
}