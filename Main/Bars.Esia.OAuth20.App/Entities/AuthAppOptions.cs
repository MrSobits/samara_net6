namespace Bars.Esia.OAuth20.App.Entities
{
    using System.Net;

    /// <summary>
    /// Общие параметры приложения
    /// </summary>
    public class AuthAppOptions
    {
        /// <summary>
        /// IP-Адрес локальной точки прослушивания
        /// </summary>
        public IPAddress SocketListeningAddress { get; set; }

        /// <summary>
        /// Попрт прослушивания
        /// </summary>
        public int? SocketListeningPort { get; set; }

        /// <summary>
        /// Размер буффера для чтения
        /// </summary>
        public int? SocketReceiveBufferLength { get; set; }

        /// <summary>
        /// Макс. число подключений
        /// </summary>
        public int? SocketConnectionsLength { get; set; }
    }
}