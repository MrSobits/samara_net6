namespace Bars.Gkh.Entities
{
    using Newtonsoft.Json;

    /// <summary>
    /// Базовая сущность ЖКХ с Идентификатором из внешней системы
    /// </summary>
    public class BaseGkhEntity : BaseImportableEntity
    {
        /// <summary>
        ///  Внешний идентификатор
        /// </summary>
        [JsonIgnore]
        public virtual string ExternalId { get; set; }
    }
}