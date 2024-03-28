namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using B4;
    using Entities;

    public class ProtocolArticleLawViewModel : BaseViewModel<ProtocolArticleLaw>
    {
        public override IDataResult List(IDomainService<ProtocolArticleLaw> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var documentId = baseParams.Params.GetAs<long>("documentId");

            var data = domainService.GetAll()
                .Where(x => x.Protocol.Id == documentId)
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