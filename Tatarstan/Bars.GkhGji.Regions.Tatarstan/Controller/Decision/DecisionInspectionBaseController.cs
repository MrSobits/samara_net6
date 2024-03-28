namespace Bars.GkhGji.Regions.Tatarstan.Controller.Decision
{
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.GkhGji.Regions.Tatarstan.DomainService.Decision;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Decision;

    /// <summary>
    /// Контроллер для сущности <see cref="DecisionInspectionBase"/>
    /// </summary>
    public class DecisionInspectionBaseController :  B4.Alt.DataController<DecisionInspectionBase>
    {
        /// <summary>
        /// Получаем список объектов <see cref="DecisionInspectionBase"/>
        /// </summary>
        public ActionResult ListInspectionBaseType(BaseParams baseParams)
        {
            var service = this.Container.Resolve<IDecisionInspBaseService>();

            using (this.Container.Using(service))
            {
                var result = service.ListInspectionBaseType(baseParams);
                return result.Success ? new JsonListResult((IList)result.Data): JsonNetResult.Failure(result.Message);
            }
        }
    }
}