namespace Bars.B4.Modules.FIAS.AutoUpdater.DownloadService
{
    using Bars.B4.Modules.Tasks.Common.Service;
    using System.Threading;

    /// <summary>
    /// Сервис получения обновлений ФИАС
    /// </summary>
    public interface IFiasDownloadService
    {
        /// <summary>
        /// Загрузить последнее обновление ФИАС
        /// </summary>
        /// <param name="baseParams">
        /// forcedownload - перезагрузить файл
        /// </param>
        /// <returns>
        /// <see cref="IDataResult.Data"/> - полный путь до архива
        /// <see cref="IDataResult.Message"/> - версия обновления
        /// </returns>
        /// <exception cref="ObjectDisposedException">Сервис загрузки остановлен</exception>
        IDataResult GetLastDeltaUpdate(BaseParams baseParams, CancellationToken ct, IProgressIndicator indicator = null);

        /// <summary>
        /// Загрузить последнюю полную версию ФИАС
        /// </summary>
        /// <param name="baseParams">
        /// forcedownload - перезагрузить файл
        /// </param>
        /// <returns>
        /// <see cref="IDataResult.Data"/> - полный путь до архива
        /// <see cref="IDataResult.Message"/> - версия обновления
        /// </returns>
        /// <exception cref="ObjectDisposedException">Сервис загрузки остановлен</exception>
        IDataResult GetLastFullUpdate(BaseParams baseParams, CancellationToken ct, IProgressIndicator indicator = null);
    }
}