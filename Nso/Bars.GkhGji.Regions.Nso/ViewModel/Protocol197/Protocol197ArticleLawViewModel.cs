namespace Bars.GkhGji.Regions.Nso.ViewModel
{
	using System.Linq;
	using Bars.B4;
	using Bars.GkhGji.Regions.Nso.Entities;

	public class Protocol197ArticleLawViewModel : BaseViewModel<Protocol197ArticleLaw>
    {
        public override IDataResult List(IDomainService<Protocol197ArticleLaw> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var documentId = baseParams.Params.GetAs<long>("documentId");

            var data = domainService.GetAll()
                .Where(x => x.Protocol197.Id == documentId)
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