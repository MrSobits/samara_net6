namespace Bars.Gkh.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;

    public class ContragentAdditionRoleController : B4.Alt.DataController<ContragentAdditionRole>
    {
        /// <summary>
        /// Сервис по работе с доп ролями контагента
        /// </summary>
        public IContragentAdditionRoleService Service { get; set; }

        /// <summary>
        /// Сервис по работе с домами РСО
        /// </summary>
        public IContragentService ContragentService { get; set; }

        public ActionResult ListAdditionRole(BaseParams baseParams)
        {
            return this.ContragentService.ListAdditionRole(baseParams).ToJsonResult();
        }

        /// <summary>
        /// Добавить доп роль к контагенту
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат</returns>
        public ActionResult AddAdditionRole(BaseParams baseParams)
        {
            return this.Service.AddAdditionRole(baseParams).ToJsonResult();
        }
    }
}