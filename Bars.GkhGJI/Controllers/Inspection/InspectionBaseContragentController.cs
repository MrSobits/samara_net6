namespace Bars.GkhGji.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    public class InspectionBaseContragentController : B4.Alt.DataController<InspectionBaseContragent>
    {
        public IInspectionBaseContragentService InspectionBaseContragentService { get; set; }

        /// <summary>
        /// Добавить Органы совместной проверки
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения</returns>
        public ActionResult AddContragents(BaseParams baseParams)
        {
            return this.InspectionBaseContragentService.AddContragents(baseParams).ToJsonResult();
        }
    }
}