namespace Bars.Gkh.RegOperator.Map.PersonalAccount.Operations
{
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.Operations;

    public class SplitAccountDetailMap : GkhBaseEntityMap<SplitAccountDetail>
    {
        /// <inheritdoc />
        public SplitAccountDetailMap()
            : base("REGOP_SPLIT_ACC_DETAIL")
        {
        }

        /// <inheritdoc />
        protected override void Map()
        {
            this.Property(x => x.Amount, "Сумма переноса").Column("AMOUNT").NotNull();
            this.Property(x => x.WalletType, "Тип кошелька").Column("WALLET_TYPE").NotNull();

            this.Reference(x => x.Account, "Лицевой счет зачисления").Column("ACCOUNT_ID").NotNull();
            this.Reference(x => x.Operation, "Операция, в рамках которой производился перенос долга").Column("CHARGE_OP_ID").NotNull();
        }
    }
}