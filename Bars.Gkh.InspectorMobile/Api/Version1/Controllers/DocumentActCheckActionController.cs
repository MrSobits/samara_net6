namespace Bars.Gkh.InspectorMobile.Api.Version1.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Description;

    using Bars.Gkh.BaseApiIntegration.Attributes;
    using Bars.Gkh.BaseApiIntegration.Controllers;
    using Bars.Gkh.BaseApiIntegration.Models;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.ActCheckAction;
    using Bars.Gkh.InspectorMobile.Api.Version1.Services;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction;

    /// <summary>
    /// API-контроллер для <see cref="ActCheckAction"/>
    /// </summary>
    [RoutePrefix("v1/docActCheck/action")]
    public class DocumentActCheckActionController : BaseApiController
    {
        /// <summary>
        /// Получить действие акта проверки
        /// </summary>
        /// <param name="actionId">Идентификатор действия акта проверки</param>
        [HttpGet]
        [Route("{actionId:long}")]
        [ResponseType(typeof(BaseApiResponse<DocumentActCheckActionGet>))]
        public async Task<IHttpActionResult> Get(long actionId) =>
            await this.ExecuteApiServiceMethodAsync<IDocumentActCheckActionService>(nameof(IDocumentActCheckActionService.Get), actionId);

        /// <summary>
        /// Получить список действий актов проверки
        /// </summary>
        /// <param name="parentDocumentIds">Идентификаторы родительских документов</param>
        [HttpGet]
        [Route("list")]
        [ResponseType(typeof(BaseApiResponse<DocumentActCheckActionGet[]>))]
        public async Task<IHttpActionResult> GetList([FromUri] long[] parentDocumentIds) =>
            await this.ExecuteApiServiceMethodAsync<IDocumentActCheckActionService>(nameof(IDocumentActCheckActionService.GetList), parentDocumentIds);
        
        /// <summary>
        /// Создать действие акта проверки
        /// </summary>
        /// <param name="createAction">Создаваемое действие</param>
        /// <returns>Идентификатор созданного действия</returns>
        [HttpPost]
        [Route]
        [ValidateModel]
        [ResponseType(typeof(BaseApiResponse<long>))]
        public async Task<IHttpActionResult> Create([FromBody] DocumentActCheckActionCreate createAction) =>
            await this.ExecuteApiServiceMethodAsync<IDocumentActCheckActionService>(nameof(IDocumentActCheckActionService.Create), createAction);
        
        /// <summary>
        /// Редактирование действия акта проверки
        /// </summary>
        /// <param name="actionId">Идентификатор действия</param>
        /// <param name="updateAction">Обновляемое действие</param>
        [HttpPut]
        [Route("{actionId:long}")]
        [ValidateModel]
        [ResponseType(typeof(BaseApiResponse<long>))]
        public async Task<IHttpActionResult> Update(long actionId, [FromBody] DocumentActCheckActionUpdate updateAction) =>
            await this.ExecuteApiServiceMethodAsync<IDocumentActCheckActionService>(nameof(IDocumentActCheckActionService.Update), actionId, updateAction);

        /// <summary>
        /// Удалить действие акта проверки
        /// </summary>
        /// <param name="actionId">Идентификатор действия акта проверки</param>
        [HttpDelete]
        [Route("{actionId:long}")]
        [ResponseType(typeof(BaseApiResponse))]
        public async Task<IHttpActionResult> Delete(long actionId) =>
            await this.ExecuteApiServiceMethodAsync<IDocumentActCheckActionService>(nameof(IDocumentVisitSheetService.DeleteAsync), actionId);
    }
}