namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;

    public class DisposalTypeSurveyViewModel : BaseViewModel<DisposalTypeSurvey>
    {
        public override IDataResult List(IDomainService<DisposalTypeSurvey> domain, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var documentId = baseParams.Params.ContainsKey("documentId")
                                 ? baseParams.Params["documentId"].ToLong()
                                 : 0;

            var data = domain
                .GetAll()
                .Where(x => x.Disposal.Id == documentId)
                .Select(x => new
                {
                    x.Id,
                    TypeSurvey = x.TypeSurvey.Name
                })
                .Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }
    }
}