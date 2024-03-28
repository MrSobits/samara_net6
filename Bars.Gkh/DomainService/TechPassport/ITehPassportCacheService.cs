namespace Bars.Gkh.DomainService.TechPassport
{
    using System;
    using System.Collections.Generic;

    using Bars.Gkh.Entities;

    /// <summary>
    /// Сервис для работы с кэшем технического пасспорта
    /// </summary>
    public interface ITehPassportCacheService
    {
        /// <summary>
        /// Обновить материализованное представление кэша в БД
        /// <para>Если представление отсутствует, то создает его</para>
        /// </summary>
        void CreateOrUpdateCacheTable();

        /// <summary>
        /// Удалить материализованное представление кэша из БД
        /// </summary>
        void DropCacheTable();

        /// <summary>
        /// Получить значение ячейки
        /// </summary>
        /// <param name="realityObjectId">Идентификатор дома <see cref="RealityObject"/></param>
        /// <param name="formCode">Код формы</param>
        /// <param name="row">Номер строки</param>
        /// <param name="column">Номер столбца</param>
        /// <returns>Возвращает null, если значение не найдено</returns>
        string GetValue(long realityObjectId, string formCode, int row, int column);

        /// <summary>
        /// Получить кэш по всем домам
        /// </summary>
        /// <param name="formCode">Код формы</param>
        /// <param name="row">Номер строки</param>
        /// <param name="column">Номер столбца</param>
        /// <param name="filterIds">Коллекция идентификаторов домов для фильтрации</param>
        /// <returns>Возвращает коллекцию дом - значение</returns>
        Dictionary<long, string> GetCacheByRealityObjects(string formCode, int row, int column, ICollection<long> filterIds = null);

        /// <summary>
        /// Получить кэш по всем домам и строкам формы
        /// </summary>
        /// <param name="formCode">Код формы</param>
        /// <param name="column">Номер столбца</param>
        /// <param name="filterIds">Коллекция идентификаторов домов для фильтрации</param>
        /// <returns>Возвращает список ячеек из стобца формы</returns>
        List<TehPassportCacheCell> GetCacheByRealityObjectsAndRows(string formCode, int column, ICollection<long> filterIds = null);

        /// <summary>
        /// Получить кэш по всем домам и столбцам формы
        /// </summary>
        /// <param name="formCode">Код формы</param>
        /// <param name="row">Номер строки</param>
        /// <param name="filterIds">Коллекция идентификаторов домов для фильтрации</param>
        /// <returns>Возвращает список ячеек из строки формы</returns>
        List<TehPassportCacheCell> GetCacheByRealityObjectsAndColumns(string formCode, int row, ICollection<long> filterIds = null);

        /// <summary>
        /// Проверить значение ячейки
        /// </summary>
        /// <param name="realityObjectId">Идентификатор дома <see cref="RealityObject"/></param>
        /// <param name="formCode">Код формы</param>
        /// <param name="row">Номер строки</param>
        /// <param name="column">Номер столбца</param>
        /// <returns>Возвращает false, если значение не найдено или равно пустой строке</returns>
        bool HasValue(long realityObjectId, string formCode, int row, int column);

        /// <summary>
        /// Получить значение ячейки
        /// </summary>
        /// <param name="realityObjectId">Идентификатор дома <see cref="RealityObject"/></param>
        /// <param name="formCode">Код формы</param>
        Dictionary<int, List<TehPassportCacheCell>> GetRows(long realityObjectId, string formCode);

        /// <summary>
        /// Найти строки по значению столбца
        /// </summary>
        /// <param name="realityObjectId">Идентификатор дома <see cref="RealityObject"/></param>
        /// <param name="formCode">Код формы</param>
        /// <param name="column">Номер строки</param>
        /// <param name="value">Номер столбца</param>
        Dictionary<int, List<TehPassportCacheCell>> FindRowsByColumnValue(long realityObjectId, string formCode, int column, string value);
    }
}