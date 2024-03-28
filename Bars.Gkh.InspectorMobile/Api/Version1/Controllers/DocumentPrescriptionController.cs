namespace Bars.Gkh.InspectorMobile.Api.Version1.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Description;

    using Bars.Gkh.BaseApiIntegration.Attributes;
    using Bars.Gkh.BaseApiIntegration.Controllers;
    using Bars.Gkh.BaseApiIntegration.Models;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Prescription;
    using Bars.Gkh.InspectorMobile.Api.Version1.Services;

    /// <summary>
    /// API контроллер для <see cref="Bars.GkhGji.Entities.Prescription"/>
    /// </summary>
    [RoutePrefix("v1/docPrescription")]
    public class DocumentPrescriptionController : BaseApiController
    {
        /// <summary>
        /// Получить документ "Предписание"
        /// </summary>
        [HttpGet]
        [Route("{documentId:long}")]
        [ResponseType(typeof(BaseApiResponse<DocumentPrescriptionGet>))]
        public async Task<IHttpActionResult> Get(long documentId) =>
            await this.ExecuteApiServiceMethodAsync<IDocumentPrescriptionService>(nameof(IDocumentPrescriptionService.GetAsync), documentId);
        
        /// <summary>
        /// Получить список документов "Предписание"
        /// </summary>
        [HttpGet]
        [Route("list")]
        [ResponseType(typeof(BaseApiResponse<List<DocumentPrescriptionGet>>))]
        public async Task<IHttpActionResult> List([FromUri] long[] parentDocumentIds) =>
            await this.ExecuteApiServiceMethodAsync<IDocumentPrescriptionService>(nameof(IDocumentPrescriptionService.GetListAsync), parentDocumentIds);

        /// <summary>
        /// Создать документ "Предписание"
        /// </summary>
        [HttpPost]
        [Route]
        [ValidateModel]
        [ResponseType(typeof(BaseApiResponse<long>))]
        public async Task<IHttpActionResult> Create([FromBody] DocumentPrescriptionCreate prescription) =>
            await this.ExecuteApiServiceMethodAsync<IDocumentPrescriptionService>(nameof(IDocumentPrescriptionService.Create), prescription);

        /// <summary>
        /// Обновить документ "Предписание"
        /// </summary>
        [HttpPut]
        [ValidateModel]
        [Route("{documentId:long}")]
        [ResponseType(typeof(BaseApiResponse<long>))]
        public async Task<IHttpActionResult> Update(long documentId, [FromBody] DocumentPrescriptionUpdate prescriptionUpdate) =>
            await this.ExecuteApiServiceMethodAsync<IDocumentPrescriptionService>(nameof(IDocumentPrescriptionService.Update), documentId, prescriptionUpdate);
        
        /// <summary>
        /// Удаление документа "Предписание"
        /// </summary>
        /// <param name="documentId">Идентификатор документа</param>
        [HttpDelete]
        [Route("{documentId:long}")]
        [ResponseType(typeof(BaseApiResponse))]
        public async Task<IHttpActionResult> Delete(long documentId) =>
            await this.ExecuteApiServiceMethodAsync<IDocumentPrescriptionService>(nameof(IDocumentPrescriptionService.DeleteAsync), documentId);
    }
}