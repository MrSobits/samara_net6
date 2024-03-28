namespace Bars.Gkh.InspectorMobile.Api.Version1.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Description;

    using Bars.Gkh.BaseApiIntegration.Controllers;
    using Bars.Gkh.BaseApiIntegration.Models;
    using Bars.Gkh.Entities;
    using Bars.Gkh.InspectorMobile.Api.Version1.Services;

    /// <summary>
    /// API контроллер для <see cref="RealityObject"/>
    /// </summary>
    [RoutePrefix("v1/realityObject")]
    public class RealityObjectController : BaseApiController
    {
        /// <summary>
        /// Получить объект жилищного фонда
        /// </summary>
        /// <param name="houseId">Идентификатор дома</param>
        [HttpGet]
        [Route("{houseId:long}")]
        [ResponseType(typeof(BaseApiResponse<Models.RealityObject.RealityObject>))]
        public async Task<IHttpActionResult> Get(long houseId) => 
            await this.ExecuteApiServiceMethodAsync<IRealityObjectService>(nameof(IRealityObjectService.GetAsync), houseId);
        
        /// <summary>
        /// Получить список объектов жилищного фонда
        /// </summary>
        /// <param name="fullList">Признак получения полного списка домов</param>
        /// <param name="houseIds">Массив идентификаторов домов</param>
        [HttpGet]
        [Route("list")]
        [ResponseType(typeof(BaseApiResponse<Models.RealityObject.RealityObject[]>))]
        public async Task<IHttpActionResult> List([FromUri] bool fullList, [FromUri] long[] houseIds = null) =>
            await this.ExecuteApiServiceMethodAsync<IRealityObjectService>(nameof(IRealityObjectService.ListAsync), fullList, houseIds);
    }
}