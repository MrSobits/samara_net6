namespace Bars.GkhGji.Regions.Tatarstan.Controller.ActionIsolated
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.GkhGji.Regions.Tatarstan.DomainService.ActionIsolated;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;

    public class TaskActionIsolatedArticleLawController : B4.Alt.DataController<TaskActionIsolatedArticleLaw>
    {
        public ActionResult AddArticles(BaseParams baseParams)
        {
            var taskActionIsolatedArticleLawService = this.Container.Resolve<ITaskActionIsolatedArticleLawService>();

            using (this.Container.Using(taskActionIsolatedArticleLawService))
            {
                return new JsonNetResult(taskActionIsolatedArticleLawService.AddArticles(baseParams));
            }
        }
    }
}
