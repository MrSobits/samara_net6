namespace Bars.Gkh.RegOperator.Map.RealityAccount
{
    using B4.DataAccess.ByCode;
    using Entities;

    public class RealityObjectSpecialOrRegOperatorAccountMap : BaseEntityMap<RealityObjectSpecialOrRegOperatorAccount>
    {
        public RealityObjectSpecialOrRegOperatorAccountMap() : base("REGOP_RO_SPEC_ACC")
        {
            Map(x => x.IsActive, "IS_ACTIVE");
            Map(x => x.RegOperator, "REG_OPERATOR");
            Map(x => x.AccountType, "ACC_TYPE");

            References(x => x.Contragent, "CA_ID", ReferenceMapConfig.Fetch);
            References(x => x.RealityObjectChargeAccount, "RO_CHARGE_ACC_ID", ReferenceMapConfig.NotNullAndFetch);
        }
    }
}