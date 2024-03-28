namespace Bars.Gkh.RegOperator.Map.PersonalAccount.Operations
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.Operations;

    /// <summary>
    /// Маппинг для <see cref="SaldoChangeDetail"/>
    /// </summary>
    public class SaldoChangeDetailMap : BaseImportableEntityMap<SaldoChangeDetail>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public SaldoChangeDetailMap()
            : base("Bars.Gkh.RegOperator.Entities.PersonalAccount.SaldoChangeDetail", "REGOP_SALDO_CHANGE_DETAIL")
        {
        }

        /// <summary>
        /// Маппинг
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.ChangeType, "Вид изменения").Column("CHANGE_TYPE").NotNull();
            this.Property(x => x.OldValue, "Старое значение").Column("OLD_VALUE").NotNull();
            this.Property(x => x.NewValue, "Новое значение").Column("NEW_VALUE").NotNull();

            this.Reference(x => x.ChargeOperation, "Инициатор изменения").Column("CHARGE_OP_ID").NotNull().Fetch();
            this.Reference(x => x.PersonalAccount, "Лицевой счет").Column("ACC_ID").NotNull().Fetch();
        }
    }
}

