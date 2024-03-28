/// <mapping-converter-backup>
/// namespace Bars.Gkh.Overhaul.Tat.Map.LongTermProgram
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Overhaul.Tat.Entities;
/// 
///     public class MinFundSizeDecisionMap : BaseJoinedSubclassMap<MinFundSizeDecision>
///     {
///         public MinFundSizeDecisionMap()
///             : base("OVRHL_PR_DEC_MIN_FUND_SIZE", "ID")
///         {
///             Map(x => x.SubjectMinFundSize, "SUBJ_MIN_FUND_SIZE");
///             Map(x => x.OwnerMinFundSize, "OWN_MIN_FUND_SIZE");
///             Map(x => x.DateStart, "DATE_START");
///             Map(x => x.DateEnd, "DATE_END");
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Overhaul.Tat.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Overhaul.Tat.Entities;
    
    
    /// <summary>Маппинг для "Bars.Gkh.Overhaul.Tat.Entities.MinFundSizeDecision"</summary>
    public class MinFundSizeDecisionMap : JoinedSubClassMap<MinFundSizeDecision>
    {
        
        public MinFundSizeDecisionMap() : 
                base("Bars.Gkh.Overhaul.Tat.Entities.MinFundSizeDecision", "OVRHL_PR_DEC_MIN_FUND_SIZE")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.SubjectMinFundSize, "SubjectMinFundSize").Column("SUBJ_MIN_FUND_SIZE");
            Property(x => x.OwnerMinFundSize, "OwnerMinFundSize").Column("OWN_MIN_FUND_SIZE");
            Property(x => x.DateStart, "DateStart").Column("DATE_START");
            Property(x => x.DateEnd, "DateEnd").Column("DATE_END");
        }
    }
}
