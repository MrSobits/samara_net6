namespace Bars.Gkh.Overhaul.Tat.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Map;
    using Bars.Gkh.Overhaul.Tat.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Tat.Entities.SpecialAccount"</summary>
    public class SpecialAccountMap : JoinedSubClassMap<SpecialAccount>
    {
        
        public SpecialAccountMap() : 
                base("Bars.Gkh.Overhaul.Tat.Entities.SpecialAccount", "OVRHL_SPECIAL_ACCOUNT")
        {
        }
        
        protected override void Map()
        {
            this.Reference(x => x.AccountOwner, "AccountOwner").Column("OWNER_ID");
            this.Reference(x => x.CreditOrganization, "CreditOrganization").Column("CREDIT_ORG_ID");
        }
    }
}
