namespace Bars.Gkh.InspectorMobile.Api.Version1.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Description;

    using Bars.Gkh.BaseApiIntegration.Attributes;
    using Bars.Gkh.BaseApiIntegration.Controllers;
    using Bars.Gkh.BaseApiIntegration.Models;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.VisitSheet;
    using Bars.Gkh.InspectorMobile.Api.Version1.Services;
    using Bars.GkhGji.Regions.Tatarstan.Entities.PreventiveAction;

    /// <summary>
    /// API контроллер для <see cref="VisitSheet"/>
    /// </summary>
    [RoutePrefix("v1/docVisitSheet")]
    public class DocumentVisitSheetController : BaseApiController
    {
        /// <summary>
        /// Получить документ "Лист визита"
        /// </summary>
        [HttpGet]
        [Route("{documentId:long}")]
        [ResponseType(typeof(BaseApiResponse<DocumentVisitSheetGet>))]
        public async Task<IHttpActionResult> Get(long documentId) =>
            await this.ExecuteApiServiceMethodAsync<IDocumentVisitSheetService>(nameof(IDocumentVisitSheetService.Get), documentId);

        /// <summary>
        /// Получить список документов "Лист визита"
        /// </summary>
        [HttpGet]
        [Route("list")]
        [ResponseType(typeof(BaseApiResponse<List<DocumentVisitSheetGet>>))]
        public async Task<IHttpActionResult> List([FromUri] long[] parentDocumentIds) =>
            await this.ExecuteApiServiceMethodAsync<IDocumentVisitSheetService>(nameof(IDocumentVisitSheetService.GetList), parentDocumentIds);

        /// <summary>
        /// Создать документ "Лист визита"
        /// </summary>
        /// <param name="createDocument">Создаваемый документ</param>
        /// <returns>Идентификатор созданного документа</returns>
        [HttpPost]
        [Route]
        [ValidateModel]
        [ResponseType(typeof(BaseApiResponse<long>))]
        public async Task<IHttpActionResult> Create([FromBody] DocumentVisitSheetCreate createDocument) =>
            await this.ExecuteApiServiceMethodAsync<IDocumentVisitSheetService>(nameof(IDocumentVisitSheetService.Create), createDocument);

        /// <summary>
        /// Редактирование документа "Лист визита"
        /// </summary>
        /// <param name="documentId">Идентификатор документа</param>
        /// <param name="updateDocument">Данные для обновления</param>
        /// <returns>Идентификатор обновленного документа</returns>
        [HttpPut]
        [Route("{documentId:long}")]
        [ValidateModel]
        [ResponseType(typeof(BaseApiResponse<long>))]
        public async Task<IHttpActionResult> Update(long documentId, [FromBody] DocumentVisitSheetUpdate updateDocument) =>
            await this.ExecuteApiServiceMethodAsync<IDocumentVisitSheetService>(nameof(IDocumentVisitSheetService.Update), documentId, updateDocument);

        /// <summary>
        /// Удаление документа "Лист визита"
        /// </summary>
        /// <param name="documentId">Идентификатор документа</param>
        /// <returns>Результат выполнения</returns>
        [HttpDelete]
        [Route("{documentId:long}")]
        [ResponseType(typeof(BaseApiResponse))]
        public async Task<IHttpActionResult> Delete(long documentId) =>
            await this.ExecuteApiServiceMethodAsync<IDocumentVisitSheetService>(nameof(IDocumentVisitSheetService.DeleteAsync), documentId);
    }
}