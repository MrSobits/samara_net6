namespace Bars.Gkh.Regions.Perm.Controllers
{
    using System.Collections;
    using B4;

    using Bars.Gkh.DomainService;

    using Domain.ParameterVersioning;

    using Microsoft.AspNetCore.Mvc;

    /// <summary>
    /// Контроллер версионируемых параметров, переопределенный для Перми
    /// </summary>
    public class ParametersController : BaseController
    {
        public ParametersController(IVersionedEntityService versionedService, IEmergencyObjectSyncService emergencyObjectSyncService)
        {
            this.versionedService = versionedService;
            this.emergencyObjectSyncService = emergencyObjectSyncService;
        }

        /// <summary>
        /// Изменить параметр
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        public ActionResult ChangeParameter(BaseParams baseParams)
        {
            var className = baseParams.Params.GetAs<string>("className", ignoreCase: true);
            var propName = baseParams.Params.GetAs<string>("propertyName", ignoreCase: true);

            IDataResult result;
            if (className.Equals("RealityObject") && propName.Equals("ConditionHouse"))
            {
                this.emergencyObjectSyncService.UpdateConditionHouse(baseParams);
                result = new BaseDataResult(true);
            }
            else
            {
                result = this.versionedService.SaveParameterVersion(baseParams);
            }

            var response = result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
            response.ContentType = "text/html";
            return response;
        }

        /// <summary>
        /// Получить историю изменений
        /// </summary>
        /// <param name="storeLoadParams">Параметры</param>
        public ActionResult ListHistory(StoreLoadParams storeLoadParams)
        {
            var result = (ListDataResult) this.versionedService.ListChanges(storeLoadParams);

            return result.Success ? new JsonListResult((IList)result.Data, result.TotalCount) : this.JsFailure(result.Message);
        }

        private readonly IVersionedEntityService versionedService;

        private readonly IEmergencyObjectSyncService emergencyObjectSyncService;
    }
}
