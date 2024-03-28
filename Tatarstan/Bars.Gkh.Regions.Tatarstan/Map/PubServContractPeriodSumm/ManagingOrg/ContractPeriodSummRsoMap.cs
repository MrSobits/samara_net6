namespace Bars.Gkh.Regions.Tatarstan.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Regions.Tatarstan.Entities;

    /// <summary>
    /// Маппинг
    /// </summary>
    public class ContractPeriodSummRsoMap : BaseEntityMap<ContractPeriodSummRso>
    {
        /// <summary>
        /// ctor
        /// </summary>
        public ContractPeriodSummRsoMap()
            : base("Bars.Gkh.Regions.Tatarstan.Entities.ContractPeriodSummRso", "GKH_CONTR_PERIOD_SUMM_RSO")
        {
        }

        /// <summary>
        /// Мап
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.PublicServiceOrg, "Ресурсоснабжающая организация").Column("PUB_SERV_ORG_ID").NotNull().Fetch();
            this.Reference(x => x.State, "Статус").Column("STATE_ID").NotNull().Fetch();

            this.Property(x => x.ChargedManOrg, "Начислено  УО").Column("CHARGED_MAN_ORG").NotNull();
            this.Property(x => x.PaidManOrg, "Оплачено УО").Column("PAID_MAN_ORG").NotNull();
            this.Property(x => x.SaldoOut, "Исходящее сальдо ").Column("SALDO_OUT").NotNull();
        }
    }
}