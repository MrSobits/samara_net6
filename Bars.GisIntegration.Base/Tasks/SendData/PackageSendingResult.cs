namespace Bars.GisIntegration.Base.Tasks.SendData
{
    /// <summary>
    /// Результат отправки пакета
    /// </summary>
    public class PackageSendingResult
    {
        /// <summary>
        /// Результат отправки: успешно - true, в противном случае false
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Идентификатор сообщения для получения результата обработки пакета
        /// </summary>
        public string AckMessageGuid { get; set; }

        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        public string Message { get; set; }
    }
}
