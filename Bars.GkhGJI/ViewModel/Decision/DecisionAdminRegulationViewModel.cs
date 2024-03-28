namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    public class DecisionAdminRegulationViewModel : BaseViewModel<DecisionAdminRegulation>
    {
        public override IDataResult List(IDomainService<DecisionAdminRegulation> domain, BaseParams baseParams)
        {
			var loadParam = baseParams.GetLoadParam();
			var documentId = baseParams.Params.GetAs<long>("documentId");

	        var data = domain.GetAll()
		        .Where(x => x.Decision.Id == documentId)
		        .Select(x => new
		        {
			        x.Id,
			        x.AdminRegulation.Code,
			        x.AdminRegulation.Name
		        })
		        .Filter(loadParam, this.Container)
		        .Order(loadParam);

			var totalCount = data.Count();

			return new ListDataResult(data.Paging(loadParam).ToList(), totalCount);
		}
    }
}
