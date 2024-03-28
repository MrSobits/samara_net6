namespace Bars.Gkh.InspectorMobile.Api.Version1.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Description;

    using Bars.Gkh.BaseApiIntegration.Controllers;
    using Bars.Gkh.BaseApiIntegration.Models;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.Dict;
    using Bars.Gkh.InspectorMobile.Api.Version1.Services;

    /// <summary>
    /// API контроллер для работы со справочниками
    /// </summary>
    [RoutePrefix("v1/dict")]
    public class DictController : BaseApiController
    {
        /// <summary>
        /// Получить список справочников с записями
        /// </summary>
        /// <param name="date">Дата последней актуализации справочников</param>
        [HttpGet]
        [Route("list")]
        [ResponseType(typeof(BaseApiResponse<Dict[]>))]
        public async Task<IHttpActionResult> GetList([FromUri] DateTime? date = null) =>
            await this.ExecuteApiServiceMethodAsync<IDictService>(nameof(IDictService.GetList), date);

        /// <summary>
        /// Получить список групп нарушений
        /// </summary>
        /// <param name="date">Дата последней актуализации справочников</param>
        [HttpGet]
        [Route("violations")]
        [ResponseType(typeof(BaseApiResponse<GroupViolations[]>))]
        public async Task<IHttpActionResult> GroupViolationsList([FromUri] DateTime? date = null) =>
            await this.ExecuteApiServiceMethodAsync<IDictService>(nameof(IDictService.GroupViolationsList), date);

        /// <summary>
        /// Получить справочник ФИАС
        /// </summary>
        /// <param name="municipalityId">Уникальный идентификатор муниципального образования</param>
        /// <param name="date">Дата последнего обновления</param>
        [HttpGet]
        [Route("fias")]
        [ResponseType(typeof(BaseApiResponse<FiasResponse>))]
        public async Task<IHttpActionResult> Fias(long municipalityId, DateTime? date = null) =>
            await this.ExecuteApiServiceMethodAsync<IDictService>(nameof(IDictService.Fias), municipalityId, date);
        
        /// <summary>
        /// Получить список статусов объектов
        /// </summary>
        [HttpGet]
        [Route("docStatus")]
        [ResponseType(typeof(BaseApiResponse<TransferDocStatus[]>))]
        public async Task<IHttpActionResult> DocStatusListAsync() =>
            await this.ExecuteApiServiceMethodAsync<IDictService>(nameof(IDictService.DocStatusListAsync));
    }
}