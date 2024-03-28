namespace Bars.B4.Modules.FIAS.AutoUpdater.ArchiveReader
{
    using System.Collections.Generic;
    using System.Threading;
    using Bars.B4.Modules.Tasks.Common.Service;

    /// <summary>
    /// Сервис работы с архивом ФИАС
    /// </summary>
    public interface IFiasArchiveReader
    {
        /// <summary>
        /// Словарь связанных файлов с сущностью "Данные ФИАС"
        /// </summary>
        IDictionary<string, string> FiasLinkedFilesDict { get; set; }
        
        /// <summary>
        /// Словарь связанных файлов с сущностью "Данные о домах ФИАС"
        /// </summary>
        IDictionary<string, string> FiasHouseLinkedFilesDict { get; set; }

        /// <summary>
        /// Распаковать файлы для обновления ФИАС
        /// </summary>
        /// <param name="archFilePath">Полный путь до архива ФИАС</param>
        /// <param name="unpackDir">Директория распаковки файлов. По умолчанию директория с архивом</param>
<<<<<<< HEAD
        /// <returns>Список распаковынных файлов в <see cref="IDataResult.Data"/>.
        /// <see cref="Dictionary{TKey,TValue}.Keys"/> - Имя сущности (e.g. <see cref="FiasEntityName"/>)
        /// <see cref="Dictionary{TKey,TValue}.Values"/> - Путь к *.dbf файлу
        /// </returns>
        IDataResult<Dictionary<string, string>> UnpackFiles(string archFilePath, CancellationToken ct, IProgressIndicator indicator = null, string unpackDir = null);
=======
        IDataResult UnpackFiles(string archFilePath, string unpackDir = null);
>>>>>>> net6
    }
}