namespace Bars.Gkh.ViewModel
{
    using System.Linq;
    using Bars.B4;
    using Bars.Gkh.Entities;

    public class ManagingOrgDocumentationViewModel : BaseViewModel<ManagingOrgDocumentation>
    {
        public override IDataResult List(IDomainService<ManagingOrgDocumentation> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var manorgId = baseParams.Params.GetAs<long>("manorgId");

            var data = domain.GetAll()
                .Where(x => x.ManagingOrganization.Id == manorgId)
                .Select(x => new
                {
                    x.Id,
                    x.DocumentNum,
                    x.DocumentName,
                    x.DocumentDate,
                    x.Description,
                    FileName = string.Format("{0}.{1}", x.File.Name,x.File.Extention),
                    x.File
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}