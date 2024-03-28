namespace Bars.Gkh.SystemDataTransfer.Caching
{
    using System;
    using System.Collections.Generic;

    using Bars.Gkh.Domain.Cache;
    using Bars.Gkh.SystemDataTransfer.Meta;

    /// <summary>
    /// Хранитель кэша Dto-объекта
    /// </summary>
    public interface ICacheDtoHolder
    {
        /// <summary>
        /// Тип Dto
        /// </summary>
        Type Type { get; }

        /// <summary>
        /// Тип сущности
        /// </summary>
        Type EntityType { get; }

        /// <summary>
        /// Вернуть сущность по сопоставляемым ключам
        /// </summary>
        /// <param name="objectMap">Импортируемый объект</param>
        /// <returns>Объект</returns>
        IImportableEntity GetFromCache(IDictionary<string, object> objectMap);

        /// <summary>
        /// Вернуть сущность по внешнему идентификатору
        /// </summary>
        /// <param name="importId">Внешний ID</param>
        /// <returns>Объект</returns>
        IImportableEntity GetFromCache(object importId);

        /// <summary>
        /// Зарегистрировать кэш
        /// </summary>
        /// <param name="cache">Экземпляр хранителя кэша</param>
        void RegisterCache(GkhCache cache);

        /// <summary>
        /// Добавить зависимость для получения внешних ID
        /// </summary>
        /// <param name="name">Имя свойства</param>
        /// <param name="depencyHolder">Хранитель</param>
        void AddDependencyType(string name, ICacheDtoHolder depencyHolder);

        /// <summary>
        /// Собрать словарь по сопоставляемым ключам
        /// </summary>
        void MakeDictionary();
        /// <summary>
        /// Добавить сущность в кэш
        /// </summary>
        /// <param name="entity">Сущность</param>
        /// <param name="objectMap">Импортируемый объект</param>
        void AddEntity(object entity, IDictionary<string, object> objectMap);
    }
}