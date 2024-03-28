namespace Bars.Gkh.InspectorMobile.Api.Version1.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Description;

    using Bars.Gkh.BaseApiIntegration.Attributes;
    using Bars.Gkh.BaseApiIntegration.Controllers;
    using Bars.Gkh.BaseApiIntegration.Models;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.HeatingSeason;
    using Bars.Gkh.InspectorMobile.Api.Version1.Services;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// API контроллер для работы с <see cref="HeatSeason"/>
    /// </summary>
    [RoutePrefix("v1/heatingSeason")]
    public class HeatingSeasonController : BaseApiController
    {
        /// <summary>
        /// Получить объект отопительного сезона 
        /// </summary>
        [HttpGet]
        [Route]
        [ResponseType(typeof(BaseApiResponse<HeatingSeasonObjectGet>))]
        public async Task<IHttpActionResult> GetList(long heatingSeasonPeriodId, long addressId) =>
            await this.ExecuteApiServiceMethodAsync<IHeatingSeasonService>(nameof(IHeatingSeasonService.Get), heatingSeasonPeriodId, addressId);

        /// <summary>
        /// Редактирование объекта отопительного сезона
        /// </summary>
        /// <param name="objectId">Идентификатор объекта</param>
        /// <param name="updateObject">Обновляемый объект</param>
        [HttpPut]
        [Route("{objectId:long}")]
        [ValidateModel]
        [ResponseType(typeof(BaseApiResponse<long>))]
        public async Task<IHttpActionResult> Update(long objectId, [FromBody] HeatingSeasonObjectUpdate updateObject) =>
            await this.ExecuteApiServiceMethodAsync<IHeatingSeasonService>(nameof(IHeatingSeasonService.Update), objectId, updateObject);
    }
}