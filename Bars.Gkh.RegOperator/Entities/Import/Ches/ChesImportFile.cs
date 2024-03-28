namespace Bars.Gkh.RegOperator.Entities.Import.Ches
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.RegOperator.Imports.Ches;
    using Bars.Gkh.RegOperator.Imports.Ches.File;

    /// <summary>
    /// Сущность для хранения данных о файле импорта
    /// </summary>
    public class ChesImportFile : BaseEntity
    {
        /// <summary>
        /// Сущность для хранения данных об импорте ЧЭС
        /// </summary>
        public virtual ChesImport ChesImport { get; set; }

        /// <summary>
        /// Тип файла импорта
        /// </summary>
        public virtual FileType FileType { get; set; }

        /// <summary>
        /// Статус проверки
        /// </summary>
        public virtual CheckState CheckState { get; set; }

        /// <summary>
        /// Файл импортирован
        /// </summary>
        public virtual bool IsImported { get; set; }

        /// <summary>
        /// отчет о проверке
        /// </summary>
        public virtual FileInfo File { get; set; }
    }
}