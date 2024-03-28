using Bars.B4.Modules.FileStorage;
using System;
using System.Collections.Generic;

namespace Sobits.GisGkh.File
{
    /// <summary>
    /// Результат скачивания файла
    /// </summary>
    public class FileDownloadResult : FileLoadResult
    {
        /// <summary>
        /// Файл
        /// </summary>
        public FileInfo File { get; private set; }

        /// <summary>
        /// Конструктор результата скачивания файла
        /// </summary>
        /// <param name="file">Файл</param>
        public FileDownloadResult(FileInfo file)
        {
            File = file;
        }

        /// <summary>
        /// Конструктор результата скачивания файла
        /// </summary>
        /// <param name="exception">Исключение при скачивании файла</param>
        public FileDownloadResult(Exception exception)
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
            {"CertificateValidationException", "Информационная система не найдена по отпечатку или заблокирована"},
            {"FileVirusInfectionException", "Содержимое файла инфицировано"},
            {"FileVirusNotCheckedException", "Проверка на вредоносное содержимое не выполнялась, выполняется или дата завершения проверки меньше даты обновления антивирусных баз данных"}
        };
    }
}