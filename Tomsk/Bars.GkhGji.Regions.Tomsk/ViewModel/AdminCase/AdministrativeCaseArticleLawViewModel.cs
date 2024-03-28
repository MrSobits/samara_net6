namespace Bars.GkhGji.Regions.Tomsk.ViewModel
{
    using System.Linq;
    using B4;
    using Bars.GkhGji.Regions.Tomsk.Entities;

    public class AdministrativeCaseArticleLawViewModel : BaseViewModel<AdministrativeCaseArticleLaw>
    {
        public override IDataResult List(IDomainService<AdministrativeCaseArticleLaw> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var documentId = baseParams.Params.GetAs<long>("documentId");

            var data = domainService.GetAll()
                .Where(x => x.AdministrativeCase.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    ArticleLaw = x.ArticleLaw.Name
                })
                .Filter(loadParam, Container);

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), data.Count());
        }
    }
}