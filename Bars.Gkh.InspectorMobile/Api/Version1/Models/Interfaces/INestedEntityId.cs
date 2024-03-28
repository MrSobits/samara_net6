namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.Interfaces
{
    /// <summary>
    /// Интерфейс вложенной сущности с идентификатором,
    /// которая привязывается к определенному объекту
    /// </summary>
    /// <remarks>
    /// Для вложенных объектов, которые обладают логикой:
    /// Создавать, если id = null
    /// Обновлять, если id != null
    /// Удалять все, что не было обновлено
    /// </remarks>
    public interface INestedEntityId
    {
        /// <summary>
        /// Уникальный идентификатор записи
        /// </summary>
        long? Id { get; set; }
    }
}