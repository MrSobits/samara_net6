namespace Bars.Gkh.Controllers.Administration
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService;

    /// <summary>
    /// Контроллер работы с локальными администраторами
    /// </summary>
    public class LocalAdminRoleController : BaseController
    {
        public ILocalAdminRoleService LocalAdminRoleService { get; set; }

        /// <summary>
        /// Список локальных администраторов
        /// </summary>
        public ActionResult List(BaseParams baseParams)
        {
            return this.LocalAdminRoleService.GetAll(baseParams).ToJsonResult();
        }

        /// <summary>
        /// Список дочерних ролей локального администратора
        /// </summary>
        public ActionResult ListChild(BaseParams baseParams)
        {
            return this.LocalAdminRoleService.GetChildRoleList(baseParams).ToJsonResult();
        }
    }
}