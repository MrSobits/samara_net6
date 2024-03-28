namespace Bars.Gkh.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.SystemDataTransfer.Meta;
    using Newtonsoft.Json;

    public class BaseImportableEntity : BaseEntity, IImportableEntity
    {
        /// <summary>
        /// Идентификатор сущности внешней системы
        /// </summary> 
        [JsonIgnore]
        public virtual long? ImportEntityId { get; set; }
    }
}