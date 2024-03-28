namespace Bars.Gkh.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;

    public class SupplyResourceOrgDocumentationViewModel : BaseViewModel<SupplyResourceOrgDocumentation>
    {
        public override IDataResult List(IDomainService<SupplyResourceOrgDocumentation> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var supplyResOrgId = baseParams.Params.GetAs<long>("supplyResOrgId");

            var data = domain.GetAll()
                .Where(x => x.SupplyResourceOrg.Id == supplyResOrgId)
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