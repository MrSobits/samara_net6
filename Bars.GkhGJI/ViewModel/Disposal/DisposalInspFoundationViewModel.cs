namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    public class DisposalInspFoundationViewModel : BaseViewModel<DisposalInspFoundation>
    {
        public override IDataResult List(IDomainService<DisposalInspFoundation> domain, BaseParams baseParams)
        {
			var loadParam = baseParams.GetLoadParam();
			var documentId = baseParams.Params.GetAs<long>("documentId");

	        var data = domain.GetAll()
		        .Where(x => x.Disposal.Id == documentId)
		        .Select(x => new
		        {
			        x.Id,
			        x.InspFoundation.Code,
			        x.InspFoundation.Name
		        })
		        .Filter(loadParam, this.Container)
		        .Order(loadParam);

			var totalCount = data.Count();

			return new ListDataResult(data.Paging(loadParam).ToList(), totalCount);
		}
    }
}
