/// <mapping-converter-backup>
/// namespace Bars.Gkh.Modules.ClaimWork.Map
/// {
///     using Bars.B4.DataAccess.ByCode;
///     using Bars.Gkh.Modules.ClaimWork.Entities;
/// 
///     public class CourtOrderClaimMap : BaseJoinedSubclassMap<CourtOrderClaim>
///     {
///         public CourtOrderClaimMap() : base("clw_court_order_claim", "id")
///         {
///             Map(x => x.ClaimDate, "claim_date");
///             Map(x => x.ObjectionArrived, "objection_arrived");
/// 
///             References(x => x.Document, "document_id", ReferenceMapConfig.Fetch);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Modules.ClaimWork.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    
    
    /// <summary>Маппинг для "Заявление о выдаче судебного приказа без искового заявления"</summary>
    public class CourtOrderClaimMap : JoinedSubClassMap<CourtOrderClaim>
    {
        
        public CourtOrderClaimMap() : 
                base("Заявление о выдаче судебного приказа без искового заявления", "CLW_COURT_ORDER_CLAIM")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ObjectionArrived, "Поступило возражение").Column("OBJECTION_ARRIVED");
            Property(x => x.ClaimDate, "Дата заявления").Column("CLAIM_DATE");
            Reference(x => x.Document, "Документ").Column("DOCUMENT_ID").Fetch();
        }
    }
}
