namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.Common
{
    using Bars.B4.Modules.States;
    using Bars.Gkh.BaseApiIntegration.Attributes;

    /// <summary>
    /// Информация о статусе. Модель выборки
    /// </summary>
    public class StateInfoGet : BaseStateInfo
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Экземпляр информации о статусе на основе <see cref="State"/>
        /// </summary>
        /// <param name="state">Экземпляр <see cref="State"/></param>
        public StateInfoGet(State state)
        {
            this.Id = state.Id;
            this.Name = state.Name;
        }
    }

    /// <summary>
    /// Информация о статусе. Модель обновления
    /// </summary>
    public class StateInfoUpdate : BaseStateInfo
    {
    }

    /// <summary>
    /// Информация о статусе. Базовая модель
    /// </summary>
    public class BaseStateInfo
    {
        /// <summary>
        /// Уникальный идентификатор записи
        /// </summary>
        [RequiredExistsEntity(typeof(State))]
        public long? Id { get; set; }
    }
}