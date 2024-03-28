namespace Bars.Gkh.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;

    public class ManagingOrgServiceViewModel : BaseViewModel<ManagingOrgService>
    {
        public override IDataResult List(IDomainService<ManagingOrgService> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var manorgId = baseParams.Params.GetAs<long>("manorgId");

            var data = domain.GetAll()
                .Where(x => x.ManagingOrganization.Id == manorgId)
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    WorkName = x.Work != null ? x.Work.Name : string.Empty,
                    x.Work
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}