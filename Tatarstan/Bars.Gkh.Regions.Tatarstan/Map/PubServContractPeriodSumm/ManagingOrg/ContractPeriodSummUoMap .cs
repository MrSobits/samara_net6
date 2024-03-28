namespace Bars.Gkh.Regions.Tatarstan.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Regions.Tatarstan.Entities;

    /// <summary>
    /// Маппинг
    /// </summary>
    public class ContractPeriodSummUoMap : BaseEntityMap<ContractPeriodSummUo>
    {
        /// <summary>
        /// ctor
        /// </summary>
        public ContractPeriodSummUoMap()
            : base("Bars.Gkh.Regions.Tatarstan.Entities.ContractPeriodSummUo", "GKH_CONTR_PERIOD_SUMM_UO")
        {
        }

        /// <summary>
        /// Мап
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.ManagingOrganization, "Управляющая компания").Column("MAN_ORG_ID").NotNull().Fetch();
            this.Reference(x => x.State, "Статус").Column("STATE_ID").NotNull().Fetch();

            this.Property(x => x.StartDebt, "Входящее сальдо  (Долг на начало месяца)").Column("START_DEBT").NotNull();
            this.Property(x => x.ChargedResidents, "Начислено жителям").Column("CHARGED_RESID").NotNull();
            this.Property(x => x.RecalcPrevPeriod, "Сумма перерасчета начисления за предыдущий период").Column("RECALC_PREV_PERIOD").NotNull();
            this.Property(x => x.ChangeSum, "Сумма изменений (перекидки)").Column("CHANGE_SUM").NotNull();
            this.Property(x => x.NoDeliverySum, "Сумма учтеной  недопоставки").Column("NO_DELIV_SUM").NotNull();
            this.Property(x => x.PaidResidents, "Оплачено жителями").Column("PAID_RESID").NotNull();
            this.Property(x => x.EndDebt, "Исходящее сальдо  (Долг на конец месяца)").Column("END_DEBT").NotNull();
            this.Property(x => x.ChargedToPay, "Начислено к оплате").Column("CHARGED_TO_PAY").NotNull();
            this.Property(x => x.TransferredPubServOrg, "Перечислено РСО").Column("TRANSFER_PUB_SERV_ORG").NotNull();
        }
    }
}