namespace Bars.Gkh.BaseApiIntegration.Services
{
    using Bars.B4.Modules.States;

    /// <summary>
    /// Сервис для авторизации пользователя
    /// </summary>
    public interface IApiLoginService
    {
        /// <summary>
        /// Авторизовать пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <returns>Результат авторизации в формате <see cref="ValidateResult"/>. В случае неудачи вернет сообщение об ошибке</returns>
        ValidateResult AuthorizeUser(long userId);
    }
}