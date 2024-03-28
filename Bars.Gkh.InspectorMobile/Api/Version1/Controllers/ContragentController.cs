using Bars.Gkh.InspectorMobile.Api.Version1.Models.Contragent;
using Bars.Gkh.InspectorMobile.Api.Version1.Services;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bars.Gkh.InspectorMobile.Api.Version1.Controllers
{
    using Bars.Gkh.BaseApiIntegration.Controllers;
    using Bars.Gkh.BaseApiIntegration.Models;

    /// <summary>
    /// API-контроллер контрагента
    /// </summary>
    [RoutePrefix("v1/contragent")]
    public class ContragentController : BaseApiController
    {
        /// <summary>
        /// Получить контрагента
        /// </summary>
        /// <param name="contragentId">Идентификатор контрагента</param>
        /// <returns>Контрагент</returns>
        [HttpGet]
        [Route("{contragentId:long}")]
        [ResponseType(typeof(BaseApiResponse<FullContragentDto>))]
        public async Task<IHttpActionResult> Get(long contragentId) =>
            await this.ExecuteApiServiceMethodAsync<IContragentService>(nameof(IContragentService.GetAsync), contragentId);

        /// <summary>
        /// Получить список контрагентов
        /// </summary>
        /// <param name="fullList">Признак получения полного списка контрагентов</param>
        /// <param name="contragentIds">Идентификаторы контрагентов</param>
        /// <returns>Список контрагентов</returns>
        [HttpGet]
        [Route("list")]
        [ResponseType(typeof(BaseApiResponse<ShortContragentDto[]>))]
        [ResponseType(typeof(BaseApiResponse<FullContragentDto[]>))]
        public async Task<IHttpActionResult> GetList([FromUri] bool fullList, [FromUri] long[] contragentIds = null) =>
            await this.ExecuteApiServiceMethodAsync<IContragentService>(nameof(IContragentService.GetListAsync), fullList, contragentIds);
    }
}