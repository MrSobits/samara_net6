namespace Bars.Gkh.SystemDataTransfer.Meta
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Интерфейс Dto сущности, которая импортируется из сторонней системы
    /// </summary>
    public interface IImportableEntity : IEntity
    {
        /// <summary>
        /// Идентификатор сущности внешней системы
        /// </summary>
        long? ImportEntityId { get; set; }
    }
}