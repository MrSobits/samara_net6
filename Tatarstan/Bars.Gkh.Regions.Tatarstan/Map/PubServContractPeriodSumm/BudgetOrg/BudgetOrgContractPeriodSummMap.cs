namespace Bars.Gkh.Regions.Tatarstan.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Regions.Tatarstan.Entities;

    public class BudgetOrgContractPeriodSummMap : BaseEntityMap<BudgetOrgContractPeriodSumm>
    {
        /// <inheritdoc />
        public BudgetOrgContractPeriodSummMap()
            : base("Bars.Gkh.Regions.Tatarstan.Entities.BudgetOrgContractPeriodSumm", "GKH_BUDGET_CONTR_PERIOD_SUMM")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.Charged, "Начислено").Column("CHARGED").NotNull();
            this.Property(x => x.Paid, "Оплачено").Column("PAID").NotNull();
            this.Property(x => x.EndDebt, "Задолженность на конец месяца").Column("END_DEBT").NotNull();

            this.Reference(x => x.Municipality, "Муниципальный район").Column("MUNICIPALITY_ID").NotNull();
            this.Reference(x => x.ContractPeriod, "Отчетный период договора").Column("PERIOD_ID").NotNull();
            this.Reference(x => x.ContractService, "Услуга по договору").Column("SERVICE_ID").NotNull();
            this.Reference(x => x.BudgetOrgContract, "Сторона договора \"Бюджетная организация\"").Column("CONTRACT_ID").NotNull();
            this.Reference(x => x.PublicServiceOrg, "РСО").Column("PUB_SERV_ORG_ID").NotNull();
        }
    }
}