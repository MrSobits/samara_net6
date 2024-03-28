namespace Bars.GkhGji.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Контроллер для "Подготовка к работе в зимних условиях"
    /// </summary>
    public class WorkWinterConditionController : B4.Alt.DataController<WorkWinterCondition>
    {
        public IHeatInputService Service { get; set; }

        /// <summary>
        ///  Сохранить изменения для Подготовка к работе в зимних условиях
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        public ActionResult SaveChangedInfo(BaseParams baseParams)
        {
            return new JsonNetResult(this.Service.SaveWorkWinterInfo(baseParams));
        }

        /// <summary>
        /// Копировать значения "Показатели о готовности ЖКС к зимнему периоду" данной сущности "Подготовка к работе в зимних условиях"
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат выполнения запроса</returns>
        public ActionResult CopyPeriodWorkWinterCondition(BaseParams baseParams)
        {
            return new JsonNetResult(this.Service.CopyPeriodWorkWinterCondition(baseParams));
        }
    }
}