namespace Bars.Gkh.FormatDataExport.ExportableEntities.ExportableFile
{
    using Bars.B4.Modules.FileStorage;

    /// <summary>
    /// Экспортируемый файл
    /// </summary>
    public class ExportableFileInfo : FileInfo
    {
        /// <summary>
        /// Описание файла
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Полный путь файла в хранилище
        /// </summary>
        public string FullPath { get; set; }

        /// <summary>
        /// Имя файла в файловом хранилище
        /// <para>Id.Extension</para>
        /// </summary>
        public string FileName => $"{this.Id}.{this.Extention}";

        /// <summary>
        /// Создать объект из базовой сущности
        /// </summary>
        /// <param name="file">Базовая сущность</param>
        /// <param name="description">Описание файла</param>
        public static ExportableFileInfo CreateFromBase(FileInfo file, string description = null)
        {
            return new ExportableFileInfo
            {
                Id = file.Id,
                ObjectCreateDate = file.ObjectCreateDate,
                ObjectEditDate = file.ObjectEditDate,
                ObjectVersion = file.ObjectVersion,
                CheckSum = file.CheckSum,
                Name = file.Name,
                Extention = file.Extention,
                Size = file.Size,
                Description = description
            };
        }
    }
}
