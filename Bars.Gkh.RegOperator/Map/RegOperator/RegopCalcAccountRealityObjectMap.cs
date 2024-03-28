namespace Bars.Gkh.RegOperator.Map
{
    using B4.DataAccess.ByCode;
    using Entities;

    public class RegopCalcAccountRealityObjectMap : BaseEntityMap<RegopCalcAccountRealityObject>
    {
        public RegopCalcAccountRealityObjectMap() : base("REGOP_CALCACC_RO")
        {
            References(x => x.RealityObject, "RO_ID", ReferenceMapConfig.NotNullAndFetchAndCascadeDelete);
            References(x => x.RegOpCalcAccount, "REGOP_CALC_ACC_ID", ReferenceMapConfig.NotNullAndFetchAndCascadeDelete);
        }
    }
}