namespace Bars.Gkh.RegOperator.Controllers
{
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.RegOperator.DomainService.PaymentDocument;
    using Bars.Gkh.RegOperator.Entities;

    using Microsoft.AspNetCore.Mvc;

    public class SberbankPaymentDocController : FileStorageDataController<SberbankPaymentDoc>
    {
        /// <summary>
        /// Сформировать реестр
        /// </summary>
        public ActionResult CreateReestr(BaseParams baseParams)
        {
            var service = Container.Resolve<ISberbankPaymentDocService>();

            var result = service.CreateReestr(baseParams);

            return result.Success ? JsSuccess(result.Message) : JsFailure(result.Message);
        }
    }
}