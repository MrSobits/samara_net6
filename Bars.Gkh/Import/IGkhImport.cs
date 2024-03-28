namespace Bars.Gkh.Import
{
    using B4;
    using B4.Modules.Tasks.Common.Service;

    /// <summary>
    /// Интерфейс для импорта данных
    /// </summary>
    public interface IGkhImport : ITaskExecutor
    {
        /// <summary>
        /// Ключ импорта
        /// </summary>
        string Key { get; }

        /// <summary>
        /// Код группировки импорта (например группировка в меню)
        /// </summary>
        string CodeImport { get; }

        /// <summary>
        /// Наименование импорта
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Разрешенные расширения файлов
        /// </summary>
        string PossibleFileExtensions { get; }

        /// <summary>
        /// Права
        /// </summary>
        string PermissionName { get; }

        /// <summary>
        /// Зависимости от других импортов
        /// </summary>
        string[] Dependencies { get; }

        /// <summary>
        /// Импорт данных
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        ImportResult Import(BaseParams baseParams);

        /// <summary>
        /// Первоночальная валидация файла перед импортом
        /// </summary>
        /// <param name="baseParams"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        bool Validate(BaseParams baseParams, out string message);
    }
}
