using Bars.Gkh.InspectorMobile.Api.Version1.Models.ActActionIsolated;
using Bars.Gkh.InspectorMobile.Api.Version1.Services;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bars.Gkh.InspectorMobile.Api.Version1.Controllers
{
    using System.Threading.Tasks;

    using Bars.Gkh.BaseApiIntegration.Attributes;
    using Bars.Gkh.BaseApiIntegration.Controllers;
    using Bars.Gkh.BaseApiIntegration.Models;

    /// <summary>
    /// API-контроллер документа "Акт КНМ без взаимодействия с контролируемыми лицами"
    /// </summary>
    [RoutePrefix("v1/docActActionIsolated")]
    public class DocumentActActionIsolatedController : BaseApiController
    {
        /// <summary>
        /// Получить документ "Акт КНМ без взаимодействия с контролируемыми лицами"
        /// </summary>
        /// <param name="documentId">Идентификатор документа</param>
        [HttpGet]
        [Route("{documentId:long}")]
        [ResponseType(typeof(BaseApiResponse<DocumentActActionIsolatedGet>))]
        public async Task<IHttpActionResult> Get(long documentId) =>
            await this.ExecuteApiServiceMethodAsync<IDocumentActActionIsolatedService>(nameof(IDocumentActActionIsolatedService.Get), documentId);

        /// <summary>
        /// Получить список документов "Акт КНМ без взаимодействия с контролируемыми лицами"
        /// </summary>
        /// <param name="parentDocumentIds">Идентификаторы родительских документов</param>
        [HttpGet]
        [Route("list")]
        [ResponseType(typeof(BaseApiResponse<DocumentActActionIsolatedGet[]>))]
        public async Task<IHttpActionResult> GetList([FromUri] long[] parentDocumentIds) =>
            await this.ExecuteApiServiceMethodAsync<IDocumentActActionIsolatedService>(nameof(IDocumentActActionIsolatedService.GetList), parentDocumentIds);
        
        /// <summary>
        /// Создать документ "Акт КНМ без взаимодействия с контролируемыми лицами"
        /// </summary>
        /// <param name="createDocument">Создаваемый документ</param>
        /// <returns>Идентификатор созданного документа</returns>
        [HttpPost]
        [Route]
        [ValidateModel]
        [ResponseType(typeof(BaseApiResponse<long>))]
        public async Task<IHttpActionResult> Create([FromBody] DocumentActActionIsolatedCreate createDocument) =>
            await this.ExecuteApiServiceMethodAsync<IDocumentActActionIsolatedService>(nameof(IDocumentActActionIsolatedService.Create), createDocument);
        
        /// <summary>
        /// Редактирование документа "Акт КНМ без взаимодействия с контролируемыми лицами"
        /// </summary>
        /// <param name="documentId">Идентификатор документа</param>
        /// <param name="updateDocument">Обновляемый документ</param>
        [HttpPut]
        [Route("{documentId:long}")]
        [ValidateModel]
        [ResponseType(typeof(BaseApiResponse<long>))]
        public async Task<IHttpActionResult> Update(long documentId, [FromBody] DocumentActActionIsolatedUpdate updateDocument) =>
            await this.ExecuteApiServiceMethodAsync<IDocumentActActionIsolatedService>(nameof(IDocumentActActionIsolatedService.Update), documentId, updateDocument);

        /// <summary>
        /// Удалить документ "Акт КНМ без взаимодействия с контролируемыми лицами"
        /// </summary>
        /// <param name="documentId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{documentId:long}")]
        public async Task<IHttpActionResult> Delete(long documentId) =>
            await this.ExecuteApiServiceMethodAsync<IDocumentActActionIsolatedService>(nameof(IDocumentActActionIsolatedService.DeleteAsync), documentId);
    }
}
