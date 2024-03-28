namespace Bars.GkhCr.Controllers
{
    using B4;

    using Bars.B4.IoC;

    using DomainService;

    using Entities;

    using Microsoft.AspNetCore.Mvc;

    public class SpecialTypeWorkCrHistoryController : B4.Alt.DataController<SpecialTypeWorkCrHistory>
    {
        /// <summary>
        /// Метод восстановления записи из истории
        /// </summary>
        public ActionResult Restore(BaseParams baseParams)
        {
            var service = this.Resolve<ISpecialTypeWorkCrHistoryService>();

            using (this.Container.Using(service))
            {
                var result = (BaseDataResult) service.Restore(baseParams);
                return result.Success ? new JsonNetResult(new { succes = true }) : JsonNetResult.Failure(result.Message);
            }
        }
    }
}
