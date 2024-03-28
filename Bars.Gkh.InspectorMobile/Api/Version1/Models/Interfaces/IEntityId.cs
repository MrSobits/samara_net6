namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.Interfaces
{
    /// <summary>
    /// Интерфейс сущности с идентификатором
    /// </summary>
    public interface IEntityId
    {
        /// <summary>
        /// Уникальный идентификатор записи
        /// </summary>
        long? Id { get; set; }
    }
}