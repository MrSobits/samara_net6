namespace Bars.Gkh.Entities.Dicts
{
    using Newtonsoft.Json;

    /// <summary>
    /// Материал стен
    /// </summary>
    public class WallMaterial : BaseGkhDict
    {
        /// <summary>
        /// Идентификатор сущности внешней системы
        /// </summary> 
        [JsonIgnore]
        public virtual long? ImportEntityId { get; set; }

        /// <summary>
        ///  Внешний идентификатор
        /// </summary>
        [JsonIgnore]
        public virtual string ExternalId { get; set; }
    }
}
