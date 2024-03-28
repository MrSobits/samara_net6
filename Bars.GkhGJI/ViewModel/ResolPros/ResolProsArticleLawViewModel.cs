namespace Bars.GkhGji.ViewModel
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Entities;

    public class ResolProsArticleLawViewModel : BaseViewModel<ResolProsArticleLaw>
    {
        public override IDataResult List(IDomainService<ResolProsArticleLaw> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var documentId = baseParams.Params.ContainsKey("documentId")
                                   ? baseParams.Params["documentId"].ToLong()
                                   : 0;

            var data = domainService.GetAll()
                .Where(x => x.ResolPros.Id == documentId)
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