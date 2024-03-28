namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.ActionIsolated
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;

    public class TaskActionIsolatedSurveyPurposeViewModel : BaseViewModel<TaskActionIsolatedSurveyPurpose>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<TaskActionIsolatedSurveyPurpose> domainService, BaseParams baseParams)
        {
            var documentId = baseParams.Params.GetAsId("documentId");
            
            return domainService.GetAll().Where(x=>x.TaskActionIsolated.Id == documentId)
                .Select(x=> new
                {
                    x.Id,
                    x.SurveyPurpose.Name
                })
                .ToListDataResult(baseParams.GetLoadParam(), Container);
        }
    }
}