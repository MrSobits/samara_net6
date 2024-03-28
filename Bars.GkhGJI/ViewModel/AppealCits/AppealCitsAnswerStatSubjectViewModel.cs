namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;

    public class AppealCitsAnswerStatSubjectViewModel : BaseViewModel<AppealCitsAnswerStatSubject>
    {
        public override IDataResult List(IDomainService<AppealCitsAnswerStatSubject> domainService, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var answerId = loadParams.Filter.GetAsId("answerId");

            return domainService.GetAll()
                .Where(x => x.AppealCitsAnswer.Id == answerId)
                .Select(x => new
                {
                    x.Id,
                    Subject = x.StatSubject.Subject.Name,
                    Subsubject = x.StatSubject.Subsubject.Name,
                    Feature = x.StatSubject.Feature.Name
                })
                .ToListDataResult(loadParams, this.Container);
        }
    }
}
