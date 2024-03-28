namespace Bars.Gkh.ViewModel
{
    using System.Linq;

    using B4;

    using Entities;

    public class InspectorSubscriptionViewModel : BaseViewModel<InspectorSubscription>
    {
        public override IDataResult List(IDomainService<InspectorSubscription> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var inpectorId = baseParams.Params.GetAs<long>("inpectorId");

            var data = domainService.GetAll()
                .Where(x => x.SignedInspector.Id == inpectorId)
                .Select(x => new
                    {
                        x.Id,
                        Inspector = x.Inspector.Fio,
                        x.Inspector.Position
                    })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}