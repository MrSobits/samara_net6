namespace Bars.Gkh.FormatDataExport.NetworkWorker.Responses
{

    internal class Error
    {
        /// <summary>
        /// Код ответа HTTP
        /// </summary>
        public string StatusCode { get; set; }

        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Детальная информация
        /// </summary>
        public string Details { get; set; }
    }
}