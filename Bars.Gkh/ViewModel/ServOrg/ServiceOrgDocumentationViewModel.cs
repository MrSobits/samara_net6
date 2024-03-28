namespace Bars.Gkh.ViewModel
{
    using System.Linq;
    using Bars.B4;
    using Bars.Gkh.Entities;

    public class ServiceOrgDocumentationViewModel : BaseViewModel<ServiceOrgDocumentation>
    {
        public override IDataResult List(IDomainService<ServiceOrgDocumentation> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var servorgId = baseParams.Params.GetAs<long>("servorgId");

            var data = domain.GetAll()
                .Where(x => x.ServiceOrganization.Id == servorgId)
                .Select(x => new
                {
                    x.Id,
                    x.DocumentNum,
                    x.DocumentName,
                    x.DocumentDate,
                    x.Description,
                    x.File
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}