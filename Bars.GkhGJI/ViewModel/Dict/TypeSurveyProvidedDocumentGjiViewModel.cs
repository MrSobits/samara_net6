namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    public class TypeSurveyProvidedDocumentGjiViewModel : BaseViewModel<TypeSurveyProvidedDocumentGji>
    {
        public override IDataResult List(IDomainService<TypeSurveyProvidedDocumentGji> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var typeSurveyId = baseParams.Params.GetAs("typeSurveyId", 0L);

            var data = domainService.GetAll()
                .Where(x => x.TypeSurvey.Id == typeSurveyId)
                .Select(x => new
                {
                    x.Id,
                    x.ProvidedDocGji.Code,
                    x.ProvidedDocGji.Name
                })
                .Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }
    }
}