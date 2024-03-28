namespace Bars.Gkh.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;

    public class PoliticAuthorityViewModel : BaseViewModel<PoliticAuthority>
    {
        public override IDataResult List(IDomainService<PoliticAuthority> domain, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var data = domain.GetAll()
                .Select(x => new
                {
                    x.Id,
                    ContragentName = x.Contragent.Name,
                    x.Contragent.Inn,
                    Municipality = x.Contragent.Municipality.Name,
                    x.NameDepartamentGkh,
                    x.OfficialSite
                })
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Municipality)
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }        
    }
} 