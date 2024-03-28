namespace Bars.Gkh.RegOperator.ViewModels.Dict
{
    using System.Linq;
    using B4;
    using Entities.Dict;


    public class TariffByPeriodForClaimWorkViewModel : BaseViewModel<TariffByPeriodForClaimWork>
    {
        public override IDataResult List(IDomainService<TariffByPeriodForClaimWork> domainService, BaseParams baseParams)
        {
            var loadParam = GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.ChargePeriod,
                    Municipality = x.Municipality.Name,
                    x.Value
                  
                }).Filter(loadParam, this.Container);

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), data.Count());

           
        }
    }
}
