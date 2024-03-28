namespace Bars.Gkh.Regions.Tatarstan.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Regions.Tatarstan.Entities;

    /// <summary>
    /// Маппинг
    /// </summary>
    public class FuelEnergyOrgContractInfoMap : BaseEntityMap<FuelEnergyOrgContractInfo>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public FuelEnergyOrgContractInfoMap()
            : base("Bars.Gkh.Regions.Tatarstan.Entities.FuelEnergyOrgContractInfo", "GKH_FUEL_ENERGY_CONTRACT_INFO")
        {
        }

        /// <summary>
        /// Мап
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.SaldoIn, "Задолженность на начало месяца").Column("SALDO_IN").NotNull();
            this.Property(x => x.DebtIn, "Входящая просроченная задолженность").Column("DEBT_IN").NotNull();
            this.Property(x => x.Charged, "Начислено за месяц").Column("CHARGED").NotNull();
            this.Property(x => x.Paid, "Оплачено за месяц").Column("PAID").NotNull();
            this.Property(x => x.SaldoOut, "Задолженность на конец месяца").Column("SALDO_OUT").NotNull();
            this.Property(x => x.DebtOut, "Исходящая просроченная задолженность").Column("DEBT_OUT").NotNull();
            this.Property(x => x.PlanPaid, "Планируемая оплата").Column("PLAN_PAID").NotNull();

            this.Reference(x => x.PeriodSummary, "Агрегация по РСО в периоде").Column("PER_SUMM_ID").NotNull();
            this.Reference(x => x.Resource, "Ресурс по договору").Column("RESOURCE_ID").NotNull();
            this.Reference(x => x.FuelEnergyResourceContract, "Сторона договора ТЭР").Column("CONTR_ID").NotNull();
        }
    }
}