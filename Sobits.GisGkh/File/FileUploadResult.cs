using System;
using System.Collections.Generic;

namespace Sobits.GisGkh.File
{
    /// <summary>
    /// Результат загрузки файла
    /// </summary>
    public class FileUploadResult : FileLoadResult
    {
        /// <summary>
        /// Идентификатор загруженного файла
        /// </summary>
        public string FileGuid { get; private set; }

        /// <summary>
        /// Конструктор результата загрузки файла
        /// </summary>
        /// <param name="fileGuid">Идентификатор загруженного файла</param>
        public FileUploadResult(string fileGuid)
        {
            FileGuid = fileGuid;
        }

        /// <summary>
        /// Конструктор результата загрузки файла
        /// </summary>
        /// <param name="exception">Исключение при загрузке файла</param>
        public FileUploadResult(Exception exception)
            : base(exception)
        {

        }

        /// <summary>
        /// Маппинг исключений ГИС
        /// </summary>
        protected override IDictionary<string, string> ExceptionMap { get; } = new Dictionary<string, string>
        {
            {"FieldValidationException", "Не пройдены проверки на корректность заполнения полей"},
            {"InvalidSizeException", "Некорректный размер файла"},
            {"HashConflictException", "Не пройдены проверки на соответствие контрольной сумме"},
            {"ContextNotFoundException", "Неправильное хранилище"},
            {"DetectionException", "Поставщик данных не найден, заблокирован или неактивен"},
            {"DataProviderValidationException", "Информационная система не найдена по отпечатку или заблокирована"},
            {"FileNotFoundException", "Не пройдены проверки на существование идентификационного кода файла"},
            {"InvalidStatusException", "Не пройдены проверки на корректный статус файла"},
            {"InvalidPartNumberException", "Не пройдены проверки на то, что что количество загруженных частей совпадает с указанным изначально количеством"},
            {"FilePermissionException", "Нарушение ограничений безопасности при работе с файловым хранилищем"},
            {"InvalidFilenameSymbolsException", "Имя файла содержит недопустимые символы"}
        };
    }
}