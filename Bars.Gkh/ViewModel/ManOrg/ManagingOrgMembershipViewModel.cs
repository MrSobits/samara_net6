namespace Bars.Gkh.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;

    public class ManagingOrgMembershipViewModel : BaseViewModel<ManagingOrgMembership>
    {
        public override IDataResult List(IDomainService<ManagingOrgMembership> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var manorgId = baseParams.Params.GetAs<long>("manorgId");

            var data = domain.GetAll()
                .Where(x => x.ManagingOrganization.Id == manorgId)
                .Select(x => new
                {
                    x.Id,
                    x.Address,
                    x.DateStart,
                    x.DateEnd,
                    x.Name,
                    x.OfficialSite
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}