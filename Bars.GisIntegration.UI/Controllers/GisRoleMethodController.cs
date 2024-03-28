namespace Bars.GisIntegration.UI.Controllers
{
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.GisIntegration.Base.Entities.GisRole;
    using Bars.GisIntegration.UI.Service;

    /// <summary>
    /// Контроллер GisRoleMethod
    /// </summary>
    public class GisRoleMethodController : B4.Alt.DataController<GisRoleMethod>
    {
        /// <summary>
        /// Добавить методы интеграции для роли ГИС
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат выполнения операции</returns>
        public ActionResult AddRoleMetods(BaseParams baseParams)
        {
            var gisRoleMethodService = this.Container.Resolve<IGisRoleMethodService>();

            try
            {
                return new JsonNetResult(gisRoleMethodService.AddRoleMetods(baseParams));
            }
            finally
            {
                this.Container.Release(gisRoleMethodService);
            }
        }
    }
}
