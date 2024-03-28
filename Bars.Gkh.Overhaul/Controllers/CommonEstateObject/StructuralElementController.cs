namespace Bars.Gkh.Overhaul.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Overhaul.DomainService;
    using Gkh.Entities.CommonEstateObject;

    /// <summary>
    /// Контроллер управления конструктивными элементами
    /// </summary>
    public class StructuralElementController : B4.Alt.DataController<StructuralElement>
    {
        /// <summary>
        /// Сервис для работы с конструктивными элементами
        /// </summary>
        public IStructuralElementService StructuralElementService { get; set; }

        /// <summary>
        /// Запросить дерево конструктивных элементов для объекта
        /// </summary>
        public ActionResult ListTree(BaseParams baseParams)
        {
            return this.StructuralElementService.ListTree(baseParams).ToJsonResult();
        }

        /// <summary>
        /// Запросить свойства конструктивного элемента
        /// </summary>
        public ActionResult GetAttributes(BaseParams baseParams)
        {
            return this.StructuralElementService.GetAttributes(baseParams).ToJsonResult();
        }
    }
}