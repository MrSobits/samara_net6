namespace Bars.Gkh.Modules.Gkh1468.Map.PublicServiceOrg.ContractParty
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Modules.Gkh1468.Entities.ContractPart;

    /// <summary>Маппинг для Сторона договора "ТЭР"</summary>
    public class FuelEnergyResourceContractMap : JoinedSubClassMap<FuelEnergyResourceContract>
    {
        /// <inheritdoc />
        public FuelEnergyResourceContractMap() :
                base("Сторона договора \"Бюджетная организация\"", "GKH_RSOCONTRACT_FUEL_ENERGY_ORG")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Reference(x => x.FuelEnergyResourceOrg, "Поставщик ресурсов").Column("FUEL_ENERGY_ORG_ID").NotNull();
        }
    }
}
