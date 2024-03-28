namespace Bars.B4.Modules.FIAS.AutoUpdater
{
    using Bars.B4.Modules.Tasks.Common.Service;
    using System.Threading;

    /// <summary>
    /// Сервис автоматического обновления справочника ФИАС
    /// </summary>
    public interface IFiasAutoUpdater
    {
        /// <summary>
        /// Загрузить базу данных ФИАС с сервера
        /// </summary>
        /// <param name="baseParams">
        /// delta - загрузить обновление на текущую дату
        /// forcedownload - перезагрузить файл при наличии
        /// </param>
        IDataResult Download(BaseParams baseParams, CancellationToken ct, IProgressIndicator indicator = null);

        /// <summary>
        /// Обновить справочник ФИАС из архива
        /// </summary>
        IDataResult Update(BaseParams baseParams, CancellationToken ct, IProgressIndicator indicator = null);

        /// <summary>
        /// Удалить файлы обновлений
        /// </summary>
        IDataResult CleanFiles(CancellationToken ct, IProgressIndicator indicator = null);
    }
}