namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.Dict
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities.Dict;

    public class SurveySubjectViewModel : BaseViewModel<SurveySubject>
    {
        public override IDataResult List(IDomainService<SurveySubject> domainService, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var ids = baseParams.Params.GetAs<long[]>("ids") ?? new long[0];

            var data = domainService.GetAll()
                .Where(x => !ids.Contains(x.Id))
                .Filter(loadParams, this.Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams), data.Count());
        }
    }
}