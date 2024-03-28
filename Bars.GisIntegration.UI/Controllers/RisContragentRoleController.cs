namespace Bars.GisIntegration.UI.Controllers
{
    using System.Web.Mvc;

    using Bars.B4;
    using Bars.GisIntegration.Base.Entities.GisRole;
    using Bars.GisIntegration.UI.Service;

    /// <summary>
    /// Контроллер RisContragentRole
    /// </summary>
    public class RisContragentRoleController : B4.Alt.DataController<RisContragentRole>
    {
        /// <summary>
        /// Добавить контрагенту роли ГИС
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат выполнения операции</returns>
        public ActionResult AddContragentRoles(BaseParams baseParams)
        {
            var risContragentRoleService = this.Container.Resolve<IRisContragentRoleService>();

            try
            {
                return new JsonNetResult(risContragentRoleService.AddContragentRoles(baseParams));
            }
            finally
            {
                this.Container.Release(risContragentRoleService);
            }
        }
    }
}
