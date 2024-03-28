namespace Bars.Gkh.ViewModel
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;

    public class RealityObjectConstrElViewModel : BaseViewModel<RealityObjectConstructiveElement>
    {
        public override IDataResult List(IDomainService<RealityObjectConstructiveElement> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var objectId = baseParams.Params.GetAs<long>("objectId");

            var nextYear = DateTime.Now.Year + 1;

            var data = domain.GetAll()
                .Where(x => x.RealityObject.Id == objectId)
                .Select(x => new
                {
                    x.Id,
                    x.LastYearOverhaul,
                    ConstructiveElementName = x.ConstructiveElement.Name,
                    ConstructiveElementGroup = x.ConstructiveElement.Group.Name,
                    ConstructiveElementRepairPlanDate = (x.LastYearOverhaul + decimal.ToInt32(x.ConstructiveElement.Lifetime)) < nextYear
                        ? nextYear
                        : (x.LastYearOverhaul + decimal.ToInt32(x.ConstructiveElement.Lifetime))
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}