namespace Bars.GkhEdoInteg.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhEdoInteg.Entities;
    using Bars.GkhGji.Entities;

    public class KindStatementCompareEdoViewModel : BaseViewModel<KindStatementCompareEdo>
    {
        public override IDataResult List(IDomainService<KindStatementCompareEdo> domainService, BaseParams baseParams)
        {
            var dict = domainService.GetAll().ToDictionary(x => x.KindStatement.Id, x => new {x.Id, x.CodeEdo});

            var loadParams = GetLoadParam(baseParams);
            var data = Container.Resolve<IDomainService<KindStatementGji>>().GetAll()
                .ToList()
                .Select(x => new
                {
                    x.Id,
                    CompareId = dict.ContainsKey(x.Id) ? dict[x.Id].Id : 0,
                    x.Name,
                    x.Code,
                    KindStatement = x,
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
