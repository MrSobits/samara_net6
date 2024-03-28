namespace Bars.Gkh.RegOperator.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.RegOperator.DomainService.PersonalAccount;

    /// <summary>
    /// Контроллер для работы с группами ЛС
    /// </summary>
    public class PersonAccountGroupController : BaseController
    {
        public IPersonalAccountGroupService Service { get; set; }

        /// <summary>
        /// Список групп, в которых состоит указанный ЛС
        /// </summary>
        /// <param name="baseParams">Базовые параметры запроса</param>
        public ActionResult ListGroups(BaseParams baseParams)
        {
            return this.Service.ListGroupsByAccount(baseParams).ToJsonResult();
        }

        /// <summary>
        /// Добавить лицевой счёт в группу
        /// </summary>
        /// <param name="baseParams">Баозовые параметры запроса</param>
        public ActionResult AddPersonalAccountToGroups(BaseParams baseParams)
        {
            return  this.Service.AddPersonalAccountToGroups(baseParams).ToJsonResult();
        }

        /// <summary>
        /// Удалить лицевой счёт из групп
        /// </summary>
        /// <param name="baseParams">Баозовые параметры запроса</param>
        public ActionResult RemovePersonalAccountFromGroups(BaseParams baseParams)
        {
            return this.Service.RemovePersonalAccountFromGroups(baseParams).ToJsonResult();
        }
    }
}