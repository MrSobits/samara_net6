namespace Bars.Gkh.Regions.Tatarstan.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Regions.Tatarstan.Entities;

    /// <summary>
    /// Маппинг
    /// </summary>
    public class FuelEnergyOrgContractDetailMap : BaseEntityMap<FuelEnergyOrgContractDetail>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public FuelEnergyOrgContractDetailMap()
            : base("Bars.Gkh.Regions.Tatarstan.Entities.FuelEnergyOrgContractDetail", "GKH_FUEL_ENERGY_CONTRACT_DETAIL")
        {
        }

        /// <summary>
        /// Мап
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.Charged, "Начислено за месяц").Column("CHARGED").NotNull();
            this.Property(x => x.Paid, "Оплачено за месяц").Column("PAID").NotNull();
            this.Property(x => x.Debt, "Задолженность на конец месяца").Column("DEBT").NotNull();

            this.Reference(x => x.PeriodSumm, "Агрегация по РСО в периоде").Column("PER_SUMM_ID").NotNull();
            this.Reference(x => x.Service, "Услуга").Column("SERV_ID").NotNull();
            this.Reference(x => x.GasEnergyPercents, "Процент планируемых оплат по газу").Column("GAS_PERC_ID").NotNull();
            this.Reference(x => x.ElectricityEnergyPercents, "Процент планируемых оплат по электроэнергии").Column("EL_PERC_ID").NotNull();
        }
    }
}