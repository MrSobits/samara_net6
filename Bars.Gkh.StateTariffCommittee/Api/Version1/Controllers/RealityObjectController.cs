namespace Bars.Gkh.StateTariffCommittee.Api.Version1.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Description;

    using Bars.Gkh.BaseApiIntegration.Controllers;
    using Bars.Gkh.BaseApiIntegration.Models;
    using Bars.Gkh.StateTariffCommittee.Api.Version1.Models.RealityObject;
    using Bars.Gkh.StateTariffCommittee.Api.Version1.Services;

    /// <summary>
    /// API контроллер для <see cref="Entities.RealityObject"/>
    /// </summary>
    [RoutePrefix("v1/realityObject")]
    public class RealityObjectController : BaseApiController
    {
        /// <summary>
        /// Получить список объектов жилищного фонда
        /// </summary>
        /// <param name="oktmo">ОКТМО</param>
        /// <param name="period">Отчетный период</param>
        [HttpGet]
        [Route("list")]
        [ResponseType(typeof(BaseApiResponse<RealityObject[]>))]
        public async Task<IHttpActionResult> List([FromUri] long? oktmo, [FromUri] DateTime? period = null) =>
            await this.ExecuteApiServiceMethodAsync<IRealityObjectService>(nameof(IRealityObjectService.List), oktmo, period);
        
        /// <summary>
        /// Получить список объектов жилищного фонда в разрезе помещений потребителей КУ
        /// </summary>
        /// <param name="oktmo">ОКТМО</param>
        /// <param name="period">Отчетный период</param>
        [HttpGet]
        [Route("premises/list")]
        [ResponseType(typeof(BaseApiResponse<RealityObjectInConsumerContext[]>))]
        public async Task<IHttpActionResult> PremisesList([FromUri] long? oktmo, [FromUri] DateTime? period) =>
            await this.ExecuteApiServiceMethodAsync<IRealityObjectService>(nameof(IRealityObjectService.ListInConsumerContext), oktmo, period);

        /// <summary>
        /// Получить список объектов жилищного фонда в разрезе тарифов помещений потребителей КУ
        /// </summary>
        /// <param name="oktmo">ОКТМО</param>
        /// <param name="period">Отчетный период</param>
        /// <param name="pageGuid">Гуид запрашиваемой страницы</param>
        [HttpGet]
        [Route("premises/tariffs/list")]
        [ResponseType(typeof(PagedApiResponse<RealityObjectInConsumerTariffContext[]>))]
        public async Task<IHttpActionResult> PremisesTariffsList([FromUri] long? oktmo, [FromUri] DateTime? period = null, [FromUri] Guid? pageGuid = null) =>
            await this.ExecuteApiServiceMethodAsync<IRealityObjectService>(nameof(IRealityObjectService.ListInConsumerTariffContext), oktmo, period, pageGuid);
    }
}