/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Nso.Map.LongTermProgram
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Nso.Entities;
/// 
///     public class OwnerAccountDecisionMap : BaseJoinedSubclassMap<OwnerAccountDecision>
///     {
///         public OwnerAccountDecisionMap()
///             : base("OVRHL_PR_DEC_OWNER_ACCOUNT", "ID")
///         {
///             Map(x => x.OwnerAccountType, "OWNER_ACCOUNT_TYPE");
///             Map(x => x.DateStart, "DATE_START");
///             Map(x => x.DateEnd, "DATE_END");
/// 
///             References(x => x.Contragent, "CONTRAGENT_ID", ReferenceMapConfig.Fetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Nso.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Nso.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Nso.Entities.OwnerAccountDecision"</summary>
    public class OwnerAccountDecisionMap : JoinedSubClassMap<OwnerAccountDecision>
    {
        
        public OwnerAccountDecisionMap() : 
                base("Bars.Gkh.Overhaul.Nso.Entities.OwnerAccountDecision", "OVRHL_PR_DEC_OWNER_ACCOUNT")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.OwnerAccountType, "OwnerAccountType").Column("OWNER_ACCOUNT_TYPE");
            Reference(x => x.Contragent, "Contragent").Column("CONTRAGENT_ID").Fetch();
            Property(x => x.DateStart, "DateStart").Column("DATE_START");
            Property(x => x.DateEnd, "DateEnd").Column("DATE_END");
        }
    }
}
