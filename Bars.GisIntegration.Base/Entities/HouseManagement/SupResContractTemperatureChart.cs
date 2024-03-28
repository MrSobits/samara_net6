namespace Bars.GisIntegration.Base.Entities.HouseManagement
{
    using Bars.GisIntegration.Base.Entities;

    /// <summary>
    /// Информация о температурном графике договора с поставщиком ресурсов
    /// </summary>
    public class SupResContractTemperatureChart : BaseRisEntity
    {
        /// <summary>
        /// Договор с поставщиком ресурсов
        /// </summary>
        public virtual SupplyResourceContract Contract { get; set; }

        /// <summary>
        /// Температура наружного воздуха
        /// </summary>
        public virtual int? OutsideTemperature { get; set; }

        /// <summary>
        /// Температура теплоносителя в подающем трубопроводе
        /// </summary>
        public virtual string FlowLineTemperature { get; set; }

        /// <summary>
        /// Температура теплоносителя в обратном трубопроводе
        /// </summary>
        public virtual string OppositeLineTemperature { get; set; }
    }
}
