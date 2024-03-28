namespace Bars.Gkh.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService.Permission;

    /// <summary>
    /// Контроллер для настройки прав доступа на форме
    /// </summary>
    public class FormPermissionController : BaseController
    {
        public IFormPermssionService FormPermssionService { get; set; }

        /// <summary>
        /// Получить права доступа для формы
        /// </summary>
        public ActionResult GetFormPermissions(BaseParams baseParams)
        {
            return this.FormPermssionService.GetFormPermissions(baseParams).ToJsonResult();
        }

        /// <summary>
        /// Получить список типов на форме
        /// </summary>
        public ActionResult GetEntityTypes(BaseParams baseParams)
        {
            return this.FormPermssionService.GetEntityTypes(baseParams).ToJsonResult();
        }
    }
}