namespace Bars.Gkh.RegOperator.Map.PersonalAccount.Operations
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.Operations;
    using Gkh.Map;
    /// <summary>
    /// Маппинг для <see cref="CancelCharge"/>
    /// </summary>
    public class CancelChargeMap : BaseImportableEntityMap<CancelCharge>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public CancelChargeMap()
            : base("Bars.Gkh.RegOperator.Entities.PersonalAccount.CancelCharge", "REGOP_CANCEL_CHARGE")
        {
        }

        /// <summary>
        /// Маппинг
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.CancelType, "Тип отмены начислений").Column("CANCEL_TYPE").NotNull();
            this.Property(x => x.CancelSum, "Сумма отмены").Column("CANCEL_SUM").NotNull();

            this.Reference(x => x.ChargeOperation, "Операция оплаты").Column("CHARGE_OP_ID").NotNull().Fetch();
            this.Reference(x => x.CancelPeriod, "Период, за который отменяем начисления").Column("CANCEL_PERIOD_ID").NotNull().Fetch();
            this.Reference(x => x.PersonalAccount, "Лицевой счет").Column("ACCOUNT_ID").NotNull().Fetch();
        }
    }
}
