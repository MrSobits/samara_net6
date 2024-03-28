namespace Bars.Gkh.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;

    public class ConstructiveElementViewModel : BaseViewModel<ConstructiveElement>
    {
        public override IDataResult List(IDomainService<ConstructiveElement> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Select(x => new
                    {
                        x.Id,
                        GroupName = x.Group.Name,
                        x.Name,
                        x.Code,
                        x.Lifetime,
                        NormativeDocName = x.NormativeDoc.Name,
                        UnitMeasure = x.UnitMeasure.Name,
                        x.RepairCost
                    })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}