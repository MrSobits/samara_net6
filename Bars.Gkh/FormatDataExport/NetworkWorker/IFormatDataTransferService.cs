namespace Bars.Gkh.FormatDataExport.NetworkWorker
{
    using System.Collections.Generic;
    using System.Threading;

    using Bars.B4;

    /// <summary>
    /// Интерфейс для сетевого взаимодействия экспорта данных
    /// </summary>
    public interface IFormatDataTransferService
    {
        /// <summary>
        /// Получить статус операции
        /// </summary>
        IDataResult GetStatus(long id);

        /// <summary>
        /// Загрузить файл
        /// </summary>
        List<IDataResult> UploadFile(string filePath, CancellationToken cancellationToken, int numberOfSegments);

        /// <summary>
        /// Получить файл
        /// </summary>
        IDataResult GetFile(long fileId);

        /// <summary>
        /// Запустить задачу импорта переданных данных
        /// </summary>
        IDataResult StartImport(long fileId, CancellationToken cancellationToken);
    }
}