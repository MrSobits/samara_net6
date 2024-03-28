namespace Bars.Gkh.Utils
{
    /// <summary>
    /// Иерархичный аггрегатор сообщений об ошибках
    /// </summary>
    public class ErrorAggregator
    {
        public ErrorAggregator(string message, ErrorAggregator innerException = null)
        {
            this.Message = message;
            this.InnerException = innerException;
        }
        
        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Внутренняя ошибка
        /// </summary>
        public ErrorAggregator InnerException { get; set; }
    }
}