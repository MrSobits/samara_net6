namespace Bars.Gkh.Modules.Gkh1468.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Сущность - "Информация о температурном графике"
    /// </summary>
    public class PublicServiceOrgTemperatureInfo : BaseImportableEntity
    {
        /// <summary>
        /// Температура наружного воздуха
        /// </summary>
        public virtual int OutdoorAirTemp { get; set; }

        /// <summary>
        /// Температура теплоносителя в подающем трубопроводе
        /// </summary>
        public virtual int CoolantTempSupplyPipeline { get; set; }

        /// <summary>
        /// Температура теплоносителя в обратном трубопроводе
        /// </summary>
        public virtual int CoolantTempReturnPipeline { get; set; }

        /// <summary>
        /// Договор поставщика ресурсов с домом
        /// </summary>
        public virtual PublicServiceOrgContract Contract { get; set; }
    }
}
