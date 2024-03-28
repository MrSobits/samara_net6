namespace Bars.GkhGji.ViewModel
{
    using System.Linq;
    using B4.Utils;
    using Bars.B4;
    using Entities.Dict;

    public class AuditPurposeSurveySubjectGjiViewModel : BaseViewModel<AuditPurposeSurveySubjectGji>
    {
        public IDomainService<SurveySubject> SurveySubjectService { get; set; }
        public override IDataResult List(IDomainService<AuditPurposeSurveySubjectGji> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var documentId = baseParams.Params.GetAs<long>("documentId");
            var list = domainService.GetAll()
                .Where(x => x.AuditPurpose.Id == documentId)
                .Select(x => x.SurveySubject.Id)
                .ToList();

            var data = SurveySubjectService.GetAll()
                .Select(x => new
                {
                    x.Id,
                    x.Code,
                    x.Name,
                    Selected = list.Contains(x.Id)
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}
