namespace Bars.Gkh.ClaimWork.ViewModel
{
    using System;
    using System.Linq;
    using B4;
    using B4.Utils;
    using Gkh.ClaimWork.Entities;

    public class ActViolIdentificationViewModel : BaseViewModel<ActViolIdentificationClw>
    {
        public override IDataResult List(IDomainService<ActViolIdentificationClw> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var dateStart = baseParams.Params.GetAs<DateTime?>("dateStart");
            var dateEnd = baseParams.Params.GetAs<DateTime?>("dateEnd");
            var roId = baseParams.Params.GetAs<long>("address");

            var data = domain.GetAll()
                .WhereIf(dateStart.HasValue, x => x.FormDate.Value.Date >= dateStart.Value.Date)
                .WhereIf(dateEnd.HasValue, x => x.FormDate.Value.Date <= dateEnd.Value.Date)
                .WhereIf(roId != 0, x => x.ClaimWork.RealityObject.Id == roId)
                .Select(x => new
                {
                    x.Id,
                    x.ClaimWork,
                    x.ClaimWork.ClaimWorkTypeBase,
                    x.FactOfSigning,
                    x.FormDate,
                    x.ClaimWork.BaseInfo,
                    Municipality = x.ClaimWork.RealityObject.Municipality.Name,
                    x.ClaimWork.RealityObject.Address
                })
                .Filter(loadParams, Container);
            
            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}