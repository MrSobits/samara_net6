/// <mapping-converter-backup>
/// namespace Bars.GkhGji.Map
/// {
///     using Bars.Gkh.Map;
///     using Bars.GkhGji.Entities;
/// 
///     public class RelatedAppealCitsMap : BaseGkhEntityMap<RelatedAppealCits>
///     {
///         public RelatedAppealCitsMap()
///             : base("GJI_REL_APPEAL_CITS")
///         {
///             References(x => x.Parent, "PARENT_ID").Not.Nullable().Fetch.Join();
///             References(x => x.Children, "CHILDREN_ID").Not.Nullable().Fetch.Join();
///         }
///     }
/// }
/// </mapping-converter-backup>

namespace Bars.GkhGji.Map
{
    using Bars.B4.Modules.Mapping.Mappers;
    using Bars.GkhGji.Entities;
    
    
    /// <summary>Маппинг для "Связанные обращения"</summary>
    public class RelatedAppealCitsMap : BaseEntityMap<RelatedAppealCits>
    {
        
        public RelatedAppealCitsMap() : 
                base("Связанные обращения", "GJI_REL_APPEAL_CITS")
        {
        }
        
        protected override void Map()
        {
            Property(x => x.ExternalId, "ExternalId").Column("EXTERNAL_ID");
            Reference(x => x.Parent, "Родительское обращение").Column("PARENT_ID").NotNull().Fetch();
            Reference(x => x.Children, "Дочернее обращение").Column("CHILDREN_ID").NotNull().Fetch();
        }
    }
}
