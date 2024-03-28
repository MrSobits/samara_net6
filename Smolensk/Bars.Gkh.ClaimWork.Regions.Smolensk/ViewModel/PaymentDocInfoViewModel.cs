namespace Bars.Gkh.ClaimWork.Regions.Smolensk.ViewModel
{
    using System.Linq;
    using B4;

    using Bars.Gkh.RegOperator.Entities.Dict;

    public class PaymentDocInfoViewModel : BaseViewModel<PaymentDocInfo>
    {
        public override IDataResult List(IDomainService<PaymentDocInfo> domainService, BaseParams baseParams)
        {
            var loadParam = GetLoadParam(baseParams);
            
            var data = domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.DateStart,
                    x.DateEnd,
                    Municipality = x.IsForRegion
                        ? "Все"//new Municipality { Name = "Все"}
                        : x.Municipality.Name,
                    x.MoSettlement,
                    RealityObject = x.RealityObject.Address,
                    x.LocalityAoGuid,
                    x.LocalityName,
                    x.Information,
                    x.IsForRegion
                })
                .Filter(loadParam, Container)
                ;

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), data.Count());
        }
    }
}
