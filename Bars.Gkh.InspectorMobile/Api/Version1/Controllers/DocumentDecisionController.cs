namespace Bars.Gkh.InspectorMobile.Api.Version1.Controllers
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Description;

    using Bars.Gkh.BaseApiIntegration.Attributes;
    using Bars.Gkh.BaseApiIntegration.Controllers;
    using Bars.Gkh.BaseApiIntegration.Models;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Decision;
    using Bars.Gkh.InspectorMobile.Api.Version1.Services;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Decision;

    /// <summary>
    /// API контроллер для <see cref="Decision"/>
    /// </summary>
    [RoutePrefix("v1/docDecision")]
    public class DocumentDecisionController : BaseApiController
    {
        /// <summary>
        /// Получить документ "Решение"
        /// </summary>
        /// <param name="documentId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{documentId:long}")]
        [ResponseType(typeof(BaseApiResponse<DocumentDecision>))]
        public async Task<IHttpActionResult> Get([FromUri] long documentId) =>
            await this.ExecuteApiServiceMethodAsync<IDocumentDecisionService>(nameof(IDocumentDecisionService.Get), documentId);
            
        /// <summary>
        /// Получить список документов "Решение"
        /// </summary>
        /// <param name="queryParams">Фильтр для решения</param>
        [HttpGet]
        [Route("list")]
        [ValidateModel]
        [ResponseType(typeof(BaseApiResponse<List<DocumentDecision>>))]
        public async Task<IHttpActionResult> List([FromUri] DecisionQueryParams queryParams) =>
            await this.ExecuteApiServiceMethodAsync<IDocumentDecisionService>(nameof(IDocumentDecisionService.GetListByQuery), queryParams);
    }
}