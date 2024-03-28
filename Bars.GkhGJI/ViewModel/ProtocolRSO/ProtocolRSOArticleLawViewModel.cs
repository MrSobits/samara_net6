namespace Bars.GkhGji.ViewModel
{
    using System.Linq;
    using B4;
    using Entities;

    public class ProtocolRSOArticleLawViewModel : BaseViewModel<ProtocolRSOArticleLaw>
    {
        public override IDataResult List(IDomainService<ProtocolRSOArticleLaw> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var documentId = baseParams.Params.GetAs("documentId", 0L);

            var data = domainService.GetAll()
                .Where(x => x.ProtocolRSO.Id == documentId)
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