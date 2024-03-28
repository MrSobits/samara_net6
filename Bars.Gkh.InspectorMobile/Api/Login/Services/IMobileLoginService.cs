namespace Bars.Gkh.InspectorMobile.Api.Login.Services
{
    using System.Threading.Tasks;

    using Bars.B4.Modules.ESIA.Auth.Dto;
    using Bars.B4.Modules.States;
    using Bars.Gkh.BaseApiIntegration.Services;

    /// <summary>
    /// Сервис для авторизации пользователя мобильного приложения
    /// </summary>
    public interface IMobileLoginService : IApiLoginService
    {
        /// <summary>
        /// Авторизовать пользователя ЕСИА
        /// </summary>
        /// <param name="userInfo">Информация о пользователе из ЕСИА</param>
        /// <returns>Результат авторизации в формате <see cref="ValidateResult"/>. В случае неудачи вернет сообщение об ошибке</returns>
        Task<ValidateResult> AuthorizeEsiaUser(UserInfoDto userInfo);
    }
}