namespace Bars.Gkh.Import
{
    using System.IO;

    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    public interface ILogImport
    {
        string Key { get; }

        /// <summary>
        /// Наименование лога
        /// </summary>
        string FileName { get; }

        /// <summary>
        /// Оператор
        /// </summary>
        Operator Operator { get; set; }

        /// <summary>
        /// Идентификатор импорта
        /// </summary>
        string ImportKey { get; set; }

        /// <summary>
        /// Количество предупреждений
        /// </summary>
        int CountWarning { get; set; }

        /// <summary>
        /// Количество ошибок
        /// </summary>
        int CountError { get; set; }

        /// <summary>
        /// Количество добавленных в систему строк
        /// </summary>
        int CountAddedRows { get; set; }

        /// <summary>
        /// Количество измененных в системе строк
        /// </summary>
        int CountChangedRows { get; set; }

        /// <summary>
        /// Общее количество импортированных строк (Добавленных и изменных импортом)
        /// </summary>
        int CountImportedRows { get; }

        /// <summary>
        /// Статус импорта
        /// </summary>
        bool IsImported { get; set; }

        /// <summary>
        /// Файл с ошибочными строками
        /// </summary>
        Stream FileErrorsRow { get; set; }

        /// <summary>
        /// Формирование наименования лога
        /// </summary>
        /// <param name="fileNameWithoutExtention">наименование импортируемого файла без расширения</param>
        void SetFileName(string fileNameWithoutExtention);

        /// <summary>
        /// записать ошибку
        /// </summary>
        /// <param name="title">Заголовок</param>
        /// <param name="description">Описание ошибки</param>
        /// <param name="args">Дополнительные аргументы</param>
        void Error(string title, string description, params string[] args);

        /// <summary>
        /// записать предупреждение
        /// </summary>
        /// <param name="title">Заголовок</param>
        /// <param name="description">Описание предупреждения</param>
        /// <param name="args">Дополнительные аргументы</param>
        void Warn(string title, string description, params string[] args);

        /// <summary>
        /// записать отладочную информацию
        /// </summary>
        /// <param name="title">Заголовок</param>
        /// <param name="description">Описание</param>
        /// <param name="args">Дополнительные аргументы</param>
        void Debug(string title, string description, params string[] args);

        /// <summary>
        /// записать информацию (Счетчики изменений данных, если надо, заполнять самим)
        /// </summary>
        /// <param name="title">Заголовок</param>
        /// <param name="description">Описание</param>
        /// <param name="args">Дополнительные аргументы</param>
        void Info(string title, string description, params string[] args);

        /// <summary>
        /// записать информацию +Увеличить необходимый счетчик по LogTypeChanged
        /// </summary>
        /// <param name="title">Заголовок</param>
        /// <param name="description">Описание</param>
        /// <param name="logTypeChanged">Тип изменения</param>
        void Info(string title, string description, LogTypeChanged logTypeChanged);

        /// <summary>
        /// Запись итогов в лог
        /// </summary>
        void PlacingResults();

        /// <summary>
        /// Вернуть файл лога
        /// </summary>
        /// <returns></returns>
        Stream GetFile();

        /// <summary>
        /// Получить файл с ошибками
        /// </summary>
        /// <returns></returns>
        Stream GetErrorRows();

        /// <summary>
        /// Добавить строку 
        /// </summary>
        /// <param name="row"></param>
        void AddErrorsRow(params string[] row);

        /// <summary>
        /// Добавить шапку в файл с ошибками
        /// </summary>
        /// <param name="header"></param>
        void AddErrorsHeader(params string[] header);
    }
}
