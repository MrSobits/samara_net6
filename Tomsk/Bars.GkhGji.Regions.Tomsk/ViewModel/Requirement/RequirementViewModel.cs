namespace Bars.GkhGji.Regions.Tomsk.ViewModel
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Regions.Tomsk.Entities;

    using Bars.Gkh.Utils;

    public class RequirementViewModel : BaseViewModel<Requirement>
    {
        public override IDataResult List(IDomainService<Requirement> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var documentId = baseParams.Params.GetAs<long>("documentId");

            var articles =
                Container.Resolve<IDomainService<RequirementArticleLaw>>().GetAll()
                    .Where(x => x.Requirement.Document.Id == documentId)
                    .Select(x => new
                    {
                        x.Requirement.Id,
                        x.ArticleLaw.Name
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.Name).AggregateWithSeparator(", "));

            var data = domainService.GetAll()
                .Where(x => x.Document.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    x.Document,
                    x.DocumentDate,
                    x.DocumentNumber,
                    x.Destination,
                    x.TypeRequirement,
                    x.State,
                    x.File
                })
                .AsEnumerable()
                .Select(x => new
                {
                    x.Id,
                    x.Document,
                    x.DocumentDate,
                    x.DocumentNumber,
                    x.Destination,
                    x.TypeRequirement,
                    x.State,
                    x.File,
                    ArticleLaw = articles.Get(x.Id)
                })
                .AsQueryable()
                .Filter(loadParam, Container);

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), data.Count());
        }
    }
}