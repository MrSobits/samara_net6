namespace Bars.Gkh.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.Tasks.Common.Entities;

    /// <summary>
    /// Журнал импортов
    /// </summary>
    public class LogImport : BaseEntity
    {
        /// <summary>
        /// Оператор
        /// </summary>
        public virtual Operator Operator { get; set; }

        /// <summary>
        /// Логин
        /// </summary>
        public virtual string Login { get; set; }

        /// <summary>
        /// Дата загрузки
        /// </summary>
        public virtual DateTime UploadDate { get; set; }

        /// <summary>
        /// Наименование импортируемого файла
        /// </summary>
        public virtual string FileName { get; set; }

        /// <summary>
        /// Ключ импорта
        /// </summary>
        public virtual string ImportKey { get; set; }

        /// <summary>
        /// Количество предупреждений
        /// </summary>
        public virtual int CountWarning { get; set; }

        /// <summary>
        /// Количество ошибок
        /// </summary>
        public virtual int CountError { get; set; }

        /// <summary>
        /// Количество импортированных строк
        /// </summary>
        public virtual int CountImportedRows { get; set; }

        /// <summary>
        /// Количество изменнных строк
        /// </summary>
        public virtual int CountChangedRows { get; set; }

        /// <summary>
        /// Количество импортированных файлов
        /// </summary>
        public virtual int CountImportedFile { get; set; }

        /// <summary>
        /// Импортируемый файл
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Файл с журналом, сформированный по результатам импорта. Его можно скачать.
        /// </summary>
        public virtual FileInfo LogFile { get; set; }

        /// <summary>
        /// Задача, которая разбирала импорт, на сервере вычислений
        /// </summary>        
        public virtual TaskEntry Task { get; set; }
    }
}