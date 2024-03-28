namespace Bars.Gkh.Regions.Volgograd.ViewModel
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;

    public class RealityObjectConstrElViewModel : BaseViewModel<RealityObjectConstructiveElement>
    {
        public override IDataResult List(IDomainService<RealityObjectConstructiveElement> domain, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var objectId = baseParams.Params.GetAs<long>("objectId");

            var nextYear = DateTime.Now.Year + 1;

            var data = domain.GetAll()
                .Where(x => x.RealityObject.Id == objectId)
                .Select(x => new
                {
                    x.Id,
                    x.LastYearOverhaul,
                    x.Volume,
                    ConstructiveElementName = x.ConstructiveElement.Name,
                    ConstructiveElementGroup = x.ConstructiveElement.Group.Name,
                    ConstructiveElementRepairPlanDate = (x.LastYearOverhaul + decimal.ToInt32(x.ConstructiveElement.Lifetime)) < nextYear 
                        ? nextYear
                        : (x.LastYearOverhaul + decimal.ToInt32(x.ConstructiveElement.Lifetime))
                })
                .Filter(loadParams, this.Container);

            int totalCount = data.Count();

            data = data.Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }
    }
}