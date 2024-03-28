namespace Bars.GkhGji.Regions.Tyumen.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.GkhGji.Regions.Tyumen.Entities;

    public class NetworkOperatorRealityObjectViewModel : BaseViewModel<NetworkOperatorRealityObject>
    {
        public override IDataResult List(IDomainService<NetworkOperatorRealityObject> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var roId = baseParams.Params.GetAs<long>("roId");

            var nopRoDecisionsDomain = Container.ResolveDomain<NetworkOperatorRealityObjectTechDecision>();

            using(Container.Using(nopRoDecisionsDomain))
            {
                var nopRoDecisions = nopRoDecisionsDomain.GetAll().Where(x => x.NetworkOperatorRealityObject.RealityObject.Id == roId)
                    .GroupBy(x => x.NetworkOperatorRealityObject.Id)
                    .ToDictionary(x => x.Key, x => x.ToArray());

                var data = domainService.GetAll()
                    .Where(x => x.RealityObject.Id == roId)
                    .ToArray()
                    .Select(x => new
                    {
                        x.Id,
                        OperatorName = x.NetworkOperator.Contragent.Name,
                        x.NetworkOperator,
                        x.Bandwidth,
                        TechDecisionsTitle = nopRoDecisions.ContainsKey(x.Id) ? string.Join(", ", nopRoDecisions[x.Id].Select(y => y.TechDecision.Name)) : string.Empty,
                        TechDecisions = nopRoDecisions.ContainsKey(x.Id) ? nopRoDecisions[x.Id] : null
                    })
                    .AsQueryable()
                    .Filter(loadParams, Container);

                var totalCount = data.Count();

                data = data.Order(loadParams).Paging(loadParams);

                return new ListDataResult(data.ToList(), totalCount);
            }
        }

        public override IDataResult Get(IDomainService<NetworkOperatorRealityObject> domainService, BaseParams baseParams)
        {
            var nopRoDecisionsDomain = Container.ResolveDomain<NetworkOperatorRealityObjectTechDecision>();

            using(Container.Using(nopRoDecisionsDomain))
            {
                var id = baseParams.Params.GetAs<long>("id");

                var nopRoDecisions = nopRoDecisionsDomain.GetAll().Where(x => x.NetworkOperatorRealityObject.Id == id)
                    .ToArray();

                var data = domainService.Get(id);
                return data != null
                    ? new BaseDataResult(new
                    {
                        data.Id,
                        OperatorName = data.NetworkOperator.Contragent.Name,
                        data.NetworkOperator,
                        data.Bandwidth,
                        TechDecisionsTitle = string.Join(", ", nopRoDecisions.Select(y => y.TechDecision.Name)),
                        TechDecisions = nopRoDecisions,
                    })
                    : new BaseDataResult();
            }
        }
    }
}