/// <mapping-converter-backup>
/// namespace Bars.Gkh.Modules.ClaimWork.Map
/// {
///     using B4.DataAccess.ByCode;
///     using Bars.Gkh.Modules.ClaimWork.Entities;
/// 
///     public class ViolClaimWorkMap : BaseEntityMap<ViolClaimWork>
///     {
///         public ViolClaimWorkMap()
///             : base("CLW_VIOL_CLAIM_WORK")
///         {
///             Map(x => x.Code, "CODE", false, 300);
///             Map(x => x.Name, "NAME", true, 1000);
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.Gkh.Modules.ClaimWork.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    
    
    /// <summary>Маппинг для "Справочник нарушений претензионной работы"</summary>
    public class ViolClaimWorkMap : BaseEntityMap<ViolClaimWork>
    {
        
        public ViolClaimWorkMap() : 
                base("Справочник нарушений претензионной работы", "CLW_VIOL_CLAIM_WORK")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.Code, "Код").Column("CODE").Length(300);
            Property(x => x.Name, "Наименваоние").Column("NAME").Length(1000);
        }
    }
}
