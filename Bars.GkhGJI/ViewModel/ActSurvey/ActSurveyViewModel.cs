namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class ActSurveyViewModel: ActSurveyViewModel<ActSurvey>
    {
    }

    public class ActSurveyViewModel<T> : BaseViewModel<T>
        where T: ActSurvey
    {
        public override IDataResult List(IDomainService<T> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var stageId = baseParams.Params.ContainsKey("stageId")
                                   ? baseParams.Params["stageId"].ToLong()
                                   : 0;

            var data = domainService.GetAll()
                .Where(x => x.Stage.Id == stageId)
                .Select(x => new
                {
                    x.Id,
                    DocumentId = x.Id,
                    InspectionId = x.Inspection.Id,
                    x.Inspection,
                    x.Stage,
                    x.TypeDocumentGji,
                    x.DocumentDate,
                    x.DocumentNumber
                })
                .Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }
    }
}