namespace Bars.GkhDi.DomainService
{
    using System.Linq;
    using B4;
    using Entities;

    public class ActionsViewModel : BaseViewModel<Actions>
    {
        public override IDataResult List(IDomainService<Actions> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var adminRespId = baseParams.Params.GetAs<long>("adminRespId");

            var data = domainService.GetAll()
                .Where(x => x.AdminResp.Id == adminRespId)
                .Select(x => new
                    {
                        x.Id,
                        x.Action
                    })
                .Filter(loadParams, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
    }
}