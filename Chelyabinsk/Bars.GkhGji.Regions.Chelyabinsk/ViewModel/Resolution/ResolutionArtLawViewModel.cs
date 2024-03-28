namespace Bars.GkhGji.Regions.Chelyabinsk.ViewModel
{
    using Entities;
    using System.Linq;
    using B4;
    using B4.Utils;

    public class ResolutionArtLawViewModel : BaseViewModel<ResolutionArtLaw>
    {
        public override IDataResult List(IDomainService<ResolutionArtLaw> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var documentId = baseParams.Params.ContainsKey("documentId")
                                   ? baseParams.Params["documentId"].ToLong()
                                   : 0;

            var data = domainService.GetAll()
                .Where(x => x.Resolution.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    x.DocumentDate,
                    ArticleLawGji = x.ArticleLawGji.Name,
                    Code = x.ArticleLawGji.Code,
                    x.Description,
                })
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).ToList(), totalCount);
        }
    }
}