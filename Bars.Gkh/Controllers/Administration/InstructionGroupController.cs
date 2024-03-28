namespace Bars.Gkh.Controllers.Administration
{
    using Bars.Gkh.Entities;
    using Bars.B4;
    using Microsoft.AspNetCore.Mvc;
    using Bars.Gkh.DomainService;

    /// <summary>
    /// Контроллер для категории документаций
    /// </summary>
    public class InstructionGroupController : Bars.B4.Alt.DataController<InstructionGroup>
    {
        /// <summary>
        /// Сервис
        /// </summary>
        public IInstructionGroupService Service { get; set; }

        /// <summary>
        /// Дерево документаций с категориями
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Дерево</returns>
        public ActionResult GetTree(BaseParams baseParams)
        {
            var result = this.Service.ListByRole(baseParams);
            return new JsonNetResult(result.Data);
        }
    }
}
