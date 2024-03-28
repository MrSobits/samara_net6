namespace Bars.GkhGji.Regions.Nso.ViewModel.Disposal
{
	using Bars.B4;
	using Bars.B4.Utils;
	using Bars.GkhGji.Regions.Nso.Entities.Disposal;
	using System.Linq;

	public class NsoDisposalProvidedDocViewModel : BaseViewModel<NsoDisposalProvidedDoc>
    {
		public override IDataResult List(IDomainService<NsoDisposalProvidedDoc> domain, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();
			var documentId = baseParams.Params.GetAs<long>("documentId");

			var data = domain
				.GetAll()
				.Where(x => x.Disposal.Id == documentId)
				.Select(x => new
				{
					x.Id,
					ProvidedDocGji = x.ProvidedDoc.Name,
					x.Description,
					x.Order
				})
				.Filter(loadParam, Container)
				.OrderBy(x => x.Order);

            var totalCount = data.Count();

            return new ListDataResult(data.Paging(loadParam).ToList(), totalCount);
        }
    }
}