namespace Bars.Gkh.Gis.ViewModel.KpSettings
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Entities.Kp50;

    public class GisDataBankViewModel : BaseViewModel<GisDataBank>
    {
        public override IDataResult List(IDomainService<GisDataBank> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domainService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    Contragent = x.Contragent.Name,
                    Municipality = x.Municipality.Name,
                    x.Name,
                    x.Key
                })
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Contragent)
                .OrderThenIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                .OrderThenIf(loadParams.Order.Length == 0, true, x => x.Name)
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}