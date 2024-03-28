namespace Bars.GkhEdoInteg.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;
    using Bars.GkhEdoInteg.Entities;

    public class InspectorCompareEdoViewModel : BaseViewModel<InspectorCompareEdo>
    {
        public override IDataResult List(IDomainService<InspectorCompareEdo> domainService, BaseParams baseParams)
        {
            var dict = domainService.GetAll().ToDictionary(x => x.Inspector.Id, x => new { x.Id, x.CodeEdo });

            var loadParams = GetLoadParam(baseParams);
            var data = Container.Resolve<IDomainService<Inspector>>().GetAll()
                .ToList()
                .Select(x => new
                {
                    x.Id,
                    CompareId = dict.ContainsKey(x.Id) ? dict[x.Id].Id : 0,
                    x.Fio,
                    x.Code,
                    Inspector = x,
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
