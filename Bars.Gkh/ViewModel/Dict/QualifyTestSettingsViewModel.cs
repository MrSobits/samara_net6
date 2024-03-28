namespace Bars.Gkh.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Entities;

    public class QualifyTestSettingsViewModel : BaseViewModel<QualifyTestSettings>
    {
        public override IDataResult List(IDomainService<QualifyTestSettings> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domain.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.AcceptebleRate,
                    x.CorrectBall,
                    x.QuestionsCount,
                    x.TimeStampMinutes,
                    x.DateTo,
                    x.DateFrom

                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}