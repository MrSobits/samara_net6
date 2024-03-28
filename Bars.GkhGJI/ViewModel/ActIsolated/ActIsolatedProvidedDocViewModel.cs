namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    public class ActIsolatedProvidedDocViewModel : BaseViewModel<ActIsolatedProvidedDoc>
    {
        public override IDataResult List(IDomainService<ActIsolatedProvidedDoc> domain, BaseParams baseParams)
        {
            var loadParam = this.GetLoadParam(baseParams);
            var documentId = baseParams.Params.GetAs<long>("documentId");

            var data = domain
                .GetAll()
                .Where(x => x.ActIsolated.Id == documentId)
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