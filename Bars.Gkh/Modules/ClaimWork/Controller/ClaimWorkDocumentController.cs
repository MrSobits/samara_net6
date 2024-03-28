namespace Bars.Gkh.Modules.ClaimWork.Controllers
{
    using B4;
    using DomainService;
    using System;

    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Базовый контроллер для всех оснований претензеонно исковой работы
    /// </summary>
    public class ClaimWorkDocumentController : BaseController
    {
        public ActionResult CreateDocument(BaseParams baseParams)
        {
            var service = Container.Resolve<IClaimWorkDocumentProvider>();
            try
            {
                var result = service.CreateDocument(baseParams);
                return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
            }
            catch (Exception e)
            {
                Container.Release(service);
                return JsFailure(e.Message);
            }
        }
    }
}