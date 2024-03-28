namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Regions.Tula.Entities;

    public class ActSurveyConclusionViewModel : BaseViewModel<ActSurveyConclusion>
    {
        public override IDataResult List(IDomainService<ActSurveyConclusion> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var documentId = baseParams.Params.GetAs("documentId", 0L);

            var data = domainService.GetAll()
                .Where(x => x.ActSurvey.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    ActSurvey = x.ActSurvey.Id,
                    Official = x.Official.Fio,
                    x.Description,
                    x.DocDate,
                    x.DocNumber,
                    x.File
                })
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).ToList(), totalCount);
        }
    }
}