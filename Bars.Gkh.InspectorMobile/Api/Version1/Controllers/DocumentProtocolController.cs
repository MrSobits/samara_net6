namespace Bars.Gkh.InspectorMobile.Api.Version1.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Description;

    using Bars.Gkh.BaseApiIntegration.Attributes;
    using Bars.Gkh.BaseApiIntegration.Controllers;
    using Bars.Gkh.BaseApiIntegration.Models;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Protocol;
    using Bars.Gkh.InspectorMobile.Api.Version1.Services;

    /// <summary>
    /// API-контроллер документа "Протокол"
    /// </summary>
    [RoutePrefix("v1/docProtocol")]
    public class DocumentProtocolController : BaseApiController
    {
        /// <summary>
        /// Получить документ "Протокол"
        /// </summary>
        /// <param name="documentId">Идентификатор документа</param>
        [HttpGet]
        [Route("{documentId:long}")]
        [ResponseType(typeof(BaseApiResponse<DocumentProtocolGet>))]
        public async Task<IHttpActionResult> Get(long documentId) =>
            await this.ExecuteApiServiceMethodAsync<IDocumentProtocolService>(nameof(IDocumentProtocolService.Get), documentId);

        /// <summary>
        /// Получить список документов "Протокол"
        /// </summary>
        /// <param name="parentDocumentIds">Идентификаторы родительских документов</param>
        [HttpGet]
        [Route("list")]
        [ResponseType(typeof(BaseApiResponse<DocumentProtocolGet[]>))]
        public async Task<IHttpActionResult> GetList([FromUri] long[] parentDocumentIds) =>
            await this.ExecuteApiServiceMethodAsync<IDocumentProtocolService>(nameof(IDocumentProtocolService.GetList), parentDocumentIds);

        /// <summary>
        /// Создать документ "Протокол"
        /// </summary>
        /// <param name="createDocument">Создаваемый документ</param>
        /// <returns>Идентификатор созданного документа</returns>
        [HttpPost]
        [Route]
        [ValidateModel]
        [ResponseType(typeof(BaseApiResponse<long>))]
        public async Task<IHttpActionResult> Create([FromBody] DocumentProtocolCreate createDocument) =>
            await this.ExecuteApiServiceMethodAsync<IDocumentProtocolService>(nameof(IDocumentProtocolService.Create), createDocument);

        /// <summary>
        /// Редактирование документа "Протокол"
        /// </summary>
        /// <param name="documentId">Идентификатор документа</param>
        /// <param name="updateDocument">Обновляемый документ</param>
        [HttpPut]
        [Route("{documentId:long}")]
        [ValidateModel]
        [ResponseType(typeof(BaseApiResponse<long>))]
        public async Task<IHttpActionResult> Update(long documentId, [FromBody] DocumentProtocolUpdate updateDocument) =>
            await this.ExecuteApiServiceMethodAsync<IDocumentProtocolService>(nameof(IDocumentProtocolService.Update), documentId, updateDocument);
        
        /// <summary>
        /// Удаление документа "Протокол"
        /// </summary>
        /// <param name="documentId">Идентификатор документа</param>
        [HttpDelete]
        [Route("{documentId:long}")]
        [ResponseType(typeof(BaseApiResponse))]
        public async Task<IHttpActionResult> Delete(long documentId) =>
            await this.ExecuteApiServiceMethodAsync<IDocumentProtocolService>(nameof(IDocumentProtocolService.DeleteAsync), documentId);
    }
}