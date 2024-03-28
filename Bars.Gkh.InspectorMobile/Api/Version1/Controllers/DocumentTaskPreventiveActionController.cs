namespace Bars.Gkh.InspectorMobile.Api.Version1.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Description;

    using Bars.Gkh.BaseApiIntegration.Attributes;
    using Bars.Gkh.BaseApiIntegration.Controllers;
    using Bars.Gkh.BaseApiIntegration.Models;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.TaskPreventiveAction;
    using Bars.Gkh.InspectorMobile.Api.Version1.Services;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;

    /// <summary>
    /// API контроллер для <see cref="PreventiveActionTask"/>
    /// </summary>
    [RoutePrefix("v1/docTaskPreventiveAction")]
    public class DocumentTaskPreventiveActionController : BaseApiController
    {
        /// <summary>
        /// Получить документ "Задание профилактического мероприятия"
        /// </summary>
        [HttpGet]
        [Route("{documentId:long}")]
        [ResponseType(typeof(BaseApiResponse<DocumentTaskPreventiveActionGet>))]
        public async Task<IHttpActionResult> Get(long documentId) =>
            await this.ExecuteApiServiceMethodAsync<IDocumentTaskPreventiveActionService>(nameof(IDocumentTaskPreventiveActionService.Get), documentId);
        
        /// <summary>
        /// Получить список документов "Задание профилактического мероприятия"
        /// </summary>
        [HttpGet]
        [Route("list")]
        [ResponseType(typeof(BaseApiResponse<List<DocumentTaskPreventiveActionGet>>))]
        public async Task<IHttpActionResult> List([FromUri] TaskPreventiveActionQueryParams queryParams) =>
            await this.ExecuteApiServiceMethodAsync<IDocumentTaskPreventiveActionService>(nameof(IDocumentTaskPreventiveActionService.GetListByQuery), queryParams);

        /// <summary>
        /// Редактирование документа "Задание профилактического мероприятия"
        /// </summary>
        /// <param name="documentId">Идентификатор документа</param>
        /// <param name="updateDocument">Обновляемый документ</param>
        [HttpPut]
        [Route("{documentId:long}")]
        [ValidateModel]
        [ResponseType(typeof(BaseApiResponse<long>))]
        public async Task<IHttpActionResult> Update(long documentId, [FromBody] DocumentTaskPreventiveActionUpdate updateDocument) =>
            await this.ExecuteApiServiceMethodAsync<IDocumentTaskPreventiveActionService>(nameof(IDocumentTaskPreventiveActionService.Update), documentId, updateDocument);
    }
}