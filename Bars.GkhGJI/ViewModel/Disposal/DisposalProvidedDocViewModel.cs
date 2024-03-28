namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class DisposalProvidedDocViewModel : BaseViewModel<DisposalProvidedDoc>
    {
        public override IDataResult List(IDomainService<DisposalProvidedDoc> domain, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var documentId = baseParams.Params.ContainsKey("documentId")
                                   ? baseParams.Params["documentId"].ToLong()
                                   : 0;

            var data = domain
                .GetAll()
                .Where(x => x.Disposal.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    ProvidedDocGji = x.ProvidedDoc.Name,
                    x.Description
                })

                .Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Paging(loadParam).ToList(), totalCount);
        }
    }
}