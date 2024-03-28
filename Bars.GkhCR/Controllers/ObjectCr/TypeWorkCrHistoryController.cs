namespace Bars.GkhCr.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using DomainService;
    using Entities;

    public class TypeWorkCrHistoryController : B4.Alt.DataController<TypeWorkCrHistory>
    {
        /// <summary>
        /// Метод  восстановления записи из истории
        /// </summary>
        public ActionResult Restore(BaseParams baseParams)
        {
            var result = (BaseDataResult)Resolve<ITypeWorkCrHistoryService>().Restore(baseParams);
            return result.Success ? new JsonNetResult(new { succes = true }) : JsonNetResult.Failure(result.Message);
        }
    }
}