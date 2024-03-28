namespace Bars.Gkh.Controllers.EfficiencyRating
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService.EfficiencyRating;
    using Bars.Gkh.Entities.EfficiencyRating;

    /// <summary>
    /// Контроллер для <see cref="EfficiencyRatingAnaliticsGraph"/>
    /// </summary>
    public class EfficiencyRatingAnaliticsGraphController : B4.Alt.DataController<EfficiencyRatingAnaliticsGraph>
    {
        /// <summary>
        /// Сервис для работы с аналитикой Рейтинга эффективности
        /// </summary>
        public IEfficiencyRatingAnaliticsGraphService Service { get; set; }

        /// <summary>
        /// Метод возвращает график по параметрам
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>результат запроса</returns>
        public ActionResult GetGraph(BaseParams baseParams)
        {
            return this.Service.GetGraph(baseParams).ToJsonResult();
        }

        /// <summary>
        /// Метод сохраняет или создает график
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>результат запроса</returns>
        public ActionResult SaveOrUpdateGraph(BaseParams baseParams)
        {
            return this.Service.SaveOrUpdateGraph(baseParams).ToJsonResult();
        }
    }
}