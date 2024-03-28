namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using B4.Utils;
    using Bars.GkhGji.Entities;

    public class ActSurveyInspectedPartViewModel : BaseViewModel<ActSurveyInspectedPart>
    {
        public override IDataResult List(IDomainService<ActSurveyInspectedPart> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var documentId = baseParams.Params.ContainsKey("documentId") ? baseParams.Params["documentId"].ToLong() : 0;

            var data = domainService.GetAll()
                .Where(x => x.ActSurvey.Id == documentId)
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