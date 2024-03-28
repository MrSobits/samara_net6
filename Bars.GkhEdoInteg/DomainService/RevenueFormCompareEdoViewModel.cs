namespace Bars.GkhEdoInteg.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhEdoInteg.Entities;
    using Bars.GkhGji.Entities;

    public class RevenueFormCompareEdoViewModel : BaseViewModel<RevenueFormCompareEdo>
    {
        public override IDataResult List(IDomainService<RevenueFormCompareEdo> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var dict = domainService.GetAll().ToDictionary(x => x.RevenueForm.Id, x => new { x.Id, x.CodeEdo });
            var data = Container.Resolve<IDomainService<RevenueFormGji>>().GetAll()
                .ToList()
                .Select(x => new
                {
                    x.Id,
                    CompareId = dict.ContainsKey(x.Id) ? dict[x.Id].Id : 0,
                    x.Name,
                    x.Code,
                    RevenueForm = x,
                    CodeEdo = dict.ContainsKey(x.Id) ? dict[x.Id].CodeEdo : 0,
                })
                .AsQueryable()
                .Filter(loadParams, Container);

            var totalCount = data.Count();
            data = data.Order(loadParams).Paging(loadParams);
            return new ListDataResult(data.ToList(), totalCount);
        }
    }
}
