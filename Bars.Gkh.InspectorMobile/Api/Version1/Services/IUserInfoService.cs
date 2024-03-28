namespace Bars.Gkh.InspectorMobile.Api.Version1.Services
{
    using System;
    using System.Threading.Tasks;

    using Bars.Gkh.InspectorMobile.Api.Version1.Models.User;

    /// <summary>
    /// Сервис для работы с данными о пользователе
    /// </summary>
    public interface IUserInfoService
    {
        /// <summary>
        /// Получить данные о пользователе
        /// </summary>
        /// <param name="date">Дата изменения учетной записи пользователя</param>
        Task<UserInfoGet> GetAsync(DateTime? date);

        /// <summary>
        /// Получить данные о пользователе
        /// </summary>
        /// <param name="model">Модель для обновления данных</param>
        Task PutAsync(UserInfoUpdate model);
    }
}