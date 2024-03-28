namespace Bars.GkhCr.Controllers
{
    using System;
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4;
    using Bars.GkhCr.Entities;

    public class PerformedWorkActPaymentController : B4.Alt.DataController<PerformedWorkActPayment>
    {
        public override ActionResult Create(BaseParams baseParams)
        {
            try
            {
                var result = DomainService.Save(baseParams);
                return new JsonNetResult(new { success = result.Success, message = result.Message, data = result.Data, summaryData = new { } });
            }
            catch (ValidationException exc)
            {
                return JsonNetResult.Failure(exc.Message);
            }
        }

        public override ActionResult Update(BaseParams baseParams)
        {
            try
            {
                var result = DomainService.Update(baseParams);
                return new JsonNetResult(new { success = result.Success, message = result.Message, data = result.Data, summaryData = new { } });
            }
            catch (ValidationException exc)
            {
                return JsonNetResult.Failure(exc.Message);
            }
        }

        public override ActionResult Delete(BaseParams baseParams)
        {
            try
            {
                var result = DomainService.Delete(baseParams);
                return new JsonNetResult(new { success = result.Success, message = result.Message, data = result.Data, summaryData = new { } });
            }
            catch (ApplicationException exc)
            {
                return JsonNetResult.Failure("Невозможно удалить. Существуют связанные записи");
            }
        }
    }
}
