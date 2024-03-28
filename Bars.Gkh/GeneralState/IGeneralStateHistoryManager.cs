namespace Bars.Gkh.GeneralState
{
    using System;
    using Bars.B4.DataModels;

    /// <summary>
    /// Интерфейс для массового создания и сохранения истории смены обощенных состояний
    /// </summary>
    public interface IGeneralStateHistoryManager
    {
        /// <summary>
        /// Инициализировать
        /// </summary>
        /// <param name="entityType">Тип сущности, который будет логироваться менеджером</param>
        /// <param name="propertyName">Имя поля</param>
        void Init(Type entityType, string propertyName = null);

        /// <summary>
        /// Создать историю смены статусов и добавить в буфер
        /// </summary>
        /// <param name="entity">Сущность</param>
        /// <param name="oldValue">Старое значенеи</param>
        /// <param name="newValue">Новое значениее</param>
        /// <param name="stronglyTyped">Строго типизирован ли менеджер</param>
        void CreateStateHistory(IHaveId entity, object oldValue, object newValue, bool stronglyTyped = true);

        /// <summary>
        /// Сохранить созданные истории
        /// </summary>
        void SaveStateHistories();
    }
}