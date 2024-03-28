namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class ServiceJuridalGjiViewModel : BaseViewModel<ServiceJuridicalGji>
    {
        public override IDataResult List(IDomainService<ServiceJuridicalGji> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var buisnesId = baseParams.Params.ContainsKey("buisnesId")
                ? baseParams.Params["buisnesId"].ToLong()
                : 0;

            var data = domainService.GetAll()
                .Where(x => x.BusinessActivityNotif.Id == buisnesId)
                .Select(x => new
                {
                    x.Id,
                    KindWorkNotifGjiName = x.KindWorkNotif.Name,
                })
                .Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }
    }
}