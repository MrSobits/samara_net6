namespace Bars.Gkh.RegOperator.ViewModel
{
    using System.Linq;
    using B4;
    using Entities;
    using Gkh.Domain;

    public class RegopCalcAccountRealityObjectViewModel : BaseViewModel<RegopCalcAccountRealityObject>
    {
        public override IDataResult List(IDomainService<RegopCalcAccountRealityObject> domainService, BaseParams baseParams)
        {
            var accId = baseParams.Params.GetAsId("accId");
            var loadParam = baseParams.GetLoadParam();

            var data = domainService.GetAll()
                .Where(x => x.RegOpCalcAccount.Id == accId)
                .Select(x => new
                {
                    x.Id,
                    x.RealityObject.Address
                }).Filter(loadParam, Container).Order(loadParam);

            return new ListDataResult(data.Paging(loadParam), data.Count());
        }
    }
}