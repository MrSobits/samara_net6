namespace Bars.GisIntegration.Base.File
{
    using System;
    using System.Linq;
    using System.Net;

    /// <summary>
    /// Результат загрузки файла
    /// </summary>
    public class FileUploadResult
    {
        /// <summary>
        /// Конструктор результата загрузки файла
        /// </summary>
        /// <param name="fileGuid">Идентификатор загруженного файла</param>
        public FileUploadResult(string fileGuid)
        {
            this.Success = true;
            this.FileGuid = fileGuid;
        }

        /// <summary>
        /// Конструктор результата загрузки файла
        /// </summary>
        /// <param name="exception">Исключение при загрузке файла</param>
        public FileUploadResult(Exception exception)
        {
            this.Success = false;
            this.Message = this.GetErrorMessage(exception);
        }

        /// <summary>
        /// Результат загрузки: успешно - true, в противном случае false
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Идентификатор загруженного файла
        /// </summary>
        public string FileGuid { get; set; }

        /// <summary>
        /// Сообщение об ошибке
        /// </summary>
        public string Message { get; set; }

        private string GetErrorMessage(Exception exception)
        {
            var aggregateException = exception as AggregateException;
            if (aggregateException != null)
            {
                return string.Join("; ", aggregateException.InnerExceptions.Select(this.GetErrorMessage));
            }

            var result = string.Empty;

            var webException = exception as WebException;

            if (webException != null && webException.Response != null)
            {
                var errorValue = webException.Response.Headers["X-Upload-Error"];

                if (!string.IsNullOrEmpty(errorValue))
                {
                    result = this.GetErrorMessage(errorValue);
                }
            }

            if (string.IsNullOrEmpty(result))
            {
                result = exception.Message;
            }

            return result;
        }

        private string GetErrorMessage(string errorValue)
        {
            switch (errorValue)
            {
                case "FieldValidationException":
                    return "Не пройдены проверки на корректность заполнения полей";
                case "InvalidSizeException":
                    return "Некорректный размер файла";
                case "HashConflictException":
                    return "Не пройдены проверки на соответствие контрольной сумме";
                case "ContextNotFoundException":
                    return "Неправильное хранилище";
                case "DetectionException":
                    return "Не удалось определить тип загружаемого файла или тип файла является недопустимым";
                case "DataProviderValidationException":
                    return "Поставщик данных не найден, заблокирован или неактивен";
                case "CertificateValidationException":
                    return "Информационная система не найдена по отпечатку или заблокирована";
                case "FileNotFoundException":
                    return "Не пройдены проверки на существование идентификационного кода файла";
                case "InvalidStatusException":
                    return "Не пройдены проверки на корректный статус файла";
                case "InvalidPartNumberException":
                    return "Не пройдены проверки на то, что что количество загруженных частей совпадает с указанным изначально количеством";
                case "FilePermissionException":
                    return "Нарушение ограничений безопасности при работе с файловым хранилищем";
                default:
                    return string.Empty;
            }
        }
    }
}
