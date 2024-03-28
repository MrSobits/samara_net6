namespace Bars.GkhCr.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.GkhCr.Entities;

    /// <summary>
    /// Контролле видов работ в договорах на услуги
    /// </summary>
    public class ContractCrTypeWorkController : B4.Alt.DataController<ContractCrTypeWork>
    {
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
            catch (ValidationException exc)
            {
                return JsonNetResult.Failure(exc.Message);
            }
        }
    }
}
