namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// View Статьи постановления Роспотребнадзора
    /// </summary>
    public class ResolutionRospotrebnadzorArticleLawViewModel
        : ResolutionRospotrebnadzorArticleLawViewModel<ResolutionRospotrebnadzorArticleLaw>
    {
    }

    /// <summary>
    /// Generic View Статьи постановления Роспотребнадзора
    /// </summary>
    public class ResolutionRospotrebnadzorArticleLawViewModel<T> : BaseViewModel<T>
        where T : ResolutionRospotrebnadzorArticleLaw
    {
        public override IDataResult List(IDomainService<T> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();
            var documentId = baseParams.Params.GetAs<long>("documentId");

            var data = domainService.GetAll()
                .WhereIf(documentId > 0, x => x.Resolution.Id == documentId)
                .ToList()
                .Select(x => new
                {
                    x.Id,
                    Name = x.ArticleLaw.Name,
                    Description = x.Description
                })
                .AsQueryable()
                .Filter(loadParam, this.Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam), totalCount);
        }
    }
}