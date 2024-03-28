namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    public class ActIsolatedInspectedPartViewModel : BaseViewModel<ActIsolatedInspectedPart>
    {
        public override IDataResult List(IDomainService<ActIsolatedInspectedPart> domainService, BaseParams baseParams)
        {
            var loadParam = this.GetLoadParam(baseParams);
            var documentId = baseParams.Params.GetAs<long>("documentId");

            var data = domainService.GetAll()
                .Where(x => x.ActIsolated.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    InspectedPart = x.InspectedPart.Name,
                    x.Character,
                    x.Description
                })
                .Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }
    }
}