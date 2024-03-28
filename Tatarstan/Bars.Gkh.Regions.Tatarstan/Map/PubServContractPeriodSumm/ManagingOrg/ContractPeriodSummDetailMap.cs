namespace Bars.Gkh.Regions.Tatarstan.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Regions.Tatarstan.Entities;

    /// <summary>
    /// Маппинг
    /// </summary>
    public class ContractPeriodSummDetailMap : BaseEntityMap<ContractPeriodSummDetail>
    {
        /// <summary>
        /// ctor
        /// </summary>
        public ContractPeriodSummDetailMap()
            : base("Bars.Gkh.Regions.Tatarstan.Entities.ContractPeriodSummDetail", "GKH_CONTR_PERIOD_SUMM_DETAIL")
        {
        }

        /// <summary>
        /// Мап
        /// </summary>
        protected override void Map()
        {
            this.Reference(x => x.PublicServiceOrgContractRealObjInContract, "Дом в Договоре РСО с Домами").Column("RO_ID").NotNull().Fetch();
            this.Reference(x => x.ContractPeriodSumm, "Информация по периоду").Column("PER_SUMM_ID").NotNull().Fetch();

            this.Property(x => x.StartDebt, "Входящее сальдо  (Долг на начало месяца)").Column("START_DEBT").NotNull();
            this.Property(x => x.ChargedResidents, "Начислено жителям").Column("CHARGED_RESID").NotNull();
            this.Property(x => x.RecalcPrevPeriod, "Сумма перерасчета начисления за предыдущий период").Column("RECALC_PREV_PERIOD").NotNull();
            this.Property(x => x.ChangeSum, "Сумма изменений (перекидки)").Column("CHANGE_SUM").NotNull();
            this.Property(x => x.NoDeliverySum, "Сумма учтеной  недопоставки").Column("NO_DELIV_SUM").NotNull();
            this.Property(x => x.PaidResidents, "Оплачено жителями").Column("PAID_RESID").NotNull();
            this.Property(x => x.EndDebt, "Исходящее сальдо  (Долг на конец месяца)").Column("END_DEBT").NotNull();
            this.Property(x => x.ChargedToPay, "Начислено к оплате").Column("CHARGED_TO_PAY").NotNull();
            this.Property(x => x.TransferredPubServOrg, "Перечислено РСО").Column("TRANSFER_PUB_SERV_ORG").NotNull();
            this.Property(x => x.ChargedManOrg, "Начислено  УО").Column("CHARGED_MAN_ORG").NotNull();
            this.Property(x => x.PaidManOrg, "Оплачено УО").Column("PAID_MAN_ORG").NotNull();
            this.Property(x => x.SaldoOut, "Исходящее сальдо ").Column("SALDO_OUT").NotNull();
        }
    }
}