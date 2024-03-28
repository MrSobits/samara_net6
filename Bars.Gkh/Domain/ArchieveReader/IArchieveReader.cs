namespace Bars.Gkh.Domain.ArchieveReader
{
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Интерфейс для чтения архивов
    /// </summary>
    public interface IArchiveReader
    {
        /// <summary>
        /// Получение файлов в архиве
        /// </summary>
        /// <param name="archive">Поток данных всего архива</param>
        /// <param name="containerName">Имя архива</param>
        /// <returns>Список файлов в архиве</returns>
        IEnumerable<ArchivePart> GetArchiveParts(Stream archive, string containerName);
    }
}