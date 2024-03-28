namespace Bars.Gkh.ViewModel
{
    using System.Linq;
    using B4;
    using Entities;

    public class ManOrgRequestAnnexViewModel : BaseViewModel<ManOrgRequestAnnex>
    {
        public override IDataResult List(IDomainService<ManOrgRequestAnnex> domain, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var requestId = loadParams.Filter.GetAs("requestId", 0L);

            var data = domain.GetAll()
                .Where(x => x.LicRequest.Id == requestId)
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}