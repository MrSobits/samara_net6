namespace Bars.Gkh.Import
{
    /// <summary>
    /// Информация об импорте
    /// </summary>
    public class GkhImportInfo
    {
        /// <summary>
        /// Ключ импорта
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// Наименование импорта
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Разрешенные расширения файлов
        /// </summary>
        public string PossibleFileExtensions { get; set; }

        /// <summary>
        /// Права
        /// </summary>
        public string PermissionName { get; set; }

        /// <summary>
        /// Зависимости от других импортов
        /// </summary>
        public string[] Dependencies { get; set; }
    }
}
