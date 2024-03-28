namespace Bars.Gkh.ViewModel
{
    using System.Linq;
    using Bars.B4;
    using Bars.Gkh.Entities;

    public class ServiceOrgServViewModel : BaseViewModel<ServiceOrgService>
    {
        public override IDataResult List(IDomainService<ServiceOrgService> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var servorgId = baseParams.Params.GetAs<long>("servorgId");

            var data = domain.GetAll()
                .Where(x => x.ServiceOrganization.Id == servorgId)
                .Select(x => new
                {
                    x.Id,
                    TypeService = x.TypeService.Name
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

    }
}