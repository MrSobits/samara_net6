namespace Bars.GkhCr.Controllers
{
    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.GkhCr.DomainService;

    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Контроллер <see cref="Entities.SpecialObjectCr"/>
    /// </summary>
    public class SpecialObjectCrController : B4.Alt.DataController<Entities.SpecialObjectCr>
    {
        /// <summary>
        /// Сервис
        /// </summary>
        public ISpecialObjectCrService Service { get; set; }

        /// <summary>
        /// Получить подрядчиков
        /// </summary>
        public ActionResult GetBuilders(BaseParams baseParams)
        {
            var listResult = (ListDataResult) this.Service.GetBuilders(baseParams);
            return new JsonNetResult(new {success = true, data = listResult.Data, totalCount = listResult.TotalCount});
        }

        /// <summary>
        /// Список конкурсов
        /// </summary>
        public ActionResult ListCompetitions(BaseParams baseParams)
        {
            var listResult = (ListDataResult) this.Resolve<IObjectCrCompetitionService>().ListCompetitions(baseParams);
            return new JsonNetResult(new {success = listResult.Success, data = listResult.Data, totalCount = listResult.TotalCount});
        }

        /// <summary>
        /// Восстановить
        /// </summary>
        public ActionResult Recover(BaseParams baseParams)
        {
            var result = this.Service.Recover(baseParams);
            return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
        }

        /// <summary>
        /// Получить дополнительные параметры
        /// </summary>
        public ActionResult GetAdditionalParams(BaseParams baseParams)
        {
            var result = this.Service.GetAdditionalParams(baseParams);

            return result.ToJsonResult();
        }
    }
}