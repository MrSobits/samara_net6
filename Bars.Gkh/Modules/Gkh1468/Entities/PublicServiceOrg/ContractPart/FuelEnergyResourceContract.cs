namespace Bars.Gkh.Modules.Gkh1468.Entities.ContractPart
{
    /// <summary>
    /// Сторона договора "ТЭР"
    /// </summary>
    public class FuelEnergyResourceContract : BaseContractPart
    {
        /// <summary>
        /// Поставщик ТЭР
        /// </summary>
        public virtual PublicServiceOrg FuelEnergyResourceOrg { get; set; }
    }
}