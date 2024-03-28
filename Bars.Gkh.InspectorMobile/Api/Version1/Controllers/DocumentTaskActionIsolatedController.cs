namespace Bars.Gkh.InspectorMobile.Api.Version1.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Description;

    using Bars.Gkh.BaseApiIntegration.Attributes;
    using Bars.Gkh.BaseApiIntegration.Controllers;
    using Bars.Gkh.BaseApiIntegration.Models;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.TaskActionIsolated;
    using Bars.Gkh.InspectorMobile.Api.Version1.Services;

    /// <summary>
    /// АPI-контроллер документа "Задание"
    /// </summary>
    [RoutePrefix("v1/docTaskActionIsolated")]
    public class DocumentTaskActionIsolatedController : BaseApiController
    {
        /// <summary>
        /// Получить список документов "Задание"
        /// </summary>
        /// <param name="queryParams">Параметры запроса</param>
        /// <returns>Список документов <see cref="DocumentTaskActionIsolated"/></returns>
        /// <response code="200">Запрос успешно обработан</response>
        /// <response code="400">Сервер не понимает запрос из-за неверного синтаксиса</response>
        /// <response code="404">Сервер не может найти запрашиваемый ресурс</response>
        /// <response code="500">Внутренняя ошибка сервера. Сервер столкнулся с ситуацией, которую он не знает как обработать.</response>
        [HttpGet]
        [Route("list")]
        [ValidateModel]
        [ResponseType(typeof(BaseApiResponse<DocumentTaskActionIsolated[]>))]
        public async Task<IHttpActionResult> GetList([FromUri] TaskActionIsolatedQueryParams queryParams) =>
            await this.ExecuteApiServiceMethodAsync<IDocumentTaskActionIsolatedService>(nameof(IDocumentTaskActionIsolatedService.GetListByQuery), queryParams);
        
        /// <summary>
        /// Получить документ "Задание"
        /// </summary>
        /// <param name="documentId">Идентификатор документа</param>
        /// <response code="200">Запрос успешно обработан</response>
        /// <response code="400">Сервер не понимает запрос из-за неверного синтаксиса</response>
        /// <response code="404">Сервер не может найти запрашиваемый ресурс</response>
        /// <response code="500">Внутренняя ошибка сервера. Сервер столкнулся с ситуацией, которую он не знает как обработать.</response>
        [HttpGet]
        [Route("{documentId:long}")]
        [ResponseType(typeof(BaseApiResponse<DocumentTaskActionIsolated>))]
        public async Task<IHttpActionResult> Get(long documentId) =>
            await this.ExecuteApiServiceMethodAsync<IDocumentTaskActionIsolatedService>(nameof(IDocumentTaskActionIsolatedService.Get), documentId);
    }
}