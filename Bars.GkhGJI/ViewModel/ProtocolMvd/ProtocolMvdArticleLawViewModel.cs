namespace Bars.GkhGji.ViewModel
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Entities;

    public class ProtocolMvdArticleLawViewModel : BaseViewModel<ProtocolMvdArticleLaw>
    {
        public override IDataResult List(IDomainService<ProtocolMvdArticleLaw> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var documentId = baseParams.Params.ContainsKey("documentId")
                                   ? baseParams.Params["documentId"].ToInt()
                                   : 0;

            var data = domainService.GetAll()
                .Where(x => x.ProtocolMvd.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    ArticleLaw = x.ArticleLaw.Name,
                    x.Description
                })
                .Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }
    }
}