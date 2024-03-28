namespace Bars.Gkh.Overhaul.Hmao.DomainService
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    /// <summary>
    /// Сервис актуализации версии
    /// </summary>
    public interface IActualizeVersionService
    {
        /// <summary>
        /// Пересчитать стоимости всех элементов версии
        /// </summary>
        void CalculateYears(ProgramVersion version, int calcfromyear, int calctoyear, int prsc);

        /// <summary>
        /// Пересчитать стоимости всех элементов версии по алгоритму 1 или 2 этапа
        /// </summary>
        void Calculate12Stage(ProgramVersion version, int calcfromyear, int calctoyear, int stage);

        /// <summary>
        /// Пересчитать стоимости всех элементов версии по алгоритму 3 этапа
        /// </summary>
        void Calculate3Stage(ProgramVersion version, int calcfromyear, int calctoyear);

        /// <summary>
        /// Акутализировать с КПКР
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        IDataResult ActualizeFromShortCr(BaseParams baseParams);

        /// <summary>
        /// Добавить новые записи
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        IDataResult AddNewRecords(BaseParams baseParams);

        /// <summary>
        /// Актуализировать стоимость
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        IDataResult ActualizeSum(BaseParams baseParams);

        /// <summary>
        /// Актуализировать год
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        IDataResult ActualizeYear(BaseParams baseParams);

        /// <summary>
        /// Актуализировать очередность
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        IDataResult ActualizePriority(BaseParams baseParams);

        /// <summary>
        /// Получить запрос списка записей на удаление
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        IQueryable<VersionRecordStage1> GetDeletedEntriesQueryable(BaseParams baseParams);

        /// <summary>
        /// Получить запрос списка записей на добавление
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <param name="afterRepaired">Признак После ремонта</param>
        /// <param name="roStrElList">Список КЭ дома</param>
        /// <returns></returns>
        IQueryable<RealityObjectStructuralElement> GetAddEntriesQueryable(
            BaseParams baseParams,
            Dictionary<long, RoStrElAfterRepair> afterRepaired = null,
            List<long> roStrElList = null);

        /// <summary>
        /// Актуализировать удаление записей
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        IDataResult ActualizeDeletedEntries(BaseParams baseParams);

        /// <summary>
        /// Получить предупреждение
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        IDataResult GetWarningMessage(BaseParams baseParams);

        /// <summary>
        /// Актуализировать год для Ставрополя
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        IDataResult ActualizeYearForStavropol(BaseParams baseParams);

        /// <summary>
        /// Получить запрос списка записей на актуализацию стоимости
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        IQueryable<ActualizeSumEntriesDto> GetActualizeSumEntriesQueryable(BaseParams baseParams);

        /// <summary>
        /// Получить запрос списка записей на актуализацию года
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        IQueryable<VersionRecord> GetActualizeYearEntriesQueryable(BaseParams baseParams);

		/// <summary>
		/// Получить запрос списка записей на актуализацию изменения года
		/// </summary>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns></returns>
		IQueryable<VersionRecord> GetActualizeYearChangeEntriesQueryable(BaseParams baseParams);

        /// <summary>
        /// Актуализировать удаление записей
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        IDataResult RoofCorrection(BaseParams baseParams);

        /// <summary>
        /// Актуализировать удаление записей
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        IDataResult CopyCorrectedYears(BaseParams baseParams);

        /// <summary>
        /// Актуализировать удаление записей
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns></returns>
        IDataResult DeleteRepeatedWorks(BaseParams baseParams);

        /// <summary>
        /// Разделить работу
        /// </summary>
        IDataResult SplitWork(BaseParams baseParams);
    }

    /// <summary>
    /// КЭ дома после ремонта
    /// </summary>
    public class RoStrElAfterRepair
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public long RoStrElId { get; set; }

        /// <summary>
        /// Срок эксплуатации
        /// </summary>
        public int LifeTime { get; set; }

        /// <summary>
        /// Срок эксплуатации после ремонта
        /// </summary>
        public int LifeTimeAfterRepair { get; set; }

        /// <summary>
        /// Год последнего ремонта
        /// </summary>
        public int LastYearRepair { get; set; }
    }

    /// <summary>
    /// Запись на актуализацию
    /// </summary>
    public class ActualizeSumEntriesDto
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
    }
}