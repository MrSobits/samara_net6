namespace Bars.GkhDi.DomainService
{
    using System.Linq;
    using Bars.B4;
    using Entities;

    public class WorkToViewModel : BaseViewModel<WorkTo>
    {
        public override IDataResult List(IDomainService<WorkTo> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Where(x => !x.IsNotActual)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    GroupWorkToName = x.GroupWorkTo.Name
                })
                .Filter(loadParams, Container);

            var totalCount = data.Count();

            data = data.Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }
    }
}