namespace Bars.Gkh.Gis.DomainService.BilConnection
{
    using Bars.B4;
    using Bars.Gkh.Gis.Enum;
    /// <summary>
    ///  Интерфейс сервиса получения строк соединения к серверам БД биллинга
    /// </summary>
    public interface IBilConnectionService
    {
        /// <summary>
        /// Получить строку соединения
        /// </summary>
        /// <param name="appUrl">Базовый URL</param>
        /// <param name="connectionType">Тип подключения</param>
        /// <returns></returns>
        string GetConnection(ConnectionType connectionType);
    }
}
