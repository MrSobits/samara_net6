namespace Bars.Esia.OAuth20.App.Enums
{
    using System;

    /// <summary>
    /// Флаги при отправке HTTP-запроса
    /// </summary>
    [Flags]
    public enum SendType
    {
        /// <summary>
        /// Нормальная отправка
        /// </summary>
        /// <remarks>
        /// Включает в себя:
        /// 1. проверку валидности токена
        /// 2. обновление токена
        /// 3. проверку срока действия токена
        /// </remarks>
        Normal = SendType.VerifyToken | SendType.RefreshToken | SendType.CheckTokenTime,

        /// <summary>
        /// Проверка срока действия токена
        /// </summary>
        CheckTokenTime = 1,

        /// <summary>
        /// Обновление токена
        /// </summary>
        RefreshToken = 2,

        /// <summary>
        /// Проверка валидности токена
        /// </summary>
        VerifyToken = 4
    }
}