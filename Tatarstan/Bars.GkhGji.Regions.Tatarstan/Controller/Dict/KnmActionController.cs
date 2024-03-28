namespace Bars.GkhGji.Regions.Tatarstan.Controller.Dict
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Regions.Tatarstan.DomainService.Dict;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict.KnmActions;

    /// <summary>
    /// Контроллер для работы с действиями в рамках КНМ
    /// </summary>
    public class KnmActionController : B4.Alt.DataController<KnmAction>
    {
        /// <summary>
        /// Добавить запланированные действия решения
        /// </summary>
        public ActionResult AddDecisionPlannedActions(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IKnmActionService>();

            using (this.Container.Using(service))
            {
                return service.AddDecisionPlannedActions(baseParams).ToJsonResult();
            }
        }

        /// <summary>
        /// Добавить запланированные действия задания КНМ
        /// </summary>
        public ActionResult AddTaskActionIsolatedPlannedActions(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IKnmActionService>();

            using (this.Container.Using(service))
            {
                return service.AddTaskActionIsolatedPlannedActions(baseParams).ToJsonResult();
            }
        }
    }
}