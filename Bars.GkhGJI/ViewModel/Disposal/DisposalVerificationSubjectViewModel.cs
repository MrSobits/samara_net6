namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;

    public class DisposalVerificationSubjectViewModel : BaseViewModel<DisposalVerificationSubject>
    {
        public IDomainService<DisposalVerificationSubject> ServiceDisposalVerificationSubject { get; set; }

        public override IDataResult List(IDomainService<DisposalVerificationSubject> domainService, BaseParams baseParams)
        {
            var dispId = baseParams.Params.GetAs<long>("documentId");

            var existingValues = this.ServiceDisposalVerificationSubject.GetAll()
                .Where(x => x.Disposal.Id == dispId)
                .Select(
                    x => new
                    {
                        x.Id,
                        x.SurveySubject.Code,
                        x.SurveySubject.Name,
                        SurveySubjectId = x.SurveySubject.Id
                    }).ToList();

            return new ListDataResult(existingValues, existingValues.Count);
        }
    }
}
