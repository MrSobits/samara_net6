namespace Bars.GkhGji.Regions.Tatarstan.Controller.ActionIsolated
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.GkhGji.Regions.Tatarstan.DomainService.ActionIsolated;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;

    public class TaskActionIsolatedSurveyPurposeController : B4.Alt.DataController<TaskActionIsolatedSurveyPurpose>
    {
        public ActionResult AddPurposes(BaseParams baseParams)
        {
            var taskActionIsolatedSurveyPurposeService = this.Container.Resolve<ITaskActionIsolatedSurveyPurposeService>();

            using (this.Container.Using(taskActionIsolatedSurveyPurposeService))
            {
                return new JsonNetResult(taskActionIsolatedSurveyPurposeService.AddPurposes(baseParams));
            }
        }
    }
}