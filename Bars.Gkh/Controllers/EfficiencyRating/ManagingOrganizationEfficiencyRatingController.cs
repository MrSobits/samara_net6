namespace Bars.Gkh.Controllers.EfficiencyRating
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService.EfficiencyRating;
    using Bars.Gkh.Entities.EfficiencyRating;

    /// <summary>
    /// Контроллер
    /// </summary>
    public class ManagingOrganizationEfficiencyRatingController : B4.Alt.DataController<ManagingOrganizationEfficiencyRating>
    {
        /// <summary>
        /// Сервис по работе с Рейтингом эффективности
        /// </summary>
        public IEfficiencyRatingService EfficiencyRatingService { get; set; }

        /// <summary>
        /// Метод возвращает все факторы, которые имеются в системе
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат запроса</returns>
        public ActionResult GetFactors(BaseParams baseParams)
        {
            return this.EfficiencyRatingService.GetFactors(baseParams).ToJsonResult();
        }
    }
}