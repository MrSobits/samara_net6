namespace Bars.Gkh.GeneralState
{
    using System;

    using Bars.B4.DataModels;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Интерфейс для работы с обощенными состояниями
    /// </summary>
    public interface IGeneralStateHistoryService
    {
        /// <summary>
        /// Получить описатель состояния
        /// </summary>
        /// <param name="type">Тип, для которого ищется описатель</param>
        /// <param name="propertyName">Имя свойства (если имя не передано, то берется первое описанное свойство)</param>
        /// <returns></returns>
        GeneralStatefulEntityInfo GetStateHistoryInfo(Type type, string propertyName = null);

        /// <summary>
        /// Получить описатель состояния
        /// </summary>
        /// <param name="code">Код описателя</param>
        /// <returns></returns>
        GeneralStatefulEntityInfo GetStateHistoryInfo(string code);

        /// <summary>
        /// Создать историю смены обобщенного состояния
        /// </summary>
        /// <param name="entity">Сущность</param>
        /// <param name="oldValue">Старое значение</param>
        /// <param name="newValue">Новое значение</param>
        /// <param name="propertyName">Наименование поля</param>
        /// <returns>Созданная история</returns>
        GeneralStateHistory CreateStateHistory(IHaveId entity, object oldValue, object newValue, string propertyName = null);

        /// <summary>
        /// Создать историю смены обощенного состояния
        /// </summary>
        /// <param name="entity">Сущность</param>
        /// <param name="info">Описатель</param>
        /// <param name="oldValue">Старое значение</param>
        /// <param name="newValue">Новое значение</param>
        /// <returns>Созданная история</returns>
        GeneralStateHistory CreateStateHistory(IHaveId entity, GeneralStatefulEntityInfo info, object oldValue, object newValue);

        /// <summary>
        /// Получить отображаемое значение
        /// </summary>
        /// <param name="info">Описатель</param>
        /// <param name="value">Значение</param>
        string GetDisplayValue(GeneralStatefulEntityInfo info, string value);
    }
}