namespace Bars.Gkh.FormatDataExport.ExportableEntities.ExportableFile
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Прокси класс файлового потока из хранилища
    /// </summary>
    public class ExportFileStream : IDisposable, IEquatable<ExportFileStream>
    {
        /// <summary>
        /// Имя файла
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Файловый поток
        /// </summary>
        public Stream FileStream { get; }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="fileStream">Файловый поток</param>
        public ExportFileStream(FileStream fileStream)
        {
            this.FileStream = fileStream;
            if (fileStream != null)
            {
                this.Name = Path.GetFileName(fileStream.Name);
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            this.FileStream?.Dispose();
        }

        public bool Equals(ExportFileStream other)
        {
            if (object.ReferenceEquals(null, other))
                return false;
            if (object.ReferenceEquals(this, other))
                return true;
            return string.Equals(this.Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(null, obj))
                return false;
            if (object.ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != this.GetType())
                return false;
            return this.Equals((ExportFileStream) obj);
        }

        public override int GetHashCode()
        {
            return this.Name?.GetHashCode() ?? 42;
        }
    }
}
