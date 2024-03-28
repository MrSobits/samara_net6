namespace Bars.Gkh.Overhaul.Hmao.DomainService
{
    using System.Linq;

    using Bars.B4;

    /// <summary>
    /// Сервис версии программы
    /// </summary>
    public interface IProgramVersionService
    {
        /// <summary>
        /// Изменить данные версии
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        IDataResult ChangeVersionData(BaseParams baseParams);

        /// <summary>
        /// Создать новую версию
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        IDataResult MakeNewVersion(BaseParams baseParams);

        /// <summary>
        /// Создать новые версии массово
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        IDataResult MakeNewVersionAll(BaseParams baseParams);

        /// <summary>
        /// Получить список записей на удаление
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        IDataResult GetDeletedEntriesList(BaseParams baseParams);

        /// <summary>
        /// Копировать версию
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        IDataResult CopyVersion(BaseParams baseParams);

        /// <summary>
        /// Список для массового изменения года
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        IDataResult ListForMassChangeYear(BaseParams baseParams);

        /// <summary>
        /// Массовое изменение года
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        IDataResult MassChangeYear(BaseParams baseParams);

        /// <summary>
        /// Получить запрос записей на удаление
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        IQueryable<DeletedEntriesDto> GetDeletedEntriesQuery(BaseParams baseParams);

        /// <summary>
        /// Получить список записей на добавление
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        IDataResult GetAddEntriesList(BaseParams baseParams);

        /// <summary>
        /// Получить список записей на актуализацию стоимости
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        IDataResult GetActualizeSumEntriesList(BaseParams baseParams);

        /// <summary>
        /// Получить список записей на добавление
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        IDataResult GetActualizeYearEntriesList(BaseParams baseParams);

        /// <summary>
        /// Получить список записей на добавление актуализации изменения года
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        IDataResult GetActualizeYearChangeEntriesList(BaseParams baseParams);

        /// <summary>
        /// Список версий ДПКР
        /// </summary>
        /// <param name="baseParams">
        /// municipalityId - массив Id МО
        /// </param>
        IDataResult ListMainVersions(BaseParams baseParams);
    }

    /// <summary>
    /// Запись на удаление
    /// </summary>
    public class DeletedEntriesDto
    {
        /// <summary>
        /// Идентификатор 3 этапа
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Дом
        /// </summary>
        public string RealityObject { get; set; }

        /// <summary>
        /// ООИ
        /// </summary>
        public string CommonEstateObjects { get; set; }

        /// <summary>
        /// Год
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Порядковый номер
        /// </summary>
        public int IndexNumber { get; set; }

        /// <summary>
        /// Стоимость
        /// </summary>
        public decimal Sum { get; set; }

        /// <summary>
        /// Скорректированный год
        /// </summary>
        public int CorrectYear { get; set; }

        /// <summary>
        /// Изменен год
        /// </summary>
        public bool IsChangedYear { get; set; }
    }
}