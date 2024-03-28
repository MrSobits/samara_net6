namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.User
{
    /// <summary>
    /// Информация о пользователе. Модель получения данных
    /// </summary>
    public class UserInfoGet : BaseUserInfo
    {
        /// <summary>
        /// Должность
        /// </summary>
        public string Position { get; set; }
    }

    /// <summary>
    /// Информация о пользователе. Модель обновления данных
    /// </summary>
    public class UserInfoUpdate : BaseUserInfo
    {
    }

    /// <summary>
    /// Информация о пользователе
    /// </summary>
    public abstract class BaseUserInfo
    {
        /// <summary>
        /// ФИО пользователя
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Почта
        /// </summary>
        public string Mail { get; set; }

        /// <summary>
        /// Телефон
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// Уникальный идентификатор файла
        /// </summary>
        public long? Photo { get; set; }
    }
}