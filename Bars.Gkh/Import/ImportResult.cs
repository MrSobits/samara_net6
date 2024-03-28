namespace Bars.Gkh.Import
{
    using Bars.Gkh.Enums.Import;
    using Bars.B4;

    /// <summary>
    /// Класс для итогов импорта
    /// </summary>
    public class ImportResult : IDataResult
    {
        public ImportResult()
        {
            Success = true;
        }

        public ImportResult(StatusImport statusImport)
        {
            StatusImport = statusImport;
            switch (statusImport)
            {
                case StatusImport.CompletedWithError:
                    Success = false;
                    break;
                default:
                    Success = true;
                    break;
            }
        }

        public ImportResult(StatusImport statusImport, int logFileId)
            : this(statusImport)
        {
            LogFileId = logFileId;
        }

        public ImportResult(StatusImport statusImport, string message)
            : this(statusImport)
        {
            Message = message;
        }

        public ImportResult(StatusImport statusImport, string message, string title)
            : this(statusImport)
        {
            Message = message;
            Title = title;
        }

        public ImportResult(StatusImport statusImport, string message, string title, long logFileId)
            : this(statusImport)
        {
            LogFileId = logFileId;
            Message = message;
            Title = title;
        }

        public StatusImport StatusImport { get; set; }

        /// <summary>
        /// Идентификатор лога
        /// </summary>
        public long LogFileId { get; set; }

        public bool Success { get; set; }

        /// <summary>
        /// Сообщение
        /// </summary>
        public string Message { get; set; }

        public object Data { get; set; }


        /// <summary>
        /// Заголовок сообщения
        /// </summary>
        public string Title { get; set; }
    }
}
