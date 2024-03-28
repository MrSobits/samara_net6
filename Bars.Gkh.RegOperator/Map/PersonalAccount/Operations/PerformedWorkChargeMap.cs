namespace Bars.Gkh.RegOperator.Map.PersonalAccount.Operations
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.Operations;

    /// <summary>
    /// Мапинг для операции по зачету средств
    /// </summary>
    public class PerformedWorkChargeMap : BaseImportableEntityMap<PerformedWorkCharge>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public PerformedWorkChargeMap()
            : base("Bars.Gkh.RegOperator.Entities.PersonalAccount.Operations.PerformedWorkCharge", "REGOP_PERF_WORK_CHARGE")
        {
        }

        /// <summary>
        /// Маппинг
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.Sum, "Сумма").Column("SUM").NotNull();
            this.Property(x => x.Active, "Флаг активности зачёта средств").Column("IS_ACTIVE").NotNull();
            this.Property(x => x.DistributeType, "Тип кошелька, на которую пошло распределение").Column("DISTRIBUTE_TYPE").NotNull();

            this.Reference(x => x.ChargeOperation, "Операция оплаты").Column("CHARGE_OP_ID").NotNull().Fetch();
            this.Reference(x => x.ChargePeriod, "Период, на который задится зачет средств").Column("PERIOD_ID").NotNull().Fetch();
        }
    }
}
