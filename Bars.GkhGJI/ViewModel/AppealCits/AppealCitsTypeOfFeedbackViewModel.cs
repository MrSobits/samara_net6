namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;

    public class AppealCitsTypeOfFeedbackViewModel : BaseViewModel<AppealCitsTypeOfFeedback>
    {
        public override IDataResult List(IDomainService<AppealCitsTypeOfFeedback> domainService, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var appealCitizensId = baseParams.Params.GetAs<long>("appealCitizensId");

            return domainService.GetAll()
                .Where(x=> x.AppealCits.Id == appealCitizensId)
                .Select(x => new
                {
                    TypeOfFeedback = x.TypeOfFeedback.Name,
                    x.AppealCits,
                    x.Id,
                    x.FileInfo
                })
                .ToListDataResult(baseParams.GetLoadParam(), this.Container);
        }
    }
}