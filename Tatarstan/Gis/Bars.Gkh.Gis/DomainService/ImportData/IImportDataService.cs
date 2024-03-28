namespace Bars.Gkh.Gis.DomainService.ImportData
{
    using System.Collections.Generic;
    using B4;
    using DataResult;
    using Entities.Register.LoadedFileRegister;

    /// <summary>
    /// Интерфейс cервиса импорта данных
    /// </summary>
    public interface IImportDataService
    {
        /// <summary>
        /// Добавляет задачу на импорт файла
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат импорта</returns>
        ImportDataResult AddTask(BaseParams baseParams);

        /// <summary>
        /// Импорт файлов
        /// </summary>
        /// <param name="loadedFileList">Загруженные файлы</param>
        /// <returns>Результат импорта файлов</returns>
        IDataResult Import(IEnumerable<LoadedFileRegister> loadedFileList);

        /// <summary>
        /// Получение списка загруженных файлов
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Список загруженных файлов</returns>
        ListDataResult GetLoadedFilesList(BaseParams baseParams);

        /// <summary>
        /// Получение списка всех  загруженных файлов
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Список загруженных файлов</returns>
        ListDataResult GetAllLoadedFiles(BaseParams baseParams);

        /// <summary>
        /// Получение списка загруженных файлов в "Открытый Татарстан"
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Список загрузок</returns>
        ListDataResult GetOpenTatarstanData(BaseParams baseParams);

        /// <summary>
        /// Удалить загруженные данные 
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns></returns>
        IDataResult DeleteLoadedData(BaseParams baseParams);

        /// <summary>
        /// Удалить загруженные данные "Открытый Татарстан"
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns></returns>
        IDataResult DeleteOtData(BaseParams baseParams);
    }
}