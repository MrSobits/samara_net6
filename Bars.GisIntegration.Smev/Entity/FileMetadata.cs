namespace Bars.GisIntegration.Smev.Entity
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;

    public class FileMetadata : BaseEntity
    {
        /// <summary>
        ///     Guid файла
        /// <remarks>для совместимости с BarsUp</remarks>
        /// </summary>
        public virtual Guid? Uid { get; set; }

        /// <summary>Наименование файла без расширения.</summary>
        public virtual string Name { get; set; }

        /// <summary>Расширение файла.</summary>
        public virtual string Extension { get; set; }

        /// <summary>Кэшированное имя файла.</summary>
        public virtual string CachedName { get; set; }

        /// <summary>Дата последнего обращения.</summary>
        public virtual DateTime LastAccess { get; set; }

        /// <summary>Контрольная сумма.</summary>
        public virtual string Checksum { get; set; }

        /// <summary>Алгоритм получения контрольной суммы.</summary>
        public virtual string ChecksumHashAlgorithm { get; set; }

        /// <summary>Размер файла в байтах.</summary>
        public virtual long Size { get; set; }

        /// <summary>Является временным.</summary>
        public virtual bool IsTemporary { get; set; }

        /// <summary>
        ///     Полное имя файла - наименование + '.' + расширение.
        /// </summary>
        public virtual string FullName => this.Name + "." + this.Extension;

        /// <summary>
        /// Преобразование <see cref="T:Bars.Gkh.FileManager.BarsUp.Entities.FileMetadata" /> в <see cref="T:BarsUp.Modules.FileStorage.FileInfo" />
        /// </summary>
        public static explicit operator FileInfo(FileMetadata metadata)
        {
            if (metadata == null)
                return (FileInfo)null;
            FileInfo fileInfo = new FileInfo();
            fileInfo.CheckSum = metadata.Checksum;
            fileInfo.Id = metadata.Id;
            fileInfo.Size = metadata.Size;
            fileInfo.CheckSum = metadata.Checksum;
            fileInfo.ObjectVersion = metadata.ObjectVersion;
            fileInfo.ObjectCreateDate = metadata.ObjectCreateDate;
            fileInfo.ObjectEditDate = metadata.ObjectEditDate;
            return fileInfo;
        }

        /// <summary>
        /// Преобразование <see cref="T:BarsUp.Modules.FileStorage.FileInfo" /> в <see cref="T:Bars.Gkh.FileManager.BarsUp.Entities.FileMetadata" />
        /// </summary>
        public static implicit operator FileMetadata(FileInfo info)
        {
            if (info == null)
                return (FileMetadata)null;
            FileMetadata fileMetadata = new FileMetadata();
            fileMetadata.Checksum = info.CheckSum;
            fileMetadata.Extension = info.Extention;
            fileMetadata.Name = info.Name;
            fileMetadata.Size = info.Size;
            fileMetadata.Uid = new Guid?(new Guid());
            fileMetadata.CachedName = info.ObjectCreateDate.Year + "/" + info.ObjectCreateDate.Month + "/" + info.Id + info.Extention;
            fileMetadata.IsTemporary = false;
            fileMetadata.Id = info.Id;
            fileMetadata.ObjectVersion = info.ObjectVersion;
            fileMetadata.ObjectCreateDate = info.ObjectCreateDate;
            fileMetadata.ObjectEditDate = info.ObjectEditDate;
            return fileMetadata;
        }
    }
}