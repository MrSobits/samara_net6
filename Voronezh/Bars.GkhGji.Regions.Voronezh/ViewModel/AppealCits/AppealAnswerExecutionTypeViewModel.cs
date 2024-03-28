namespace Bars.GkhGji.Regions.Voronezh.ViewModel
{
    using B4;
    using Entities;
    using System.Linq;

    public class AppealAnswerExecutionTypeViewModel : BaseViewModel<AppealAnswerExecutionType>
    {
        public override IDataResult List(IDomainService<AppealAnswerExecutionType> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var appealCitAnswerId = baseParams.Params.GetAs<long>("appealCitAnswerId");

            var data = domainService.GetAll()
            .Where(x => x.AppealCitsAnswer.Id == appealCitAnswerId)
            .Select(x => new
            {
                x.Id,
                x.AppealExecutionType.Name,
              
            })
            .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).ToList(), totalCount);
        }        

    }
}