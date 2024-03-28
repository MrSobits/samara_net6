using Bars.B4;
using Bars.Gkh.InspectorMobile.Entities;
using Bars.Gkh.InspectorMobile.Services;
using System.Web.Mvc;

namespace Bars.Gkh.InspectorMobile.Controllers
{
    /// <summary>
    /// Контроллер ролей мобильного приложения
    /// </summary>
    public class MpRoleController : B4.Alt.DataController<MpRole>
    {
        /// <inheritdoc />
        public IMpRoleService MpRoleService { get; set; }

        /// <summary>
        /// Добавить роли
        /// </summary>
        /// <param name="baseParams">Параметры с идентификаторами ролей</param>
        /// <returns>Результат выполнения</returns>
        public ActionResult AddRoles(BaseParams baseParams)
        {
            var result = this.MpRoleService.AddRoles(baseParams);
            if (result.Success)
            {
                return new JsonNetResult(new { success = true });
            }

            return JsonNetResult.Failure(result.Message);
        }

        /// <summary>
        /// Получить список ролей мобильного приложения
        /// </summary>
        /// <param name="baseParams">Входные параметры</param>
        /// <returns>Результат выполнения</returns>
        public ActionResult GetRoles(BaseParams baseParams)
        {
            var result = this.MpRoleService.GetRoles(baseParams);
            if (result.Success)
            {
                return new JsonNetResult(new { success = true, message = result.Message, data = result.Data });
            }

            return JsonNetResult.Failure(result.Message);
        }
    }
}