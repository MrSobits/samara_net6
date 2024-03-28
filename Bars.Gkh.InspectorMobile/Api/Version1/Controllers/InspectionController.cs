namespace Bars.Gkh.InspectorMobile.Api.Version1.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Description;

    using Bars.Gkh.BaseApiIntegration.Controllers;
    using Bars.Gkh.BaseApiIntegration.Models;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Inspection;
    using Bars.Gkh.InspectorMobile.Api.Version1.Services;

    /// <summary>
    /// API-контроллер для получения документов
    /// </summary>
    [RoutePrefix("v1/inspection")]
    public class InspectionController : BaseApiController
    {
        /// <summary>
        /// Получить список документов "Решение", "Задание"
        /// </summary>
        [HttpGet]
        [Route("controlaction")]
        [ResponseType(typeof(BaseApiResponse<InspectionControlAction[]>))]
        public async Task<IHttpActionResult> GetControlActionDocuments() => 
            await this.ExecuteApiServiceMethodAsync<IInspectionControlActionService>(nameof(IInspectionControlActionService.GetControlActionDocuments));

        /// <summary>
        /// Получить список документов "Профилактическое мероприятие"
        /// </summary>
        [HttpGet]
        [Route("preventiveaction")]
        [ResponseType(typeof(BaseApiResponse<InspectionPreventiveActionTask[]>))]
        public async Task<IHttpActionResult> GetPreventiveActionDocuments() =>
            await this.ExecuteApiServiceMethodAsync<IInspectionPreventiveActionService>(nameof(IInspectionPreventiveActionService.GetListAsync));
    }
}