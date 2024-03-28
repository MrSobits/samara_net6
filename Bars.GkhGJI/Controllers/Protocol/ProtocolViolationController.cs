namespace Bars.GkhGji.Controllers
{
    using System;
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    public class ProtocolViolationController : B4.Alt.DataController<ProtocolViolation>
    {
        /// <summary>
        /// Данный метод получает список домов по нарушениям Протокола
        /// </summary>
        public ActionResult ListRealityObject(BaseParams baseParams)
        {
            var service = Container.Resolve<IProtocolViolationService>();
            try
            {
                var result = service.ListRealityObject(baseParams);
                return result.Success ? new JsonListResult((IList)result.Data) : JsonNetResult.Failure(result.Message);
            }
            finally 
            {
                Container.Release(service);
            }
        }

        /// <summary>
        /// Данный метод получает добавляет нарушения в список
        /// </summary>
        public ActionResult AddViolations(BaseParams baseParams)
        {
            var service = Container.Resolve<IProtocolViolationService>();
            try
            {
                var result = service.AddViolations(baseParams);
                return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
            }
            finally
            {
                Container.Release(service);
            }
        }
    }
}
