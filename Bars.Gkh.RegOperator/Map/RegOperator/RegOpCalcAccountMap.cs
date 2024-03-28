namespace Bars.Gkh.RegOperator.Map
{
    using Entities;

    using FluentNHibernate.Mapping;

    public class RegOpCalcAccountMap : SubclassMap<RegOpCalcAccount>
    {
        public RegOpCalcAccountMap()
        {
            Table("OVRHL_REG_OP_CALC_ACC");
            KeyColumn("ID");

            Map(x => x.BalanceIncome, "BALANCE_OUT");
            Map(x => x.BalanceOut, "BALANCE_INCOME");
            Map(x => x.IsSpecial, "IS_SPECIAL");

            //References(x => x.CreditOrg, "CREDIT_ORG_ID").Not.Nullable().Fetch.Join();
            References(x => x.RegOperator, "REG_OP_ID").Not.Nullable().Fetch.Join();
            References(x => x.ContragentBankCrOrg, "CA_BANK_CR_ORG_ID").Nullable().Fetch.Join();
        }
    }
}
