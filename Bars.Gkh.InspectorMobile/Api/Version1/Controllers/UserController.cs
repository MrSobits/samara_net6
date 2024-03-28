namespace Bars.Gkh.InspectorMobile.Api.Version1.Controllers
{
    using System;
    using System.Threading.Tasks;
    using System.Web.Http;
    using System.Web.Http.Description;

    using Bars.Gkh.BaseApiIntegration.Controllers;
    using Bars.Gkh.BaseApiIntegration.Models;
    using Bars.Gkh.InspectorMobile.Api.Version1.Models.User;
    using Bars.Gkh.InspectorMobile.Api.Version1.Services;

    /// <summary>
    /// Контроллер для работы с данными о пользователе
    /// </summary>
    [RoutePrefix("v1/user")]
    public class UserController: BaseApiController
    {
        /// <summary>
        /// Получить данные о пользователе
        /// </summary>
        /// <param name="date">Дата изменения учетной записи пользователя</param>
        [HttpGet]
        [Route]
        [ResponseType(typeof(BaseApiResponse<UserInfoGet>))]
        public async Task<IHttpActionResult> GetAsync(DateTime? date = null) => 
            await this.ExecuteApiServiceMethodAsync<IUserInfoService>(nameof(IUserInfoService.GetAsync), date);

        /// <summary>
        /// Обновить данные пользователя
        /// </summary>
        /// <param name="model">Модель для обновления данных</param>
        [HttpPut]
        [Route]
        [ResponseType(typeof(BaseApiResponse))]
        public async Task<IHttpActionResult> PutAsync([FromBody]UserInfoUpdate model) => 
            await this.ExecuteApiServiceMethodAsync<IUserInfoService>(nameof(IUserInfoService.PutAsync), model);
    }
}