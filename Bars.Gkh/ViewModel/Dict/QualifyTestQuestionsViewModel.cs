namespace Bars.Gkh.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;

    public class QualifyTestQuestionsViewModel : BaseViewModel<QualifyTestQuestions>
    {
        public override IDataResult List(IDomainService<QualifyTestQuestions> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domain.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.Name,
                    x.IsActual,
                    x.Question,
                    x.Code
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}