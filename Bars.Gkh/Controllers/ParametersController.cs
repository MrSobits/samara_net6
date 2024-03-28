namespace Bars.Gkh.Controllers
{
    using System.Collections;
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using Domain.ParameterVersioning;

    public class ParametersController : BaseController
    {
        public ParametersController(IVersionedEntityService service)
        {
            _service = service;
        }

        public ActionResult ChangeParameter(BaseParams baseParams)
        {
            var result = _service.SaveParameterVersion(baseParams);

            // стремная магия, решающая проблемы кроссбраузерности — IE и Firefox не
            // хотят парсить ответ на POST-запрос, пришедший с Content-Type: application/json
            var response = result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
            response.ContentType = "text/html";
            return response;
        }

        public ActionResult ListHistory(StoreLoadParams storeLoadParams)
        {
            var result = (ListDataResult)_service.ListChanges(storeLoadParams);

            return result.Success ? new JsonListResult((IList)result.Data, result.TotalCount) : JsFailure(result.Message);
        }

        private readonly IVersionedEntityService _service;
    }
}