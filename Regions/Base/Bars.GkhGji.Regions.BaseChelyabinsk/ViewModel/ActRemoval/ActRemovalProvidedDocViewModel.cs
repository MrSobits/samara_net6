namespace Bars.GkhGji.Regions.BaseChelyabinsk.ViewModel.ActRemoval
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActRemoval;

    public class ActRemovalProvidedDocViewModel : BaseViewModel<ActRemovalProvidedDoc>
    {
		public override IDataResult List(IDomainService<ActRemovalProvidedDoc> domain, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var documentId = baseParams.Params.ContainsKey("documentId")
                                   ? baseParams.Params["documentId"].ToLong()
                                   : 0;

            var data = domain
                .GetAll()
                .Where(x => x.ActRemoval.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    x.DateProvided,
                    ProvidedDocGji = x.ProvidedDoc.Name
                })
                .Filter(loadParam, this.Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }
    }
}