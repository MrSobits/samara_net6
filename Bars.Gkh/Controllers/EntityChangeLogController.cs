namespace Bars.Gkh.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService;

    /// <summary>
    /// Контроллер журнала изменений сущностей
    /// </summary>
    public class EntityChangeLogController : BaseController
    {
        /// <summary>
        /// Список изменений для сущности
        /// </summary>
        public ActionResult List(BaseParams baseParams)
        {
            var inheritEntityChangeLogCode = baseParams.Params.GetAs<string>("code");

            var entityChangeLogService = string.IsNullOrEmpty(inheritEntityChangeLogCode)
                ? this.Container.Resolve<IEntityChangeLog>()
                : this.Container.Resolve<IInheritEntityChangeLog>(inheritEntityChangeLogCode);

            using (this.Container.Using(entityChangeLogService))
            {
                return entityChangeLogService.List(baseParams).ToJsonResult();
            }
        }
    }
}