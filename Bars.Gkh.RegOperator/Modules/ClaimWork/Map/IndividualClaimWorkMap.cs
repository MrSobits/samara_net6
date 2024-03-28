namespace Bars.Gkh.RegOperator.Map
{
    using Bars.Gkh.Map;
    using Bars.Gkh.RegOperator.Entities;
    
    public class IndividualClaimWorkMap : GkhJoinedSubClassMap<IndividualClaimWork>
    {
        public IndividualClaimWorkMap() : base("CLW_INDIVIDUAL_CLAIM_WORK")
        {
        }
        
        protected override void Map()
        {
        }
    }
}