namespace Bars.GkhGji.Controllers
{
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    public class DisposalViolController : B4.Alt.DataController<DisposalViolation>
    {
        /// <summary>
        /// Данный метод получает список домов по нарушениям Распоряжения
        /// </summary>
        public ActionResult ListRealityObject(BaseParams baseParams)
        {
            var result = Container.Resolve<IDisposalViolService>().ListRealityObject(baseParams);
            return result.Success ? new JsonListResult((IList)result.Data) : JsonNetResult.Failure(result.Message);
        }

        /// <summary>
        /// Метод добавления нарушений 
        /// </summary>
        public ActionResult AddViolations(BaseParams baseParams)
        {
            var result = Container.Resolve<IDisposalViolService>().AddViolations(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }
    }
}