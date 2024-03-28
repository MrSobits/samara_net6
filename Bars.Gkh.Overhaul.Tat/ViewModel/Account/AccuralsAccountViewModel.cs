namespace Bars.Gkh.Overhaul.Tat.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Overhaul.Tat.Entities;

    public class AccrualsAccountViewModel : BaseViewModel<AccrualsAccount>
    {
        public override IDataResult List(IDomainService<AccrualsAccount> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var roId = baseParams.Params.GetAs<long>("roId");

            var data = domainService.GetAll()
                .Where(x => x.RealityObject.Id == roId)
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}