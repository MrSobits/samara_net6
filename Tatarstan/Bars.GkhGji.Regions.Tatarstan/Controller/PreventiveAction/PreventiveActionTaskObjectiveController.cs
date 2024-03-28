namespace Bars.GkhGji.Regions.Tatarstan.Controller.PreventiveAction
{
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    //using Bars.GisIntegration.Base.Domain;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;
    using System;
    using System.Linq;

    using Bars.Gkh.Domain;

    using Microsoft.AspNetCore.Mvc;

    public class PreventiveActionTaskObjectiveController : B4.Alt.DataController<PreventiveActionTaskObjective>
    {
        public ActionResult AddObjective(BaseParams baseParams)
        {
            var documentId = baseParams.Params.GetAsId("documentId");
            var objectiveIds = baseParams.Params.GetAs<long[]>("objectiveIds");

            var taskRepo = this.Container.Resolve<IRepository<PreventiveActionTask>>();
            var objectiveRepo = this.Container.Resolve<IRepository<ObjectivesPreventiveMeasure>>();
            var taskObjectiveRepo = this.Container.Resolve<IRepository<PreventiveActionTaskObjective>>();

            using (this.Container.Using(taskRepo, objectiveRepo, taskObjectiveRepo))
            {
                try
                {
                    var task = taskRepo.Get(documentId);

                    var existsObjectiveIds = taskObjectiveRepo.GetAll()
                        .Where(x => x.PreventiveActionTask.Id == documentId && objectiveIds.Contains(x.ObjectivesPreventiveMeasure.Id))
                        .Select(x => x.ObjectivesPreventiveMeasure.Id);

                    objectiveIds = objectiveIds.Except(existsObjectiveIds).ToArray();

                    if (objectiveIds.Any())
                    {
                        var objectives = objectiveRepo.GetAll()
                        .Where(x => objectiveIds.Contains(x.Id));

                        objectives.ForEach(
                            x =>
                            {
                                var taskObjective = new PreventiveActionTaskObjective
                                {
                                    PreventiveActionTask = task,
                                    ObjectivesPreventiveMeasure = x
                                };

                                taskObjectiveRepo.Save(taskObjective);
                            });
                    }

                    return new JsonNetResult(new { success = true });
                }
                catch (Exception e)
                {
                    return JsonNetResult.Failure(e.Message);
                }
            }
        }
    }
}
