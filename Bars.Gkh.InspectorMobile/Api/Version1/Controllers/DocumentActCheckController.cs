namespace Bars.Gkh.InspectorMobile.Api.Version1.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Description;

    using Bars.Gkh.BaseApiIntegration.Attributes;
    using Bars.Gkh.BaseApiIntegration.Controllers;
    using Bars.Gkh.BaseApiIntegration.Models;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.ActCheck;
    using Bars.Gkh.InspectorMobile.Api.Version1.Services;

    /// <summary>
    /// API-контроллер документа "Акт проверки"
    /// </summary>
    [RoutePrefix("v1/docActCheck")]
    public class DocumentActCheckController : BaseApiController
    {
        /// <summary>
        /// Получить документ "Акт проверки"
        /// </summary>
        /// <param name="documentId">Идентификатор документа</param>
        [HttpGet]
        [Route("{documentId:long}")]
        [ResponseType(typeof(BaseApiResponse<DocumentActCheckGet>))]
        public async Task<IHttpActionResult> Get(long documentId) =>
            await this.ExecuteApiServiceMethodAsync<IDocumentActCheckService>(nameof(IDocumentActCheckService.Get), documentId);

        /// <summary>
        /// Получить список документов "Акт проверки"
        /// </summary>
        /// <param name="parentDocumentIds">Идентификаторы родительских документов</param>
        [HttpGet]
        [Route("list")]
        [ResponseType(typeof(BaseApiResponse<DocumentActCheckGet[]>))]
        public async Task<IHttpActionResult> GetList([FromUri] long[] parentDocumentIds) =>
            await this.ExecuteApiServiceMethodAsync<IDocumentActCheckService>(nameof(IDocumentActCheckService.GetList), parentDocumentIds);

        /// <summary>
        /// Создать документ "Акт проверки"
        /// </summary>
        /// <param name="createDocument">Создаваемый документ</param>
        /// <returns>Идентификатор созданного документа</returns>
        [HttpPost]
        [Route]
        [ValidateModel]
        [ResponseType(typeof(BaseApiResponse<long>))]
        public async Task<IHttpActionResult> Create([FromBody] DocumentActCheckCreate createDocument) =>
            await this.ExecuteApiServiceMethodAsync<IDocumentActCheckService>(nameof(IDocumentActCheckService.Create), createDocument);

        /// <summary>
        /// Редактирование документа "Акт проверки"
        /// </summary>
        /// <param name="documentId">Идентификатор документа</param>
        /// <param name="updateDocument">Обновляемый документ</param>
        [HttpPut]
        [Route("{documentId:long}")]
        [ValidateModel]
        [ResponseType(typeof(BaseApiResponse<long>))]
        public async Task<IHttpActionResult> Update(long documentId, [FromBody] DocumentActCheckUpdate updateDocument) =>
            await this.ExecuteApiServiceMethodAsync<IDocumentActCheckService>(nameof(IDocumentActCheckService.Update), documentId, updateDocument);

        /// <summary>
        /// Удаление документа "Акт проверки"
        /// </summary>
        /// <param name="documentId">Идентификатор документа</param>
        [HttpDelete]
        [Route("{documentId:long}")]
        public async Task<IHttpActionResult> Delete(long documentId) =>
            await this.ExecuteApiServiceMethodAsync<IDocumentActCheckService>(nameof(IDocumentActCheckService.DeleteAsync), documentId);
    }
}