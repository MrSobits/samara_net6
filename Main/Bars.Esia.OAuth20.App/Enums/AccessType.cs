namespace Bars.Esia.OAuth20.App.Enums
{
    /// <summary>
    /// Тип доступа к данным ЕСИА
    /// </summary>
    public enum AccessType
    {
        /// <summary>
        /// Без активной сессии пользователя
        /// </summary>
        Offline = 1,

        /// <summary>
        /// Сессия пользователя обязательна
        /// </summary>
        Online = 2
    }
}