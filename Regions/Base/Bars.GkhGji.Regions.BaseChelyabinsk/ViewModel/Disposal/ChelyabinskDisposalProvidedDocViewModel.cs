namespace Bars.GkhGji.Regions.BaseChelyabinsk.ViewModel.Disposal
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Disposal;

    public class ChelyabinskDisposalProvidedDocViewModel : BaseViewModel<ChelyabinskDisposalProvidedDoc>
    {
		public override IDataResult List(IDomainService<ChelyabinskDisposalProvidedDoc> domain, BaseParams baseParams)
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
                    x.ProvidedDoc.Code,
					x.Order
				})
				.Filter(loadParam, this.Container)
				.OrderBy(x => x.Order);

            var totalCount = data.Count();

            return new ListDataResult(data.Paging(loadParam).ToList(), totalCount);
        }
    }
}