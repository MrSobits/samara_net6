namespace Bars.Gkh.RegOperator.ViewModels.Dict
{
    using System.Linq;
    using B4;
    using Entities.Dict;
    using Gkh.Entities;

    public class PaymentDocInfoViewModel : BaseViewModel<PaymentDocInfo>
    {
        public override IDataResult List(IDomainService<PaymentDocInfo> domainService, BaseParams baseParams)
        {
            var loadParam = GetLoadParam(baseParams);

            var query = domainService.GetAll().Filter(loadParam, Container);

            var total = query.Count();

            var data = query
                .Order(loadParam)
                .Paging(loadParam)
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    x.DateStart,
                    x.DateEnd,
                    Municipality = x.IsForRegion
                        ? new Municipality { Name = "Все"}
                        : x.Municipality,
                    x.MoSettlement,
                    x.RealityObject,
                    x.LocalityAoGuid,
                    x.LocalityName,
                    x.Information,
                    x.IsForRegion
                })
                .ToArray();

            return new ListDataResult(data, total);
        }
    }
}
