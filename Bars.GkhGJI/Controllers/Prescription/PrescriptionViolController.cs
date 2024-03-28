namespace Bars.GkhGji.Controllers
{
    using System;
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    public class PrescriptionViolController : B4.Alt.DataController<PrescriptionViol>
    {
        /// <summary>
        /// Данный метод получает список домов по нарушениям Предписания
        /// </summary>
        public ActionResult ListRealityObject(BaseParams baseParams)
        {
            var result = Container.Resolve<IPrescriptionViolService>().ListRealityObject(baseParams);
            return result.Success ? new JsonListResult((IList)result.Data) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult AddViolations(BaseParams baseParams)
        {
            var result = Container.Resolve<IPrescriptionViolService>().AddViolations(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }


        public ActionResult ListPrescriptionViolation(BaseParams baseParams)
        {
            var prescriptionViolService = Container.Resolve<IPrescriptionViolService>();
            try
            {
                return prescriptionViolService.ListPrescriptionViolation(baseParams).ToJsonResult();
            }
            finally
            {
                //  Container.Release(service);
            }
        }

        public ActionResult AddPrescriptionViolations(BaseParams baseParams)
        {
            var result = Container.Resolve<IPrescriptionViolService>().AddPrescriptionViolations(baseParams);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }

        public ActionResult SetNewDatePlanRemoval(BaseParams baseParams, DateTime paramdate, long documentId)
        {
            var result = Container.Resolve<IPrescriptionViolService>().SetNewDatePlanRemoval(baseParams, paramdate, documentId);
            return result.Success ? new JsonNetResult(new { success = true }) : JsonNetResult.Failure(result.Message);
        }
    }
}