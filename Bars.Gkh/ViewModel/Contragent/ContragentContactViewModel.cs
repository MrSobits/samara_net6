using Bars.B4.Utils;

namespace Bars.Gkh.ViewModel
{
    using System.Linq;

    using B4;
    using Entities;

    public class ContragentContactViewModel : BaseViewModel<ContragentContact>
    {
        public override IDataResult List(IDomainService<ContragentContact> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var contragentId = baseParams.Params.GetAs<long>("contragentId");

            var byContragent = baseParams.Params.ContainsKey("contragentId");

            var data = domain.GetAll()
                .WhereIf(byContragent, x => x.Contragent.Id == contragentId)
                .Select(x => new
                {
                    x.Id,
                    x.FullName,
                    x.DateStartWork,
                    x.DateEndWork,
                    Position = x.Position.Name,
                    x.Phone,
                    x.Email,
                    Contragent = x.Contragent.Name,
                    ContragentMunicipality = x.Contragent.Municipality.Name
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}