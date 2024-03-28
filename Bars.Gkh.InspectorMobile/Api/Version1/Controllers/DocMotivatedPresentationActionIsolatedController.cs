namespace Bars.Gkh.InspectorMobile.Api.Version1.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Description;

    using Bars.Gkh.BaseApiIntegration.Attributes;
    using Bars.Gkh.BaseApiIntegration.Controllers;
    using Bars.Gkh.BaseApiIntegration.Models;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.MotivationPresentation;
    using Bars.Gkh.InspectorMobile.Api.Version1.Services;

    /// <summary>
    /// Контроллер для МП документа "Мотивированное представление" 
    /// </summary>
    [RoutePrefix("v1/docMotivatedPresentationActionIsolated")]
    public class DocMotivatedPresentationActionIsolatedController : BaseApiController
    {
        /// <summary>
        /// Получить документ "Мотивированное представление"
        /// </summary>
        /// <param name="documentId">Идентификатор документа</param>
        [HttpGet]
        [Route("{documentId:long}")]
        [ResponseType(typeof(BaseApiResponse<MotivationPresentationGet>))]
        public async Task<IHttpActionResult> Get(long documentId) =>
            await this.ExecuteApiServiceMethodAsync<IDocMotivatedPresentationService>(nameof(IDocMotivatedPresentationService.Get),
                documentId);

        /// <summary>
        /// Получить список документов "Мотивированное представление"
        /// </summary>
        /// <param name="parentDocumentIds">Идентификаторы родительских документов</param>
        [HttpGet]
        [Route("list")]
        [ResponseType(typeof(BaseApiResponse<MotivationPresentationGet[]>))]
        public async Task<IHttpActionResult> GetList([FromUri] long[] parentDocumentIds) =>
            await this.ExecuteApiServiceMethodAsync<IDocMotivatedPresentationService>(nameof(IDocMotivatedPresentationService.GetList),
                parentDocumentIds);

        /// <summary>
        /// Создать новый документ "Мотивированное представление"
        /// </summary>
        /// <param name="createDocument">Модель данных, по которым будет создан документ</param>
        /// <returns>Идентификатор созданного документа</returns>
        [HttpPost]
        [Route]
        [ValidateModel]
        [ResponseType(typeof(BaseApiResponse<long>))]
        public async Task<IHttpActionResult> Create([FromBody] MotivationPresentationCreate createDocument) =>
            await this.ExecuteApiServiceMethodAsync<IDocMotivatedPresentationService>(nameof(IDocMotivatedPresentationService.Create),
                createDocument);
        
        /// <summary>
        /// Редактирование документа "Мотивированное представление"
        /// </summary>
        /// <param name="documentId">Идентификатор документа</param>
        /// <param name="updateDocument">Обновляемый документ</param>
        [HttpPut]
        [Route("{documentId:long}")]
        [ValidateModel]
        [ResponseType(typeof(BaseApiResponse<long>))]
        public async Task<IHttpActionResult> Update(long documentId, [FromBody] MotivationPresentationUpdate updateDocument) =>
            await this.ExecuteApiServiceMethodAsync<IDocMotivatedPresentationService>(nameof(IDocMotivatedPresentationService.Update), documentId, updateDocument);

        /// <summary>
        /// Удаление документа "Мотивированное представление"
        /// </summary>
        /// <param name="documentId">Идентификатор документа</param>
        [HttpDelete]
        [Route("{documentId:long}")]
        [ResponseType(typeof(BaseApiResponse))]
        public async Task<IHttpActionResult> Delete(long documentId) =>
            await this.ExecuteApiServiceMethodAsync<IDocMotivatedPresentationService>(nameof(IDocMotivatedPresentationService.DeleteAsync), documentId);
    }
}