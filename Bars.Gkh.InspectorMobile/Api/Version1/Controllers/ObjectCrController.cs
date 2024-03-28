namespace Bars.Gkh.InspectorMobile.Api.Version1.Controllers
{
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Description;

    using Bars.Gkh.BaseApiIntegration.Attributes;
    using Bars.Gkh.BaseApiIntegration.Controllers;
    using Bars.Gkh.BaseApiIntegration.Models;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.ObjectCr;
    using Bars.Gkh.InspectorMobile.Api.Version1.Services;

    /// <summary>
    /// Контроллер для работы с данными об объекте капитального ремонта
    /// </summary>
    [RoutePrefix("v1/objectCr")]
    public class ObjectCrController: BaseApiController
    {
        /// <summary>
        /// Получить данные об объекте капитального ремонта по идентификатору объекта
        /// </summary>
        /// <param name="objectId">Идентификатор объекта капитального ремонта</param>
        [HttpGet]
        [Route("{objectId:long}")]
        [ResponseType(typeof(BaseApiResponse<ObjectCrGet>))]
        public async Task<IHttpActionResult> Get(long objectId) =>
            await this.ExecuteApiServiceMethodAsync<IObjectCrService>(nameof(IObjectCrService.GetByObjectCr), objectId);

        /// <summary>
        /// Получить данные об объекте капитального ремонта по идентификаторам программы КР и дома
        /// </summary>
        /// <param name="programId">Идентификатор программы капитального ремонта</param>
        /// <param name="addressId">Идентификатор дома</param>
        [HttpGet]
        [Route]
        [ResponseType(typeof(BaseApiResponse<ObjectCrGet>))]
        public async Task<IHttpActionResult> Get(long programId, long addressId) => 
            await this.ExecuteApiServiceMethodAsync<IObjectCrService>(nameof(IObjectCrService.GetByProgramAndAddress), programId, addressId);

        /// <summary>
        /// Редактирование объекта капитального ремонта
        /// </summary>
        /// <param name="objectId">Идентификатор объекта</param>
        /// <param name="updateObject">Обновляемый объект</param>
        [HttpPut]
        [Route("{objectId:long}")]
        [ValidateModel]
        [ResponseType(typeof(BaseApiResponse<long>))]
        public async Task<IHttpActionResult> Update(long objectId, [FromBody] ObjectCrUpdate updateObject) =>
            await this.ExecuteApiServiceMethodAsync<IObjectCrService>(nameof(IObjectCrService.Update), objectId, updateObject);
    }
}