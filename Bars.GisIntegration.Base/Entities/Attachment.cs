namespace Bars.GisIntegration.Base.Entities
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.Enums;

    /// <summary>
    /// Файл-вложение
    /// </summary>
    public class Attachment : BaseEntity
    {
        /// <summary>
        /// Идентификатор исходного b4_file_info
        /// </summary>
        public virtual long SourceFileInfoId { get; set; }

        /// <summary>
        /// Ссылка на b4_file_info
        /// </summary>
        public virtual FileInfo FileInfo { get; set; }

        /// <summary>
        /// Идентификатор файла в ГИС для передачи в методах
        /// </summary>
        public virtual string Guid { get; set; }

        /// <summary>
        /// Хэш файла по алгоритму Gost
        /// </summary>
        public virtual string Hash { get; set; }

        /// <summary>
        /// Наименование вложения
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Имя файлового хранилища ГИС
        /// </summary>
        public virtual FileStorageName FileStorageName { get; set; }

        /// <summary>
        /// Описание вложения
        /// </summary>
        public virtual string Description { get; set; }

        public virtual bool IsValid()
        {
            return this.Name.IsNotEmpty() && this.Hash.IsNotEmpty();
        }

        public virtual bool IsUploaded()
        {
            return this.Guid.IsNotEmpty();
        }
    }
}